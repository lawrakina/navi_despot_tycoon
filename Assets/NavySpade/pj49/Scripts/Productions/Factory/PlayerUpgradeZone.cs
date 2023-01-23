using EventSystem.Runtime.Core.Managers;
using UnityEngine;

public class PlayerUpgradeZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            EventManager.Invoke(PopupsEnum.OpenPlayerUpgradePopup);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            EventManager.Invoke(PopupsEnum.CloseUpgradePopup);
        }
    }
}
