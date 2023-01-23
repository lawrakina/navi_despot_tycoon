import Foundation
import Compression

enum NetworkError: JustTrackError {
    case badUrl(String)
    case networkError(Error)
    case badResponseType(URLResponse?)
    case unexpectedResponse(Int, Environment, String, String?)
    case missingResponseData

    public func describeError() -> String {
        switch self {
        case .badUrl(let url):
            return "NetworkError.badUrl(\(url))"
        case .networkError(let error):
            return "NetworkError.networkError(\(error.localizedDescription))"
        case .badResponseType(let response):
            return "NetworkError.badResponseType(\(response?.debugDescription ?? "nil response"))"
        case .unexpectedResponse(let statusCode, let environment, let token, let responseBody):
            if statusCode == 401 {
                return "NetworkError.unexpectedResponse(\(statusCode))\n\n\(formatInvalidToken(environment: environment, token: token, responseBody: responseBody))"
            }
            return "NetworkError.unexpectedResponse(\(statusCode))"
        case .missingResponseData:
            return "NetworkError.missingResponseData"
        }
    }

    func formatInvalidToken(environment: Environment, token: String, responseBody: String?) -> String {
        var lines = [
            "Request could not be authenticated. Is the API token correct?",
            "See https://docs.justtrack.io/sdk/android-sdk-readme#getting-an-api-token how to get an API token.",
            "",
            "Used API Token: \(environment.getName())-\(token)",
        ]

        if var responseBody = responseBody {
            if responseBody.count > 64 {
                responseBody = responseBody.prefix(64) + "..."
            }
            lines.append("Response Body: \(responseBody)")
        }

        return formatBoxed(lines: lines)
    }

    func formatBoxed(lines: [String]) -> String {
        var longest = 0
        for line in lines {
            longest = max(longest, line.count)
        }

        let stars = String(repeating: "*", count: longest + 4)
        var output = [stars]
        for line in lines {
            output.append("* \(line)\(String(repeating: " ", count: longest - line.count)) *")
        }
        output.append(stars)

        return output.joined(separator: "\n")
    }
}

enum ErrorClassification {
    case unrecoverable
    case recoverable(TimeInterval)
    case retryDefault
}

protocol ErrorClassifier {
    func classify(error: NetworkError) -> ErrorClassification
}

struct AttributionErrorClassifier: ErrorClassifier {
    func classify(error: NetworkError) -> ErrorClassification {
        switch error {
        case .unexpectedResponse(let code, _, _, _):
            if code >= 400 && code < 500 {
                // something from our request was wrong, no point in retrying, the backend needs to be fixed
                return .unrecoverable
            }

            if code >= 500 {
                // the backend is having problems right now, so we wait up to 5 minutes before retrying.
                // while this is harsh, there is no point in bombarding the backend, most users will
                // have quit the app by then.
                let minutes = Double.random(in: 0...5)
                return .recoverable(minutes * 60)
            }

            return .retryDefault
        case .badUrl(_):
            return .unrecoverable
        case .missingResponseData:
            return .retryDefault
        case .badResponseType(_):
            return .retryDefault
        case .networkError(_):
            return .retryDefault
        }
    }
}

struct TrackingEventErrorClassifier: ErrorClassifier {
    func classify(error: NetworkError) -> ErrorClassification {
        switch error {
        case .unexpectedResponse(_, _, _, _):
            // the backend seems to be having problems. Better not to overload it with too many requests,
            // we will retry later anyway
            return .unrecoverable
        case .badUrl(_):
            return .unrecoverable
        case .missingResponseData:
            return .retryDefault
        case .badResponseType(_):
            return .retryDefault
        case .networkError(_):
            return .retryDefault
        }
    }
}

protocol HttpClient {
    func postAttribution(request: DTOAttributionRequest, idfa: String?) -> Future<Data>
    func postEvent(events: DTOUserEvent, idfa: String?, uuid: UUID) -> Future<Data>
    func postFirebaseInstanceId(json: Data, idfa: String?, uuid: UUID) -> Future<Data>
    func postLogs(logLevel: String, input: DTOLogInput, idfa: String?, uuid: UUID?) -> Future<Data>
}

class HttpClientImpl: HttpClient {
    private typealias Headers = [String: String]

