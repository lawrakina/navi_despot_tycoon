using UnityEngine;
using UnityEngine.UI;

public class UIGrayMode : MonoBehaviour
{
    [SerializeField] private Material _grayMaterial;
    [SerializeField] private Image[] _renderers;

    public bool GrayMode
    {
        set
        {
            if (value)
            {
                foreach (var rend in _renderers)
                {
                    rend.material = _grayMaterial;
                }
            }
            else
            {
                foreach (var rend in _renderers)
                {
                    rend.material = null;
                }
            }
        }
    }
}
