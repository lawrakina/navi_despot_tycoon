import Foundation

public protocol SDKBuilder {
    func set(logger: Logger) -> Self
    func set(firebaseInstanceId: String) -> Self
    func set(inactivityTimeFrameHours: Int) -> Self
    func set(reAttributionTimeFrameDays: Int) -> Self
    func set(reFetchReAttributionDelaySeconds: Int) -> Self

    func build() throws -> JustTrackSDK
}

public class JustTrackSDKBuilder: SDKBuilder {
    private var logger: Logger?
    private var firebaseInstanceId: String?
    private var trackingId: String
    private var trackingProvider: String
    private let apiToken: String
    private var inactivityTimeFrameHours: Int
    private var reAttributionTimeFrameDays: Int
    private var reFetchReAttributionDelaySeconds: Int

    public init(apiToken: String, trackingId: String, trackingProvider: String) {
        self.apiToken = apiToken
        self.trackingId = trackingId
        self.trackingProvider = trackingProvider
        self.inactivityTimeFrameHours = 48
        self.reAttributionTimeFrameDays = 14
        self.reFetchReAttributionDelaySeconds = 5
    }

    public func set(logger: Logger) -> Self {
        self.logger = logger

        return self
    }

    public func set(firebaseInstanceId: String) -> Self {
        self.firebaseInstanceId = firebaseInstanceId

        return self
    }

    public func set(inactivityTimeFrameHours: Int) -> Self {
        self.inactivityTimeFrameHours = inactivityTimeFrameHours

        return self
    }

    public func set(reAttributionTimeFrameDays: Int) -> Self {
        self.reAttributionTimeFrameDays = reAttributionTimeFrameDays

        return self
    }

    public func set(reFetchReAttributionDelaySeconds: Int) -> Self {
        self.reFetchReAttributionDelaySeconds = reFetchReAttributionDelaySeconds

        return self
    }

    public func build() throws -> JustTrackSDK {
        let sdk = try JustTrackSDKImpl(prefixedApiToken: apiToken, trackingId: trackingId, trackingProvider: trackingProvider, inactivityTimeFrameHours: inactivityTimeFrameHours, reAttributionTimeFrameDays: reAttributionTimeFrameDays, reFetchReAttributionDelaySeconds: reFetchReAttributionDelaySeconds, logger: logger)
        if let firebaseInstanceId = firebaseInstanceId {
            _ = sdk.publishFirebaseInstanceId(firebaseInstanceId: firebaseInstanceId)
        }

        return sdk
    }
}
