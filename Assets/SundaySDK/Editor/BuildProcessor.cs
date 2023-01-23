
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Sunday
{
    public class BuildProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 1;

        [PostProcessBuild(1)]
        public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
        {
#if UNITY_IOS
            if (buildTarget == BuildTarget.iOS)
            {
                // Get Settings
                var settings = AssetDatabase.LoadAssetAtPath<Settings>(Settings.SUNDAY_SETTINGS_PATH);

                // Get plist file and read it.
                string plistPath = pathToBuiltProject + "/Info.plist";
                Debug.Log("Info.plist Path is: " + plistPath);
                PlistDocument plist = new PlistDocument();
                plist.ReadFromString(File.ReadAllText(plistPath));

                // Get root
                PlistElementDict rootDict = plist.root;

                //iOS 14 Pop-Up message
                rootDict.SetString("NSUserTrackingUsageDescription", "Your data will only be used to deliver personalized ads to you");

#if USE_IRON_SOURCE_SDK
                //AdMob Id
                rootDict.SetString("GADApplicationIdentifier", settings.adMobAndroidID);
#endif

                PlistElementArray array = plist.root.CreateArray("SKAdNetworkItems");

                string[] skaIds = new string[] {
                "su67r6k2v3.skadnetwork",  //IronSrc
                "4pfyvq9l8r.skadnetwork",  //AdColony
                "cstr6suwn9.skadnetwork",  //AdMob
                "ludvb6z3bs.skadnetwork",  //AppLovin
                "f38h382jlk.skadnetwork",  //Charboost
                "v9wttpbfk9.skadnetwork",  //Facebook
                "n38lu8286q.skadnetwork",  //Facebook
                "9g2aggbj52.skadnetwork",  //Fyber
                "wzmmz9fp6w.skadnetwork",  //InMobi
                "22mmun2rn5.skadnetwork",  //Pangle (Non CN)
                "ecpz2srf59.skadnetwork",  //Tapjoy
                "4dzt52r2t5.skadnetwork",  //UnityAds
                "gta9lk7p23.skadnetwork",  //Vungle
                "275upjj5gd.skadnetwork",
                "2u9pt9hc89.skadnetwork",
                "3rd42ekr43.skadnetwork",
                "4468km3ulz.skadnetwork",
                "44jx6755aq.skadnetwork",
                "4fzdc2evr5.skadnetwork",
                "5lm9lj6jb7.skadnetwork",
                "6g9af3uyq4.skadnetwork",
                "7rz58n8ntl.skadnetwork",
                "7ug5zh24hu.skadnetwork",
                "8s468mfl3y.skadnetwork",
                "9nlqeag3gk.skadnetwork",
                "9rd848q2bz.skadnetwork",
                "9t245vhmpl.skadnetwork",
                "c6k4g5qg8m.skadnetwork",
                "cg4yq2srnc.skadnetwork",
                "ejvt5qm6ak.skadnetwork",
                "g28c52eehv.skadnetwork",
                "hs6bdukanm.skadnetwork",
                "klf5c3l5u5.skadnetwork",
                "m8dbw4sv7c.skadnetwork",
                "mlmmfzh3r3.skadnetwork",
                "mtkv5xtk9e.skadnetwork",
                "ppxm28t8ap.skadnetwork",
                "prcb7njmu6.skadnetwork",
                "rx5hdcabgc.skadnetwork",
                "t38b2kh725.skadnetwork",
                "tl55sbb4fm.skadnetwork",
                "u679fj5vs4.skadnetwork",
                "uw77j35x4d.skadnetwork",
                "v72qych5uu.skadnetwork",
                "yclnxrl5pm.skadnetwork",
                "5a6flpkh64.skadnetwork",
                "av6w8kgt66.skadnetwork",
                "kbd757ywx3.skadnetwork",
                "ydx93a7ass.skadnetwork",
                "nu4557a4je.skadnetwork",
                "3sh42y64q3.skadnetwork",
                "424m5254lk.skadnetwork",
                "578prtvx9j.skadnetwork",
                "5l3tpt7t6e.skadnetwork",
                "e5fvkxwrpn.skadnetwork",
                "f73kdq92p3.skadnetwork",
                "ggvn48r87g.skadnetwork",
                "p78axxw29g.skadnetwork",
                "pwa73g5rt2.skadnetwork",
                "w9q455wk68.skadnetwork",
                "wg4vff78zm.skadnetwork",
                "n9x2a789qt.skadnetwork",
                "r26jy69rpl.skadnetwork",
                "488r3q3dtq.skadnetwork",
                "6xzpu9s2p8.skadnetwork",
                "8m87ys6875.skadnetwork",
                "97r2b46745.skadnetwork",
                "cj5566h2ga.skadnetwork",
                "mls7yz5dvl.skadnetwork",
                "zmvfpc5aq8.skadnetwork",
                "238da6jt44.skadnetwork",
                "24t9a8vw3c.skadnetwork",
                "252b5q8x7y.skadnetwork",
                "3qy4746246.skadnetwork",
                "44n7hlldy6.skadnetwork",
                "523jb4fst2.skadnetwork",
                "52fl2v3hgk.skadnetwork",
                "5tjdwbrq8w.skadnetwork",
                "737z793b9f.skadnetwork",
                "9yg77x724h.skadnetwork",
                "dzg6xy7pwj.skadnetwork",
                "glqzh8vgby.skadnetwork",
                "gvmwg8q7h5.skadnetwork",
                "hdw39hrw9y.skadnetwork",
                "lr83yxwka7.skadnetwork",
                "n66cz3y3bx.skadnetwork",
                "nzq8sh4pbs.skadnetwork",
                "pu4na253f3.skadnetwork",
                "s39g8k73mm.skadnetwork",
                "v79kvwwj4g.skadnetwork",
                "xy9t38ct57.skadnetwork",
                "y45688jllp.skadnetwork",
                "yrqqpx2mcb.skadnetwork",
                "z4gj7hsk7h.skadnetwork",
                "bvpn9ufa9b.skadnetwork",
                "7953jerfzd.skadnetwork",
                "32z4fx6l9h.skadnetwork",
                "f7s53z58qe.skadnetwork"};

                for (int i = 0; i < skaIds.Length; i++)
                {
                    PlistElementDict dict = array.AddDict();
                    dict.SetString("SKAdNetworkIdentifier", skaIds[i]);
                }

                //IronSource now requires this boolean
                PlistElementDict securityDict = plist.root.CreateDict("NSAppTransportSecurity");
                securityDict.SetBoolean("NSAllowsArbitraryLoads", true);

                Debug.Log("PLIST: " + plist.WriteToString());

                // Write to file
                File.WriteAllText(plistPath, plist.WriteToString());
            }
#endif
        }


        [PostProcessBuild(2)]
        public static void AddIOSCapabilities(BuildTarget buildTarget, string path)
        {
#if UNITY_IOS
            if (buildTarget == BuildTarget.iOS)
            {
                string projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

                var project = new PBXProject();
                project.ReadFromString(System.IO.File.ReadAllText(projectPath));
                var manager = new ProjectCapabilityManager(projectPath, "Entitlements.entitlements", null, project.GetUnityMainTargetGuid());

                manager.AddPushNotifications(false);

                manager.WriteToFile();
            }
#endif
        }

        [PostProcessBuild(3)]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
        {
#if UNITY_IOS
            if (buildTarget == BuildTarget.iOS)
            {
                string projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

                PBXProject pbxProject = new PBXProject();
                pbxProject.ReadFromFile(projectPath);

                string frameworkTarget = pbxProject.GetUnityFrameworkTargetGuid();
                pbxProject.SetBuildProperty(frameworkTarget, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");

                pbxProject.AddFrameworkToProject(frameworkTarget, "AppTrackingTransparency.framework", false);

                string unityTarget = pbxProject.GetUnityMainTargetGuid();
                pbxProject.AddCapability(unityTarget, PBXCapabilityType.PushNotifications);

                pbxProject.WriteToFile(projectPath);
            }
#endif
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            var settings = AssetDatabase.LoadAssetAtPath<Settings>(Settings.SUNDAY_SETTINGS_PATH);
            settings.ValidateAll();
        }
    }
}
