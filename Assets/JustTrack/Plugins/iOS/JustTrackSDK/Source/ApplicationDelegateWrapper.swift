import UIKit

class ApplicationDelegateWrapper: NSObject, UIApplicationDelegate {
    var delegate: UIApplicationDelegate?
    weak var sessionManager: SessionManager?
    weak var eventTimeTracker: EventTimeTracker?

    init(delegate: UIApplicationDelegate?, sessionManager: SessionManager?, eventTimeTracker: EventTimeTracker?) {
        self.delegate = delegate
        self.eventTimeTracker = eventTimeTracker
    }

    override func responds(to aSelector: Selector!) -> Bool {
        delegate?.responds(to: aSelector) ?? super.responds(to: aSelector)
    }

    override func forwardingTarget(for aSelector: Selector!) -> Any? {
        if let delegate = delegate {
            if delegate.responds(to: aSelector) {
                return delegate
            }
        }

        return super.forwardingTarget(for: aSelector)
    }

    public func applicationDidFinishLaunching(_ application: UIApplication) {
        delegate?.applicationDidFinishLaunching?(application)
        sessionManager?.moveToForeground()
        eventTimeTracker?.handleAppStart()
    }

    public func applicationDidBecomeActive(_ application: UIApplication) {
        delegate?.applicationDidBecomeActive?(application)
        sessionManager?.moveToForeground()
        eventTimeTracker?.handleAppStart()
    }

    public func applicationWillResignActive(_ application: UIApplication) {
        delegate?.applicationWillResignActive?(application)
        sessionManager?.moveToBackground()
        eventTimeTracker?.handleAppStop()
    }

    public func applicationWillTerminate(_ application: UIApplication) {
        delegate?.applicationWillTerminate?(application)
        sessionManager?.applicationWillTerminate()
        eventTimeTracker?.handleAppStop()
    }

    public func applicationDidEnterBackground(_ application: UIApplication) {
        delegate?.applicationDidEnterBackground?(application)
        sessionManager?.moveToBackground()
        eventTimeTracker?.handleAppStop()
    }

    public func applicationWillEnterForeground(_ application: UIApplication) {
        delegate?.applicationWillEnterForeground?(application)
        sessionManager?.moveToForeground()
        eventTimeTracker?.handleAppStart()
    }

    public func application(_ application: UIApplication, continue userActivity: NSUserActivity, restorationHandler: @escaping ([UIUserActivityRestoring]?) -> Void) -> Bool {
        if let url = userActivity.webpageURL {
            sessionManager?.applicationWillContinue(with: url)
            eventTimeTracker?.handleAppStart()
        }
        return delegate?.application?(application, continue: userActivity, restorationHandler: restorationHandler) ?? false
    }
}