    private let environment: Environment
    private let apiToken: String
    private let crc32Coder: CRC32Coder
    private let logger: Logger

    init(environment: Environment, apiToken: String) {
        self.environment = environment
        self.apiToken = apiToken
        self.crc32Coder = CRC32Coder()
        self.logger = LoggerImpl() // just log to console, we don't want to publish logs about HTTP requests with an HTTP request
    }

    func postAttribution(request: DTOAttributionRequest, idfa: String?) -> Future<Data> {
        let headers = getHeaders(idfa: idfa)
        let url = environment.getUrl(.attribution)
        return executeAsyncPostRequestWithRetry(retries: 3, classifier: AttributionErrorClassifier(), urlString: url, headers: headers, body: request.json())
    }

    func postEvent(events: DTOUserEvent, idfa: String?, uuid: UUID) -> Future<Data> {
        let headers = getHeaders(idfa: idfa, uuid: uuid)
        let url = environment.getUrl(.trackEvent)
        let f = executeAsyncPostRequestWithRetry(retries: 3, classifier: TrackingEventErrorClassifier(), urlString: url, headers: headers, body: events.json())
        f.observe(using: { result in
            switch result {
            case .failure(let error):
                for event in events.events {
                    self.logger.error("Event failed to publish", LoggerFieldsImpl().with("id", event.id).with("event", event.name).with("error", error.localizedDescription))
                }
            case .success(_):
                for event in events.events {
                    self.logger.debug("Published event in batch", LoggerFieldsImpl().with("id", event.id).with("event", event.name))
                }
            }
        })
        return f
    }

    func postFirebaseInstanceId(json: Data, idfa: String?, uuid: UUID) -> Future<Data> {
        let headers = getHeaders(idfa: idfa, uuid: uuid)
        let url = environment.getUrl(.publishFirebaseInstanceId)
        return executeAsyncPostRequestWithRetry(retries: 3, classifier: AttributionErrorClassifier(), urlString: url, headers: headers, body: json)
    }

    func postLogs(logLevel: String, input: DTOLogInput, idfa: String?, uuid: UUID?) -> Future<Data> {
        let headers = getHeaders(idfa: idfa, uuid: uuid)
        let url = environment.getUrl(.log, logLevel)
        return executeAsyncPostRequestWithRetry(retries: 3, classifier: AttributionErrorClassifier(), urlString: url, headers: headers, body: input.json())
    }

    private func getHeaders(idfa: String?, uuid: UUID? = nil) -> Headers {
        let bundleIdentifier = Bundle.main.bundleIdentifier

        var headers = Headers()
        headers["X-CLIENT-ID"] = bundleIdentifier
        headers["X-CLIENT-TOKEN"] = apiToken
        headers["X-ADVERTISER-ID"] = idfa

        if let uuid = uuid {
            headers["X-USER-ID"] = uuid.uuidString.lowercased()
        }

        // no need to set accept-encoding as the client we are using is already supporting that
        headers["Content-Type"] = "application/json; charset=utf-8"
        headers["User-Agent"] = "JustTrack iOS SDK \(currentSdkVersion().getName())"

        return headers
    }

    private func executeAsyncPostRequestWithRetry(retries: Int, classifier: ErrorClassifier, urlString: String, headers: Headers, body: Data) -> Future<Data> {
        return RetryingFuture(retries: retries, classifier: classifier, for: {
            self.logger.debug("Starting new HTTP request", LoggerFieldsImpl().with("url", urlString))

            return self.executeAsyncPostRequest(urlString: urlString, headers: headers, body: body)
        }).toFuture()
    }

