using System;
using NavySpade.Modules.Configuration.Runtime.Factory;
using UnityEngine;

namespace NavySpade.Modules.Configuration.Runtime.SO
{
    public abstract class ObjectConfig : ScriptableObject
    {
        private static readonly IConfigFactory Factory;

        public static ObjectConfig GetConfig(Type type)
        {
            return Factory.CreateAndLoad(type);
        }

        static ObjectConfig()
        {
            Factory = new ObjectConfigFactory();
        }
    }
}