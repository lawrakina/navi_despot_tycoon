#if UNITY_ANDROID
using System;
using UnityEngine;

namespace JustTrack {
    internal class Promise : AndroidJavaProxy {
        private const string CLASS = "Promise";

        internal Promise(Action<AndroidJavaObject> pResolve, Action<string> pReject) : base($"{SDKAndroidAgent.PACKAGE}.{CLASS}") {
            m_OnResolve = pResolve;
            m_OnFailure = pReject;
        }

        Action<AndroidJavaObject> m_OnResolve;
        Action<string> m_OnFailure;

        /**
        * Called after the operation was successful.
        *
        * @param pResponse The data the operation produced.
        */
        void resolve(AndroidJavaObject pResponse) {
            m_OnResolve(pResponse);
        }

        /**
        * Called in case an error which can noy be handled occurs. If this method is called, {@link #resolve(Object)}
        * will not be called anymore.
        *
        * @param pException The error which occurred.
        */
        void reject(AndroidJavaObject pException) {
            m_OnFailure.Invoke(pException.Call<string>("toString"));
        }
    }
}
#endif
