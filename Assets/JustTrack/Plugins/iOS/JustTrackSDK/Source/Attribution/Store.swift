import Foundation

struct Store {
    static let ATTRIBUTION_KEY = "io.justtrack.attribution"
    private static let TIMESTAMPS_KEY = "io.justtrack.attribution.timestamps"
    private static let APP_VERSION_AT_INSTALL_KEY = "io.justtrack.appVersion.atInstall"
    private static let CURRENT_APP_VERSION_KEY = "io.justtrack.appVersion.current"
    private static let VERSION: Int = 2

    private let userDefaults: UserDefaults

    init() {
        self.userDefaults = .standard
    }

    func storeAttribution(attribution: AttributionResponse) {
        var dict = [String: Any]()

        let now = Date()
        let firstAttributedAt = getFirstAttributedAt() ?? now

        dict["firstAttributedAt"] = formatDate(firstAttributedAt)
        dict["lastAttributedAt"] = formatDate(now)
        dict["lastOpenAt"] = formatDate(now)

        userDefaults.setValue(dict, forKey: Store.TIMESTAMPS_KEY)

        dict = [String: Any]()
        dict["version"] = Store.VERSION
        dict["userId"] = attribution.getUserId().uuidString
        dict["installId"] = attribution.getInstallId().uuidString
        dict["userType"] = attribution.getUserType()
        dict["campaignId"] = attribution.getCampaign().id
        dict["campaignName"] = attribution.getCampaign().name
        dict["campaignType"] = attribution.getCampaign().type
        dict["type"] = attribution.getType()
        dict["channelId"] = attribution.getChannel().id
        dict["channelName"] = attribution.getChannel().name
        dict["channelIncent"] = attribution.getChannel().incent
        dict["networkId"] = attribution.getNetwork().id
        dict["networkName"] = attribution.getNetwork().name
        dict["createdAt"] = formatDate(attribution.getCreatedAt())
        if let sourceId = attribution.getSourceId() {
            dict["sourceId"] = sourceId
        }
        if let sourceBundleId = attribution.getSourceBundleId() {
            dict["sourceBundleId"] = sourceBundleId
        }
        if let sourcePlacement = attribution.getSourcePlacement() {
            dict["sourcePlacement"] = sourcePlacement
        }
        if let adsetId = attribution.getAdsetId() {
            dict["adsetId"] = adsetId
        }
        if let recruiter = attribution.getRecruiter() {
            dict["recruiterAdvertiserId"] = recruiter.advertiserId
            dict["recruiterUserId"] = recruiter.userId
            dict["recruiterPackageId"] = recruiter.packageId
            dict["recruiterPlatform"] = recruiter.platform
        }

        userDefaults.setValue(dict, forKey: Store.ATTRIBUTION_KEY)
    }

    private func getFirstAttributedAt() -> Date? {
        guard let dict = userDefaults.dictionary(forKey: Store.TIMESTAMPS_KEY) else {
            return nil
        }

        guard let firstAttributedAt = dict["firstAttributedAt"] as? String else {
            return nil
        }

        return parseDate(firstAttributedAt)
    }

    func getStoredResponse() -> AttributionResponse? {
        guard let dict = userDefaults.dictionary(forKey: Store.ATTRIBUTION_KEY) else { return nil }
        guard let version = dict["version"] as? Int else { return nil }
        if version != Store.VERSION { return nil }
        guard let userIdString = dict["userId"] as? String else { return nil }
        guard let userId = UUID(uuidString: userIdString) else { return nil }
        guard let installIdString = dict["installId"] as? String else { return nil }
        guard let installId = UUID(uuidString: installIdString) else { return nil }
        guard let userType = dict["userType"] as? String else { return nil }
        guard let campaignId = dict["campaignId"] as? Int else { return nil }
        guard let campaignName = dict["campaignName"] as? String else { return nil }
        guard let campaignType = dict["campaignType"] as? String else { return nil }
        guard let type = dict["type"] as? String else { return nil }
        guard let channelId = dict["channelId"] as? Int else { return nil }
        guard let channelName = dict["channelName"] as? String else { return nil }
        guard let channelIncent = dict["channelIncent"] as? Bool else { return nil }
        guard let networkId = dict["networkId"] as? Int else { return nil }
        guard let networkName = dict["networkName"] as? String else { return nil }
        guard let createdAtString = dict["createdAt"] as? String else { return nil }
        guard let createdAt = parseDate(createdAtString) else { return nil }
        let sourceId = dict["sourceId"] as? String
        let sourceBundleId = dict["sourceBundleId"] as? String
        let sourcePlacement = dict["sourcePlacement"] as? String
        let adsetId = dict["adsetId"] as? String

        var recruiter: Recruiter? = nil
        if let recruiterAdvertiserId = dict["recruiterAdvertiserId"] as? String {
            if let recruiterUserId = dict["recruiterUserId"] as? String {
                if let recruiterPackageId = dict["recruiterPackageId"] as? String {
                    if let recruiterPlatform = dict["recruiterPlatform"] as? String {
                        recruiter = Recruiter(advertiserId: recruiterAdvertiserId, userId: recruiterUserId, packageId: recruiterPackageId, platform: recruiterPlatform)
                    }
                }
            }
        }

        return AttributionResponseImpl(userId: userId, installId: installId, userType: userType, campaign: Campaign(id: campaignId, name: campaignName, type: campaignType), type: type, channel: Channel(id: channelId, name: channelName, incent: channelIncent), network: Network(id: networkId, name: networkName), sourceId: sourceId, sourceBundleId: sourceBundleId, sourcePlacement: sourcePlacement, adsetId: adsetId, recruiter: recruiter, createdAt: createdAt)
    }

