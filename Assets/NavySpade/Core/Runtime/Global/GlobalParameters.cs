using NavySpade.Core.Runtime.Levels;
using NavySpade.Modules.Saving.Runtime;

public static class GlobalParameters
{
    public static int SkinID
    {
        get => SaveManager.Load("SkinIDG", 0);
        set => SaveManager.Save("SkinIDG", value);
    }

    public static double DoubleLevelNumber
    {
        get => double.Parse(SaveManager.Load("LEVELNUM", "1"));
        set => SaveManager.Save("LEVELNUM", value.ToString());
    }

    public static int AttemptCount
    {
        get => SaveManager.Load($"ATTEMPT_COUNT_{LevelManager.CurrentLevelIndex}", 0);
        set => SaveManager.Save($"ATTEMPT_COUNT_{LevelManager.CurrentLevelIndex}", value);
    }
}
