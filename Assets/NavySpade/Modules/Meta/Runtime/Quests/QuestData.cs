using System;
using Core.Meta.Analytics;
using NavySpade.Meta.Runtime.Quests;
using NavySpade.Meta.Runtime.Quests.LifeTimeTypes;
using NavySpade.Modules.Meta.Runtime.Quests.Configuration;
using UnityEngine;

namespace Core.Meta.Quests
{
    [Serializable]
    public class QuestData
    {
        public QuestData(Quest so, ulong startTime, ulong currentTime, VariableData startData)
        {
            Scriptable = so;
            StartTime = startTime;
            StartData = startData;
            Time = currentTime;
        }
        
        public QuestData(Quest so, ulong startTime, ulong currentTime)
        {
            Scriptable = so;
            StartTime = startTime;
            StartData = so.AttachedVariable.GetData();
            Time = currentTime;
        }
        
        public QuestData(Quest so, ulong startTime)
        {
            Scriptable = so;
            StartTime = startTime;
            StartData = so.AttachedVariable.GetData();
            Time = startTime;
        }
        
        public Quest Scriptable
        {
            get
            {
                if (_scriptable == null)
                {
                    if (string.IsNullOrEmpty(ScriptableName))
                        return null;
                    
                    var path = $"{QuestsConfig.Instance.ScriptablesResourcesPath}/{ScriptableName}";
                    _scriptable = Resources.Load<Quest>(path);
                }
                
                return _scriptable;
            }
            set
            {
                ScriptableName = value.name;
                _scriptable = value;
            }
        }

        public string ScriptableName;

        //data
        public VariableData StartData;
        
        //time
        public ulong StartTime;
        public ulong Time;
        
        [NonSerialized] private Quest _scriptable;

        /// <summary>
        /// 0 to 1
        /// </summary>
        public float Progress => Scriptable.CompletionType.Progress(StartData, Scriptable.AttachedVariable.GetData());
        public float Value => Scriptable.CompletionType.Value(StartData, Scriptable.AttachedVariable.GetData());
        public float StartValue => Scriptable.CompletionType.StartValue(StartData, Scriptable.AttachedVariable.GetData());
        public float EndValue => StartValue + Scriptable.CompletionType.TargetScore;
        public bool IsCompleted => Progress >= 1;

        public bool TryGetRemainingTime(out TimeSpan time)
        {
            if (Scriptable.LifeTime == null || Scriptable.LifeTime is Infinity)
            {
                return false;
            }
            
            time = TimeSpan.FromTicks((long) (Scriptable.LifeTime as CountedLifeTime).RemainingTicks);

            return true;
        }
    }
}