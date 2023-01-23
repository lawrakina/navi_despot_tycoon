import Foundation

public struct PublishableUserEvent: Equatable {
    let name: EventDetails
    let dimensions: [Dimension: String]
    let value: Double
    let unit: Unit?
    let happenedAt: Date?

    init(name: EventDetails, dimensions: Dictionary<Dimension, String>, value: Double, unit: Unit?, happenedAt: Date?) {
        self.name = name
        self.dimensions = dimensions
        if unit == .some(.seconds) {
            self.value = value * 1000
            self.unit = .milliseconds
        } else {
            self.value = value
            self.unit = unit
        }
        self.happenedAt = happenedAt
    }

    init?(encoded: [String: Any]) {
        guard let nameObject = encoded["name"] as? [String: Any] else { return nil }
        guard let name = EventDetails(encoded: nameObject) else { return nil }
        self.name = name
        guard let dimensionsObject = encoded["dimensions"] as? [String: Any] else { return nil }
        var parsedDimensions: [Dimension: String] = [:]
        for dimension in dimensionsObject {
            if let value = dimension.value as? String {
                if let dimensionKey = Dimension.from(string: dimension.key) {
                    parsedDimensions[dimensionKey] = value
                }
            }
        }
        self.dimensions = parsedDimensions
        guard let value = encoded["value"] as? Double else { return nil }
        self.value = value
        if let unit = encoded["unit"] as? String {
            guard let unit = Unit.from(string: unit) else { return nil }
            self.unit = unit
        } else {
            self.unit = nil
        }
        self.happenedAt = nil
    }

    func encode() -> [String: Any] {
        var encodedDimensions: [String: Any] = Dictionary()
        for dimension in dimensions {
            encodedDimensions[dimension.key.stringValue] = dimension.value
        }
        var result: [String: Any] = [
            "name": name.encode(),
            "dimensions": encodedDimensions,
            "value": value,
        ]
        if let unit = unit {
            result["unit"] = unit.stringValue
        }

        return result
    }

    func toFields() -> LoggerFields {
        var fields: LoggerFieldsBuilder = LoggerFieldsImpl()
        fields = fields.with("event_name", name.getName())
        if let category = name.getCategory() {
            fields = fields.with("event_category", category)
        }
        if let element = name.getElement() {
            fields = fields.with("event_element", element)
        }
        if let action = name.getAction() {
            fields = fields.with("event_action", action)
        }
        fields = fields.with("event_value", value)
        for (dimension, value) in dimensions {
            if !value.isEmpty {
                fields = fields.with("event_dimension_\(dimension.stringValue)", value)
            }
        }
        fields = fields.with("event_unit", unit?.stringValue ?? "")

        return fields
    }

    static func build(events: [PublishingEvent], idfa: String?, trackingId: String, trackingProvider: String, userId: UUID, installId: UUID, bundleVersion: Int) -> DTOUserEvent {
        let deviceInfo = DeviceInfo()
        let user = DTOUserEventUser(idfv: deviceInfo.idfv, idfa: idfa, trackingId: trackingId, trackingProvider: trackingProvider, countryIso: getCurrentCountry(), localeCode: getCurrentLocale(), userId: userId, installId: installId)
        let device = DTOUserEventDevice(getNetworkType().stringValue, DTOUserEventDeviceOS(deviceInfo.osVersion, deviceInfo.osLevel))
        var batch: [DTOUserEventEvent] = []
        batch.reserveCapacity(events.count)
        for event in events {
            let baseEvent = event.baseEvent()
            batch.append(DTOUserEventEvent(id: event.eventId(), name: baseEvent.name, dimensions: baseEvent.dimensions, value: baseEvent.value, unit: baseEvent.unit, happenedAt: event.happenedAt()))
        }
        let appVersion = DTOInputNamedVersion(readAppVersion())
        let sdkVersion = DTOInputVersion(currentSdkVersion())
        let dto = DTOUserEvent(appVersion: appVersion, sdkVersion: sdkVersion, bundleVersion: bundleVersion, user: user, device: device, events: batch)

        return dto
    }
}
