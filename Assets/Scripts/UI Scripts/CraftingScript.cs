using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingScript : MonoBehaviour {
	PlayerInventoryScript playerInventoryScript;

	public string[][] productNamesByIndex; 

	public GameObject craftingSlot1;
	public GameObject craftingSlot2;
	public GameObject craftingSlot3; 

	public int craftingIndex; 
	public string craftingCategory; 

	void Start () {
		playerInventoryScript = GameObject.Find ("Player").GetComponent<PlayerInventoryScript> ();

		productNamesByIndex = new string[][] {
			//tools (0)
			new string[] {
				"flintAxe",
				"flintPickaxe"
			},
			//buildings (1)
			new string[] {
				"campfire"
			},
			//materials (2)
			new string[] {
				"rope"
			},
			//weapons (3)
			new string[] {

			},
			//clothing (4)
			new string[] {

			},
			//food (5)
			new string[] {

			},
			//special (6)
			new string[] {

			}
		};
		setRecipe ();
	}
	public void changeCraftingCategory (string category) {
		craftingCategory = category;
		setRecipe ();
	}
	public void changeCraftingIndex (bool addIndexCount) {
		if (addIndexCount) {
			if (productNamesByIndex [findCraftingCategoryIndexByName (craftingCategory)].Length > craftingIndex + 3)
				craftingIndex++;
		} else {
			if (craftingIndex > 0) 
				craftingIndex--; 
		}
		setRecipe ();
	}
	public void setRecipe () {
		if (productNamesByIndex [findCraftingCategoryIndexByName (craftingCategory)].Length > craftingIndex) {
			craftingSlot1.GetComponent<CraftingRecipeScript> ().myRecipe = playerInventoryScript.findRecipeWithProductName (productNamesByIndex [findCraftingCategoryIndexByName (craftingCategory)] [craftingIndex]);
			craftingSlot1.GetComponent<CraftingRecipeScript> ().updateImage (true);
		} else
			craftingSlot1.GetComponent<CraftingRecipeScript> ().updateImage (false);
		if (productNamesByIndex [findCraftingCategoryIndexByName (craftingCategory)].Length > craftingIndex + 1) {
			craftingSlot2.GetComponent<CraftingRecipeScript> ().myRecipe = playerInventoryScript.findRecipeWithProductName (productNamesByIndex [findCraftingCategoryIndexByName (craftingCategory)] [craftingIndex + 1]);
			craftingSlot2.GetComponent<CraftingRecipeScript> ().updateImage (true);
		} else
			craftingSlot2.GetComponent<CraftingRecipeScript> ().updateImage (false);
		if (productNamesByIndex [findCraftingCategoryIndexByName (craftingCategory)].Length > craftingIndex + 2) {
			craftingSlot3.GetComponent<CraftingRecipeScript> ().myRecipe = playerInventoryScript.findRecipeWithProductName (productNamesByIndex [findCraftingCategoryIndexByName (craftingCategory)] [craftingIndex + 2]);
			craftingSlot3.GetComponent<CraftingRecipeScript> ().updateImage (true);
		} else 
			craftingSlot3.GetComponent<CraftingRecipeScript> ().updateImage (false);
	}
	int findCraftingCategoryIndexByName (string name) {
		if (name == "tools") 
			return 0;
		if (name == "buildings")
			return 1;
		if (name == "materials")
			return 2;
		if (name == "weapons")
			return 3;
		if (name == "clothing")
			return 4;
		if (name == "food")
			return 5;
		if (name == "special")
			return 6;
		return 0;
	}
	void Update () {
	}
	public void craftItem (string productName) {
		CraftingRecipe myRecipe = playerInventoryScript.findRecipeWithProductName (productName);
		bool canBuy = true;
		for (int i = 0; i < myRecipe.materialNames.Length; i++) {
			if (playerInventoryScript.findItemQuantityInPlayerInventory (myRecipe.materialNames [i]) < myRecipe.materialQuantities [i]) {
				canBuy = false;
				break;
			}
		}
		if (canBuy) {
			playerInventoryScript.addObjectToPlayerInventory (myRecipe.productQuantity, myRecipe.productName, myRecipe.productDisplayName, false, Vector3.zero);
			for (int i = 0; i < myRecipe.materialNames.Length; i++) {
				playerInventoryScript.takeItemFromPlayerInventory (myRecipe.materialQuantities [i], myRecipe.materialNames [i]);
			}
		}
	}
}
