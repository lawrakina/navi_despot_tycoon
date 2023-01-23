using NavySpade.Core.Runtime.Levels.Data;
using UnityEngine;

namespace NavySpade.Core.Runtime.Levels.Generation.Abstract
{
    public abstract class LevelGeneratorBase : MonoBehaviour
    {
        public abstract void Generate(LevelDataBase data);
        public abstract void CleanUp();
    }
}