using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingCategoryScript : MonoBehaviour {
	CraftingScript craftingScript;
	public string categoryName;

	void Start () {
		craftingScript = GameObject.Find ("CraftingUI").GetComponent<CraftingScript> ();
	}

	void Update () {
		if (craftingScript.craftingCategory == categoryName) {
			GetComponent<Image> ().color = new Color (255, 255, 255);
		} else {
			GetComponent<Image> ().color = new Color (0, 0, 0);
		}
	}
}
