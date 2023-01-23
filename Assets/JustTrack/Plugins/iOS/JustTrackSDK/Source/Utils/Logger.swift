import os.log

public protocol Logger {
    func debug(_ message: String, _ fields: LoggerFields...)
    func info(_ message: String, _ fields: LoggerFields...)
    func warn(_ message: String, _ fields: LoggerFields...)
    func error(_ message: String, _ fields: LoggerFields...)
    func error(_ message: String, _ exception: Error, _ fields: LoggerFields...)

    func debug(_ message: String, _ fields: [LoggerFields])
    func info(_ message: String, _ fields: [LoggerFields])
    func warn(_ message: String, _ fields: [LoggerFields])
    func error(_ message: String, _ fields: [LoggerFields])
    func error(_ message: String, _ exception: Error, _ fields: [LoggerFields])
}

extension Logger {
    public func debug(_ message: String, _ fields: LoggerFields...) {
        debug(message, fields)
    }

    public func info(_ message: String, _ fields: LoggerFields...) {
        info(message, fields)
    }

    public func warn(_ message: String, _ fields: LoggerFields...) {
        warn(message, fields)
    }

    public func error(_ message: String, _ fields: LoggerFields...) {
        error(message, fields)
    }

    public func error(_ message: String, _ exception: Error, _ fields: LoggerFields...) {
        error(message, exception, fields)
    }
}

public protocol LoggerFields {
    func getFields() -> Dictionary<String, String>
}

public protocol LoggerFieldsBuilder: LoggerFields {
    func with(_ field: String, _ value: String) -> LoggerFieldsBuilder
    func with(_ field: String, _ value: Error) -> LoggerFieldsBuilder
    func with(_ field: String, _ value: Character) -> LoggerFieldsBuilder
    func with(_ field: String, _ value: Bool) -> LoggerFieldsBuilder
    func with(_ field: String, _ value: UInt8) -> LoggerFieldsBuilder
    func with(_ field: String, _ value: Int8) -> LoggerFieldsBuilder
    func with(_ field: String, _ value: UInt16) -> LoggerFieldsBuilder
    func with(_ field: String, _ value: Int16) -> LoggerFieldsBuilder
    func with(_ field: String, _ value: UInt32) -> LoggerFieldsBuilder
    func with(_ field: String, _ value: Int32) -> LoggerFieldsBuilder
    func with(_ field: String, _ value: UInt64) -> LoggerFieldsBuilder
    func with(_ field: String, _ value: Int64) -> LoggerFieldsBuilder
    func with(_ field: String, _ value: UInt) -> LoggerFieldsBuilder
    func with(_ field: String, _ value: Int) -> LoggerFieldsBuilder
    func with(_ field: String, _ value: Float) -> LoggerFieldsBuilder
    func with(_ field: String, _ value: Double) -> LoggerFieldsBuilder
}

extension LoggerFieldsBuilder {
    public func with(_ field: String, _ value: Character) -> LoggerFieldsBuilder {
        return with(field, String(value))
    }

    public func with(_ field: String, _ value: Bool) -> LoggerFieldsBuilder {
        return with(field, String(value))
    }

    public func with(_ field: String, _ value: UInt8) -> LoggerFieldsBuilder {
        return with(field, String(value))
    }

    public func with(_ field: String, _ value: Int8) -> LoggerFieldsBuilder {
        return with(field, String(value))
    }

    public func with(_ field: String, _ value: UInt16) -> LoggerFieldsBuilder {
        return with(field, String(value))
    }

    public func with(_ field: String, _ value: Int16) -> LoggerFieldsBuilder {
        return with(field, String(value))
    }

    public func with(_ field: String, _ value: UInt32) -> LoggerFieldsBuilder {
        return with(field, String(value))
    }

    public func with(_ field: String, _ value: Int32) -> LoggerFieldsBuilder {
        return with(field, String(value))
    }

    public func with(_ field: String, _ value: UInt64) -> LoggerFieldsBuilder {
        return with(field, String(value))
    }

    public func with(_ field: String, _ value: Int64) -> LoggerFieldsBuilder {
        return with(field, String(value))
    }

    public func with(_ field: String, _ value: UInt) -> LoggerFieldsBuilder {
        return with(field, String(value))
    }

    public func with(_ field: String, _ value: Int) -> LoggerFieldsBuilder {
        return with(field, String(value))
    }

    public func with(_ field: String, _ value: Float) -> LoggerFieldsBuilder {
        return with(field, String(value))
    }

    public func with(_ field: String, _ value: Double) -> LoggerFieldsBuilder {
        return with(field, String(value))
    }
}

class LoggerFieldsImpl: LoggerFieldsBuilder {
    private var fields: Dictionary<String, String>

    init() {
        fields = Dictionary()
    }

    public func with(_ field: String, _ value: String) -> LoggerFieldsBuilder {
        fields[field] = value

        return self
    }

    public func with(_ field: String, _ value: Error) -> LoggerFieldsBuilder {
        fields[field] = value.localizedDescription

        return self
    }

    public func getFields() -> Dictionary<String, String> {
        return fields
    }
}

@available(iOS 10.0, *)
class LoggerImpl: Logger {
    @available(iOS 14.0, *)
    private static let log = os.Logger(subsystem: "io.justtrack", category: "network")

    public func debug(_ message: String, _ fields: [LoggerFields]) {
        doLog(message: message, level: .debug, fields)
    }

    public func info(_ message: String, _ fields: [LoggerFields]) {
        doLog(message: message, level: .info, fields)
    }

    public func warn(_ message: String, _ fields: [LoggerFields]) {
        // os_log does not support warnings, so log them as info instead
        doLog(message: message, level: .info, fields)
    }

    public func error(_ message: String, _ fields: [LoggerFields]) {
        doLog(message: message, level: .error, fields)
    }

    public func error(_ message: String, _ exception: Error, _ fields: [LoggerFields]) {
        let newFields = [
            LoggerFieldsImpl().with("exception", exception),
        ] + fields
        doLog(message: message, level: .error, newFields)
    }

    private func doLog(message: String, level: OSLogType, _ fields: [LoggerFields]) {
        var logMessage = message

        for loggerFields in fields {
            for field in loggerFields.getFields() {
                logMessage += ", " + field.key + " = " + field.value
            }
        }

        if #available(iOS 14.0, *) {
            LoggerImpl.log.log(level: level, "JustTrackSDK: \(logMessage, privacy: .public)")
        } else {
            os_log("JustTrackSDK: %s", type: level, logMessage)
        }
    }
}
