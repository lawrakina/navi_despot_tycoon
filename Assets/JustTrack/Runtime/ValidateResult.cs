using System;
using System.Collections.Generic;
using UnityEngine;

namespace JustTrack {
    public class ValidateResult {
        private ValidateResult(bool pValid, RetargetingParameters pValidParameters, AttributionResponse pAttributionResponse) {
            this.Valid = pValid;
            this.ValidParameters = pValidParameters;
            this.AttributionResponse = pAttributionResponse;
        }

        public bool Valid { get; private set; }
        public RetargetingParameters ValidParameters { get; private set; }
        public AttributionResponse AttributionResponse { get; private set; }

        #if UNITY_ANDROID
            internal static ValidateResult FromAndroidObject(AndroidJavaObject pObject) {
                AndroidJavaObject validParameters = pObject.Call<AndroidJavaObject>("validParameters");

                return new ValidateResult(
                    pObject.Call<bool>("isValid"),
                    validParameters == null ? null : RetargetingParameters.FromAndroidObject(validParameters),
                    AttributionResponse.FromAndroidObject(pObject.Call<AndroidJavaObject>("attributionResponse"))
                );
            }
        #endif
        #if UNITY_IOS
            internal static ValidateResult CreateValidateResult(RetargetingParameters pValidParameters, AttributionResponse pAttributionResponse) {
                return new ValidateResult(pValidParameters != null, pValidParameters, pAttributionResponse);
            }
        #endif
        #if UNITY_EDITOR
            internal static ValidateResult CreateFakeResult() {
                return new ValidateResult(false, null, AttributionResponse.CreateFakeResponse());
            }
        #endif
    }
}
