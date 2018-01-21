using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAlignmentScript : MonoBehaviour {
	//1 is default text
	float scale; 
	public bool dontChangeRect;
	void Awake () {
		//align scale and anchored position to fit device measurements
		scale = Screen.width / 1500f * 0.85f;

		if (GetComponent<Text> () != null) {
			GetComponent<Text> ().fontSize = (int)(GetComponent<Text> ().fontSize * scale);
		}
		if (!dontChangeRect) {
			GetComponent<RectTransform> ().anchoredPosition = new Vector2 (GetComponent<RectTransform> ().anchoredPosition.x * scale, GetComponent<RectTransform> ().anchoredPosition.y * scale);
			GetComponent<RectTransform> ().sizeDelta = new Vector2 (GetComponent<RectTransform> ().rect.width * scale, GetComponent<RectTransform> ().rect.height * scale);
		}
	}
}
