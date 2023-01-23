using System;
using NavySpade.Core.Runtime.Levels.Data;

namespace NavySpade.Core.Runtime.Levels.Generation.Abstract
{
    public abstract class LevelGenerator<T> : LevelGeneratorBase where T : LevelDataBase
    {
        private LevelDataBase.AdditionData[] _dataset;

        public override void Generate(LevelDataBase dataBase)
        {
            if (dataBase is T == false)
            {
                throw new Exception("This level has a different type!");
            }
            
            StartGeneration((T)dataBase);
            
            if (_dataset == null)
            {
                _dataset = dataBase.AdditionsData;
                
                foreach (var data in _dataset)
                {
                    data.ExtensionData.Apply();
                }
            }
            else
            {
                foreach (var data in _dataset)
                {
                    data.ExtensionData.Clear();
                }

                _dataset = dataBase.AdditionsData;

                foreach (var data in _dataset)
                {
                    data.ExtensionData.Apply();
                }
            }
        }

        public override void CleanUp()
        {
            OnCleanUp();
        }

        protected abstract void StartGeneration(T levelData);
        protected abstract void OnCleanUp();
    }
}