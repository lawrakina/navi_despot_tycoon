import Foundation

public struct Future<Value> {
    public typealias Result = Swift.Result<Value, Error>
    public typealias ObserveCallback = (Result) -> Void

    let observeHandler: (@escaping ObserveCallback) -> Void

    fileprivate init(handler: @escaping (@escaping ObserveCallback) -> Void) {
        self.observeHandler = handler
    }

    public func observe(using callback: @escaping ObserveCallback) {
        self.observeHandler(callback)
    }
}

protocol ToFuture {
    associatedtype Value
    func toFuture() -> Future<Value>
}

protocol Promise {
    associatedtype Value
    func resolve(_ value: Value) -> Future<Value>
    func reject(_ error: Error) -> Future<Value>
}

class FutureImpl<Value>: ToFuture, Promise {
    typealias Value = Value
    typealias Result = Swift.Result<Value, Error>

    private var result: Result?
    private var callbacks = [(Result) -> Void]()

    init(_ value: Value? = nil) {
        // If the value was already known at the time the promise
        // was constructed, we can report it directly:
        result = value.map(Result.success)
    }

    func resolve(_ value: Value) -> Future<Value> {
        return fulfill(.success(value))
    }

    func reject(_ error: Error) -> Future<Value> {
        return fulfill(.failure(error))
    }

    func toFuture() -> Future<Value> {
        return Future(handler: self.observe(using:))
    }

    func observe(using callback: @escaping (Result) -> Void) {
        objc_sync_enter(self)
        // If a result has already been set, call the callback directly:
        if let result = result {
            objc_sync_exit(self)

            return callback(result)
        }

        defer { objc_sync_exit(self) }

        callbacks.append(callback)
    }

    func fulfill(_ result: Result) -> Future<Value> {
        let toCall = doFulfill(result)

        // will always call with the correct result. if we are called twice,
        // there are no new callbacks because they all already find a result available
        toCall.forEach { $0(result) }

        return self.toFuture()
    }

    func handle(_ callback: () throws -> Void) {
        do {
            try callback()
        } catch {
            _ = self.reject(error)
        }
    }

    func fulfillWith(_ callback: () throws -> Value) -> FutureImpl<Value> {
        var result: Value? = nil
        do {
            result = try callback()
        } catch {
            _ = self.reject(error)
            return self
        }
        if let result = result {
            _ = self.resolve(result)
            return self
        }
        return self
    }

    private func doFulfill(_ result: Result) -> [(Result) -> Void] {
        var toCall = [(Result) -> Void]()

        objc_sync_enter(self)
        defer { objc_sync_exit(self) }

        // if we set a value twice, just ignore it
        if self.result == nil {
            self.result = result
        }

        toCall = callbacks
        callbacks = []

        return toCall
    }
}

struct TransformingFuture<Source, Target>: ToFuture {
    typealias Value = Target
    private let mapped: Future<Source>
    private let mapper: (Source) -> Target

    init(_ mapped: Future<Source>, _ mapper: @escaping (Source) -> Target) {
        self.mapped = mapped
        self.mapper = mapper
    }

    func toFuture() -> Future<Target> {
        return Future(handler: self.observe(using:))
    }

    func observe(using callback: @escaping (Result<Target, Error>) -> Void) {
        mapped.observe(using: {
            switch $0 {
            case .failure(let err):
                callback(.failure(err))
            case .success(let source):
                callback(.success(self.mapper(source)))
            }
        })
    }
}

class RetryingFuture<Value>: FutureImpl<Value> {
    private var retries: Int
    private let totalRetries: Int
    private var getTry: (() -> Future<Value>)?
    private let errorClassifier: ErrorClassifier

    init(retries: Int, classifier: ErrorClassifier, for getTry: @escaping () -> Future<Value>) {
        self.retries = 0
        self.totalRetries = retries
        self.errorClassifier = classifier
        self.getTry = getTry
        super.init()

        getTry().observe(using: self.observeChild)
    }

    private func observeChild(response: Future<Value>.Result) {
        if retries >= totalRetries {
            _ = fulfill(response)
            return
        }

        switch response {
        case .failure(let error):
            retries += 1

            let waitFor: TimeInterval
            switch self.classifyError(error: error) {
            case .unrecoverable:
                _ = fulfill(response)
                return
            case .retryDefault:
                waitFor = min(15.0, 0.25 * pow(2.0, Double(retries)) * Double.random(in: 0.5...1.5))
            case .recoverable(let waitTime):
                waitFor = waitTime
            }

            // we have to switch to the main thread to schedule a timer, otherwise
            // it won't have a run loop associcated with it and will never fire
            DispatchQueue.main.async {
                Timer.scheduledTimer(withTimeInterval: waitFor, repeats: false) { [self] _ in
                    if let getTry = self.getTry {
                        getTry().observe(using: self.observeChild)
                    } else {
                        _ = self.reject(error)
                    }
                }
            }
        case .success(let data):
            self.getTry = nil
            _ = resolve(data)
        }
    }

    private func classifyError(error: Error) -> ErrorClassification {
        if let justTrackError = error as? JustTrackErrorWrapper {
            if let networkError = justTrackError.error as? NetworkError {
                return errorClassifier.classify(error: networkError)
            }
        }

        return .retryDefault
    }
}
