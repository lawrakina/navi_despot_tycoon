using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

namespace JustTrack {
    public class Reflection {

        #if JUSTTRACK_APPSFLYER_INTEGRATION
        internal static string GetAppsflyerId() {
            try {
                return GetAppsflyerAssembly().GetAppsflyerId();
            } catch (System.Exception error) {
                JustTrack.JustTrackSDK.AGENT.LogError("Failed to get appsflyer id: " + error);
                return "";
            }
        }
        #endif

        #if JUSTTRACK_IRONSOURCE_INTEGRATION
        internal static void IronSourceInit(IronSourceSettings ironSourceSettings) {
            try {
                var assembly = GetIronSourceAssembly();
                List<string> adUnits = new List<string>();
                if (ironSourceSettings.enableBanner) {
                    adUnits.Add(assembly.bannerType);
                }
                if (ironSourceSettings.enableInterstitial) {
                    adUnits.Add(assembly.interstitialType);
                }
                if (ironSourceSettings.enableRewardedVideo) {
                    adUnits.Add(assembly.rewardedVideoType);
                }
                if (ironSourceSettings.enableOfferwall) {
                    adUnits.Add(assembly.offerwallType);
                }
                assembly.Init(ironSourceSettings.appKey, adUnits.ToArray());
            } catch (System.Exception error) {
                JustTrack.JustTrackSDK.AGENT.LogError("Failed to initialize IronSource: " + error);
            }
        }

        internal static void IronSourceSetUserId(string userId) {
            try {
                GetIronSourceAssembly().SetUserId(userId);
            } catch (System.Exception error) {
                JustTrack.JustTrackSDK.AGENT.LogError("Failed to set IronSource user id: " + error);
            }
        }

        internal static void IronSourceAddImpressionListener(Action<IronSourceImpressionData> listener) {
            try {
                var assembly = GetIronSourceAssembly();
                Action<object> proxy = (impression) => {
                    try {
                        listener(assembly.ToImpressionData(impression));
                    } catch (System.Exception error) {
                        JustTrack.JustTrackSDK.AGENT.LogError("Failed to process IronSource impression listener call: " + error);
                    }
                };

                assembly.AddImpressionListener(proxy);
            } catch (System.Exception error) {
                JustTrack.JustTrackSDK.AGENT.LogError("Failed to setup IronSource impression listener: " + error);
            }
        }
        #endif

        internal class IronSourceImpressionData {
            internal readonly string adUnit;
            internal readonly string placement;
            internal readonly string adNetwork;
            internal readonly string abTesting;
            internal readonly string segmentName;
            internal readonly string instanceName;
            internal readonly double revenue;

            internal IronSourceImpressionData(string adUnit, string placement, string adNetwork, string abTesting, string segmentName, string instanceName, double revenue) {
                this.adUnit = adUnit;
                this.placement = placement;
                this.adNetwork = adNetwork;
                this.abTesting = abTesting;
                this.segmentName = segmentName;
                this.instanceName = instanceName;
                this.revenue = revenue;
            }
        }

        public class IronSourceAssembly {
            internal readonly string bannerType;
            internal readonly string interstitialType;
            internal readonly string rewardedVideoType;
            internal readonly string offerwallType;
            private readonly PropertyInfo agentField;
            private readonly MethodInfo init;
            private readonly MethodInfo setUserId;
            private readonly Type impressionDataType;
            private readonly FieldInfo impressionDataAdUnit;
            private readonly FieldInfo impressionDataPlacement;
            private readonly FieldInfo impressionDataAdNetwork;
            private readonly FieldInfo impressionDataAbTesting;
            private readonly FieldInfo impressionDataSegmentName;
            private readonly FieldInfo impressionDataInstanceName;
            private readonly FieldInfo impressionDataRevenue;
            private readonly EventInfo onImpressionEvent;

            private readonly IronSourceAdapter ironSourceAdapter;

            internal IronSourceAssembly(Assembly assembly) {
                var adUnitsType = assembly.GetType("IronSourceAdUnits", true);
                this.bannerType = (string) GetStaticField("BANNER", adUnitsType).GetValue(null);
                this.interstitialType = (string) GetStaticField("INTERSTITIAL", adUnitsType).GetValue(null);
                this.rewardedVideoType = (string) GetStaticField("REWARDED_VIDEO", adUnitsType).GetValue(null);
                this.offerwallType = (string) GetStaticField("OFFERWALL", adUnitsType).GetValue(null);

                var ironSourceType = assembly.GetType("IronSource", true);
                this.agentField = GetStaticField("Agent", ironSourceType);
                this.init = ironSourceType.GetMethod("init", BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.Any, new Type[] { typeof(string), typeof(string[]) }, null);
                this.setUserId = ironSourceType.GetMethod("setUserId", BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.Any, new Type[] { typeof(string) }, null);

                this.impressionDataType = assembly.GetType("IronSourceImpressionData", true);
                this.impressionDataAdUnit = this.impressionDataType.GetField("adUnit");
                this.impressionDataPlacement = this.impressionDataType.GetField("placement");
                this.impressionDataAdNetwork = this.impressionDataType.GetField("adNetwork");
                this.impressionDataAbTesting = this.impressionDataType.GetField("ab");
                this.impressionDataSegmentName = this.impressionDataType.GetField("segmentName");
                this.impressionDataInstanceName = this.impressionDataType.GetField("instanceName");
                this.impressionDataRevenue = this.impressionDataType.GetField("revenue");

                var eventsType = assembly.GetType("IronSourceEvents", true);
                var onImpressionEvents = eventsType.GetMember("onImpressionSuccessEvent", BindingFlags.Public | BindingFlags.Static);
                foreach (var onImpressionEvent in onImpressionEvents) {
                    if (onImpressionEvent.MemberType != MemberTypes.Event) {
                        continue;
                    }

                    this.onImpressionEvent = (EventInfo) onImpressionEvent;
                    break;
                }

                this.ironSourceAdapter = GetIronSourceAdapter();
            }

