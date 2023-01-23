using NavySpade.Modules.Saving.Runtime;
using UnityEditor;

namespace NavySpade.Modules.Saving.Editor
{
    public static class SavingUtility
    {
        [MenuItem("Tools/Saves/Clear", priority = 1)]
        public static void ClearSaves()
        {
            SaveManager.DeleteAll();
        }

        [MenuItem("Tools/Saves/Inspector", priority = 2)]
        public static void OpenInspector()
        {
            SavingsViewWindow.ShowWindow();
        }
    }
}