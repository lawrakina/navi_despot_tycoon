import Foundation
import StoreKit

public protocol JustTrackSDK: AnyObject {
    func attributeUser() -> Future<AttributionResponse>
    func getRetargetingParameters() -> Future<RetargetingParameters?>
    func getPreliminaryRetargetingParameters() -> PreliminaryRetargetingParameters?
    func getCachedAttribution() -> AttributionResponse?
    func publishEvent(event: PublishableUserEvent) -> Future<Void>
    func getAppVersionAtInstall() -> Version
    func getSdkVersion() -> Version
    func onDestroy()
    func getAffiliateLink(channel: String?) -> Future<String>
    func publishFirebaseInstanceId(firebaseInstanceId: String) -> Future<Void>
    func integrateWithIronSource() -> Future<Void>
    func registerAttributionListener(listener: @escaping (AttributionResponse) -> Void) -> Subscription
    func registerRetargetingParametersListener(listener: @escaping (RetargetingParameters) -> Void) -> Subscription
    func registerPreliminaryRetargetingParametersListener(listener: @escaping (PreliminaryRetargetingParameters) -> Void) -> Subscription
}

@available(iOS 10.0, *)
class JustTrackSDKImpl: JustTrackSDK {
    private let logger: HttpLogger
    private let idfa: String?
    private let trackingId: String
    private let trackingProvider: String
    private var attributionOutput: Future<AttributionOutput>?
    private var attributionDidFail: Bool
    private var preliminaryRetargetingParametersImpl: PreliminaryRetargetingParametersImpl?
    private let appVersionAtInstall: Version
    private let appVersionUpdateInfo: AppVersionUpdateInfo
    private let environment: Environment
    private let httpClient: HttpClient
    private let store: Store
    private var sessionManager: SessionManager?
    private let eventTimeTracker: EventTimeTracker
    private var userEventQueue: PublishEventsQueue?
    private let attributionSubscriptions: SubscriptionManager<AttributionResponse>
    private let retargetingParametersSubscriptions: SubscriptionManager<RetargetingParameters>
    private let preliminaryRetargetingParametersSubscriptions: SubscriptionManager<PreliminaryRetargetingParameters>
    private let reAttributionDecider: ReAttributionDecider
    private let reFetchReAttributionDelay: TimeInterval
    private var connectivityManager: ConnectivityManager?

    convenience init(prefixedApiToken: String, trackingId: String, trackingProvider: String, inactivityTimeFrameHours: Int, reAttributionTimeFrameDays: Int, reFetchReAttributionDelaySeconds: Int, logger: Logger? = nil, httpClient: HttpClient? = nil) throws {
        if trackingId.isEmpty || trackingProvider.isEmpty {
            throw MissingTrackingId()
        }
        let environment: Environment
        let apiToken: String
        if prefixedApiToken.hasPrefix("sandbox-") {
            environment = .sandbox
            apiToken = String(prefixedApiToken.suffix(from: "sandbox-".endIndex))
        } else if prefixedApiToken.hasPrefix("prod-") {
            environment = .production
            apiToken = String(prefixedApiToken.suffix(from: "prod-".endIndex))
        } else {
            throw BadApiTokenError(prefixedApiToken)
        }
        let httpClient = httpClient ?? HttpClientImpl(environment: environment, apiToken: apiToken)
        let logger = logger ?? LoggerImpl()
        self.init(environment: environment, trackingId: trackingId, trackingProvider: trackingProvider, inactivityTimeFrameHours: inactivityTimeFrameHours, reAttributionTimeFrameDays: reAttributionTimeFrameDays, reFetchReAttributionDelaySeconds: reFetchReAttributionDelaySeconds, logger: logger, httpClient: httpClient, sessionManagerBuilder: { sdk in
            return SessionManagerImpl(sdk)
        }, connectivityManagerBuilder: { sdk in
            do {
                return try ConnectivityManagerImpl()
            } catch {
                sdk.logError("Failed to initialize connectivity manager: \(error.localizedDescription)")
                return nil
            }
        })
    }