    private func executeAsyncPostRequest(urlString: String, headers: Headers, body: Data) -> Future<Data> {
        var result = FutureImpl<Data>()
        guard let url = URL(string: urlString) else {
            return result.reject(JustTrackErrorWrapper(NetworkError.badUrl(urlString)))
        }

        var request = URLRequest(url: url)
        request.httpMethod = "POST"
        request.timeoutInterval = 30.0
        for header in headers {
            request.setValue(header.value, forHTTPHeaderField: header.key)
        }
        if let compressedBody = gzip(body) {
            request.setValue("gzip", forHTTPHeaderField: "Content-Encoding")
            request.httpBody = compressedBody
        } else {
            request.httpBody = body
        }

        let task = URLSession.shared.dataTask(with: request) { (data, response, error) in
            result = result.fulfillWith({
                if let error = error {
                    self.logger.error("HTTP request failed with network error", LoggerFieldsImpl().with("url", urlString).with("error", error.localizedDescription))

                    throw JustTrackErrorWrapper(NetworkError.networkError(error))
                }

                guard let httpResponse = response as? HTTPURLResponse else {
                    self.logger.error("HTTP request failed with bad response type", LoggerFieldsImpl().with("url", urlString))

                    throw JustTrackErrorWrapper(NetworkError.badResponseType(response))
                }

                if !(200...299).contains(httpResponse.statusCode) {
                    var responseBody: String? = nil
                    if let data = data {
                        responseBody = String(data: data, encoding: .utf8)
                    }
                    self.logger.error("HTTP request failed with bad status code", LoggerFieldsImpl().with("url", urlString).with("code", httpResponse.statusCode))

                    throw JustTrackErrorWrapper(NetworkError.unexpectedResponse(httpResponse.statusCode, self.environment, self.apiToken, responseBody))
                }

                guard let data = data else {
                    self.logger.error("HTTP request failed with missing response data", LoggerFieldsImpl().with("url", urlString))

                    throw JustTrackErrorWrapper(NetworkError.missingResponseData)
                }

                self.logger.debug("HTTP request succeeded", LoggerFieldsImpl().with("url", urlString))

                return data
            })
        }

        task.resume()

        return result.toFuture()
    }

    private func gzip(_ data: Data) -> Data? {
        guard let deflatedData = deflate(data) else {
            return nil
        }

        let now = UInt32(Date().timeIntervalSince1970)
        let crc32 = crc32Coder.crc32(data)
        let size = UInt32(data.count & 0xFFFFFFFF)

        let header: [UInt8] = [
            0x1f, // magic number 1
            0x8b, // magic number 2
            8, // deflate compression
            0, // no flags set
            UInt8(now & 0xFF), // 32 bit little endian modification time
            UInt8((now >> 8) & 0xFF),
            UInt8((now >> 16) & 0xFF),
            UInt8((now >> 24) & 0xFF),
            0, // neither best nor fastest compression
            255, // unknown OS
        ]
        let trailer: [UInt8] = [
            UInt8(crc32 & 0xFF),
            UInt8((crc32 >> 8) & 0xFF),
            UInt8((crc32 >> 16) & 0xFF),
            UInt8((crc32 >> 24) & 0xFF),
            UInt8(size & 0xFF),
            UInt8((size >> 8) & 0xFF),
            UInt8((size >> 16) & 0xFF),
            UInt8((size >> 24) & 0xFF),
        ]

        var result = Data(capacity: 10 + deflatedData.count + 8)
        result.append(contentsOf: header)
        result.append(deflatedData)
        result.append(contentsOf: trailer)
        return result
    }

    private func deflate(_ data: Data) -> Data? {
        return data.withUnsafeBytes { dataBufferRawPtr in
            let destinationBuffer = UnsafeMutablePointer<UInt8>.allocate(capacity: data.count)
            defer { destinationBuffer.deallocate() }
            let algorithm = COMPRESSION_ZLIB

            let dataBufferPtr = dataBufferRawPtr.bindMemory(to: UInt8.self)
            guard let dataPtr = dataBufferPtr.baseAddress else {
                return nil
            }

            let compressedSize = compression_encode_buffer(destinationBuffer, data.count, dataPtr, data.count, nil, algorithm)

            if compressedSize == 0 {
                return nil
            }

            return Data(bytes: destinationBuffer, count: compressedSize)
        }
    }
}

fileprivate struct CRC32Coder {
    private let table: [UInt32]

    fileprivate init() {
        var table: [UInt32] = []

        for n in 0..<256 {
            var c = UInt32(n)
            for _ in 0..<8 {
                if c & 1 != 0 {
                    c = 0xedb88320 ^ (c >> 1)
                } else {
                    c = c >> 1
                }
            }
            table.append(c)
        }

        self.table = table
    }

    fileprivate func crc32(_ data: Data) -> UInt32 {
        var c: UInt32 = 0 ^ 0xffffffff

        for b in data {
            c = table[Int(UInt8(c & 0xFF) ^ b)] ^ (c >> 8)
        }

        return c ^ 0xffffffff
    }
}
