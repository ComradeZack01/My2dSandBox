using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveFourLegs : MonoBehaviour {
	public float walkSpeed;
	public float runSpeed;
	public float maxHealth;
	public float health;
	public string mobName;

	float currentSpeed = 3;

	GameObject rightFrontLeg;
	GameObject leftFrontLeg;
	GameObject rightBackLeg;
	GameObject leftBackLeg;
	GameObject player;

	public bool moving;
	float moveCountdown = 6;
	float pauseMoveCountdown = 2;

	public float hostileCooldown;

	//use for knockback
	float knockbackDelay = 0;
	float deathDelay = 1.5f;

	//only when attacked by mob or player
	bool attacked;
	float attackedCountdown;

	public PlayerInventoryScript playerInventoryScript;

	public bool swingForward;

	void Start () {
		player = GameObject.Find ("Player");
		playerInventoryScript = player.GetComponent<PlayerInventoryScript> ();
		rightFrontLeg = transform.GetChild (1).gameObject;
		leftFrontLeg = transform.GetChild (2).gameObject;
		rightBackLeg = transform.GetChild (3).gameObject;
		leftBackLeg = transform.GetChild (0).gameObject;
	}
	Quaternion originalRotation;
	public void takeDamage (float damage) {
		attacked = true;
		attackedCountdown = hostileCooldown;
		originalRotation = transform.rotation;
		health -= damage;
		if (health > 0) {
			knockbackDelay = 0.15f;
		}
	}
	void FixedUpdate () {
		customUpdate (); 
	}

	public void customUpdate () {
		if (health <= 0) {
			GetComponent<Rigidbody> ().useGravity = false;
			foreach (BoxCollider i in GetComponents<BoxCollider>()) {
				i.enabled = false;
			}
			transform.Rotate (new Vector3(0, 0, 60) * Time.deltaTime, Space.Self);
			deathDelay -= Time.deltaTime;
			if (deathDelay <= 0) {
				//loot drop
				for (int i = 0; i < Random.Range(2, 4); i++) {
					GameObject insItem = Instantiate (playerInventoryScript.droppedItemPrefab, new Vector3 (Random.Range (transform.position.x - 1, transform.position.x + 1), -0.7f, Random.Range (transform.position.z - 1, transform.position.z + 1)), playerInventoryScript.droppedItemPrefab.transform.rotation) as GameObject;
					insItem.transform.GetChild (0).GetComponent<DroppedItemScript> ().myValue = new InventoryItem (1, "meat", "Meat");
					playerInventoryScript.addDroppedItemToSaves (insItem);
				}
				Destroy (gameObject);
			}
		} else if (knockbackDelay > 0) {
			knockbackDelay -= Time.deltaTime;
			transform.LookAt (new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
			transform.Translate (new Vector3(0, 0.4f, -1) * Time.deltaTime * 10, Space.Self);
			transform.rotation = originalRotation;
		} else if (Vector3.Distance (transform.position, player.transform.position) < 40 && health > 0) {
			if (!attacked) {
				if (moveCountdown > 0) {
					moving = true;
					moveCountdown -= Time.deltaTime;
					transform.Translate (Vector3.forward * Time.deltaTime * currentSpeed);
				} else if (pauseMoveCountdown > 0) {
					moving = false;
					pauseMoveCountdown -= Time.deltaTime;
				} else {
					pauseMoveCountdown = Random.Range (1f, 4.3f);
					moveCountdown = Random.Range (2, 15f);
					transform.Rotate (new Vector3 (0, Random.Range (0, 360), 0), Space.World);
				}
				currentSpeed = walkSpeed;
			} else {
				attackedCountdown -= Time.deltaTime;
				if (attackedCountdown < 0) {
					attacked = false;
				}
				currentSpeed = runSpeed;
				moving = true;
				transform.LookAt (new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
				transform.Rotate (new Vector3(0, 180, 0));
				transform.Translate (Vector3.forward * Time.deltaTime * currentSpeed);
			}

			if (moving) {
				if (rightFrontLeg.transform.eulerAngles.x > 30 && rightFrontLeg.transform.eulerAngles.x < 100 && swingForward)
					swingForward = false;
				if (((rightFrontLeg.transform.eulerAngles.x < 330 && rightFrontLeg.transform.eulerAngles.x > 200) || rightFrontLeg.transform.eulerAngles.x < -30) && !swingForward)
					swingForward = true;
				if (swingForward) {
					rightFrontLeg.transform.Rotate (new Vector3 (Time.deltaTime * 45 * currentSpeed / walkSpeed, 0, 0), Space.Self);
					leftFrontLeg.transform.Rotate (new Vector3 (Time.deltaTime * -45 * currentSpeed / walkSpeed, 0, 0), Space.Self);
					rightBackLeg.transform.Rotate (new Vector3 (Time.deltaTime * 45 * currentSpeed / walkSpeed, 0, 0), Space.Self);
					leftBackLeg.transform.Rotate (new Vector3 (Time.deltaTime * -45 * currentSpeed / walkSpeed, 0, 0), Space.Self);
				} else {
					rightFrontLeg.transform.Rotate (new Vector3 (Time.deltaTime * -45 * currentSpeed / walkSpeed, 0, 0), Space.Self);
					leftFrontLeg.transform.Rotate (new Vector3 (Time.deltaTime * 45 * currentSpeed / walkSpeed, 0, 0), Space.Self);
					rightBackLeg.transform.Rotate (new Vector3 (Time.deltaTime * -45 * currentSpeed / walkSpeed, 0, 0), Space.Self);
					leftBackLeg.transform.Rotate (new Vector3 (Time.deltaTime * 45 * currentSpeed / walkSpeed, 0, 0), Space.Self);
				}
			} else {
				if (rightFrontLeg.transform.eulerAngles.x > 3 && rightFrontLeg.transform.eulerAngles.x < 100) {
					rightFrontLeg.transform.Rotate (new Vector3 (Time.deltaTime * -45 * currentSpeed / walkSpeed, 0, 0), Space.Self);
					leftFrontLeg.transform.Rotate (new Vector3 (Time.deltaTime * 45 * currentSpeed / walkSpeed, 0, 0), Space.Self);
					rightBackLeg.transform.Rotate (new Vector3 (Time.deltaTime * -45 * currentSpeed / walkSpeed, 0, 0), Space.Self);
					leftBackLeg.transform.Rotate (new Vector3 (Time.deltaTime * 45 * currentSpeed / walkSpeed, 0, 0), Space.Self);
				} else if ((rightFrontLeg.transform.eulerAngles.x < 355 && rightFrontLeg.transform.eulerAngles.x > 200) || rightFrontLeg.transform.eulerAngles.x < -5) {
					rightFrontLeg.transform.Rotate (new Vector3 (Time.deltaTime * 45 * currentSpeed / walkSpeed, 0, 0), Space.Self);
					leftFrontLeg.transform.Rotate (new Vector3 (Time.deltaTime * -45 * currentSpeed / walkSpeed, 0, 0), Space.Self);
					rightBackLeg.transform.Rotate (new Vector3 (Time.deltaTime * 45 * currentSpeed / walkSpeed, 0, 0), Space.Self);
					leftBackLeg.transform.Rotate (new Vector3 (Time.deltaTime * -45 * currentSpeed / walkSpeed, 0, 0), Space.Self);
				}
			}
		}
	}
}
