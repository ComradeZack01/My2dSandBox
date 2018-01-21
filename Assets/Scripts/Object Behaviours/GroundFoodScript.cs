using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFoodScript : MonoBehaviour {
	PlayerInventoryScript playerInventoryScript;
	public string harvestItemName;
	public string buildingIDName;
	public int harvestItemQuantity;

	//update by interval to reduce lag
	float intervalCountdown = 1;
	bool far = false;

	void Start () {
		playerInventoryScript = GameObject.Find ("Player").GetComponent<PlayerInventoryScript>();
	}
	public void harvestItem () {
		playerInventoryScript.addObjectToPlayerInventory(harvestItemQuantity, harvestItemName, playerInventoryScript.findItemDisplayNameWithName(harvestItemName), false, Vector3.zero);
		Destroy (gameObject);
	}
	void Update () {
	}
}
