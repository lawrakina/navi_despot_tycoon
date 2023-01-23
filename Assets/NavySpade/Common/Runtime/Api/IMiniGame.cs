using System;
using UnityEngine.Events;

namespace NavySpade.Common.Runtime.Api
{
    public interface IMiniGame
    {
        UnityEvent Started { get; set; }
        UnityEvent Completed { get; set; }

        float Value { get; }
        bool IsActive { get; }

        void Activate(Action started = null, Action completed = null);

        void Deactivate();

        void OnUpdate();
    }
}