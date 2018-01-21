using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassScript : MonoBehaviour {
	public bool cut;
	public float growTimer;
	public float growTimerCooldown;
	PlayerInventoryScript playerInventoryScript;

	//update by interval to reduce lag
	float intervalCountdown = 1;
	GameObject player;
	bool far = false;
	void Start () {
		player = GameObject.Find ("Player");
		playerInventoryScript = GameObject.Find ("Player").GetComponent<PlayerInventoryScript>();
		for (int i = 0; i < 16; i++) {
			transform.GetChild(i).position = new Vector3 (transform.GetChild(i).position.x + Random.Range(0f, 0.2f), transform.GetChild(i).position.y + Random.Range(0.6f, 1.1f), transform.GetChild(i).position.z + Random.Range(0f, 0.2f));
			float scale = Random.Range (1f, 1.6f);
			transform.GetChild (i).localScale = new Vector3 (transform.GetChild (i).localScale.x * scale, transform.GetChild (i).localScale.y, transform.GetChild (i).localScale.z * scale);
			transform.GetChild (i).eulerAngles = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
		}
	}
	public void harvestItem () {
		if (!cut) {
			cut = true;
			transform.Translate (Vector3.down * 0.9f, Space.Self);
			growTimer = growTimerCooldown;
			playerInventoryScript.addObjectToPlayerInventory(1, "grass", "Grass", false, Vector3.zero);
		}
	}
	void Update () {
		intervalCountdown -= Time.deltaTime;
		if (intervalCountdown <= 0) {
			intervalCountdown = 1;
			if (cut) {
				growTimer--;
				if (growTimer <= 0) {
					cut = false;
					transform.Translate (Vector3.up * 0.9f, Space.Self);
				}
			}
		}
	}
}
