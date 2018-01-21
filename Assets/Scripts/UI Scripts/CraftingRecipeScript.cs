using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingRecipeScript : MonoBehaviour {
	int buyStage = 0;
	float buyTimer = 2;
	public CraftingRecipe myRecipe;
	PlayerInventoryScript playerInventoryScript;

	void Start () {
		playerInventoryScript = GameObject.Find ("Player").GetComponent<PlayerInventoryScript> ();
		updateImage (true);
	}
	public void updateImage (bool showImage) {
		if (showImage) {
			GetComponent<Button> ().enabled = true;
			GetComponent<Image> ().enabled = true;
			GetComponent<Image> ().sprite = myRecipe.productSprite;
		} else {
			GetComponent<Image> ().enabled = false;
			GetComponent<Button> ().enabled = false;
			buyTimer = 0;
			buyStage = 0;
		}
	}
	void Update () {
		if (buyStage == 1 && myRecipe != null) {
			buyTimer -= Time.deltaTime;
			if (buyTimer <= 0) {
				buyStage = 0;
			}
			transform.GetChild (0).GetComponent<Text> ().text = "";
			for (int i = 0; i < myRecipe.materialNames.Length; i++) {
				transform.GetChild (0).GetComponent<Text> ().text += myRecipe.materialDisplayNames [i] + ": " + myRecipe.materialQuantities [i].ToString () + "\n";
			}
		} else {
			transform.GetChild (0).GetComponent<Text> ().text = "";
		}
	}
	public void updateCraft () {
		if (myRecipe.productName != "") { 
			if (buyStage == 0) {
				buyStage = 1;
				buyTimer = 2;
			} else {
				buyTimer = 2;
				GameObject.Find ("CraftingUI").GetComponent<CraftingScript> ().craftItem (myRecipe.productName);
			}
		}
	}
}
