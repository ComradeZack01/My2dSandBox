using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour {
	public int health = 10;
	float deathDelay = 3;
	float healthTintDelay = 0.1f;
	bool tintTree = false;
	Color originalMaterialColour1;
	Color originalMaterialColour2;
	PlayerInventoryScript playerInventoryScript;

	void Start () {
		playerInventoryScript = GameObject.Find ("Player").GetComponent<PlayerInventoryScript>();
		originalMaterialColour1 = transform.GetChild (0).GetComponent<Renderer> ().material.color;
		originalMaterialColour2 = transform.GetChild (1).GetComponent<Renderer> ().material.color;
	}
	void Update () {
		if (health <= 0) {
			GetComponent<BoxCollider> ().enabled = false;
			transform.Rotate (new Vector3(1, 0, 0) * Time.deltaTime * 30, Space.Self);
			deathDelay -= Time.deltaTime;
			if (deathDelay <= 0) {
				//loot drop
				for (int i = 0; i < 3; i++) {
					GameObject insItem = Instantiate (playerInventoryScript.droppedItemPrefab, new Vector3 (Random.Range (transform.position.x - 1, transform.position.x + 1), -0.7f, Random.Range (transform.position.z - 1, transform.position.z + 1)), playerInventoryScript.droppedItemPrefab.transform.rotation) as GameObject;
					insItem.transform.GetChild (0).GetComponent<DroppedItemScript> ().myValue = new InventoryItem (1, "wood", "Wood");
					playerInventoryScript.addDroppedItemToSaves (insItem);
				}
				float rand = Random.Range (0f, 100f);
				if (rand < 3) {
					GameObject insItem = Instantiate (playerInventoryScript.droppedItemPrefab, new Vector3 (Random.Range (transform.position.x - 1, transform.position.x + 1), -0.7f, Random.Range (transform.position.z - 1, transform.position.z + 1)), playerInventoryScript.droppedItemPrefab.transform.rotation) as GameObject;
					insItem.transform.GetChild (0).GetComponent<DroppedItemScript> ().myValue = new InventoryItem (1, "apple", "Apple");
					playerInventoryScript.addDroppedItemToSaves (insItem);
				} else if (rand < 6) {
					GameObject insItem = Instantiate (playerInventoryScript.droppedItemPrefab, new Vector3 (Random.Range (transform.position.x - 1, transform.position.x + 1), -0.7f, Random.Range (transform.position.z - 1, transform.position.z + 1)), playerInventoryScript.droppedItemPrefab.transform.rotation) as GameObject;
					insItem.transform.GetChild (0).GetComponent<DroppedItemScript> ().myValue = new InventoryItem (1, "orange", "Orange");
					playerInventoryScript.addDroppedItemToSaves (insItem);
				}
				Destroy (gameObject);
			}
		} else if (tintTree) {
			healthTintDelay -= Time.deltaTime;
			if (healthTintDelay <= 0) {
				tintTree = false;
				transform.GetChild (0).GetComponent<Renderer> ().material.color = originalMaterialColour1;
				transform.GetChild (1).GetComponent<Renderer> ().material.color = originalMaterialColour2;
			}
		}
	}

	public void loseHealth (int hp) {
		health -= hp;
		if (health > 0) {
			tintTree = true;
			healthTintDelay = 0.1f;
			transform.GetChild (0).GetComponent<Renderer> ().material.color = new Color (100, 100, 100);
			transform.GetChild (1).GetComponent<Renderer> ().material.color = new Color (100, 100, 100);
		}
	}
}
