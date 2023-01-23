using UnityEngine;

namespace NavySpade.Modules.Utils.Serialization.Interfaces.Samples.Scripts
{
	public class InteractableComponent : MonoBehaviour, IInteractable
	{
		public string DebugText;

		public void Interact()
		{
			Debug.Log($"Interacted with component: {name}");
		}
	}
}
