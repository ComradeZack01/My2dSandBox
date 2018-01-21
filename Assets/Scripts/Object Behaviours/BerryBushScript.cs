using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryBushScript : MonoBehaviour {
	public bool cut;
	public float growTimer;
	public float growTimerCooldown;
	PlayerInventoryScript playerInventoryScript;

	//update by interval to reduce lag
	float intervalCountdown = 1;
	bool far = false;

	GameObject player;

	void Start () {
		player = GameObject.Find ("Player");
		playerInventoryScript = GameObject.Find ("Player").GetComponent<PlayerInventoryScript>();
		if (cut) {
			for (int i = 0; i <= 26; i++) {
				transform.GetChild (i).GetComponent<Renderer> ().enabled = false;
			}
		}
	}
	public void harvestItem () {
		if (!cut) {
			cut = true;
			for (int i = 0; i < 26; i++) {
				transform.GetChild (i).GetComponent<Renderer> ().enabled = false;
			}
			growTimer = growTimerCooldown;
			playerInventoryScript.addObjectToPlayerInventory(1, "berries", "Berries", false, Vector3.zero);
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
					for (int i = 0; i < 26; i++) {
						transform.GetChild (i).GetComponent<Renderer> ().enabled = true;
					}
				}
			}
			if (!far && Vector3.Distance (transform.position, player.transform.position) > playerInventoryScript.masterControllerScript.changeRendererRange) {
				far = true;
				foreach (Renderer i in transform.GetComponentsInChildren<Renderer>()) {
					i.enabled = false;
				}
			} else if (far && Vector3.Distance (transform.position, player.transform.position) <= playerInventoryScript.masterControllerScript.changeRendererRange) {
				far = false;
				if (!cut) {
					foreach (Renderer i in transform.GetComponentsInChildren<Renderer>()) {
						i.enabled = true;
					}
				}
			}
		}
	}
}
