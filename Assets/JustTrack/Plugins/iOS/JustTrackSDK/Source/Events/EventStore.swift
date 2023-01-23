import Foundation

struct EventStore {
    private static let VERSION: Int = 1
    internal static let KEY = "io.justtrack.attribution.storedEvents"

    private let userDefaults: UserDefaults
    private var storedEvents: [StorableEvent]

    init() {
        self.userDefaults = .standard
        self.storedEvents = []

        guard let storedData = userDefaults.dictionary(forKey: EventStore.KEY) else { return }
        let version = storedData["version"] as? Int ?? -1
        if version != EventStore.VERSION {
            let dict: [String: Any] = [:]
            userDefaults.setValue(dict, forKey: EventStore.KEY)
            return
        }

        for entry in storedData {
            if !entry.key.hasPrefix("event-") {
                continue
            }
            guard let id = Int(entry.key["event-".endIndex...]) else { continue }
            guard let eventObject = entry.value as? [String: Any] else { continue }
            guard let event = StorableEvent(id: id, encoded: eventObject) else { continue }
            storedEvents.append(event)
        }

        // ensure we have a consistent ordering of events, mainly useful for the test, but also
        // ensures we roughly publish events in more or less the same order
        storedEvents.sort(by: { $0.id > $1.id })
    }

    func storeEvent(event: StorableEvent) {
        var storedData = userDefaults.dictionary(forKey: EventStore.KEY) ?? [:]
        if storedData["version"] as? Int != EventStore.VERSION {
            storedData = ["version": EventStore.VERSION]
        }
        storedData["event-\(event.id)"] = event.encode()
        userDefaults.setValue(storedData, forKey: EventStore.KEY)
    }

    func removeEvent(event: StorableEvent) {
        var storedData = userDefaults.dictionary(forKey: EventStore.KEY) ?? [:]
        if storedData["version"] as? Int != EventStore.VERSION {
            storedData = ["version": EventStore.VERSION]
        }
        storedData.removeValue(forKey: "event-\(event.id)")
        userDefaults.setValue(storedData, forKey: EventStore.KEY)
    }

    mutating func readStoredEvent() -> StorableEvent? {
        return storedEvents.popLast()
    }

    func getNextId() -> Int {
        var storedData = userDefaults.dictionary(forKey: EventStore.KEY) ?? [:]
        if storedData["version"] as? Int != EventStore.VERSION {
            storedData = ["version": EventStore.VERSION]
        }
        let nextId = storedData["nextId"] as? Int ?? 1
        storedData["nextId"] = nextId + 1
        userDefaults.setValue(storedData, forKey: EventStore.KEY)
        return nextId
    }
}

struct StorableEvent: Equatable {
    let id: Int
    let eventId: UUID
    let event: PublishableUserEvent
    let happenedAt: Date

    init(id: Int, event: PublishableUserEvent) {
        self.id = id
        self.eventId = UUID()
        self.event = event
        self.happenedAt = event.happenedAt ?? Date(timeIntervalSince1970: floor(Date().timeIntervalSince1970))
    }

    init?(id: Int, encoded: [String: Any]) {
        self.id = id
        guard let eventIdString = encoded["eventId"] as? String else { return nil }
        guard let eventId = UUID(uuidString: eventIdString) else { return nil }
        self.eventId = eventId
        guard let eventObject = encoded["event"] as? [String: Any] else { return nil }
        guard let event = PublishableUserEvent(encoded: eventObject) else { return nil }
        self.event = event
        guard let happenedAtString = encoded["happenedAt"] as? String else { return nil }
        guard let happenedAt = parseDate(happenedAtString) else { return nil }
        self.happenedAt = happenedAt
    }

    func encode() -> [String: Any] {
        return [
            "eventId": eventId.uuidString,
            "event": event.encode(),
            "happenedAt": formatDate(happenedAt),
        ]
    }
}
