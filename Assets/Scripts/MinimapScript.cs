using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour {
	public GameObject player;

	void Start () {
		player = GameObject.Find ("Player");
	}
	void LateUpdate () {
		transform.position = new Vector3 (player.transform.position.x, 12, player.transform.position.z);
	}
}
