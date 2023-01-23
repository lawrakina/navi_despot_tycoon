using UnityEngine;

namespace NavySpade.Modules.Utils.Serialization.Interfaces.Samples.Scripts
{
	public class InteractableAsset : ScriptableObject, IInteractable
	{
		public void Interact()
		{
			Debug.Log($"Interacted with asset: {name}");
		}
	}
}
