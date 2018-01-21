using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBallScript : MonoBehaviour {
	public ControllerScript masterControllerScript;
	public GameObject player;
	RectTransform parentRectTransform;
	float ballScrollRadius;
	void Start () {
		masterControllerScript = GameObject.Find ("EventSystem").GetComponent<ControllerScript> ();
		parentRectTransform = transform.parent.GetComponent<RectTransform> ();
		ballScrollRadius = parentRectTransform.rect.width / 2;
	}

	void Update () {
		//determine if finger inside area
		bool touchContainsUI = false;
		foreach (Touch t in Input.touches) {
			foreach (Rect rect in masterControllerScript.onScreenUI) {
				if (rect.Contains (t.position)) {
					touchContainsUI = true;
					break;
				}
			}
		}


		if (Input.GetMouseButton (0) && !touchContainsUI && !player.GetComponent<PlayerScript>().sleeping) {
			transform.position = Input.mousePosition; 
			if (!masterControllerScript.touchContainsUI && !masterControllerScript.UIBusy && !player.GetComponent<PlayerScript>().working) {
				//changes player rotation
				Quaternion rotation = Quaternion.LookRotation (transform.position - transform.parent.position, transform.parent.TransformDirection (Vector3.up));
				player.transform.rotation = new Quaternion (0, -rotation.z, 0, rotation.w);
				player.transform.Rotate (0, 90 + player.GetComponent<PlayerScript>().mainCamera.transform.eulerAngles.y, 0);

				if (Input.mousePosition.x < transform.parent.position.x) {
					player.transform.Rotate (0, 180, 0);
				}
				//move player
				float moveDistance = Vector3.Distance (Input.mousePosition, transform.parent.position);
				if (moveDistance > 250) {
					moveDistance = 250;
				}
				player.transform.Translate (Vector3.forward * moveDistance / 80 * Time.deltaTime);
				player.GetComponent<PlayerScript> ().moving = true;
				player.GetComponent<PlayerScript> ().moveSpeed = moveDistance / 90;
				player.GetComponent<PlayerScript> ().updateCamera ();
			}
		} else {
			//stops player movement and reverts UI position
			transform.position = transform.parent.position;
			player.GetComponent<PlayerScript> ().moving = false;
		}
	}
}
