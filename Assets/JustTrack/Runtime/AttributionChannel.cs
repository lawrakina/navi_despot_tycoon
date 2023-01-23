using UnityEngine;

namespace JustTrack {
    /**
    * A representation of a channel.
    */
    public class AttributionChannel {
        private AttributionChannel(int pId, string pName, bool pIncent) {
            this.Id = pId;
            this.Name = pName;
            this.Incent = pIncent;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public bool Incent { get; private set; }

        #if UNITY_ANDROID
            internal static AttributionChannel FromAndroidObject(AndroidJavaObject pChannel) {
                return new AttributionChannel(
                    pChannel.Call<int>("getId"),
                    pChannel.Call<string>("getName"),
                    pChannel.Call<bool>("isIncent")
                );
            }
        #endif
        #if UNITY_IOS
            internal static AttributionChannel CreateChannel(int pId, string pName, bool pIncent) {
                return new AttributionChannel(pId, pName, pIncent);
            }
        #endif
        #if UNITY_EDITOR
            internal static AttributionChannel CreateFakeChannel(int pId, string pName, bool pIncent) {
                return new AttributionChannel(pId, pName, pIncent);
            }
        #endif
    }
}