    init(environment: Environment, trackingId: String, trackingProvider: String, inactivityTimeFrameHours: Int, reAttributionTimeFrameDays: Int, reFetchReAttributionDelaySeconds: Int, logger: Logger, httpClient: HttpClient, sessionManagerBuilder: (_ sdk: JustTrackSDKImpl) -> SessionManager, connectivityManagerBuilder: (_ sdk: JustTrackSDKImpl) -> ConnectivityManager?) {
        // for now we just ensure we get the notification... and we can call that method as often as we want,
        // so we ensure we call it at least once
        if #available(iOS 11.3, *) {
            SKAdNetwork.registerAppForAdNetworkAttribution()
        }
        self.store = Store()
        self.trackingId = trackingId
        self.trackingProvider = trackingProvider
        self.appVersionAtInstall = self.store.readAppVersionAtInstall(currentVersion: readAppVersion())
        self.appVersionUpdateInfo = self.store.getAppVersionUpdateInfo(currentVersion: readAppVersion())
        self.reAttributionDecider = ChainedReAttributionDecider(
            ResolveOrganicAttributionDecider(),
            TimeBasedReAttributionDecider(inactivityTimeFrameHours: inactivityTimeFrameHours, reAttributionTimeFrameDays: reAttributionTimeFrameDays)
        )
        self.reFetchReAttributionDelay = TimeInterval(reFetchReAttributionDelaySeconds)
        self.environment = environment
        self.idfa = getIDFA()
        self.httpClient = httpClient
        self.logger = HttpLoggerImpl(fallback: logger, httpClient: httpClient)
        if let idfa = self.idfa {
            self.logger.setIDFA(idfa: idfa)
        }
        self.attributionDidFail = false
        self.attributionSubscriptions = SubscriptionManager()
        self.retargetingParametersSubscriptions = SubscriptionManager()
        self.preliminaryRetargetingParametersSubscriptions = SubscriptionManager()
        self.preliminaryRetargetingParametersImpl = nil
        self.eventTimeTracker = EventTimeTracker()
        self.connectivityManager = connectivityManagerBuilder(self)
        self.userEventQueue = PublishEventsQueue(self, self.connectivityManager)
        self.sessionManager = sessionManagerBuilder(self)
        self.sessionManager?.setupEventTimeTracker(eventTimeTracker: self.eventTimeTracker)
        _ = self.connectivityManager?.registerOnReconnect(self.retryAttributionAfterReconnect)
        if let cachedResponse = getCachedAttribution() {
            self.logger.setUuid(uuid: cachedResponse.getUserId())
        }
    }

    public func attributeUser() -> Future<AttributionResponse> {
        return TransformingFuture(getAttributionOutput(forcedDecision: nil), { $0.getAttributionResponse() }).toFuture()
    }

    private func getAttributionOutput(forcedDecision: AttributionDecision?) -> Future<AttributionOutput> {
        objc_sync_enter(self)
        defer { objc_sync_exit(self) }

        if forcedDecision == nil {
            if let response = attributionOutput {
                return response
            }
        }

        let attributionTimestamps = store.getAttributionTimestamps()
        // after getting the last timestamps we can write into the store our open timestamp so we know the
        // next time whether the user opened the app not for quite some time
        store.setLastOpen()

        let attributionDecision = forcedDecision ?? reAttributionDecider.needsReAttribution(attributionTimestamps: attributionTimestamps)
        let storedResponse = store.getStoredResponse()

        if !attributionDecision.shouldFetchAttribution() {
            if let storedResponse = storedResponse {
                let attributionOutput = FutureImpl(AttributionOutput(attributionResponse: storedResponse, retargetingParameters: nil)).toFuture()
                self.attributionOutput = attributionOutput
                logger.setUuid(uuid: storedResponse.getUserId())
                logger.debug("Using cached attribution")
                attributionSubscriptions.call(value: storedResponse)
                return attributionOutput
            }
        }

        let attributionOutputPromise = FutureImpl<AttributionOutput>()
        let attributionOutput = attributionOutputPromise.toFuture()
        self.attributionOutput = attributionOutput

        let deviceInfo = DeviceInfo()
        let user = DTOAttributionRequestUser(idfv: deviceInfo.idfv, idfa: idfa, trackingId: trackingId, trackingProvider: trackingProvider, countryIso: getCurrentCountry())
        let device = DTOAttributionRequestDevice(name: deviceInfo.name, model: deviceInfo.model, product: deviceInfo.product, type: deviceInfo.type, os: DTOAttributionRequestDeviceOS(deviceInfo.osVersion, deviceInfo.osLevel), display: DTOAttributionRequestDeviceDisplay(width: deviceInfo.displayWidth, height: deviceInfo.displayHeight))
        let request = DTOAttributionRequest(DTOInputNamedVersion(readAppVersion()), user, device, nil)
        httpClient.postAttribution(request: request, idfa: idfa).observe(using: { result in
            switch result {
            case .failure(let error):
                _ = attributionOutputPromise.reject(error)
            case .success(let responseData):
                do {
                    let response = try DTOAttributionResponse(data: responseData, wasAlreadyInstalled: false) // TODO: wasAlreadyInstalled
                    self.store.storeAttribution(attribution: response)
                    _ = attributionOutputPromise.resolve(AttributionOutput(attributionResponse: response, retargetingParameters: response.retargetingParameters))
                } catch {
                    _ = attributionOutputPromise.reject(error)
                }
            }
        })

        attributionOutput.observe(using: { result in
            switch result {
            case .failure(let error):
                self.attributionDidFail = true
                self.logger.error("Failed to get attribution", error)
            case .success(let response):
                self.attributionDidFail = false
                self.logger.setUuid(uuid: response.getAttributionResponse().getUserId())
                if let attributionTimestamps = attributionTimestamps {
                    if !attributionDecision.shouldFetchAttribution() {
                        self.logger.info("Attribution was not needed, but was not cached")
                    } else {
                        let attributionAge = attributionTimestamps.lastAttributedAt.timeIntervalSince(attributionTimestamps.firstAttributedAt)
                        let now = Date()
                        let sinceLastOpen = now.timeIntervalSince(attributionTimestamps.lastOpenAt)
                        let sinceLastAttribution = now.timeIntervalSince(attributionTimestamps.lastAttributedAt)
                        let fields = LoggerFieldsImpl().with("attributionAge", attributionAge).with("sinceLastOpen", sinceLastOpen).with("sinceLastAttribution", sinceLastAttribution).with("force", forcedDecision != nil)
                        self.logger.debug("Attribution was fetched again because re-attribution was needed", fields)
                    }
                } else {
                    self.logger.debug("Fetched first attribution")
                }
                self.attributionSubscriptions.call(value: response.getAttributionResponse())
                self.callRetargetingParametersSubscriptions(retargetingParameters: response.getRetargetingParameters())

                // check if we expected a re-attribution and did not get one
                if (attributionDecision.isFetchRetargetingAttribution() && self.hasOldInstallId(storedResponse: storedResponse, newResponse: response) && self.reFetchReAttributionDelay > 0) {
                    self.fetchAttributionAgainAfter(delay: self.reFetchReAttributionDelay)
                } else if let preliminaryRetargetingParametersImpl = self.preliminaryRetargetingParametersImpl {
                    _ = preliminaryRetargetingParametersImpl.resolve(response)
                }
            }
        })

        return attributionOutput
    }

    private func hasOldInstallId(storedResponse: AttributionResponse?, newResponse: AttributionOutput) -> Bool {
        guard let storedResponse = storedResponse else {
            return true
        }

        return storedResponse.getInstallId() == newResponse.getAttributionResponse().getInstallId()
    }

    private func fetchAttributionAgainAfter(delay: TimeInterval) {
        DispatchQueue.main.asyncAfter(deadline: .now() + delay) {
            self.logger.info("Fetching attribution again because the retargeting delay expired and previously the attribution did not change")
            _ = self.getAttributionOutput(forcedDecision: .fetch_retargeting_attribution_delayed)
        }
    }

    private func retryAttributionAfterReconnect() {
        if attributionDidFail {
            attributionOutput = nil
            attributionDidFail = false
            logger.info("Fetching attribution again as it failed and we got a new network connection")
            _ = getAttributionOutput(forcedDecision: .fetch_first_attribution)
        }
    }

    private func callRetargetingParametersSubscriptions(retargetingParameters: RetargetingParameters?) {
        guard let retargetingParameters = retargetingParameters else { return }

        retargetingParametersSubscriptions.call(value: retargetingParameters)
    }

    private func callPreliminaryRetargetingParametersSubscriptions(preliminaryRetargetingParameters: PreliminaryRetargetingParameters) {
        preliminaryRetargetingParametersSubscriptions.call(value: preliminaryRetargetingParameters)
    }

    public func getCachedAttribution() -> AttributionResponse? {
        return store.getStoredResponse()
    }

    public func getRetargetingParameters() -> Future<RetargetingParameters?> {
        return TransformingFuture(getAttributionOutput(forcedDecision: nil), { $0.getRetargetingParameters() }).toFuture()
    }

    public func getPreliminaryRetargetingParameters() -> PreliminaryRetargetingParameters? {
        return preliminaryRetargetingParametersImpl
    }

    public func publishEvent(event: PublishableUserEvent) -> Future<Void> {
        var builder: UserEventBase = UserEventBase(event: event)

        if let duration = eventTimeTracker.measureDuration(event: event) {
            if event.unit == nil {
                builder = builder.with(duration: duration)
            }
        }

        if builder.needsSessionId() {
            if let sessionId = sessionManager?.getLastSessionId(self) {
                builder = builder.with(sessionId: sessionId)
            }
        }

        return getUserEventQueue().publishEvent(event: builder.build())
    }

    internal func getUserEventQueue() -> PublishEventsQueue {
        guard let userEventQueue = userEventQueue else {
            // should normally not happen, but if it happens, just setup the queue and use it
            let userEventQueue = PublishEventsQueue(self, self.connectivityManager)
            self.userEventQueue = userEventQueue

            return userEventQueue
        }

        return userEventQueue
    }

    internal func publishEventBatch(batch: [PublishingEvent]) -> Future<Void> {
        let result = FutureImpl<Void>()
        attributeUser().observe(using: { attribution in
            switch attribution {
            case .failure(let error):
                self.logger.debug("Not publishing events batch, attribution failed", LoggerFieldsImpl().with("exception", error))
                _ = result.reject(error)
            case .success(let response):
                let events = PublishableUserEvent.build(events: batch, idfa: self.idfa, trackingId: self.trackingId, trackingProvider: self.trackingProvider, userId: response.getUserId(), installId: response.getInstallId(), bundleVersion: -1)
                let request = self.httpClient.postEvent(events: events, idfa: self.idfa, uuid: response.getUserId())
                request.observe(using: { eventResponse in
                    switch eventResponse {
                    case .failure(let error):
                        self.logger.debug("Publishing events in batch failed", LoggerFieldsImpl().with("exception", error.localizedDescription))
                        _ = result.reject(error)
                    case .success(_):
                        _ = result.resolve(Void())
                    }
                })
            }
        })

        return result.toFuture()
    }

    public func getAppVersionAtInstall() -> Version {
        return appVersionAtInstall
    }

    public func getSdkVersion() -> Version {
        return currentSdkVersion()
    }

    public func onDestroy() {
        sessionManager?.onDestroy()
        sessionManager = nil
        connectivityManager?.shutdown()
        connectivityManager = nil
    }

    public func getAffiliateLink(channel: String? = nil) -> Future<String> {
        let result = FutureImpl<String>()
        attributeUser().observe(using: { attribution in
            switch attribution {
            case .failure(let error):
                _ = result.reject(error)
            case .success(let response):
                guard let packageId = Bundle.main.bundleIdentifier else {
                    _ = result.reject(MissingBundleIdentifierError())
                    return
                }
                var url = self.environment.getAffiliateDomain() + "/ios/" + packageId + "/" + response.getUserId().uuidString.lowercased()

                if let channel = channel?.addingPercentEncoding(withAllowedCharacters: .urlQueryAllowed) {
                    url += "?channel=" + channel
                }

                _ = result.resolve(url)
            }
        })

        return result.toFuture()
    }

    public func publishFirebaseInstanceId(firebaseInstanceId: String) -> Future<Void> {
        return FutureImpl<Void>().reject(NotImplementedError())
    }

    public func integrateWithIronSource() -> Future<Void> {
        #if ENABLE_IRONSOURCE_INTEGRATION
        let result = FutureImpl<Void>()
        attributeUser().observe(using: { attribution in
            switch attribution {
            case .failure(let error):
                _ = result.reject(error)
            case .success(let response):
                JustTrackObjCBridge.initIronSourceIntegration(response.getUuid().uuidString.lowercased())
                _ = result.resolve(Void())
            }
        })

        return result.toFuture()
        #else
        return FutureImpl<Void>().reject(NotImplementedError())
        #endif
    }

    public func registerAttributionListener(listener: @escaping (AttributionResponse) -> Void) -> Subscription {
        return attributionSubscriptions.subscribe(listener: listener)
    }

    public func registerRetargetingParametersListener(listener: @escaping (RetargetingParameters) -> Void) -> Subscription {
        return retargetingParametersSubscriptions.subscribe(listener: listener)
    }

    public func registerPreliminaryRetargetingParametersListener(listener: @escaping (PreliminaryRetargetingParameters) -> Void) -> Subscription {
        return preliminaryRetargetingParametersSubscriptions.subscribe(listener: listener)
    }

    func logDebug(_ message: String) {
        logger.debug(message)
    }

    func logInfo(_ message: String) {
        logger.info(message)
    }

    func logWarning(_ message: String) {
        logger.warn(message)
    }

    func logError(_ message: String) {
        logger.error(message)
    }

    func notifyAppStart(_ event: AppEvent) {
        guard let sessionManager = sessionManager else { return }
        let sessionId = sessionManager.getLastSessionId(self)
        let duration = event.startingTook
        let happenedAt = event.startedAt
        _ = publishEvent(event: SessionAppOpenEvent(sessionId: sessionId, duration: duration, happenedAt: happenedAt).build())
        switch appVersionUpdateInfo.kind {
        case .installed_app:
            _ = publishEvent(event: SessionAppInstallEvent(sessionId: sessionId, duration: duration, happenedAt: happenedAt).build())
        case .updated_app:
            _ = publishEvent(event: SessionAppUpdateEvent(sessionId: sessionId, previousAppVersionCode: appVersionUpdateInfo.lastAppVersion.getName(), duration: duration, happenedAt: happenedAt).build())
        case .no_change:
            // no change
            break
        }
    }

    func notifyAppLoad(_ event: AppEvent) {
        guard let sessionManager = sessionManager else { return }
        let sessionId = sessionManager.getLastSessionId(self)
        let duration = event.startingTook
        let happenedAt = event.startedAt
        _ = publishEvent(event: SessionAppLoadEvent(sessionId: sessionId, duration: duration, happenedAt: happenedAt).build())
    }

    func applicationWillContinue(with url: URL) {
        var parameters: [String: String] = [:]
        URLComponents(url: url, resolvingAgainstBaseURL: false)?.queryItems?.forEach {
            parameters[$0.name] = $0.value
        }
        // TODO: implement this
    }
}

struct MissingTrackingId: Error {}

class BadApiTokenError: NSObject, LocalizedError {
    let apiToken: String

    init(_ apiToken: String) {
        self.apiToken = apiToken
    }

    public override var description: String {
        get {
            return "BadApiTokenError: \(apiToken)"
        }
    }

    public var errorDescription: String? {
        get {
            return self.description
        }
    }
}

struct MissingBundleIdentifierError: Error {}

struct NotImplementedError: Error {}
