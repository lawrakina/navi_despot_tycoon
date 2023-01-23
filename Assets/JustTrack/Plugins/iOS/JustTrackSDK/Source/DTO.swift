import Foundation

protocol DTO {
    func json() -> Data
}

struct DTOInputVersion: Codable {
    let major: UInt32
    let minor: UInt32

    init(_ version: Version) {
        self.major = version.getMajor()
        self.minor = version.getMinor()
    }
}

struct DTOInputNamedVersion: Codable {
    let major: UInt32
    let minor: UInt32
    let name: String

    init(_ version: Version) {
        self.major = version.getMajor()
        self.minor = version.getMinor()
        self.name = version.getName()
    }
}

struct DTOLogInput: DTO, Encodable {
    let message: String
    let fields: [String: String]
    let appVersion: DTOInputNamedVersion
    let sdkVersion: DTOInputVersion

    init(_ message: String, _ fields: [String: String], appVersion: DTOInputNamedVersion, sdkVersion: DTOInputVersion) {
        self.message = message
        self.fields = fields
        self.appVersion = appVersion
        self.sdkVersion = sdkVersion
    }

    func json() -> Data {
        try! JSONEncoder().encode(self)
    }
}

struct DTOUserEvent: DTO, Encodable {
    let appVersion: DTOInputNamedVersion
    let sdkVersion: DTOInputVersion
    let bundleVersion: Int
    let user: DTOUserEventUser
    let device: DTOUserEventDevice
    let events: [DTOUserEventEvent]

    init(appVersion: DTOInputNamedVersion, sdkVersion: DTOInputVersion, bundleVersion: Int, user: DTOUserEventUser, device: DTOUserEventDevice, events: [DTOUserEventEvent]) {
        self.appVersion = appVersion
        self.sdkVersion = sdkVersion
        self.bundleVersion = bundleVersion
        self.user = user
        self.device = device
        self.events = events
    }

    func json() -> Data {
        try! JSONEncoder().encode(self)
    }
}

struct DTOUserEventUser: Encodable {
    let deviceId: String
    let advertiserId: String?
    let trackingId: String
    let trackingProvider: String
    let countryIso: String?
    let localeCode: String?
    let userId: String
    let installId: String

    init(idfv: String, idfa: String?, trackingId: String, trackingProvider: String, countryIso: String?, localeCode: String?, userId: UUID, installId: UUID) {
        self.deviceId = idfv
        self.advertiserId = idfa
        self.trackingId = trackingId
        self.trackingProvider = trackingProvider
        self.countryIso = countryIso
        self.localeCode = localeCode
        self.userId = userId.uuidString.lowercased()
        self.installId = installId.uuidString.lowercased()
    }
}

struct DTOUserEventDevice: Encodable {
    let connectionType: String
    let os: DTOUserEventDeviceOS
    let date: String

    init(_ connectionType: String, _ os: DTOUserEventDeviceOS) {
        self.connectionType = connectionType
        self.os = os
        self.date = formatDate(Date())
    }
}

struct DTOUserEventDeviceOS: Encodable {
    let version: String
    let level: Int

    init(_ version: String, _ level: Int) {
        self.version = version
        self.level = level
    }
}

struct DTOUserEventEvent: Encodable {
    let id: String
    let name: String
    let category: String
    let element: String
    let action: String
    let dimensions: DTOUserEventDimensions
    let value: Double
    let unit: String
    let happenedAt: String

    init(id: UUID, name: EventDetails, dimensions: Dictionary<Dimension, String>, value: Double, unit: Unit?, happenedAt: Date) {
        self.id = id.uuidString.lowercased()
        self.name = name.getName()
        self.category = name.getCategory() ?? ""
        self.element = name.getElement() ?? ""
        self.action = name.getAction() ?? ""
        self.dimensions = DTOUserEventDimensions(dimensions: dimensions)
        self.value = value
        if let unit = unit {
            self.unit = unit.stringValue
        } else {
            self.unit = ""
        }
        self.happenedAt = formatDate(happenedAt)
    }
}

struct DTOAttributionRequest: DTO, Encodable {
    let appVersion: DTOInputNamedVersion
    let user: DTOAttributionRequestUser
    let device: DTOAttributionRequestDevice
    let referrer: DTOAttributionRequestReferrer?

