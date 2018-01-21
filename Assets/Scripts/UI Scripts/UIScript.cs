using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour {
	ControllerScript masterControllerScript;
	int rectIndex;
	Rect rect;
	public bool repeatUpdate;
	void Start () {
		rect = Custom2D.generatePointDetectionRect (GetComponent<RectTransform>().position, GetComponent<RectTransform>().rect);
		masterControllerScript = GameObject.Find ("EventSystem").GetComponent<ControllerScript> ();
		for (int i = 0; i < masterControllerScript.onScreenUI.Length; i++) {
			if (masterControllerScript.onScreenUI [i] == new Rect(0, 0, 0, 0)) {
				masterControllerScript.onScreenUI [i] = rect;
				rectIndex = i;
				break;
			}
		}
	}

	void Update () {
		if (repeatUpdate) {
			rect = Custom2D.generatePointDetectionRect (GetComponent<RectTransform> ().position, GetComponent<RectTransform> ().rect);
			masterControllerScript.onScreenUI [rectIndex] = rect;
		}
	}
}
public static class Custom2D {
	public static Rect generatePointDetectionRect (Vector2 position, Rect rect) {
		Rect newRect = new Rect (new Vector2 (position.x - rect.width / 2, position.y - rect.height / 2), new Vector2 (rect.width, rect.height));
		return newRect;
	}
}
