using UnityEngine;

namespace NavySpade.Core.Runtime.Levels.Generation.Segments
{
    public interface ISceneFactory<T>
    {
        T Create(T prefab, Vector3 position, Quaternion rotation, Transform parent = null);
    }

    public class LevelSegmentFactory : ISceneFactory<LevelSegmentBase>
    {
        public LevelSegmentBase Create(LevelSegmentBase prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var element = Object.Instantiate(prefab, position, rotation, parent);
            return element;
        }
    }
}