    init(_ appVersion: DTOInputNamedVersion, _ user: DTOAttributionRequestUser, _ device: DTOAttributionRequestDevice, _ referrer: DTOAttributionRequestReferrer?) {
        self.appVersion = appVersion
        self.user = user
        self.device = device
        self.referrer = referrer
    }

    func json() -> Data {
        try! JSONEncoder().encode(self)
    }
}

struct DTOAttributionRequestUser: Encodable {
    // yes, they are called like that on the backend...
    let androidId: String
    let advertiserId: String?
    let trackingId: String
    let trackingProvider: String
    let countryIso: String?

    init(idfv: String, idfa: String?, trackingId: String, trackingProvider: String, countryIso: String?) {
        self.androidId = idfv
        self.advertiserId = idfa
        self.trackingId = trackingId
        self.trackingProvider = trackingProvider
        self.countryIso = countryIso
    }
}

struct DTOAttributionRequestDevice: Encodable {
    let name: String
    let model: String
    let product: String
    let type: String
    let os: DTOAttributionRequestDeviceOS
    let display: DTOAttributionRequestDeviceDisplay

    init(name: String, model: String, product: String, type: DeviceType, os: DTOAttributionRequestDeviceOS, display: DTOAttributionRequestDeviceDisplay) {
        self.name = name
        self.model = model
        self.product = product
        self.type = type.stringValue
        self.os = os
        self.display = display
    }
}

struct DTOAttributionRequestDeviceOS: Encodable {
    let version: String
    let level: Int

    init(_ version: String, _ level: Int) {
        self.version = version
        self.level = level
    }
}

struct DTOAttributionRequestDeviceDisplay: Encodable {
    let width: Int
    let height: Int

    init(width: Int, height: Int) {
        self.width = width
        self.height = height
    }
}

struct DTOAttributionRequestReferrer: Encodable {
    let value: String
    let clickDate: String
    let installBeginDate: String
    let clientDate: String
    let serverClickDate: String
    let serverInstallBeginDate: String
    let installVersion: String?

    init(value: String, clickDate: Date, installBeginDate: Date, clientDate: Date, serverClickDate: Date, serverInstallBeginDate: Date, installVersion: String?) {
        self.value = value
        self.clickDate = formatDate(clickDate)
        self.installBeginDate = formatDate(installBeginDate)
        self.clientDate = formatDate(clientDate)
        self.serverClickDate = formatDate(serverClickDate)
        self.serverInstallBeginDate = formatDate(serverInstallBeginDate)
        self.installVersion = installVersion
    }
}

class DTOAttributionResponse: AttributionResponseImpl {
    let retargetingParameters: RetargetingParameters?

