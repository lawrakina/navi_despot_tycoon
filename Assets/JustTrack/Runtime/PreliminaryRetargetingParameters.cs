﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace JustTrack {
    public class PreliminaryRetargetingParameters : RetargetingParameters {
        #if UNITY_ANDROID
            private readonly AndroidJavaObject ObjectInstance;
            private readonly AndroidJavaObject SdkInstance;

            private PreliminaryRetargetingParameters(RetargetingParameters retargetingParameters, AndroidJavaObject pObjectInstance, AndroidJavaObject pSdkInstance)
                : base(retargetingParameters.WasAlreadyInstalled, retargetingParameters.Uri, retargetingParameters.Parameters, retargetingParameters.PromotionParameter) {
                this.ObjectInstance = pObjectInstance;
                this.SdkInstance = pSdkInstance;
            }
        #else
            private Action<ValidateResult> OnSuccess;
            private Action<string> OnFailure;
            private ValidateResult Result;
            private string Error;

            private PreliminaryRetargetingParameters(bool pWasAlreadyInstalled, string pUri, Dictionary<string, string> pParameters, string pPromotionParameter)
                : base(pWasAlreadyInstalled, pUri, pParameters, pPromotionParameter) {
                this.OnSuccess = null;
                this.OnFailure = null;
                this.Result = null;
                this.Error = null;
            }
        #endif

        /**
        * Find out whether these possible retargeting parameters turn out to be valid by calling the
        * JustTrack backend.
        */
        public void Validate(Action<ValidateResult> pOnSuccess, Action<string> pOnFailure) {
            #if UNITY_ANDROID
                var responseFuture = ObjectInstance.Call<AndroidJavaObject>("validate");
                Action<AndroidJavaObject> onResolve = (parameters) => {
                    var response = ValidateResult.FromAndroidObject(parameters);
                    JustTrackSDKBehaviour.CallOnMainThread(() => {
                        pOnSuccess(response);
                    });
                };
                SdkInstance.Call("toPromise", responseFuture, new Promise(onResolve, pOnFailure));
            #endif
            #if UNITY_IOS
                lock(this) {
                    if (Result != null) {
                        JustTrackSDKBehaviour.CallOnMainThread(() => {
                            pOnSuccess(Result);
                        });
                        return;
                    }
                    if (Error != null) {
                        JustTrackSDKBehaviour.CallOnMainThread(() => {
                            pOnFailure(Error);
                        });
                        return;
                    }
                    if (OnSuccess == null) {
                        OnSuccess = pOnSuccess;
                    } else {
                        OnSuccess += pOnSuccess;
                    }
                    if (OnFailure == null) {
                        OnFailure = pOnFailure;
                    } else {
                        OnFailure += pOnFailure;
                    }
                }
            #endif
            #if UNITY_EDITOR
                JustTrackSDKBehaviour.CallOnMainThread(() => {
                    pOnSuccess(ValidateResult.CreateFakeResult());
                });
            #endif
        }

        #if UNITY_ANDROID
            internal static PreliminaryRetargetingParameters FromAndroidObject(AndroidJavaObject pObject, AndroidJavaObject pSdkInstance) {
                RetargetingParameters baseResult = RetargetingParameters.FromAndroidObject(pObject);

                return new PreliminaryRetargetingParameters(baseResult, pObject, pSdkInstance);
            }
        #endif
        #if UNITY_IOS
            internal static PreliminaryRetargetingParameters CreatePreliminaryRetargetingParameters(bool pWasAlreadyInstalled, string pUri, Dictionary<string, string> pParameters, string pPromotionParameter) {
                return new PreliminaryRetargetingParameters(pWasAlreadyInstalled, pUri, pParameters, pPromotionParameter);
            }

            internal void Resolve(ValidateResult pResult) {
                lock(this) {
                    if (Result != null || Error != null) {
                        return;
                    }
                    Result = pResult;
                    var localOnSuccess = OnSuccess;
                    OnSuccess = null;
                    OnFailure = null;
                    if (localOnSuccess != null) {
                        JustTrackSDKBehaviour.CallOnMainThread(() => {
                            localOnSuccess(pResult);
                        });
                    }
                }
            }

            internal void Reject(string pError) {
                lock(this) {
                    if (Result != null || Error != null) {
                        return;
                    }
                    Error = pError;
                    var localOnFailure = OnFailure;
                    OnSuccess = null;
                    OnFailure = null;
                    if (localOnFailure != null) {
                        JustTrackSDKBehaviour.CallOnMainThread(() => {
                            localOnFailure(pError);
                        });
                    }
                }
            }
        #endif
    }
}
