using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public string Format = "{0}:{1:D2}";
    
    public void UpdateTimer(float leftTime)
    {
        var dt = TimeSpan.FromSeconds(leftTime);
        _text.text = string.Format(Format, dt.Minutes, dt.Seconds);
    }
}
