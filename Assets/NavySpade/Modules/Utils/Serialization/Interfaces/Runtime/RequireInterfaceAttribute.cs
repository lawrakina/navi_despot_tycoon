﻿using System;
using UnityEngine;

namespace NavySpade.Modules.Utils.Serialization.Interfaces.Runtime
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class RequireInterfaceAttribute : PropertyAttribute
	{
		public readonly Type InterfaceType;

		public RequireInterfaceAttribute(Type interfaceType)
		{
			Debug.Assert(interfaceType.IsInterface, $"{nameof(interfaceType)} needs to be an interface.");
			InterfaceType = interfaceType;
		}
	}
}