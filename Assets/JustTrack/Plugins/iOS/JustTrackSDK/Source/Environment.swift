import Foundation

enum Route {
    case health
    case attribution
    case trackEvent
    case publishFirebaseInstanceId
    case log

    func getUrl(_ environment: Environment) -> String {
        switch self {
        case .health:
            return environment.getAttributionDomain() + "/health"
        case .attribution:
            return environment.getAttributionDomain() + "/v1/attribute"
        case .trackEvent:
            return environment.getUserEventDomain() + "/v1/track"
        case .publishFirebaseInstanceId:
            return environment.getUserEventDomain() + "/v0/firebase/instanceId/publish"
        case .log:
            return environment.getLogsDomain() + "/v0/log/"
        }
    }
}

enum Environment {
    case production
    case sandbox

    func getName() -> String {
        switch self {
        case .production:
            return "prod"
        case .sandbox:
            return "sandbox"
        }
    }

    func getDomains() -> [String] {
        return [
            getAttributionDomain(),
            getUserEventDomain(),
            getLogsDomain(),
        ]
    }

    func getAttributionDomain() -> String {
        switch self {
        case .production:
            return "attribution.justtrack.io"
        case .sandbox:
            return "attribution.marketing-sandbox.info"
        }
    }

    func getUserEventDomain() -> String {
        switch self {
        case .production:
            return "user-events.justtrack.io"
        case .sandbox:
            return "user-events.marketing-sandbox.info"
        }
    }

    func getLogsDomain() -> String {
        switch self {
        case .production:
            return "justtrack-logs.justtrack.io"
        case .sandbox:
            return "justtrack-logs.marketing-sandbox.info"
        }
    }

    func getAffiliateDomain() -> String {
        switch self {
        case .production:
            return "https://affiliate.justtrack.io"
        case .sandbox:
            return "https://affiliate.marketing-sandbox.info"
        }
    }

    func getUrl(_ route: Route, _ suffix: String? = nil) -> String {
        return "https://" + route.getUrl(self) + (suffix ?? "")
    }
}