    init(data: Data, wasAlreadyInstalled: Bool) throws {
        guard let json = try JSONSerialization.jsonObject(with: data, options: []) as? [String: Any] else {
            throw DTODecodingError("failed to parse json as object")
        }

        guard let userObject = json["user"] as? [String: Any] else {
            throw DTODecodingError("failed to decode user object")
        }
        guard let userIdString = userObject["id"] as? String else {
            throw DTODecodingError("failed to decode user id string")
        }
        guard let userId = UUID(uuidString: userIdString) else {
            throw DTODecodingError("invalid user id string: " + userIdString)
        }
        guard let installIdString = userObject["installId"] as? String else {
            throw DTODecodingError("failed to decode install id string")
        }
        guard let installId = UUID(uuidString: installIdString) else {
            throw DTODecodingError("invalid install id string: " + installIdString)
        }
        guard let userType = userObject["type"] as? String else {
            throw DTODecodingError("failed to decode user type string")
        }

        guard let attributionObject = json["attribution"] as? [String: Any] else {
            throw DTODecodingError("failed to decode attribution object")
        }
        guard let campaignObject = attributionObject["campaign"] as? [String: Any] else {
            throw DTODecodingError("failed to decode campaign object")
        }
        guard let campaignId = campaignObject["id"] as? Int else {
            throw DTODecodingError("failed to decode campaign id")
        }
        guard let campaignName = campaignObject["name"] as? String else {
            throw DTODecodingError("failed to decode campaign name")
        }
        guard let campaignType = campaignObject["type"] as? String else {
            throw DTODecodingError("failed to decode campaign name")
        }
        guard let type = attributionObject["type"] as? String else {
            throw DTODecodingError("failed to decode attribution type")
        }
        guard let channelObject = attributionObject["channel"] as? [String: Any] else {
            throw DTODecodingError("failed to decode channel object")
        }
        guard let channelId = channelObject["id"] as? Int else {
            throw DTODecodingError("failed to decode channel id")
        }
        guard let channelName = channelObject["name"] as? String else {
            throw DTODecodingError("failed to decode channel name")
        }
        guard let channelIncent = channelObject["incent"] as? Bool else {
            throw DTODecodingError("failed to decode channel incent flag")
        }
        guard let networkObject = attributionObject["network"] as? [String: Any] else {
            throw DTODecodingError("failed to decode network object")
        }
        guard let networkId = networkObject["id"] as? Int else {
            throw DTODecodingError("failed to decode network id")
        }
        guard let networkName = networkObject["name"] as? String else {
            throw DTODecodingError("failed to decode network name")
        }
        let sourceId = attributionObject["sourceId"] as? String
        let sourceBundleId = attributionObject["sourceBundleId"] as? String
        let sourcePlacement = attributionObject["sourcePlacement"] as? String
        let adsetId = attributionObject["adsetId"] as? String
        guard let attributedAt = attributionObject["attributedAt"] as? String else {
            throw DTODecodingError("failed to decode attributed at")
        }
        guard let createdAt = parseDate(attributedAt) else {
            throw DTODecodingError("failed to parse attributed at")
        }

        var recruiter: Recruiter? = nil
        if let recruiterObject = json["recruiter"] as? [String: Any] {
            if let recruiterAdvertiserId = recruiterObject["advertiserId"] as? String {
                if let recruiterUserId = recruiterObject["userId"] as? String {
                    if let recruiterPackageId = recruiterObject["packageId"] as? String {
                        if let recruiterPlatform = recruiterObject["platform"] as? String {
                            recruiter = Recruiter(advertiserId: recruiterAdvertiserId, userId: recruiterUserId, packageId: recruiterPackageId, platform: recruiterPlatform)
                        }
                    }
                }
            }
        }

        if let retargetingObject = json["retargeting"] as? [String: Any] {
            let url: URL?
            if let urlString = retargetingObject["url"] as? String {
                url = URL(string: urlString)
            } else {
                url = nil
            }
            var attributes: Dictionary<String, String> = Dictionary()
            if let attributesObject = retargetingObject["attributes"] as? [String: Any] {
                for attribute in attributesObject {
                    if let value = attribute.value as? String {
                        attributes[attribute.key] = value
                    }
                }
            }

            self.retargetingParameters = RetargetingParametersImpl(alreadyInstalled: wasAlreadyInstalled, url: url, parameters: attributes)
        } else {
            self.retargetingParameters = nil
        }

        super.init(userId: userId, installId: installId, userType: userType, campaign: Campaign(id: campaignId, name: campaignName, type: campaignType), type: type, channel: Channel(id: channelId, name: channelName, incent: channelIncent), network: Network(id: networkId, name: networkName), sourceId: sourceId, sourceBundleId: sourceBundleId, sourcePlacement: sourcePlacement, adsetId: adsetId, recruiter: recruiter, createdAt: createdAt)
    }
}

let enUs = Locale.init(identifier: "en_US")
let utc = TimeZone(abbreviation: "UTC")
let dateFormat = "yyyy-MM-dd'T'HH:mm:ss'Z'"

func formatDate(_ date: Date) -> String {
    let formatter = DateFormatter()
    formatter.dateFormat = dateFormat
    formatter.timeZone = utc
    formatter.locale = enUs

    return formatter.string(from: date)
}

func parseDate(_ date: String) -> Date? {
    let formatter = DateFormatter()
    formatter.dateFormat = dateFormat
    formatter.timeZone = utc
    formatter.locale = enUs

    return formatter.date(from: date)
}

class DTODecodingError: NSObject, LocalizedError {
    let message: String

    init(_ message: String) {
        self.message = message
    }

    public override var description: String {
        get {
            return "DTODecodingError: \(message)"
        }
    }

    public var errorDescription: String? {
        get {
            return self.description
        }
    }
}
