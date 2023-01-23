#if UNITY_IOS
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace JustTrack {
    public static class SwiftPostProcessor {
        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath) {
            if (buildTarget == BuildTarget.iOS) {
                // We need to construct our own PBX project path that corrently refers to the Bridging header
                // var projPath = PBXProject.GetPBXProjectPath(buildPath);
                var projPath = buildPath + "/Unity-iPhone.xcodeproj/project.pbxproj";
                var proj = new PBXProject();
                proj.ReadFromFile(projPath);

                var targetGuid = proj.GetUnityMainTargetGuid();

                // Configure build settings
                proj.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
                proj.SetBuildProperty(targetGuid, "SWIFT_OBJC_BRIDGING_HEADER", "Libraries/Plugins/iOS/JustTrackSDK/Source/JustTrackSDK-Bridging-Header.h");
                proj.SetBuildProperty(targetGuid, "SWIFT_OBJC_INTERFACE_HEADER_NAME", "JustTrackSDK-Swift.h");
                // we can't integrate with ironsource from ios like that, we have to integrate with the ironsource unity sdk
                proj.SetBuildProperty(targetGuid, "GCC_PREPROCESSOR_DEFINITIONS", "ENABLE_IRONSOURCE_INTEGRATION=0");
                proj.AddBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks");

                proj.WriteToFile(projPath);

                // Configure Info.plist to contain our SKAdNetwork endpoint
                string pListPath = buildPath + "/Info.plist";
                PlistDocument plist = new PlistDocument();
                plist.ReadFromString(File.ReadAllText(pListPath));

                // set the attribution endpoint for our app to our backend
                plist.root.SetString("NSAdvertisingAttributionReportEndpoint", "https://justtrack-skadnetwork.io");

                // Write to file
                File.WriteAllText(pListPath, plist.WriteToString());
            }
        }
    }
}
#endif