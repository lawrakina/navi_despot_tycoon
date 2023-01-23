using UnityEngine;

namespace JustTrack {
    /**
    * A representation of a campaign.
    */
    public class AttributionCampaign {
        private AttributionCampaign(int pId, string pName, string pType) {
            this.Id = pId;
            this.Name = pName;
            this.Type = pType;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }

        #if UNITY_ANDROID
            internal static AttributionCampaign FromAndroidObject(AndroidJavaObject pCampaign) {
                return new AttributionCampaign(
                    pCampaign.Call<int>("getId"),
                    pCampaign.Call<string>("getName"),
                    pCampaign.Call<string>("getType")
                );
            }
        #endif
        #if UNITY_IOS
            internal static AttributionCampaign CreateCampaign(int pId, string pName, string pType) {
                return new AttributionCampaign(pId, pName, pType);
            }
        #endif
        #if UNITY_EDITOR
            internal static AttributionCampaign CreateFakeCampaign(int pId, string pName, string pType) {
                return new AttributionCampaign(pId, pName, pType);
            }
        #endif
    }
}