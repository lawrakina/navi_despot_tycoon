using System;
using UnityEngine;

public class AnimationEventsHandler : MonoBehaviour
{
    public event Action JumpTrigger;

    private void OnBigJumpTrigger()
    {
        JumpTrigger?.Invoke();
    }
}
