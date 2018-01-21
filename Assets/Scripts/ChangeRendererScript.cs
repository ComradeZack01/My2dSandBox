using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeRendererScript : MonoBehaviour {

	//checking interval to reduce lag; set
	public float intervalCountdown = 1;
	public bool far = false;
	GameObject player;
	void Start () {
		player = GameObject.Find ("Player");
	}
	void Update () {
		intervalCountdown -= Time.deltaTime;
		if (intervalCountdown <= 0) {
			intervalCountdown = 1;
			if (!far && Vector3.Distance (transform.position, player.transform.position) > player.GetComponent<PlayerInventoryScript> ().masterControllerScript.changeRendererRange) {
				far = true;
				foreach (Renderer i in GetComponentsInChildren<Renderer>()) {
					i.enabled = false;
				}
				if (GetComponent<Renderer> () != null) 
					GetComponent<Renderer> ().enabled = false;
				if (GetComponent<Image> () != null)
					GetComponent<Image> ().enabled = false;
				if (GetComponent<ParticleSystem> () != null)
					GetComponent<ParticleSystem> ().Stop (true);
			} else if (far && Vector3.Distance (transform.position, player.transform.position) <= player.GetComponent<PlayerInventoryScript> ().masterControllerScript.changeRendererRange) {
				far = false;
				foreach (Renderer i in GetComponentsInChildren<Renderer>()) {
					i.enabled = true;
				}
				if (GetComponent<Renderer> () != null) 
					GetComponent<Renderer> ().enabled = true;
				if (GetComponent<Image> () != null)
					GetComponent<Image> ().enabled = true;
				if (GetComponent<ParticleSystem> () != null)
					GetComponent<ParticleSystem> ().Play(true);
			}
		}
	}
}
