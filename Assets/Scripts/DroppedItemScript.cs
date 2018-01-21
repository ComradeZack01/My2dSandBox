using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroppedItemScript : MonoBehaviour {
	PlayerInventoryScript playerInventoryScript;
	public InventoryItem myValue;
	float spawnTimer = 1f;
	public GameObject player;

	void Start () {
		player = GameObject.Find ("Player");
		playerInventoryScript = player.GetComponent<PlayerInventoryScript> ();
		GetComponent<Image> ().sprite = playerInventoryScript.findItemSpriteWithName(myValue.name);
	}
	void Update () {
		if (!GetComponent<ChangeRendererScript> ().far) {
			transform.rotation = player.GetComponent<PlayerScript> ().mainCamera.transform.GetChild(0).rotation;
		}
		if (spawnTimer > 0) {
			spawnTimer -= Time.deltaTime;
			return;
		}
		Vector3 playerPos = player.transform.position;
		if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(playerPos.x, playerPos.z)) < 0.8f) {
			Destroy (transform.parent.gameObject);
			playerInventoryScript.addObjectToPlayerInventory (myValue.quantity, myValue.name, myValue.displayName, true, transform.position);
		}
	}
}
