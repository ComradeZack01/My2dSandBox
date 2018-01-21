using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolScript : MonoBehaviour {
	public bool doDamage;
	public string toolName;
	PlayerScript playerScript;
	PlayerInventoryScript playerInventoryScript;
	void Start () {
		playerScript = GameObject.Find ("Player").GetComponent<PlayerScript> ();
		playerInventoryScript = GameObject.Find ("Player").GetComponent<PlayerInventoryScript> ();
	}
	void Update () {
		if (doDamage && playerScript.working && playerScript.equipTool) {
			playerScript.transform.Translate (Vector3.forward);
			foreach (Collider coll in Physics.OverlapBox(playerScript.transform.position, new Vector3(0.5f, 0.5f, 0.5f))) {
				bool hit = true;
				if (coll.GetComponent<MobScript> () != null) {
					coll.GetComponent<MobScript> ().takeDamage (playerInventoryScript.findToolWithName (toolName).damage);
				} else if (toolName == "flintAxe") {
					if (coll.GetComponent<TreeScript> () != null) {
						coll.GetComponent<TreeScript> ().loseHealth (1);
					} else {
						hit = false;
					}
				} else if (toolName == "flintPickaxe") {
					if (coll.GetComponent<BoulderScript> () != null) {
						coll.GetComponent<BoulderScript> ().loseHealth (1);
					} else {
						hit = false;
					}
				} else {
					hit = false;
				}
				doDamage = false;
				if (hit) {
					playerInventoryScript.playerInventory [playerInventoryScript.playerInventory.Length - 1].quantity -= (int)playerInventoryScript.findToolWithName (toolName).useDamage;
					playerInventoryScript.updateAllSprites ();
				}
			}
			playerScript.transform.Translate (Vector3.back);
			doDamage = false;
		}
	}
}
