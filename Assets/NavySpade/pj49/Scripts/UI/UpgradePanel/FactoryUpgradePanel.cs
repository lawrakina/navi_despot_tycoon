using System;
using System.Collections;
using EventSystem.Runtime.Core.Dispose;
using EventSystem.Runtime.Core.Managers;
using NavySpade.pj49.Scripts.Productions.Factory;
using NavySpade.pj49.Scripts.Productions.Gym;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace NavySpade.pj49.Scripts.UI{
    public class FactoryUpgradePanel : UpgradePanel{
        [SerializeField]
        protected FactoryUpgradeButton[] _upgradeButtons;

        protected override void Start(){
            // EventManager.Add<Factory>(PopupsEnum.OpenFactoryUpgradePopup, InitPopup).AddTo(_disposal);
            EventManager.Add<FactoryUpgradeData>(PopupsEnum.OpenFactoryUpgradePopup, InitPopup).AddTo(_disposal);
            EventManager.Add(PopupsEnum.CloseUpgradePopup, Close).AddTo(_disposal);
            base.Start();
        }

        private void InitPopup(FactoryUpgradeData data){
            ShowPopup();
            for (int i = 0; i < _upgradeButtons.Length; i++){
                if (data.IsUseCertainUpdates){
                    if (i >= data.UnitsUpgradeParameters.Length){
                        _upgradeButtons[i].gameObject.SetActive(false);
                    } else{
                        _upgradeButtons[i].gameObject.SetActive(true);
                        _upgradeButtons[i].Init(data.Factory.Stats, data.UnitsUpgradeParameters[i]);
                    }
                } else
                    _upgradeButtons[i].gameObject.SetActive(true);
                    _upgradeButtons[i].Init(data.Factory.Stats, data.Factory.Config.ChangeableUpgrades[i]);
            }
        }
    }
}