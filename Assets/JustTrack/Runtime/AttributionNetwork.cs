using UnityEngine;

namespace JustTrack {
    /**
    * A representation of a network.
    */
    public class AttributionNetwork {
        private AttributionNetwork(int pId, string pName) {
            this.Id = pId;
            this.Name = pName;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }

        #if UNITY_ANDROID
            internal static AttributionNetwork FromAndroidObject(AndroidJavaObject pNetwork) {
                return new AttributionNetwork(
                    pNetwork.Call<int>("getId"),
                    pNetwork.Call<string>("getName")
                );
            }
        #endif
        #if UNITY_IOS
            internal static AttributionNetwork CreateNetwork(int pId, string pName) {
                return new AttributionNetwork(pId, pName);
            }
        #endif
        #if UNITY_EDITOR
            internal static AttributionNetwork CreateFakeNetwork(int pId, string pName) {
                return new AttributionNetwork(pId, pName);
            }
        #endif
    }
}