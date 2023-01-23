using System;
using UnityEngine;

namespace Project19.Weapons.CirculImpuls
{
    [CustomSerializeReferenceName("Круговой импульс")]
    [Serializable]
    public class CirculImpulsData : SpellDataBase<StatBase>
    {
        [SerializeField] private AnimationCurve _sizeMultiplyOverLifetime;

        public AnimationCurve SizeMultiplyOverLifetime => _sizeMultiplyOverLifetime;
    }
}