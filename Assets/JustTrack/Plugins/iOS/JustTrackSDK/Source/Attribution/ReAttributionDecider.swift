import Foundation

enum AttributionDecision {
    case fetch_first_attribution
    case refresh_first_attribution
    case fetch_retargeting_attribution
    case fetch_retargeting_attribution_delayed
    case use_stored_attribution

    func shouldFetchAttribution() -> Bool {
        switch self {
        case .fetch_first_attribution, .refresh_first_attribution, .fetch_retargeting_attribution, .fetch_retargeting_attribution_delayed:
            return true
        case .use_stored_attribution:
            return false
        }
    }

    func isFetchRetargetingAttribution() -> Bool {
        switch self {
        case .fetch_first_attribution, .refresh_first_attribution, .fetch_retargeting_attribution_delayed, .use_stored_attribution:
            return false
        case .fetch_retargeting_attribution:
            return true
        }
    }

    func merge(_ other: AttributionDecision) -> AttributionDecision {
        if priority() > other.priority() {
            return self
        } else {
            return other
        }
    }

    private func priority() -> Int {
        switch self {
        case .fetch_first_attribution:
            return 3
        case .refresh_first_attribution:
            return 3
        case .fetch_retargeting_attribution:
            return 2
        case .fetch_retargeting_attribution_delayed:
            return 1
        case .use_stored_attribution:
            return 0
        }
    }
}

struct AttributionTimestamps {
    let firstAttributedAt: Date
    let lastAttributedAt: Date
    let lastOpenAt: Date

    init(firstAttributedAt: Date, lastAttributedAt: Date, lastOpenAt: Date) {
        self.firstAttributedAt = firstAttributedAt
        self.lastAttributedAt = lastAttributedAt
        self.lastOpenAt = lastOpenAt
    }
}

protocol ReAttributionDecider {
    func needsReAttribution(attributionTimestamps: AttributionTimestamps?) -> AttributionDecision
}

struct ChainedReAttributionDecider: ReAttributionDecider {
    private let deciders: [ReAttributionDecider]

    init(_ deciders: ReAttributionDecider...) {
        self.deciders = deciders
    }

    func needsReAttribution(attributionTimestamps: AttributionTimestamps?) -> AttributionDecision {
        var result: AttributionDecision = .use_stored_attribution

        for decider in deciders {
            result = result.merge(decider.needsReAttribution(attributionTimestamps: attributionTimestamps))
        }

        return result
    }
}

struct ResolveOrganicAttributionDecider: ReAttributionDecider {
    private static let OLD_ATTRIBUTION_AGE: TimeInterval = 15 * 60 * 1000

    init() {}

    func needsReAttribution(attributionTimestamps: AttributionTimestamps?) -> AttributionDecision {
        guard let attributionTimestamps = attributionTimestamps else {
            return .fetch_first_attribution
        }

        let attributionAge = attributionTimestamps.lastAttributedAt.timeIntervalSince(attributionTimestamps.firstAttributedAt)

        // we need to attribute a user again (because a new postback could have arrived) should
        // the last attribution we have (if any) be not older than 15 minutes of the first attribution
        // we performed
        if attributionAge <= ResolveOrganicAttributionDecider.OLD_ATTRIBUTION_AGE {
            return .refresh_first_attribution
        } else {
            return .use_stored_attribution
        }
    }
}

struct TimeBasedReAttributionDecider: ReAttributionDecider {
    private let inactivityTimeFrame: TimeInterval
    private let reAttributionTimeFrame: TimeInterval

    init(inactivityTimeFrameHours: Int, reAttributionTimeFrameDays: Int) {
        self.inactivityTimeFrame = 3600 * TimeInterval(inactivityTimeFrameHours)
        self.reAttributionTimeFrame = 3600 * 24 * TimeInterval(reAttributionTimeFrameDays)
    }

    func needsReAttribution(attributionTimestamps: AttributionTimestamps?) -> AttributionDecision {
        guard let attributionTimestamps = attributionTimestamps else {
            // user was never attributed before
            return .fetch_first_attribution
        }

        let now = Date()
        let attributeAfterAppOpen = attributionTimestamps.lastOpenAt + inactivityTimeFrame
        let attributeAfterAttribution = attributionTimestamps.lastAttributedAt + reAttributionTimeFrame

        if now >= attributeAfterAppOpen || now >= attributeAfterAttribution {
            return .fetch_retargeting_attribution
        } else {
            return .use_stored_attribution
        }
    }
}
