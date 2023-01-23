import Foundation

public struct EventDetails: Equatable, Hashable {
    private let name: String
    private let category: String?
    private let element: String?
    private let action: String?

    public init(name: String) {
        self.name = name
        self.category = nil
        self.element = nil
        self.action = nil
    }

    public init(name: String, category: String, element: String, action: String) {
        self.name = name
        self.category = category
        self.element = element
        self.action = action
    }

    init?(encoded: [String: Any]) {
        guard let name = encoded["name"] as? String else { return nil }
        self.name = name
        self.category = encoded["category"] as? String
        self.element = encoded["element"] as? String
        self.action = encoded["action"] as? String
    }

    func encode() -> [String: Any] {
        var result = [
            "name": name,
        ]
        if let category = category {
            result["category"] = category
        }
        if let element = element {
            result["element"] = element
        }
        if let action = action {
            result["action"] = action
        }

        return result
    }


    public func getName() -> String {
        return name
    }

    public func getCategory() -> String? {
        return category
    }

    public func getElement() -> String? {
        return element
    }

    public func getAction() -> String? {
        return action
    }
}
