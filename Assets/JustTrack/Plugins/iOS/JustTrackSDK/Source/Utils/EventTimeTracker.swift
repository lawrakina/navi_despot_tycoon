import Foundation

class EventTimeTracker {
    private static let VERSION: Int = 1
    internal static let KEY = "io.justtrack.attribution.trackedEvents"

    private let userDefaults: UserDefaults
    private let registry: [EventDetails: TrackedEvents]
    private let timeSource: TimeSource

    convenience init() {
        self.init(timeSource: SystemTimeSource())
    }

    init(timeSource: TimeSource) {
        var registry: [EventDetails: TrackedEvents] = [:]
        registry = registerEvents(startEvents: [UserEvent.PROGRESSION_LEVEL_START], stopEvents: [UserEvent.PROGRESSION_LEVEL_FINISH, UserEvent.PROGRESSION_LEVEL_FAIL], relevantDimensions: [Dimension.element_name, Dimension.element_id], registry: registry)
        registry = registerEvents(startEvents: [UserEvent.PROGRESSION_QUEST_START], stopEvents: [UserEvent.PROGRESSION_QUEST_FINISH, UserEvent.PROGRESSION_QUEST_FAIL], relevantDimensions: [Dimension.element_name, Dimension.element_id], registry: registry)
        self.userDefaults = .standard
        self.registry = registry
        self.timeSource = timeSource
    }

    func measureDuration(event: PublishableUserEvent) -> TimeInterval? {
        guard let tracked = registry[event.name] else {
            return nil
        }

        if let stopFor = tracked.stopFor {
            return handleStop(event: event, stopFor: stopFor, relevantDimensions: tracked.relevantDimensions)
        }

        handleStart(event: event, relevantDimensions: tracked.relevantDimensions)

        return nil
    }

    func handleAppStart() {
        map({ event in
            var newEvent = event
            newEvent.resume(timeSource: self.timeSource)

            return newEvent
        })
    }

    func handleAppStop() {
        map({ event in
            var newEvent = event
            newEvent.pause(timeSource: self.timeSource)

            return newEvent
        })
    }

    private func handleStart(event: PublishableUserEvent, relevantDimensions: [Dimension]) {
        let key = EventTimeTracker.getEventKey(name: event.name, dimensions: event.dimensions, relevantDimensions: relevantDimensions)
        mapOne(key: key, { _ in
            return TrackedEvent(key: key, timeSource: self.timeSource)
        })
    }

    private func handleStop(event: PublishableUserEvent, stopFor: [EventDetails], relevantDimensions: [Dimension]) -> TimeInterval? {
        var events = readTrackedEvents()
        var found = false
        defer {
            if found {
                writeTrackedEvents(events: events.values)
            }
        }

        for stopForEvent in stopFor {
            let key = EventTimeTracker.getEventKey(name: stopForEvent, dimensions: event.dimensions, relevantDimensions: relevantDimensions)

            guard var trackedEvent = events.removeValue(forKey: key) else {
                continue
            }

            found = true

            trackedEvent.pause(timeSource: self.timeSource)

            return trackedEvent.interval()
        }

        return nil
    }

    private static func getEventKey(name: EventDetails, dimensions: [Dimension: String], relevantDimensions: [Dimension]) -> String {
        var key = String()

        key += name.getName()
        key += ":"
        key += name.getCategory() ?? ""
        key += ":"
        key += name.getElement() ?? ""
        key += ":"
        key += name.getAction() ?? ""

        for dimension in relevantDimensions {
            key += ":"
            key += dimension.stringValue
            key += ":"
            key += dimensions[dimension] ?? ""
        }

        return key
    }

    private func map(_ f: (TrackedEvent) -> TrackedEvent) {
        var events = readTrackedEvents()

        for (key, event) in events {
            events[key] = f(event)
        }

        writeTrackedEvents(events: events.values)
    }

    private func mapOne(key: String, _ f: (TrackedEvent?) -> TrackedEvent?) {
        var newEvents: [TrackedEvent] = []
        var found = false

        readTrackedEvents({ event in
            if event.key == key {
                found = true
                if let newEvent = f(event) {
                    newEvents.append(newEvent)
                }
            } else {
                newEvents.append(event)
            }
        })

        if !found {
            if let newEvent = f(nil) {
                newEvents.append(newEvent)
            }
        }

        writeTrackedEvents(events: newEvents)
    }

