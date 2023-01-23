using NavySpade.Modules.Utils.Singletons.Runtime.Unity;
using UnityEngine;

namespace NavySpade.pj49.Scripts.UI
{
    public class ResourceCanvas : MonoSingleton<ResourceCanvas>
    {
        [SerializeField] private Canvas _uiCanvas;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Camera _uiCamera;
        
        public Vector3 GetUIPosition(Vector3 worldPos)
        {
            Vector3 screenPos = _mainCamera.WorldToScreenPoint(worldPos);
            screenPos.z = _uiCanvas.transform.position.z;
            return _uiCamera.ScreenToWorldPoint(screenPos);
        }
    } 
}