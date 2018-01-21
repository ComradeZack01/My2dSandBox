using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour {
	public Slider rotationSliderY;
	void Update () {
		transform.rotation = Quaternion.Euler (transform.eulerAngles.x, rotationSliderY.value * 360, transform.eulerAngles.z);
	}
}
