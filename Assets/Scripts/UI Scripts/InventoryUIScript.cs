using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIScript : MonoBehaviour {
	ControllerScript masterControllerScript;
	public int rectIndex;
	float updateClock = 0.5f; 
	Rect rect;
	void Awake () {
		rectIndex--;
	}
	void Start () {
		rect = new Rect (new Vector2 (GetComponent<RectTransform>().position.x - GetComponent<RectTransform>().rect.width / 2, GetComponent<RectTransform>().position.y - GetComponent<RectTransform>().rect.height / 2), new Vector2 (GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height));
		masterControllerScript = GameObject.Find ("EventSystem").GetComponent<ControllerScript> ();
		masterControllerScript.inventoryUI [rectIndex] = rect;
		masterControllerScript.inventoryUIObject [rectIndex] = gameObject;
	}

	void Update () {
		if (updateClock > 0) {
			updateClock -= Time.deltaTime;
		} else {
			masterControllerScript.inventoryUI [rectIndex] = rect;
			updateClock = 0.5f;
		}
	}
}
