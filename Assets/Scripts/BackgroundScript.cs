using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScript : MonoBehaviour {
	public Sprite[] images;
	void Start () {
		GetComponent<Image>().sprite = images[Random.Range(0, images.Length)];
	}
}
