using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderScript : MonoBehaviour {
	public int health = 10;
	float healthTintDelay = 0.1f;
	bool tintBoulder = false;
	Color originalMaterialColour;
	PlayerInventoryScript playerInventoryScript;

	void Start () {
		playerInventoryScript = GameObject.Find ("Player").GetComponent<PlayerInventoryScript>();
		originalMaterialColour = GetComponent<Renderer> ().material.color;
	}
	void Update () {
		if (health <= 0) {
			//loot drop
			for (int i = 0; i < Random.Range(3, 6); i++) {
				GameObject insItem = Instantiate (playerInventoryScript.droppedItemPrefab, new Vector3 (Random.Range (transform.position.x - 1, transform.position.x + 1), -0.7f, Random.Range (transform.position.z - 1, transform.position.z + 1)), playerInventoryScript.droppedItemPrefab.transform.rotation) as GameObject;
				insItem.transform.GetChild (0).GetComponent<DroppedItemScript> ().myValue = new InventoryItem (1, "rocks", "Rocks");
				playerInventoryScript.addDroppedItemToSaves (insItem);
			}
			for (int i = 0; i < Random.Range(1, 3); i++) {
				GameObject insItem = Instantiate (playerInventoryScript.droppedItemPrefab, new Vector3 (Random.Range (transform.position.x - 1, transform.position.x + 1), -0.7f, Random.Range (transform.position.z - 1, transform.position.z + 1)), playerInventoryScript.droppedItemPrefab.transform.rotation) as GameObject;
				insItem.transform.GetChild (0).GetComponent<DroppedItemScript> ().myValue = new InventoryItem (1, "flint", "Flint");
				playerInventoryScript.addDroppedItemToSaves (insItem);
			}
			for (int i = 0; i < Random.Range(0, 2); i++) {
				GameObject insItem = Instantiate (playerInventoryScript.droppedItemPrefab, new Vector3 (Random.Range (transform.position.x - 1, transform.position.x + 1), -0.7f, Random.Range (transform.position.z - 1, transform.position.z + 1)), playerInventoryScript.droppedItemPrefab.transform.rotation) as GameObject;
				insItem.transform.GetChild (0).GetComponent<DroppedItemScript> ().myValue = new InventoryItem (1, "coal", "Coal");
				playerInventoryScript.addDroppedItemToSaves (insItem);
			}
			Destroy (gameObject);
		} else if (tintBoulder) {
			healthTintDelay -= Time.deltaTime;
			if (healthTintDelay <= 0) {
				tintBoulder = false;
				GetComponent<Renderer> ().material.color = originalMaterialColour;
			}
		}
	}

	public void loseHealth (int hp) {
		health -= hp;
		if (health > 0) {
			tintBoulder = true;
			healthTintDelay = 0.1f;
			GetComponent<Renderer> ().material.color = new Color (100, 100, 100);
		}
	}
}
