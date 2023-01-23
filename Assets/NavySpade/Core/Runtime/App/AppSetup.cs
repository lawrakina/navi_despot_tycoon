using UnityEngine;

namespace NavySpade.Core.Runtime.App
{
    public static class AppSetup
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            SetTargetFrameRate(ApplicationConfig.Instance.TargetFrameRate);
        }

        private static void SetTargetFrameRate(int frameRate)
        {
            Application.targetFrameRate = frameRate;
        }
    }
}