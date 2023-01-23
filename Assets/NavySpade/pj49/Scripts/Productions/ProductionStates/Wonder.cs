using NavySpade.pj49.Scripts.Saving;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Productions.ProductionStates
{
    public class Wonder : MonoBehaviour, ISaveable
    {
        [SerializeField] private BuildingBuilder _buildingBuilder;
        
        public void Init()
        {
            LevelSaver.Instance.Register(this);
            WonderSavingData savingData = LevelSaver.LoadWonderSaving();
            // _buildingBuilder.Init(savingData.BuildingProgress);
        }

        public void Save()
        {
            WonderSavingData savingData = new WonderSavingData();
            // savingData.BuildingProgress = _buildingBuilder.ActualCount;
            LevelSaver.SaveWonder(savingData);
        }
    }
}