            internal void Init(string appKey, string[] adUnits) {
                var agent = agentField.GetValue(null);
                init.Invoke(agent, new object[] { appKey, adUnits });
            }

            internal void SetUserId(string userId) {
                var agent = agentField.GetValue(null);
                setUserId.Invoke(agent, new object[] { userId });
            }

            internal IronSourceImpressionData ToImpressionData(object impression) {
                var adUnit = (string) impressionDataAdUnit.GetValue(impression);
                var placement = (string) impressionDataPlacement.GetValue(impression);
                var adNetwork = (string) impressionDataAdNetwork.GetValue(impression);
                var abTesting = (string) impressionDataAbTesting.GetValue(impression);
                var segmentName = (string) impressionDataSegmentName.GetValue(impression);
                var instanceName = (string) impressionDataInstanceName.GetValue(impression);
                var revenue = (double?) impressionDataRevenue.GetValue(impression);

                return new IronSourceImpressionData(adUnit, placement, adNetwork, abTesting, segmentName, instanceName, revenue ?? 0);
            }

            internal void AddImpressionListener(Action<object> proxy) {
                // with IL2CPP, we can't actually use reflection to generate code, so we use another trick:
                // - first, we use reflection to lookup a type we inject into the main assembly
                // - that type has direct code references to the ironSource assembly
                // - thus, we create an instance of that type via reflection and simply call it here
                if (ironSourceAdapter != null) {
                    ironSourceAdapter.SetIronSourceOnImpressionHandler(proxy);
                } else {
                    onImpressionEvent.GetAddMethod().Invoke(null, new object[] { BuildDelegate(proxy, onImpressionEvent.EventHandlerType) });
                }
            }

            private static object BuildDelegate(Action<object> proxy, Type delegateType) {
                ParameterInfo[] methodParams = delegateType.GetMethod("Invoke").GetParameters();
                ParameterExpression[] paramsOfDelegate = new ParameterExpression[methodParams.Length];

                for (int i = 0; i < methodParams.Length; i++) {
                    paramsOfDelegate[i] = Expression.Parameter(methodParams[i].ParameterType, methodParams[i].Name);
                }

                var expr = Expression.Lambda(
                    delegateType,
                    Expression.Invoke(Expression.Constant(proxy), paramsOfDelegate),
                    paramsOfDelegate
                );

                return expr.Compile();
            }

            private static PropertyInfo GetStaticField(string fieldName, Type type) {
                var members = type.GetMember(fieldName, BindingFlags.Public | BindingFlags.Static);
                foreach (var member in members) {
                    if (member.MemberType != MemberTypes.Property) {
                        continue;
                    }

                    return (PropertyInfo) member;
                }

                return null;
            }
        }

        private static IronSourceAssembly ironSourceAssembly = null;

        public static IronSourceAssembly GetIronSourceAssembly() {
            if (ironSourceAssembly != null) {
                return ironSourceAssembly;
            }
            try {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies) {
                    var type = assembly.GetType("IronSource", false);
                    if (type != null) {
                        ironSourceAssembly = new IronSourceAssembly(assembly);
                        return ironSourceAssembly;
                    }
                }
                return null;
            } catch (System.Exception) {
                return null;
            }
        }

        public static IronSourceAdapter GetIronSourceAdapter() {
            try {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies) {
                    var type = assembly.GetType("JustTrackInjected.IronSourceAdapterImpl", false);
                    if (type != null) {
                        var constructor = type.GetConstructor(new Type[0]);
                        return (IronSourceAdapter) constructor.Invoke(null);
                    }
                }
                return null;
            } catch (System.Exception) {
                return null;
            }
        }

        public class AppsflyerAssembly {
            private readonly MethodInfo getAppsFlyerId;

            internal AppsflyerAssembly(MethodInfo getAppsFlyerId) {
                this.getAppsFlyerId = getAppsFlyerId;
            }

            internal string GetAppsflyerId() {
                return (string) getAppsFlyerId.Invoke(null, null);
            }
        }

        private static AppsflyerAssembly appsflyerAssembly = null;

        public static AppsflyerAssembly GetAppsflyerAssembly() {
            if (appsflyerAssembly != null) {
                return appsflyerAssembly;
            }
            try {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies) {
                    var type = assembly.GetType("AppsFlyerSDK.AppsFlyer", false);
                    if (type == null) {
                        continue;
                    }

                    var method = type.GetMethod("getAppsFlyerId", BindingFlags.Public | BindingFlags.Static);
                    if (method == null) {
                        continue;
                    }

                    appsflyerAssembly = new AppsflyerAssembly(method);
                    return appsflyerAssembly;
                }
                return null;
            } catch (System.Exception) {
                return null;
            }
        }
    }
}