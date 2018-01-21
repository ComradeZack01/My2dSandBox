using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour {
	public float fuel;
	public float maxFuel;

	void Start () {
		
	}
	void Update () {
		if (fuel > maxFuel)
			fuel = maxFuel;
		if (fuel > 0) {
			fuel -= Time.deltaTime;
		} else 
			Destroy (gameObject);
		if (!GetComponent<ChangeRendererScript> ().far) {
			if (fuel < maxFuel * 0.1f) {
				transform.GetChild (4).GetComponent<Light> ().range = 6.2f;
				if (!transform.GetChild (3).GetComponent<ParticleSystemRenderer> ().enabled)
					transform.GetChild (3).GetComponent<ParticleSystemRenderer> ().enabled = true;
				if (transform.GetChild (2).GetComponent<ParticleSystemRenderer> ().enabled)
					transform.GetChild (2).GetComponent<ParticleSystemRenderer> ().enabled = false;
				if (transform.GetChild (1).GetComponent<ParticleSystemRenderer> ().enabled)
					transform.GetChild (1).GetComponent<ParticleSystemRenderer> ().enabled = false;
				if (transform.GetChild (0).GetComponent<ParticleSystemRenderer> ().enabled)
					transform.GetChild (0).GetComponent<ParticleSystemRenderer> ().enabled = false;
			} else if (fuel < maxFuel * 0.4f) {
				transform.GetChild (4).GetComponent<Light> ().range = 8;
				if (transform.GetChild (3).GetComponent<ParticleSystemRenderer> ().enabled)
					transform.GetChild (3).GetComponent<ParticleSystemRenderer> ().enabled = false;
				if (!transform.GetChild (2).GetComponent<ParticleSystemRenderer> ().enabled)
					transform.GetChild (2).GetComponent<ParticleSystemRenderer> ().enabled = true;
				if (transform.GetChild (1).GetComponent<ParticleSystemRenderer> ().enabled)
					transform.GetChild (1).GetComponent<ParticleSystemRenderer> ().enabled = false;
				if (transform.GetChild (0).GetComponent<ParticleSystemRenderer> ().enabled)
					transform.GetChild (0).GetComponent<ParticleSystemRenderer> ().enabled = false;
			} else if (fuel < maxFuel * 0.7f) {
				transform.GetChild (4).GetComponent<Light> ().range = 13;
				if (transform.GetChild (3).GetComponent<ParticleSystemRenderer> ().enabled)
					transform.GetChild (3).GetComponent<ParticleSystemRenderer> ().enabled = false;
				if (transform.GetChild (2).GetComponent<ParticleSystemRenderer> ().enabled)
					transform.GetChild (2).GetComponent<ParticleSystemRenderer> ().enabled = false;
				if (!transform.GetChild (1).GetComponent<ParticleSystemRenderer> ().enabled)
					transform.GetChild (1).GetComponent<ParticleSystemRenderer> ().enabled = true;
				if (transform.GetChild (0).GetComponent<ParticleSystemRenderer> ().enabled)
					transform.GetChild (0).GetComponent<ParticleSystemRenderer> ().enabled = false;
			} else {
				transform.GetChild (4).GetComponent<Light> ().range = 18;
				if (transform.GetChild (3).GetComponent<ParticleSystemRenderer> ().enabled)
					transform.GetChild (3).GetComponent<ParticleSystemRenderer> ().enabled = false;
				if (transform.GetChild (2).GetComponent<ParticleSystemRenderer> ().enabled)
					transform.GetChild (2).GetComponent<ParticleSystemRenderer> ().enabled = false;
				if (transform.GetChild (1).GetComponent<ParticleSystemRenderer> ().enabled)
					transform.GetChild (1).GetComponent<ParticleSystemRenderer> ().enabled = false;
				if (!transform.GetChild (0).GetComponent<ParticleSystemRenderer> ().enabled)
					transform.GetChild (0).GetComponent<ParticleSystemRenderer> ().enabled = true;
			}
		}
	}
}
