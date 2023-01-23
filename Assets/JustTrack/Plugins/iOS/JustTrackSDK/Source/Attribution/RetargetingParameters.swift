import Foundation

public protocol RetargetingParameters {
    func wasAlreadyInstalled() -> Bool
    func getUrl() -> URL?
    func getParameters() -> Dictionary<String, String>
    func getPromotionParameter() -> String?
}

public protocol PreliminaryRetargetingParameters: RetargetingParameters {
    func validate() -> Future<ValidateResult>
}

public protocol ValidateResult {
    func isValid() -> Bool
    func validParameters() -> RetargetingParameters?
    func attributionResponse() -> AttributionResponse
}

class RetargetingParametersImpl: RetargetingParameters {
    private let alreadyInstalled: Bool
    private let url: URL?
    private let parameters: Dictionary<String, String>

    init(alreadyInstalled: Bool, url: URL?, parameters: Dictionary<String, String>) {
        self.alreadyInstalled = alreadyInstalled
        self.url = url
        self.parameters = parameters
    }

    public func wasAlreadyInstalled() -> Bool {
        return alreadyInstalled
    }

    public func getUrl() -> URL? {
        return url
    }

    public func getParameters() -> Dictionary<String, String> {
        return parameters
    }

    public func getPromotionParameter() -> String? {
        if let promoCode = parameters["promo_code"] {
            if !promoCode.isEmpty {
                return promoCode
            }
        }

        return nil
    }
}

class PreliminaryRetargetingParametersImpl: RetargetingParametersImpl, PreliminaryRetargetingParameters, Promise {
    private let validateResult: FutureImpl<ValidateResult>

    private init(url: URL, parameters: Dictionary<String, String>) {
        validateResult = FutureImpl()
        super.init(alreadyInstalled: true, url: url, parameters: parameters)
    }

    public func validate() -> Future<ValidateResult> {
        return validateResult.toFuture()
    }

    func resolve(_ value: ValidateResult) -> Future<ValidateResult> {
        return validateResult.resolve(value)
    }

    func reject(_ error: Error) -> Future<ValidateResult> {
        return validateResult.reject(error)
    }
}
