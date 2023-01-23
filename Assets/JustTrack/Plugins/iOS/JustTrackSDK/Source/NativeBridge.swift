import Foundation

@objc public class NativeBridge: NSObject {
    @objc public static let shared: NativeBridge = NativeBridge()
    private var sdk: JustTrackSDKImpl? = nil

    private override init() {
        super.init()
    }

    @objc public func initSdk(apiToken: String, trackingId: String, trackingProvider: String, inactivityTimeFrameHours: Int, reAttributionTimeFrameDays: Int, reFetchReAttributionDelaySeconds: Int) {
        if #available(iOS 10.0, *) {
            do {
                try self.sdk = JustTrackSDKImpl(prefixedApiToken: apiToken, trackingId: trackingId, trackingProvider: trackingProvider, inactivityTimeFrameHours: inactivityTimeFrameHours, reAttributionTimeFrameDays: reAttributionTimeFrameDays, reFetchReAttributionDelaySeconds: reFetchReAttributionDelaySeconds)
            } catch {
                sendMessage(receiver: "OnAttributionError", message: "Failed to initialize SDK: " + error.localizedDescription)
                return
            }
        } else {
            sendMessage(receiver: "OnAttributionError", message: "Not available on this platform")
            return
        }
        guard let sdk = self.sdk else { return }
        _ = sdk.registerAttributionListener(listener: { response in
            self.sendMessage(receiver: "OnAttributionListenerReceived", message: NativeBridge.encodeAttributionResponse(response: response))
        })
        _ = sdk.registerRetargetingParametersListener(listener: { parameters in
            self.sendMessage(receiver: "OnRetargetingParametersListenerReceived", message: NativeBridge.encodeRetargetingParameters(parameters: parameters, preliminaryId: nil))
        })
        _ = sdk.registerPreliminaryRetargetingParametersListener(listener: { parameters in
            let preliminaryId = self.observePreliminaryParameters(parameters: parameters)
            self.sendMessage(receiver: "OnPreliminaryRetargetingParametersListenerReceived", message: NativeBridge.encodeRetargetingParameters(parameters: parameters, preliminaryId: preliminaryId))
        })
        sdk.attributeUser().observe(using: { result in
            switch result {
            case .failure(let error):
                self.sendMessage(receiver: "OnAttributionError", message: "Failed to attribute user: " + error.localizedDescription)
            case .success(let response):
                self.sendMessage(receiver: "OnAttributionDone", message: NativeBridge.encodeAttributionResponse(response: response))
            }
        })
    }

    private static func encodeAttributionResponse(response: AttributionResponse) -> String {
        var dto: [String: String?] = [
            "userId": response.getUserId().uuidString.lowercased(),
            "installId": response.getInstallId().uuidString.lowercased(),
            "userType": response.getUserType(),
            "type": response.getType(),
            "campaignId": String(response.getCampaign().id),
            "campaignName": response.getCampaign().name,
            "campaignType": response.getCampaign().type,
            "channelId": String(response.getChannel().id),
            "channelName": response.getChannel().name,
            "channelIncent": String(response.getChannel().incent),
            "networkId": String(response.getNetwork().id),
            "networkName": response.getNetwork().name,
            "sourceId": response.getSourceId(),
            "sourceBundleId": response.getSourceBundleId(),
            "sourcePlacement": response.getSourcePlacement(),
            "adsetId": response.getAdsetId(),
            "createdAt": formatDate(response.getCreatedAt()),
        ]

        if let recruitedBy = response.getRecruiter() {
            dto["recruiterAdvertiserId"] = recruitedBy.advertiserId
            dto["recruiterUserId"] = recruitedBy.userId
            dto["recruiterPackageId"] = recruitedBy.packageId
            dto["recruiterPlatform"] = recruitedBy.platform
        }

        return NativeBridge.encodeDto(dto: dto)
    }

    @objc public func getRetargetingParameters() {
        guard let sdk = sdk else {
            sendError(error: "Not initialized")
            return
        }
        sdk.getRetargetingParameters().observe(using: { result in
            switch result {
            case .failure(let error):
                self.sendMessage(receiver: "OnGetRetargetingParametersError", message: "Failed to get retargeting parameters: " + error.localizedDescription)
            case .success(let parameters):
                guard let parameters = parameters else {
                    self.sendMessage(receiver: "OnGetRetargetingParametersDone", message: "")
                    return
                }
                self.sendMessage(receiver: "OnGetRetargetingParametersDone", message: NativeBridge.encodeRetargetingParameters(parameters: parameters, preliminaryId: nil))
            }
        })
    }

    private static func encodeDto<T>(dto: T) -> String where T: Encodable {
        let dtoJson = try! JSONEncoder().encode(dto)
        return String.init(decoding: dtoJson, as: UTF8.self)
    }

    private static func encodeRetargetingParameters(parameters: RetargetingParameters, preliminaryId: UUID?) -> String {
        var parametersList: [[String: String]] = []
        for parameter in parameters.getParameters() {
            parametersList.append([ "parameter": parameter.key, "value": parameter.value ])
        }
        let parametersJson = try! JSONEncoder().encode(["parameters": parametersList])
        var dto = [
            "wasAlreadyInstalled": parameters.wasAlreadyInstalled() ? "true" : "false",
            "url": parameters.getUrl()?.absoluteString,
            "parameters": String.init(decoding: parametersJson, as: UTF8.self),
            "promoCode": parameters.getPromotionParameter(),
        ]
        if let preliminaryId = preliminaryId {
            dto["preliminaryId"] = preliminaryId.uuidString
        }
        return NativeBridge.encodeDto(dto: dto)
    }

    @objc public func getPreliminaryRetargetingParameters() -> String {
        guard let sdk = sdk else {
            sendError(error: "Not initialized")
            return ""
        }
        guard let parameters = sdk.getPreliminaryRetargetingParameters() else {
            return ""
        }
        let preliminaryId = observePreliminaryParameters(parameters: parameters)
        return NativeBridge.encodeRetargetingParameters(parameters: parameters, preliminaryId: preliminaryId)
    }

    private func observePreliminaryParameters(parameters: PreliminaryRetargetingParameters) -> UUID {
        let preliminaryId = UUID()
        parameters.validate().observe(using: { result in
            switch (result) {
            case .failure(let error):
                self.sendMessage(receiver: "OnValidatePreliminaryRetargetingParametersError", message: NativeBridge.encodeDto(dto: [
                    "preliminaryId": preliminaryId.uuidString,
                    "error": error.localizedDescription,
                ]))
            case .success(let parameters):
                var dto = [
                    "preliminaryId": preliminaryId.uuidString,
                    "response": NativeBridge.encodeAttributionResponse(response: parameters.attributionResponse()),
                ]
                if let parameters = parameters.validParameters() {
                    dto["parameters"] = NativeBridge.encodeRetargetingParameters(parameters: parameters, preliminaryId: nil)
                }
                self.sendMessage(receiver: "OnValidatePreliminaryRetargetingParametersDone", message: NativeBridge.encodeDto(dto: dto))
            }
        })
        return preliminaryId
    }

    @objc public func publishEvent(name: String, category: String, element: String, action: String, dimensions: String) {
        guard let sdk = sdk else {
            sendError(error: "Not initialized")
            return
        }
        if #available(iOS 10.0, *) {
            let details = EventDetails(name: name, category: category, element: element, action: action)
            guard let dimensionData = dimensions.data(using: .utf8) else {
                sendError(error: "Failed to create data from json string")
                return
            }
            do {
                let decodedDimensions = try DTOUserEventDimensions(dimensionData)
                _ = sdk.publishEvent(event: UserEvent(name: details, dimensions: decodedDimensions.dimensions(), value: 1.0, unit: nil, happenedAt: nil).build())
            } catch {
                sendError(error: "Failed to parse json string")
            }
        } else {
            sendError(error: "Not available on this platform")
        }
    }

    @objc public func publishEvent(name: String, category: String, element: String, action: String, dimensions: String, value: Double, unit: String) {
        guard let sdk = sdk else {
            sendError(error: "Not initialized")
            return
        }
        if #available(iOS 10.0, *) {
            let details = EventDetails(name: name, category: category, element: element, action: action)
            guard let dimensionData = dimensions.data(using: .utf8) else {
                sendError(error: "Failed to create data from json string")
                return
            }
            do {
                let decodedDimensions = try DTOUserEventDimensions(dimensionData)
                var unitVal: Unit? = nil
                switch unit {
                case Unit.count.stringValue:
                    unitVal = .count
                case Unit.milliseconds.stringValue:
                    unitVal = .milliseconds
                case Unit.seconds.stringValue:
                    unitVal = .seconds
                default:
                    sendError(error: "Unknown unit type: " + unit)
                    return
                }
                guard let unitResult = unitVal else { return }
                _ = sdk.publishEvent(event: UserEvent(name: details, dimensions: decodedDimensions.dimensions(), value: value, unit: unitResult, happenedAt: nil).build())
            } catch {
                sendError(error: "Failed to parse json string")
            }
        } else {
            sendError(error: "Not available on this platform")
        }
    }

    @objc public func getAffiliateLink(channel: String?) {
        guard let sdk = sdk else {
            sendError(error: "Not initialized")
            return
        }
        sdk.getAffiliateLink(channel: channel).observe(using: { result in
            switch result {
            case .failure(let error):
                self.sendMessage(receiver: "OnGetAffiliateLinkError", message: "Failed to get affiliate link: " + error.localizedDescription)
            case .success(let link):
                self.sendMessage(receiver: "OnGetAffiliateLinkDone", message: link)
            }
        })
    }

    @objc public func publishFirebaseInstanceId(firebaseInstanceId: String) {
        guard let sdk = sdk else {
            sendError(error: "Not initialized")
            return
        }
        _ = sdk.publishFirebaseInstanceId(firebaseInstanceId: firebaseInstanceId)
    }

    @objc public func ironSourceImpressionDidSucceed(adUnit: String?, adNetwork: String?, placement: String?, abTesting: String?, segmentName: String?, instanceName: String?, revenue: NSNumber?) {
        guard let adUnit = adUnit else {
            return;
        }
        let revenueVal = revenue?.doubleValue ?? 0
        let event: PredefinedUserEvent;
        switch adUnit {
        case "rewarded_video":
            event = AdRewardedSuccessEvent(adSdkName: "ironsource", adNetwork: adNetwork, adPlacement: placement, testGroup: abTesting, adSegmentName: segmentName, adInstanceName: instanceName, revenue: revenueVal, happenedAt: Date())
        case "interstitial":
            event = AdInterstitialSuccessEvent(adSdkName: "ironsource", adNetwork: adNetwork, adPlacement: placement, testGroup: abTesting, adSegmentName: segmentName, adInstanceName: instanceName, revenue: revenueVal, happenedAt: Date())
        case "banner":
            event = AdBannerSuccessEvent(adSdkName: "ironsource", adNetwork: adNetwork, adPlacement: placement, testGroup: abTesting, adSegmentName: segmentName, adInstanceName: instanceName, revenue: revenueVal, happenedAt: Date())
        default:
            return;
        }
        _ = sdk?.publishEvent(event: event.build())
    }

    @objc public func logDebug(message: String) {
        guard let sdk = sdk else {
            LoggerImpl().debug(message, LoggerFieldsImpl().with("initialized", false))
            return
        }
        sdk.logDebug(message)
    }

    @objc public func logInfo(message: String) {
        guard let sdk = sdk else {
            LoggerImpl().info(message, LoggerFieldsImpl().with("initialized", false))
            return
        }
        sdk.logInfo(message)
    }

    @objc public func logWarning(message: String) {
        guard let sdk = sdk else {
            LoggerImpl().warn(message, LoggerFieldsImpl().with("initialized", false))
            return
        }
        sdk.logWarning(message)
    }

    @objc public func logError(message: String) {
        guard let sdk = sdk else {
            LoggerImpl().error(message, LoggerFieldsImpl().with("initialized", false))
            return
        }
        sdk.logError(message)
    }

    private func sendError(error: String) {
        sendMessage(receiver: "OnHandleError", message: error)
    }

    private func sendMessage(receiver: String, message: String) {
        UnityFramework.getInstance()?.sendMessageToGO(withName: "JustTrackSDKNativeBridgeUnity", functionName: receiver, message: message)
    }
}