    func getAttributionTimestamps() -> AttributionTimestamps? {
        guard let dict = userDefaults.dictionary(forKey: Store.TIMESTAMPS_KEY) else {
            return nil
        }

        guard let firstAttributedAtString = dict["firstAttributedAt"] as? String else {
            return nil
        }
        guard let firstAttributedAt = parseDate(firstAttributedAtString) else {
            return nil
        }

        let lastAttributedAt: Date
        if let lastAttributedAtString = dict["lastAttributedAt"] as? String {
            lastAttributedAt = parseDate(lastAttributedAtString) ?? firstAttributedAt
        } else {
            lastAttributedAt = firstAttributedAt
        }

        let lastOpenAt: Date
        if let lastOpenAtString = dict["lastOpenAt"] as? String {
            lastOpenAt = parseDate(lastOpenAtString) ?? firstAttributedAt
        } else {
            lastOpenAt = firstAttributedAt
        }

        return AttributionTimestamps(firstAttributedAt: firstAttributedAt, lastAttributedAt: lastAttributedAt, lastOpenAt: lastOpenAt)
    }

    func setLastOpen() {
        var dict = userDefaults.dictionary(forKey: Store.TIMESTAMPS_KEY) ?? Dictionary()
        dict["lastOpenAt"] = formatDate(Date())

        userDefaults.setValue(dict, forKey: Store.TIMESTAMPS_KEY)
    }

    func readAppVersionAtInstall(currentVersion: Version) -> Version {
        if let oldVersion = readStoredAppVersion(key: Store.APP_VERSION_AT_INSTALL_KEY) {
            return oldVersion
        }

        setAppVersion(currentVersion: currentVersion, key: Store.APP_VERSION_AT_INSTALL_KEY)

        return currentVersion
    }

    private func readStoredAppVersion(key: String) -> Version? {
        guard let dict = userDefaults.dictionary(forKey: key) else { return nil }
        guard let major = dict["major"] as? UInt32 else { return nil }
        guard let minor = dict["minor"] as? UInt32 else { return nil }
        let name = dict["name"] as? String

        return VersionImpl(major: major, minor: minor, name: name ?? "\(major).\(minor)")
    }

    private func setAppVersion(currentVersion: Version, key: String) {
        var dict = [String: Any]()
        dict["major"] = currentVersion.getMajor()
        dict["minor"] = currentVersion.getMinor()
        dict["name"] = currentVersion.getName()
        userDefaults.setValue(dict, forKey: key)
    }

    func getAppVersionUpdateInfo(currentVersion: Version) -> AppVersionUpdateInfo {
        guard let atInstall = readStoredAppVersion(key: Store.APP_VERSION_AT_INSTALL_KEY) else {
            setAppVersion(currentVersion: currentVersion, key: Store.APP_VERSION_AT_INSTALL_KEY)
            setAppVersion(currentVersion: currentVersion, key: Store.CURRENT_APP_VERSION_KEY)

            return AppVersionUpdateInfo(appVersionAtInstall: currentVersion, lastAppVersion: currentVersion, kind: .installed_app)
        }

        let lastAppVersion = readStoredAppVersion(key: Store.CURRENT_APP_VERSION_KEY) ?? atInstall
        setAppVersion(currentVersion: currentVersion, key: Store.CURRENT_APP_VERSION_KEY)

        if lastAppVersion.compare(currentVersion) == .orderedSame {
            return AppVersionUpdateInfo(appVersionAtInstall: atInstall, lastAppVersion: lastAppVersion, kind: .no_change)
        }

        return AppVersionUpdateInfo(appVersionAtInstall: atInstall, lastAppVersion: lastAppVersion, kind: .updated_app)
    }
}

internal struct AppVersionUpdateInfo {
    internal let appVersionAtInstall: Version
    internal let lastAppVersion: Version
    internal let kind: AppVersionUpdateKind

    fileprivate init(appVersionAtInstall: Version, lastAppVersion: Version, kind: AppVersionUpdateKind) {
        self.appVersionAtInstall = appVersionAtInstall
        self.lastAppVersion = lastAppVersion
        self.kind = kind
    }
}

internal enum AppVersionUpdateKind {
    case installed_app
    case updated_app
    case no_change
}
