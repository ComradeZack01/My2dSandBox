using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTextColourScript : MonoBehaviour {
	public Color targetColour;
	public bool constantlyUpdateRect;
	public Button button;
	Color originalColour;
	Rect myRect;
	void Awake () {
		originalColour = GetComponent<Text> ().color;
	}
	void Start () {
		myRect = Custom2D.generatePointDetectionRect (GetComponent<RectTransform>().position, GetComponent<RectTransform>().rect);
	}
	void Update () {
		if (Input.GetMouseButton(0) && myRect.Contains(new Vector2(Input.mousePosition.x, Input.mousePosition.y))) {
			GetComponent<Text> ().color = targetColour;
		} else {
			GetComponent<Text> ().color = originalColour;
		}
		if (constantlyUpdateRect) {
			myRect = Custom2D.generatePointDetectionRect (GetComponent<RectTransform>().position, GetComponent<RectTransform>().rect);
		}
	}
}