    private func readTrackedEvents() -> [String: TrackedEvent] {
        var result: [String: TrackedEvent] = [:]

        readTrackedEvents({ trackedEvent in
            result[trackedEvent.key] = trackedEvent
        })

        return result
    }

    private func readTrackedEvents(_ f: (TrackedEvent) -> Void) -> Void {
        guard var storedData = userDefaults.dictionary(forKey: EventTimeTracker.KEY) else {
            return
        }

        let version = storedData.removeValue(forKey: "version") as? Int ?? -1
        if version != EventTimeTracker.VERSION {
            userDefaults.removeObject(forKey: EventTimeTracker.KEY)
            return
        }

        for entry in storedData {
            if let eventData = entry.value as? String {
                if let trackedEvent = TrackedEvent(key: entry.key, serialized: eventData) {
                    if !trackedEvent.expired() {
                        f(trackedEvent)
                    }
                }
            }
        }
    }

    private func writeTrackedEvents<T: Sequence>(events: T) where T.Element == TrackedEvent {
        var result: [String: Any] = [:]
        result["version"] = EventTimeTracker.VERSION

        for event in events {
            result[event.key] = event.serialize()
        }

        userDefaults.setValue(result, forKey: EventTimeTracker.KEY)
    }
}

fileprivate func registerEvents(startEvents: [EventDetails], stopEvents: [EventDetails], relevantDimensions: [Dimension], registry: [EventDetails: TrackedEvents]) -> [EventDetails: TrackedEvents] {
    var newRegistry = registry
    let sortedDimensions = relevantDimensions.sorted(by: { (a, b) in
        a.rawValue < b.rawValue
    })

    for start in startEvents {
        newRegistry[start] = TrackedEvents(relevantDimensions: sortedDimensions)
    }
    for stop in stopEvents {
        newRegistry[stop] = TrackedEvents(stopFor: startEvents, relevantDimensions: sortedDimensions)
    }

    return newRegistry
}

fileprivate struct TrackedEvents {
    fileprivate let stopFor: [EventDetails]?
    fileprivate let relevantDimensions: [Dimension]

    init(stopFor: [EventDetails], relevantDimensions: [Dimension]) {
        self.stopFor = stopFor
        self.relevantDimensions = relevantDimensions
    }

    init(relevantDimensions: [Dimension]) {
        self.stopFor = nil
        self.relevantDimensions = relevantDimensions
    }
}

fileprivate struct TrackedEvent {
    fileprivate let key: String
    private let createdAt: Date
    private var accumulated: TimeInterval
    private var lastStart: Date?

    fileprivate init(key: String, timeSource: TimeSource) {
        self.key = key
        self.createdAt = timeSource.getCurrentTime()
        self.accumulated = 0
        self.lastStart = self.createdAt
    }

    fileprivate init?(key: String, serialized: String) {
        self.key = key

        let parts = serialized.split(separator: ":")
        if parts.count != 3 {
            return nil
        }

        guard let createdAt = Double(parts[0]) else {
            return nil
        }
        self.createdAt = Date(timeIntervalSince1970: createdAt)

        guard let accumulated = Double(parts[1]) else {
            return nil
        }
        self.accumulated = accumulated

        guard let lastStart = Double(parts[2]) else {
            return nil
        }
        if lastStart != 0 {
            self.lastStart = Date(timeIntervalSince1970: lastStart)
        } else {
            self.lastStart = nil
        }
    }

    fileprivate mutating func pause(timeSource: TimeSource) {
        if let lastStart = self.lastStart {
            self.accumulated += timeSource.getCurrentTime().timeIntervalSince(lastStart)
        }
        self.lastStart = nil
    }

    fileprivate mutating func resume(timeSource: TimeSource) {
        self.lastStart = timeSource.getCurrentTime()
    }

    fileprivate func expired() -> Bool {
        createdAt.timeIntervalSinceNow >= 30 * 24 * 3600
    }

    fileprivate func interval() -> TimeInterval {
        return accumulated
    }

    fileprivate func serialize() -> String {
        return "\(createdAt.timeIntervalSince1970):\(accumulated):\(lastStart?.timeIntervalSince1970 ?? 0)"
    }
}

protocol TimeSource {
    func getCurrentTime() -> Date
}

fileprivate struct SystemTimeSource: TimeSource {
    fileprivate func getCurrentTime() -> Date {
        return Date(timeIntervalSince1970: floor(Date().timeIntervalSince1970))
    }
}
