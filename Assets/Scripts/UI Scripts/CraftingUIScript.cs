using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingUIScript : MonoBehaviour {
	public bool isCrafting;
	Vector3 originalPos;

	void Start () {
		originalPos = transform.position;
	}
	public void swapPos () {
		isCrafting = !isCrafting;
	}
	void Update () {
		if (isCrafting && transform.position.x < originalPos.x * 9f) {
			transform.parent.Translate (Vector3.right * Time.deltaTime * 300);
			if (Mathf.Abs (transform.position.x - originalPos.x * 9f) < Time.deltaTime * 330)
				transform.parent.position = new Vector3 (originalPos.x * 9f - (transform.position.x - transform.parent.position.x), transform.position.y, transform.position.z);
		} else if (!isCrafting && transform.position.x > originalPos.x) {
			transform.parent.Translate (Vector3.left * Time.deltaTime * 300);
			if (Mathf.Abs (transform.position.x - originalPos.x) < Time.deltaTime * 330)
				transform.parent.position = new Vector3 (originalPos.x - (transform.position.x - transform.parent.position.x), transform.position.y, transform.position.z);
		}
	}
}
