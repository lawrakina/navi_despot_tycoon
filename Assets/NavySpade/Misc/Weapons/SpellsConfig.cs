using System;
using NavySpade.Modules.Configuration.Runtime.SO;
using UnityEngine;

namespace Project19.Weapons
{
    public class SpellsConfig : ObjectConfig<SpellsConfig>
    {
        [SerializeField] private SpellData[] _startSpells;
        [SerializeField] private SpellData[] _spells;

        private SpellData[] _spellWithOutStart;
        private bool _isInitStartSpell;
        
        public SpellData[] StartSpells => _startSpells;
        public SpellData[] Spells => _spells;

        public SpellData GetStartSpell(PickupType mainType)
        {
            for (int i = 0; i < _startSpells.Length; i++)
            {
                var element = _startSpells[i];
                
                if(element.MainType == mainType)
                    return element;
            }

            throw new Exception($"базовый спелл для пикапа {mainType.ToString()}");
        }
    }
}