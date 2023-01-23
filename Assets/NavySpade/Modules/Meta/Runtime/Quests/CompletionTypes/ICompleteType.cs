using Core.Meta.Analytics;

namespace NavySpade.Meta.Runtime.Quests.CompletionTypes
{
    public interface ICompleteType
    {
        float TargetScore { get; }

        float StartValue(VariableData startData, VariableData currentData);
        float Value(VariableData startData, VariableData currentData);
        float Progress(VariableData startData, VariableData currentData);
    }
}