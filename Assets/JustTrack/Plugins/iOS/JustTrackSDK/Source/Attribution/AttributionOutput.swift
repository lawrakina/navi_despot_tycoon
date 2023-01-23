import Foundation

struct AttributionOutput: ValidateResult {
    private let attributionResponseVal: AttributionResponse
    private let retargetingParameters: RetargetingParameters?

    init(attributionResponse: AttributionResponse, retargetingParameters: RetargetingParameters?) {
        self.attributionResponseVal = attributionResponse
        self.retargetingParameters = retargetingParameters
    }

    public func getAttributionResponse() -> AttributionResponse {
        return attributionResponseVal
    }

    public func getRetargetingParameters() -> RetargetingParameters? {
        return retargetingParameters
    }

    public func isValid() -> Bool {
        return retargetingParameters != nil
    }

    public func validParameters() -> RetargetingParameters? {
        return retargetingParameters
    }

    public func attributionResponse() -> AttributionResponse {
        return attributionResponseVal
    }
}
