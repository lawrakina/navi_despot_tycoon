import Foundation

public protocol JustTrackError: Error {
    func describeError() -> String
}

public class JustTrackErrorWrapper: NSObject, LocalizedError {
    public let error: JustTrackError

    public init(_ error: JustTrackError) {
        self.error = error
    }

    public override var description: String {
        get {
            return self.error.describeError()
        }
    }

    public var errorDescription: String? {
        get {
            return self.description
        }
    }
}
