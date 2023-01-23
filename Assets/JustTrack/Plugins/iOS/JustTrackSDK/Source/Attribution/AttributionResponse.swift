import Foundation

public protocol AttributionResponse {
    func getUserId() -> UUID
    func getInstallId() -> UUID
    func getUserType() -> String
    func getCampaign() -> Campaign
    func getType() -> String
    func getChannel() -> Channel
    func getNetwork() -> Network
    func getSourceId() -> String?
    func getSourceBundleId() -> String?
    func getSourcePlacement() -> String?
    func getAdsetId() -> String?
    func getRecruiter() -> Recruiter?
    func getCreatedAt() -> Date
}

public struct Campaign {
    public let id: Int
    public let name: String
    public let type: String

    public init(id: Int, name: String, type: String) {
        self.id = id
        self.name = name
        self.type = type
    }
}

public struct Channel {
    public let id: Int
    public let name: String
    public let incent: Bool

    public init(id: Int, name: String, incent: Bool) {
        self.id = id
        self.name = name
        self.incent = incent
    }
}

public struct Network {
    public let id: Int
    public let name: String

    public init(id: Int, name: String) {
        self.id = id
        self.name = name
    }
}

public struct Recruiter {
    public let advertiserId: String
    public let userId: String
    public let packageId: String
    public let platform: String

    public init(advertiserId: String, userId: String, packageId: String, platform: String) {
        self.advertiserId = advertiserId
        self.userId = userId
        self.packageId = packageId
        self.platform = platform
    }
}

class AttributionResponseImpl: AttributionResponse {
    private let userId: UUID
    private let installId: UUID
    private let userType: String
    private let campaign: Campaign
    private let type: String
    private let channel: Channel
    private let network: Network
    private let sourceId: String?
    private let sourceBundleId: String?
    private let sourcePlacement: String?
    private let adsetId: String?
    private let recruiter: Recruiter?
    private let createdAt: Date

    init(userId: UUID, installId: UUID, userType: String, campaign: Campaign, type: String, channel: Channel, network: Network, sourceId: String?, sourceBundleId: String?, sourcePlacement: String?, adsetId: String?, recruiter: Recruiter?, createdAt: Date) {
        self.userId = userId
        self.installId = installId
        self.userType = userType
        self.campaign = campaign
        self.type = type
        self.channel = channel
        self.network = network
        self.sourceId = sourceId
        self.sourceBundleId = sourceBundleId
        self.sourcePlacement = sourcePlacement
        self.adsetId = adsetId
        self.recruiter = recruiter
        self.createdAt = createdAt
    }

    public func getUserId() -> UUID {
        return userId
    }

    public func getInstallId() -> UUID {
        return installId
    }

    public func getUserType() -> String {
        return userType
    }

    public func getCampaign() -> Campaign {
        return campaign
    }

    public func getType() -> String {
        return type
    }

    public func getChannel() -> Channel {
        return channel
    }

    public func getNetwork() -> Network {
        return network
    }

    public func getSourceId() -> String? {
        return sourceId
    }

    public func getSourceBundleId() -> String? {
        return sourceBundleId
    }

    public func getSourcePlacement() -> String? {
        return sourcePlacement
    }

    public func getAdsetId() -> String? {
        return adsetId
    }

    public func getRecruiter() -> Recruiter? {
        return recruiter
    }

    public func getCreatedAt() -> Date {
        return createdAt
    }
}
