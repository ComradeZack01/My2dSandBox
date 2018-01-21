using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushScript : MonoBehaviour {
	public bool cut;
	public float growTimer;
	public float growTimerCooldown;

	//update by interval to reduce lag
	float intervalCountdown = 1;
	bool far = false;
	PlayerInventoryScript playerInventoryScript;
	public GameObject player;

	void Start () {
		player = GameObject.Find ("Player");
		playerInventoryScript = GameObject.Find ("Player").GetComponent<PlayerInventoryScript>();
		if (cut) {
			for (int i = 1; i <= 9; i++) {
				transform.GetChild (i).GetComponent<Renderer> ().enabled = false;
			}
		}
	}
	public void harvestItem () {
		if (!cut) {
			cut = true;
			transform.Translate (Vector3.down * 0.9f, Space.Self);
			for (int i = 1; i <= 9; i++) {
				transform.GetChild (i).GetComponent<Renderer> ().enabled = false;
			}
			growTimer = growTimerCooldown;
			//give loot
			playerInventoryScript.addObjectToPlayerInventory(1, "sticks", "Sticks", false, Vector3.zero);
		}
	}
	void Update () {
		//disable children renderers when far away
		intervalCountdown -= Time.deltaTime;
		if (intervalCountdown <= 0) {
			intervalCountdown = 1;
			if (cut) {
				growTimer--;
				if (growTimer <= 0) {
					cut = false;
					transform.Translate (Vector3.up * 0.9f, Space.Self);
					for (int i = 1; i <= 9; i++) {
						transform.GetChild (i).GetComponent<Renderer> ().enabled = true;
					}
				}
			}
			if (!far && Vector3.Distance (transform.position, player.transform.position) > playerInventoryScript.masterControllerScript.changeRendererRange) {
				far = true;
				for (int i = 0; i <= 9; i++) {
					transform.GetChild (i).GetComponent<Renderer> ().enabled = false;
				}
			} else if (far && Vector3.Distance (transform.position, player.transform.position) <= playerInventoryScript.masterControllerScript.changeRendererRange) {
				far = false;
				if (!cut) {
					for (int i = 0; i <= 9; i++) {
						transform.GetChild (i).GetComponent<Renderer> ().enabled = true;
					}
				}
			}
		}
	}
}
