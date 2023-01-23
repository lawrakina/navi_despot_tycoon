import Foundation

public protocol Version {
    func getMajor() -> UInt32
    func getMinor() -> UInt32
    func getName() -> String
    func compare(_ other: Version) -> ComparisonResult
}

struct VersionImpl : Version {
    private var version: UInt64
    private var name: String

    init(major: UInt32, minor: UInt32, name: String) {
        self.version = UInt64(major) << 32 | UInt64(minor)
        self.name = name
    }

    init(version: UInt64, name: String) {
        self.version = version
        self.name = name
    }

    public func getMajor() -> UInt32 {
        return UInt32(version >> 32)
    }

    public func getMinor() -> UInt32 {
        return UInt32(version & 0xFFFFFFFF)
    }

    public func getName() -> String {
        return name
    }

    func compare(_ other: Version) -> ComparisonResult {
        if getMajor() > other.getMajor() {
            return .orderedDescending
        }
        if getMajor() < other.getMajor() {
            return .orderedAscending
        }
        if getMinor() > other.getMinor() {
            return .orderedDescending
        }
        if getMinor() < other.getMinor() {
            return .orderedAscending
        }
        return getName().compare(other.getName())
    }
}

func readAppVersion() -> Version {
    if let buildString = Bundle.main.object(forInfoDictionaryKey: "CFBundleVersion") as? String {
        if let build = UInt32(buildString) {
            return VersionImpl(major: 0, minor: build, name: buildString)
        }
    }

    guard let versionString = Bundle.main.object(forInfoDictionaryKey: "CFBundleShortVersionString") as? String else {
        return VersionImpl(major: 0, minor: 0, name: "0.0")
    }
    let parts = versionString.filter { (c) -> Bool in
        return (c >= "0" && c <= "9") || c == "."
    }.split(separator: ".")
    switch parts.count {
    case 0:
        return VersionImpl(major: 0, minor: 0, name: versionString)
    case 1:
        let major = UInt32(parts[0]) ?? 0

        return VersionImpl(major: major, minor: 0, name: versionString)
    default:
        let major = UInt32(parts[0]) ?? 0
        let minor = UInt32(parts[1]) ?? 0

        return VersionImpl(major: major, minor: minor, name: versionString)
    }
}

func currentSdkVersion() -> Version {
    return VersionImpl(major: 4, minor: 23, name: "4.2.3")
}
