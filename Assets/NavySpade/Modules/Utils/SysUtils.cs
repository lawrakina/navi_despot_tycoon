using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SysUtils : MonoBehaviour
{
	public static SysUtils Instance;

	static int targetWidth = 1080;
	static int targetHeight = 1920;

	private void Awake() {
		Instance = this;
	}

	// если значение должно всегда быть пропорциональным разрешению экрана под которое оно вводилось
	// то метод снизу вернет исправленное значение при не том соотношении сторон
	// по ширине
	public static float GetFloatValueProportionatelyTargetScreenWidth(float _curVal) {
		float v = _curVal;

		float targetScreenRatio = v / targetWidth;
		float currentScreenRatio = v / Screen.width;

		return v * targetScreenRatio / currentScreenRatio;
	}
	// по высоте
	public static float GetFloatValueProportionatelyTargetScreenHeight(float _curVal) {
		float v = _curVal;

		float targetScreenRatio = v / targetHeight;
		float currentScreenRatio = v / Screen.height;

		return v * targetScreenRatio / currentScreenRatio;
	}
}
