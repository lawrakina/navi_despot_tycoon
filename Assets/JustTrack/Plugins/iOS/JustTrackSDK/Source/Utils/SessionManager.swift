import UIKit

protocol SessionManager: AnyObject {
    func moveToForeground()
    func moveToBackground()
    func applicationWillTerminate()
    func applicationWillContinue(with url: URL)
    func onDestroy()
    func setupEventTimeTracker(eventTimeTracker: EventTimeTracker)
    func getLastSessionId(_ sdkRef: JustTrackSDKImpl) -> String
}

@available(iOS 10.0, *)
class SessionManagerImpl: SessionManager {
    private let delegate: ApplicationDelegateWrapper
    weak var sdk: JustTrackSDKImpl?
    private var session: Session?
    private var lastSessionId: String?
    private weak var timer: Timer?

    init(_ sdk: JustTrackSDKImpl?) {
        self.sdk = sdk
        self.session = nil
        self.lastSessionId = nil
        self.timer = nil
        let application = UIApplication.shared
        self.delegate = ApplicationDelegateWrapper(delegate: application.delegate, sessionManager: nil, eventTimeTracker: nil)
        delegate.sessionManager = self
        application.delegate = delegate
        reportLastSession()
        moveToForeground()
    }

    func setupEventTimeTracker(eventTimeTracker: EventTimeTracker) {
        delegate.eventTimeTracker = eventTimeTracker
        eventTimeTracker.handleAppStart()
    }

    func getLastSessionId(_ sdkRef: JustTrackSDKImpl) -> String {
        objc_sync_enter(self)
        defer { objc_sync_exit(self) }

        if let lastSessionId = lastSessionId {
            return lastSessionId
        }

        if let session = session {
            return session.sessionId.uuidString.lowercased()
        }

        return startSession(sdkRef).sessionId.uuidString.lowercased()
    }

    func moveToForeground() {
        objc_sync_enter(self)
        defer { objc_sync_exit(self) }

        if session == nil {
            if let sdkRef = sdk {
                _ = startSession(sdkRef)
            }
        }

        // we should be on the main thread, but just to be sure... so we have a run loop for our timer
        DispatchQueue.main.async {
            self.timer?.invalidate()
            self.timer = Timer.scheduledTimer(withTimeInterval: 60.0, repeats: true) { [weak self] _ in
                self?.saveSession()
            }
        }
    }

    func moveToBackground() {
        objc_sync_enter(self)
        defer { objc_sync_exit(self) }

        timer?.invalidate()
        timer = nil
        endSession()
    }

    func applicationWillContinue(with url: URL) {
        sdk?.applicationWillContinue(with: url)
    }

    func applicationWillTerminate() {
        onDestroy()
    }

    func onDestroy() {
        moveToBackground()
        let application = UIApplication.shared
        if application.delegate === self.delegate {
            application.delegate = self.delegate.delegate
        }
    }

    private func startSession(_ sdkRef: JustTrackSDKImpl) -> Session {
        let newSession = Session()
        self.session = newSession
        _ = sdkRef.publishEvent(event: SessionTrackingStartEvent(sessionId: newSession.sessionId.uuidString.lowercased(), happenedAt: Date()).build())

        return newSession
    }

    private func endSession() {
        guard let session = session else {
            return
        }

        lastSessionId = endSession(sessionToEnd: session, now: Date())
        self.session = nil
    }

    private func endSession(sessionToEnd: Session, now: Date) -> String {
        let sessionId = sessionToEnd.sessionId.uuidString.lowercased()
        let duration = now.timeIntervalSince(sessionToEnd.sessionStart)
        _ = sdk?.publishEvent(event: SessionTrackingEndEvent(sessionId: sessionId, duration: duration, happenedAt: now).build())

        return sessionId
    }

    private func reportLastSession() {
        if let lastSession = Session(restoreFrom: .standard) {
            _ = endSession(sessionToEnd: lastSession, now: lastSession.sessionLastTick)
        }
    }

    private func saveSession() {
        objc_sync_enter(self)
        defer { objc_sync_exit(self) }

        session?.persist(storeTo: .standard)
    }
}

fileprivate struct Session {
    private static let KEY = "io.justtrack.attribution.session"

    fileprivate let sessionId: UUID
    fileprivate let sessionStart: Date
    fileprivate let sessionLastTick: Date

    fileprivate init() {
        sessionId = UUID()
        sessionStart = Date()
        sessionLastTick = sessionStart
    }

    fileprivate init?(restoreFrom: UserDefaults) {
        guard let dict = restoreFrom.dictionary(forKey: Session.KEY) else {
            return nil
        }

        guard let sessionIdString = dict["sessionId"] as? String else {
            return nil
        }
        guard let sessionId = UUID(uuidString: sessionIdString) else {
            return nil
        }
        self.sessionId = sessionId

        guard let sessionStart = dict["sessionStart"] as? Date else {
            return nil
        }
        self.sessionStart = sessionStart

        guard let sessionLastTick = dict["sessionLastTick"] as? Date else {
            return nil
        }
        self.sessionLastTick = sessionLastTick
    }

    fileprivate func persist(storeTo: UserDefaults) {
        let dict: [String: Any] = [
            "sessionId": sessionId.uuidString,
            "sessionStart": sessionStart,
            "sessionLastTick": Date(),
        ]

        storeTo.setValue(dict, forKey: Session.KEY)
    }
}
