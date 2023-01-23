import Foundation

protocol HttpLogger: Logger {
    func setIDFA(idfa: String)
    func setUuid(uuid: UUID)
}

class HttpLoggerImpl: HttpLogger {
    private let fallback: Logger
    private let httpClient: HttpClient
    private var idfa: String?
    private var uuid: UUID?

    init(fallback: Logger, httpClient: HttpClient) {
        self.fallback = fallback
        self.httpClient = httpClient
        self.idfa = nil
        self.uuid = nil
    }

    func setIDFA(idfa: String) {
        self.idfa = idfa
    }

    func setUuid(uuid: UUID) {
        self.uuid = uuid
    }

    func debug(_ message: String, _ fields: [LoggerFields]) {
        fallback.debug(message, fields)
        log("debug", message, fields)
    }

    func info(_ message: String, _ fields: [LoggerFields]) {
        fallback.info(message, fields)
        log("info", message, fields)
    }

    func warn(_ message: String, _ fields: [LoggerFields]) {
        fallback.warn(message, fields)
        log("warn", message, fields)
    }

    func error(_ message: String, _ fields: [LoggerFields]) {
        fallback.error(message, fields)
        log("error", message, fields)

    }

    func error(_ message: String, _ exception: Error, _ fields: [LoggerFields]) {
        fallback.error(message, exception, fields)
        log("error", message, fields, exception)
    }

    private func log(_ level: String, _ message: String, _ fields: [LoggerFields], _ exception: Error? = nil) {
        var encodedFields = [String: String]()

        for loggerFields in fields {
            addFields(loggerFields, &encodedFields)
        }

        if let exception = exception {
            addFields(LoggerFieldsImpl().with("exception", exception), &encodedFields)
        }

        let appVersion = readAppVersion()
        let sdkVersion = currentSdkVersion()
        let input = DTOLogInput(message, encodedFields, appVersion: DTOInputNamedVersion(appVersion), sdkVersion: DTOInputVersion(sdkVersion))
        let f = httpClient.postLogs(logLevel: level, input: input, idfa: idfa, uuid: uuid)
        f.observe(using: { result in
            switch result {
            case .success:
                self.fallback.debug("Published log message", LoggerFieldsImpl().with("message", message))
            case .failure(let error):
                self.fallback.error("Failed to publish log message", error)
            }
        })
    }

    private func addFields(_ fields: LoggerFields, _ encodedFields: inout [String: String]) {
        for field in fields.getFields() {
            encodedFields[field.key] = field.value
        }
    }
}
