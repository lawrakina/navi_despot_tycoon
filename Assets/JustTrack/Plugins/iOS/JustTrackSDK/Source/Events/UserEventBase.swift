import Foundation

public protocol UserEventBuilder {
    func with(dimension1: String) -> UserEventBuilder
    func with(dimension2: String) -> UserEventBuilder
    func with(dimension3: String) -> UserEventBuilder
    func with(value: Double, unit: Unit) -> UserEventBuilder
    func build() -> PublishableUserEvent

    func with(count: Double) -> UserEventBuilder
    func with(seconds: Double) -> UserEventBuilder
    func with(milliseconds: Double) -> UserEventBuilder
}

extension UserEventBuilder {
    public func with(count: Double) -> UserEventBuilder {
        with(value: count, unit: .count)
    }

    public func with(seconds: Double) -> UserEventBuilder {
        with(value: seconds, unit: .seconds)
    }

    public func with(milliseconds: Double) -> UserEventBuilder {
        with(value: milliseconds, unit: .milliseconds)
    }
}

public protocol PredefinedUserEvent {
    func build() -> PublishableUserEvent
}

public protocol HasCustomDimensions: PredefinedUserEvent {
    func with(dimension1: String) -> HasCustomDimensions
    func with(dimension2: String) -> HasCustomDimensions
    func with(dimension3: String) -> HasCustomDimensions
}

@available(iOS 10.0, *)
public class UserEventBase: UserEventBuilder {
    private let name: EventDetails
    private let dimensions: Dictionary<Dimension, String>
    private let value: Double
    private let unit: Unit?
    private let happenedAt: Date?

    public init(_ name: String) {
        self.name = EventDetails(name: name)
        self.dimensions = Dictionary()
        self.value = 0.0
        self.unit = nil
        self.happenedAt = nil
    }

    public init(name: EventDetails) {
        self.name = name
        self.dimensions = Dictionary()
        self.value = 0.0
        self.unit = nil
        self.happenedAt = nil
    }

    public init(name: EventDetails, value: Double, unit: Unit) {
        self.name = name
        self.dimensions = Dictionary()
        self.value = value
        self.unit = unit
        self.happenedAt = nil
    }

    public init(name: EventDetails, dimension1: String) {
        self.name = name
        self.dimensions = [Dimension.custom_1: dimension1]
        self.value = 0.0
        self.unit = nil
        self.happenedAt = nil
    }

    public init(name: EventDetails, dimension1: String, dimension2: String) {
        self.name = name
        self.dimensions = [Dimension.custom_1: dimension1, Dimension.custom_2: dimension2]
        self.value = 0.0
        self.unit = nil
        self.happenedAt = nil
    }

    public init(name: EventDetails, dimension1: String, dimension2: String, dimension3: String) {
        self.name = name
        self.dimensions = [Dimension.custom_1: dimension1, Dimension.custom_2: dimension2, Dimension.custom_3: dimension3]
        self.value = 0.0
        self.unit = nil
        self.happenedAt = nil
    }

    public init(name: EventDetails, dimension1: String, dimension2: String, dimension3: String, value: Double, unit: Unit) {
        self.name = name
        self.dimensions = [Dimension.custom_1: dimension1, Dimension.custom_2: dimension2, Dimension.custom_3: dimension3]
        self.value = value
        self.unit = unit
        self.happenedAt = nil
    }

    internal init(name: EventDetails, dimensions: Dictionary<Dimension, String>, value: Double, unit: Unit?, happenedAt: Date?) {
        self.name = name
        self.dimensions = dimensions
        self.value = value
        self.unit = unit
        self.happenedAt = happenedAt
    }

    internal init(event: PublishableUserEvent) {
        self.name = event.name
        self.dimensions = event.dimensions
        self.value = event.value
        self.unit = event.unit
        self.happenedAt = event.happenedAt
    }

    public func getName() -> EventDetails {
        return name
    }

    public func with(dimension1: String) -> UserEventBuilder {
        var newDimensions = dimensions
        newDimensions[Dimension.custom_1] = dimension1
        return UserEventBase(name: name, dimensions: newDimensions, value: value, unit: unit, happenedAt: happenedAt)
    }

    public func with(dimension2: String) -> UserEventBuilder {
        var newDimensions = dimensions
        newDimensions[Dimension.custom_2] = dimension2
        return UserEventBase(name: name, dimensions: newDimensions, value: value, unit: unit, happenedAt: happenedAt)
    }

    public func with(dimension3: String) -> UserEventBuilder {
        var newDimensions = dimensions
        newDimensions[Dimension.custom_3] = dimension3
        return UserEventBase(name: name, dimensions: newDimensions, value: value, unit: unit, happenedAt: happenedAt)
    }

    public func with(value: Double, unit: Unit) -> UserEventBuilder {
        return UserEventBase(name: name, dimensions: dimensions, value: value, unit: unit, happenedAt: happenedAt)
    }

    internal func needsSessionId() -> Bool {
        if let _ = dimensions[Dimension.session_id] {
            return false
        }

        return true
    }

    internal func with(sessionId: String) -> UserEventBase {
        var newDimensions = dimensions
        newDimensions[Dimension.session_id] = sessionId
        return UserEventBase(name: name, dimensions: newDimensions, value: value, unit: unit, happenedAt: happenedAt)
    }

    internal func with(duration: TimeInterval) -> UserEventBase {
        return UserEventBase(name: name, dimensions: dimensions, value: duration, unit: .seconds, happenedAt: happenedAt)
    }

    public func build() -> PublishableUserEvent {
        return PublishableUserEvent(name: name, dimensions: dimensions, value: value, unit: unit, happenedAt: happenedAt)
    }
}
