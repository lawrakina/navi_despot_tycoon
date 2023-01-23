using System;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace JustTrack {
    internal class JustTrackCodeGenerator {
        private const string INJECTED_CODE_DIRECTORY = "Assets/JustTrack/Injected";
        private bool IntegrateWithIronSource;

        internal JustTrackCodeGenerator(bool IntegrateWithIronSource_) {
            IntegrateWithIronSource = IntegrateWithIronSource_;
        }

        internal void Run(bool force) {
            if (!NeedsInjectedCode()) {
                if (force || ShouldRemoveInjectedCode()) {
                    RemoveInjectedCode();
                }
            } else {
                if (force || Directory.Exists(INJECTED_CODE_DIRECTORY) || ShouldInjectCode()) {
                    GenerateInjectedCode();
                }
            }
        }

        const string REMOVE_KEY_ASKED = "io.justtrack.unity.shouldRemoveInjectedCode.asked";
        const string REMOVE_KEY_RESULT = "io.justtrack.unity.shouldRemoveInjectedCode.result";
        const string INJECT_KEY_ASKED = "io.justtrack.unity.shouldInjectCode.asked";
        const string INJECT_KEY_RESULT = "io.justtrack.unity.shouldInjectCode.result";

        private bool ShouldRemoveInjectedCode() {
            if (!Directory.Exists(INJECTED_CODE_DIRECTORY)) {
                // nothing to remove, no need to remove anything
                return false;
            }
            if (SessionState.GetBool(REMOVE_KEY_ASKED, false)) {
                return SessionState.GetBool(REMOVE_KEY_RESULT, false);
            }
            SessionState.SetBool(REMOVE_KEY_ASKED, true);
            var result = EditorUtility.DisplayDialog("JustTrack SDK", "The code injected by the JustTrack SDK into your game (located in " + INJECTED_CODE_DIRECTORY + ") is no longer needed. Do you want to remove it?", "Remove injected code", "Keep injected code");
            SessionState.SetBool(REMOVE_KEY_RESULT, result);
            if (result) {
                // reset the state of the other message to be able to ask again
                SessionState.SetBool(INJECT_KEY_ASKED, false);
            }

            return result;
        }

        private bool ShouldInjectCode() {
            if (SessionState.GetBool(INJECT_KEY_ASKED, false)) {
                return SessionState.GetBool(INJECT_KEY_RESULT, false);
            }
            SessionState.SetBool(INJECT_KEY_ASKED, true);
            var result = EditorUtility.DisplayDialog("JustTrack SDK", "The JustTrack SDK needs to add some code to your game to help with the integration of some of the SDKs (like the IronSource SDK). It will be placed in " + INJECTED_CODE_DIRECTORY + " and needs to be compiled in the default assembly (Assembly-CSharp.dll).", "Create or update the injected code", "Don't add code to my project");
            SessionState.SetBool(INJECT_KEY_RESULT, result);
            if (result) {
                // reset the state of the other message to be able to ask again
                SessionState.SetBool(REMOVE_KEY_ASKED, false);
            }

            return result;
        }

        private bool NeedsInjectedCode() {
            return IntegrateWithIronSource;
        }

        private static void RemoveInjectedCode() {
            if (!Directory.Exists(INJECTED_CODE_DIRECTORY)) {
                return;
            }
            Directory.Delete(INJECTED_CODE_DIRECTORY, true);
            File.Delete(INJECTED_CODE_DIRECTORY + ".meta");
            if (Directory.GetFiles("Assets/JustTrack").Length == 0) {
                Directory.Delete("Assets/JustTrack", true);
                File.Delete("Assets/JustTrack.meta");
            }
            AssetDatabase.Refresh();
        }

        private void GenerateInjectedCode() {
            Directory.CreateDirectory(INJECTED_CODE_DIRECTORY);
            bool needsRefresh = false;

            if (!File.Exists(INJECTED_CODE_DIRECTORY + "/ReadMe.md") || File.ReadAllText(INJECTED_CODE_DIRECTORY + "/ReadMe.md") != GetReadmeString()) {
                needsRefresh = true;
                File.WriteAllText(INJECTED_CODE_DIRECTORY + "/ReadMe.md", GetReadmeString());
            }

            if (IntegrateWithIronSource) {
                if (!File.Exists(INJECTED_CODE_DIRECTORY + "/IronSourceAdapterImpl.cs") || File.ReadAllText(INJECTED_CODE_DIRECTORY + "/IronSourceAdapterImpl.cs") != GetIronSourceAdapterString()) {
                    needsRefresh = true;
                    File.WriteAllText(INJECTED_CODE_DIRECTORY + "/IronSourceAdapterImpl.cs", GetIronSourceAdapterString());
                }
            } else if (File.Exists(INJECTED_CODE_DIRECTORY + "/IronSourceAdapterImpl.cs")) {
                needsRefresh = true;
                File.Delete(INJECTED_CODE_DIRECTORY + "/IronSourceAdapterImpl.cs");
                File.Delete(INJECTED_CODE_DIRECTORY + "/IronSourceAdapterImpl.cs.meta");
            }

            if (needsRefresh) {
                AssetDatabase.Refresh();
            }
        }

        private static void GenerateIronSourceAdapter() {
            File.WriteAllText("Assets/JustTrack/Injected/", GetIronSourceAdapterString());
            AssetDatabase.Refresh();
        }

        private static string GetReadmeString() {
            return @"# JustTrack SDK - Injected Code

This folder contains code managed by the JustTrack SDK.

## Why is this needed?

To make it easy to integrate and update the JustTrack SDK it is provieded as a Unity package.
However, this means that the C# code of the JustTrack SDK can't access code from the default
assembly (Assembly-CSharp.dll). This is normally a good thing and can speed up builds by some
bit, but it also comes with some drawbacks. Mainly, some SDKs like the IronSource SDK are
provided as a .unitypackage by default and therefore end up in the default assembly if no futher
changes are made to them by a developer. This means, we can only access them via reflection at
runtime.

For the most part, using reflection is sufficient to perform the needed tasks. However, sometimes
we need to generate a small amount of code at runtime and this might not be supported if you are
using the IL2CPP backend instead of the Mono backend for greater speed. Thus, we need to add some
small bridging code to the default assembly to avoid having to generate that code at runtime.

## Can I edit these files?

There should be no need to change any of these files and your changes might be overwritten at any
time during futher development by the code which generated them in the first place. Instead, if
you need to make some changes, please tell us why you need to change them, what needs to be
changed and we will find a solution fitting you and improving the JustTrack SDK for everyone
else, too. You can contact us at <mailto:contact@justtrack.io> or <https://justtrack.io/contact/>.

## Should I add these files to version control?

You should add the files in this folder to version control. This will make it simpler along your
team to work with the JustTrack SDK as not every developer has to make the decision whether to
actually create the generated code in the first place. It is also needed if you are building your
game in some CI pipeline to ensure a consistent version of your game is produced every time.
";
        }

        private static string GetIronSourceAdapterString() {
            return @"#if JUSTTRACK_IRONSOURCE_INTEGRATION
using System;
using JustTrack;

// DO NOT EDIT - AUTOMATICALLY GENERATED BY THE JUSTTRACK SDK

namespace JustTrackInjected {
    public class IronSourceAdapterImpl: IronSourceAdapter {
        public void SetIronSourceOnImpressionHandler(Action<object> proxy) {
            IronSourceEvents.onImpressionSuccessEvent += (eventObj) => proxy(eventObj);
        }
    }
}
#endif
";
        }
    }
}
