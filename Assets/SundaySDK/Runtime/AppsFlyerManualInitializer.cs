using System.Collections;
using System.Collections.Generic;
using AppsFlyerSDK;
using UnityEngine;

namespace Sunday.Internal
{
    public class AppsFlyerManualInitializer : MonoBehaviour, IAppsFlyerConversionData
    {
        void Start()
        {
            AppsFlyer.initSDK(Settings.Instance.appsFlyerDevKey, Settings.Instance.iOSAppId, this);
            AppsFlyer.startSDK();
        }

        public void onAppOpenAttribution(string attributionData)
        {
            AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
        }

        public void onAppOpenAttributionFailure(string error)
        {
            AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
        }

        public void onConversionDataFail(string error)
        {
            AppsFlyer.AFLog("onConversionDataFail", error);
        }

        public void onConversionDataSuccess(string conversionData)
        {
            AppsFlyer.AFLog("onConversionDataSuccess", conversionData);
        }


    }
}
