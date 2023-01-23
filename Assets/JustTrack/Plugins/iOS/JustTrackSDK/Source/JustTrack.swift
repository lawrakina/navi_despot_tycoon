import Foundation

public class JustTrack {
    private static let appStartedAt: Date = Date()
    private static var started = false
    private static var loadStartedAt: Date? = nil
    private static var loadDoneAt: Date? = nil
    private static var loaded = false
    private static weak var sdk: JustTrackSDKImpl? = nil
    private static let lock: JustTrack = JustTrack()

    private init() {}

    public static func notifyLoadStart() {
        objc_sync_enter(lock)
        defer { objc_sync_exit(lock) }

        if loadStartedAt == nil {
            loadStartedAt = Date()
        }

        notifyQueuedEvents()
    }

    public static func notifyLoadDone() {
        objc_sync_enter(lock)
        defer { objc_sync_exit(lock) }

        if loadDoneAt == nil {
            loadDoneAt = Date()
        }

        notifyQueuedEvents()
    }

    internal static func setSdk(_ sdkInstance: JustTrackSDKImpl) {
        objc_sync_enter(lock)
        defer { objc_sync_exit(lock) }

        sdk = sdkInstance
        notifyQueuedEvents()
    }

    private static func getAppStartedAt() -> AppEvent? {
        if started {
            return nil
        }

        started = true

        return AppEvent(startedAt: appStartedAt, doneAt: Date())
    }

    private static func getAppLoadedAt() -> AppEvent? {
        guard let loadStartedAt = loadStartedAt else { return nil }
        guard let loadDoneAt = loadDoneAt else { return nil }

        if loaded {
            return nil
        }

        loaded = true

        return AppEvent(startedAt: loadStartedAt, doneAt: loadDoneAt)
    }

    private static func notifyQueuedEvents() {
        guard let sdk = sdk else { return }

        if let event = getAppStartedAt() {
            sdk.notifyAppStart(event)
        }
        if let event = getAppLoadedAt() {
            sdk.notifyAppLoad(event)
        }
    }
}

internal struct AppEvent {
    let startedAt: Date
    let startingTook: TimeInterval

    fileprivate init(startedAt: Date, doneAt: Date) {
        self.startedAt = startedAt
        self.startingTook = doneAt.timeIntervalSince(startedAt)
    }
}
