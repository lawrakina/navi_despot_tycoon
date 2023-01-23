using Main.UI;
using NavySpade.pj49.Scripts.Productions;
using UnityEngine;

public class BuildProgressBar : MonoBehaviour
{
    [SerializeField] private Progressbar _progressbar;
    [SerializeField] private BuildingBuilder _builder;

    private void Start()
    {
        _builder.ProgressUpdated += UpdateProgress;
        UpdateProgress();
    }

    private void OnDestroy()
    {
        _builder.ProgressUpdated -= UpdateProgress;
    }

    private void UpdateProgress()
    {
        // _progressbar.SetupProgressbar(_builder.TargetCount, _builder.ActualCount);
        // _progressbar.UpdateProgressbar(_builder.ActualCount);
    }
}
