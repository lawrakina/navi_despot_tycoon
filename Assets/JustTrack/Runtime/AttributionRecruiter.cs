using UnityEngine;

namespace JustTrack {
    /**
    * A representation of a recruiter.
    */
    public class AttributionRecruiter {
        private AttributionRecruiter(string pAdvertiserId, string pUserId, string pPackageId, string pPlatform) {
            this.AdvertiserId = pAdvertiserId;
            this.UserId = pUserId;
            this.PackageId = pPackageId;
            this.Platform = pPlatform;
        }

        public string AdvertiserId { get; private set; }
        public string UserId { get; private set; }
        public string PackageId { get; private set; }
        public string Platform { get; private set; }

        #if UNITY_ANDROID
            internal static AttributionRecruiter FromAndroidObject(AndroidJavaObject pRecruiter) {
                if (pRecruiter == null) {
                    return null;
                }

                return new AttributionRecruiter(
                    pRecruiter.Call<string>("getAdvertiserId"),
                    pRecruiter.Call<string>("getUserId"),
                    pRecruiter.Call<string>("getClientId"),
                    pRecruiter.Call<string>("getPlatform")
                );
            }
        #endif
        #if UNITY_IOS
            internal static AttributionRecruiter CreateRecruiter(string pAdvertiserId, string pUserId, string pPackageId, string pPlatform) {
                return new AttributionRecruiter(pAdvertiserId, pUserId, pPackageId, pPlatform);
            }
        #endif
        #if UNITY_EDITOR
            internal static AttributionRecruiter CreateFakeRecruiter() {
                return new AttributionRecruiter("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000", "fake.package.id", "fakeplatform");
            }
        #endif
    }
}