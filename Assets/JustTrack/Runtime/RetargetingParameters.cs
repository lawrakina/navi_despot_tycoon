using System;
using System.Collections.Generic;
using UnityEngine;

namespace JustTrack {
    public class RetargetingParameters {
        protected RetargetingParameters(bool pWasAlreadyInstalled, string pUri, Dictionary<string, string> pParameters, string pPromotionParameter) {
            this.WasAlreadyInstalled = pWasAlreadyInstalled;
            this.Uri = pUri;
            this.Parameters = pParameters;
            this.PromotionParameter = pPromotionParameter;
        }

        public bool WasAlreadyInstalled { get; private set; }
        public string Uri { get; private set; }
        public Dictionary<string, string> Parameters { get; private set; }
        public string PromotionParameter { get; private set; }

        #if UNITY_ANDROID
            internal static RetargetingParameters FromAndroidObject(AndroidJavaObject pObject) {
                AndroidJavaObject uri = pObject.Call<AndroidJavaObject>("getUri");

                var dictionary = new Dictionary<string, string>();

                AndroidJavaObject parametersIterator = pObject.Call<AndroidJavaObject>("getParameters").Call<AndroidJavaObject>("entrySet").Call<AndroidJavaObject>("iterator");
                while (parametersIterator.Call<bool>("hasNext")) {
                    AndroidJavaObject entry = parametersIterator.Call<AndroidJavaObject>("next");
                    dictionary[entry.Call<string>("getKey")] = entry.Call<string>("getValue");
                }

                return new RetargetingParameters(
                    pObject.Call<bool>("wasAlreadyInstalled"),
                    uri == null ? null : uri.Call<string>("toString"),
                    dictionary,
                    pObject.Call<string>("getPromotionParameter")
                );
            }
        #endif
        #if UNITY_IOS
            internal static RetargetingParameters CreateRetargetingParameters(bool pWasAlreadyInstalled, string pUri, Dictionary<string, string> pParameters, string pPromotionParameter) {
                return new RetargetingParameters(pWasAlreadyInstalled, pUri, pParameters, pPromotionParameter);
            }
        #endif
    }
}
