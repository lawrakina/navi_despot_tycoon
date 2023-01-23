import Foundation
import AdSupport
import UIKit
import CoreTelephony

func getIDFA() -> String? {
    let manager = ASIdentifierManager.shared()
    if !manager.isAdvertisingTrackingEnabled {
        return nil
    }
    if manager.advertisingIdentifier.uuidString == "00000000-0000-0000-0000-000000000000" {
        return nil
    }
    return manager.advertisingIdentifier.uuidString.lowercased()
}

func getCurrentCountry() -> String? {
    let iso = NSLocale.current.regionCode

    switch iso {
    case "AC":
        return "GB"
    case "CP":
        return "FR"
    case "CQ":
        return "GB"
    case "DG":
        return "GB"
    case "EA":
        return "ES"
    case "EU":
        return nil // european union
    case "EZ":
        return nil // euro zone
    case "FX":
        return "FR"
    case "IC":
        return "ES"
    case "SU":
        return "RU" // soviet union
    case "TA":
        return "GB"
    case "UK":
        return "GB" // united kingdom
    case "UN":
        return nil // united nations
    default:
        return iso
    }
}

func getCurrentLocale() -> String {
    return NSLocale.current.identifier
}

enum ConnectionType {
    case offline
    case cellular_unknown
    case cellular_2g
    case cellular_3g
    case cellular_4g
    case cellular_5g
    case wifi
    case unknown

    var stringValue: String {
        get {
            switch self {
            case .offline:
                return "offline"
            case .cellular_unknown:
                return "cellular_unknown"
            case .cellular_2g:
                return "cellular_2g"
            case .cellular_3g:
                return "cellular_3g"
            case .cellular_4g:
                return "cellular_4g"
            case .cellular_5g:
                return "cellular_5g"
            case .wifi:
                return "wifi"
            case .unknown:
                return "unknown"
            }
        }
    }

    private var preferenceValue: Int {
        get {
            switch self {
            case .offline:
                return 0
            case .cellular_unknown:
                return 10
            case .cellular_2g:
                return 11
            case .cellular_3g:
                return 12
            case .cellular_4g:
                return 13
            case .cellular_5g:
                return 14
            case .wifi:
                return 100
            case .unknown:
                return 1
            }
        }
    }

    func betterThan(_ other: ConnectionType) -> Bool {
        return self.preferenceValue > other.preferenceValue
    }
}

func getTechnologyMap() -> [String: ConnectionType] {
    var result: [String: ConnectionType] = [
        CTRadioAccessTechnologyGPRS: .cellular_2g,
        CTRadioAccessTechnologyEdge: .cellular_2g,
        CTRadioAccessTechnologyCDMA1x: .cellular_2g,
        CTRadioAccessTechnologyWCDMA: .cellular_3g,
        CTRadioAccessTechnologyHSDPA: .cellular_3g,
        CTRadioAccessTechnologyHSUPA: .cellular_3g,
        CTRadioAccessTechnologyCDMAEVDORev0: .cellular_3g,
        CTRadioAccessTechnologyCDMAEVDORevA: .cellular_3g,
        CTRadioAccessTechnologyCDMAEVDORevB: .cellular_3g,
        CTRadioAccessTechnologyeHRPD: .cellular_3g,
        CTRadioAccessTechnologyLTE: .cellular_4g,
    ]

    if #available(iOS 14.1, *) {
        result[CTRadioAccessTechnologyNRNSA] = .cellular_5g
        result[CTRadioAccessTechnologyNR] = .cellular_5g
    }

    return result
}

let technologyMap: [String: ConnectionType] = getTechnologyMap()

func getNetworkType() -> ConnectionType {
    do {
        let reachability: Reachability = try Reachability()
        try reachability.startNotifier()
        defer { reachability.stopNotifier() }
        let status = reachability.connection
        if (status == .unavailable) {
            return .offline
        }
        if (status == .wifi) {
            return .wifi
        }
        if (status == .cellular) {
            let networkInfo = CTTelephonyNetworkInfo()
            if #available(iOS 12.0, *) {
                var result = ConnectionType.cellular_unknown

                for (_, carrierType) in networkInfo.serviceCurrentRadioAccessTechnology ?? [:] {
                    if let technology = technologyMap[carrierType] {
                        if technology.betterThan(result) {
                            result = technology
                        }
                    }
                }

                return result
            } else {
                if let carrierType = networkInfo.currentRadioAccessTechnology {
                    return technologyMap[carrierType] ?? .cellular_unknown
                }
            }
            return .cellular_unknown
        }
        return .unknown
    } catch {
        return .unknown
    }
}

enum DeviceType {
    case phone
    case tablet

    var stringValue: String {
        get {
            switch self {
            case .phone:
                return "phone"
            case .tablet:
                return "tablet"
            }
        }
    }
}

struct DeviceInfo {
    let idfv: String
    let name: String
    let model: String
    let product: String
    let type: DeviceType
    let osVersion: String
    let osLevel: Int
    let displayWidth: Int
    let displayHeight: Int

    init() {
        let device = UIDevice.current
        self.idfv = device.identifierForVendor?.uuidString.lowercased() ?? "00000000-0000-0000-0000-000000000000"
        self.name = device.name
        self.model = device.model
        self.product = device.model // not really supported on iOS

        if device.userInterfaceIdiom == .pad {
            self.type = .tablet
        } else {
            self.type = .phone
        }

        self.osVersion = device.systemVersion
        self.osLevel = Int(self.osVersion.split(separator: ".")[0]) ?? 0

        let screen = UIScreen.main.bounds
        self.displayWidth = Int(screen.width)
        self.displayHeight = Int(screen.height)
    }
}
