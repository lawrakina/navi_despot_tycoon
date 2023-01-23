using System.Collections.Generic;
using NavySpade.Modules.Utils.Serialization.Interfaces.Runtime;
using UnityEngine;

namespace NavySpade.Modules.Utils.Serialization.Interfaces.Samples.Scripts
{
    public class InteractableTest : MonoBehaviour
    {
        [RequireInterface(typeof(IInteractable))]
        public MonoBehaviour ReferenceWithAttribute;

        public InterfaceReference<IInteractable> InterfaceReference;
        public InterfaceReference<IInteractable, ScriptableObject> InterfaceReferenceWithConstraint;

        [RequireInterface(typeof(IInteractable))]
        public MonoBehaviour[] ReferenceWithAttributeArray;

        [RequireInterface(typeof(IInteractable))]
        public List<Object> ReferenceWithAttributeList;

        private void Awake()
        {
            (ReferenceWithAttribute as IInteractable).Interact();
            InterfaceReference.Value.Interact();
            InterfaceReferenceWithConstraint.Value.Interact();
        }
    }
}