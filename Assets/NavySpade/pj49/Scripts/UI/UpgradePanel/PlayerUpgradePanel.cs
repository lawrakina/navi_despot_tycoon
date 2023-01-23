using System;
using System.Collections;
using System.Collections.Generic;
using EventSystem.Runtime.Core.Dispose;
using EventSystem.Runtime.Core.Managers;
using NavySpade.Core.Runtime.Player.Logic;
using NavySpade.pj49.Scripts.UI;
using UnityEngine;

public class PlayerUpgradePanel : UpgradePanel
{
    [SerializeField] protected PlayerUpgradeButton[] _upgradeButtons;
    
    protected override void Start()
    {
        EventManager.Add(PopupsEnum.OpenPlayerUpgradePopup, InitPopup).AddTo(_disposal);
        EventManager.Add(PopupsEnum.CloseUpgradePopup, Close).AddTo(_disposal);
        base.Start();
    }
        
    private void InitPopup()
    {
        ShowPopup();
        for (int i = 0; i < _upgradeButtons.Length; i++)
        {
            _upgradeButtons[i].Init(SinglePlayer.Instance.CapacityHandler);
        }
    }
}
