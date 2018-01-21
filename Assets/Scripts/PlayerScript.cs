using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
	//stats
	public float maxHealth;
	public float maxHunger;
	public float maxEnergy;
	public float health;
	public float hunger;
	public float energy;

	//implement later
	public float happiness;
	public float maxHappiness;

	public GameObject hungerBar;
	public GameObject healthBar;
	public GameObject energyBar;

	public float rightArmRotationEuler;
	public Sprite buildingSprite;
	public Sprite workingSprite;
	public GameObject workButton;
	public ControllerScript masterControllerScript;

	public GameObject constructionHoverPrefab;

	GameObject constructionHover;
	bool canBuildBuilding = false;

	//determines move (arms and legs swing) animation
	public bool moving;

	bool prevSwingRight = false;

	//determine tool usage
	bool prevWorking;
	public bool working; 
	public bool equipTool;

	//determined in ScrollBallScript
	public float moveSpeed;

	//body parts
	public GameObject rightArm;
	public GameObject leftArm;
	public GameObject rightLeg;
	public GameObject leftLeg;

	//tools
	public GameObject axePrefab;
	public GameObject pickaxePrefab;

	//for swinging animation when moving
	 bool swingRightMove;
	bool rightArmStop;
	 bool swingRightWork;

	public GameObject mainCamera;

	//fps counter
	public float framesPerSecond;
	float checkInterval = 1;

	public bool sleeping;

	Vector3 prevPos = Vector3.zero;

	void Start () {
		masterControllerScript = GameObject.Find ("EventSystem").GetComponent<ControllerScript> ();

		updateCamera ();
	}
	public void updateConstructionHover () {
		if (constructionHover != null) {
			constructionHover.transform.position = transform.position;
			constructionHover.transform.rotation = transform.rotation;
			constructionHover.transform.Translate (Vector3.forward * 1.5f, Space.Self);
			if (Physics.CheckBox (constructionHover.transform.position, new Vector3 (constructionHover.transform.localScale.x / 2 + 0.2f, 2, constructionHover.transform.localScale.z / 2 + 0.2f), constructionHover.transform.rotation, 9)) {
				canBuildBuilding = false;
			} else {
				canBuildBuilding = true;
			}
			if (canBuildBuilding) {
				constructionHover.GetComponent<MeshRenderer> ().material.color = new Color (0, 0.8f, 0);
			} else
				constructionHover.GetComponent<MeshRenderer> ().material.color = new Color (0.9f, 0, 0);
			workButton.GetComponent<Image> ().sprite = buildingSprite;
		} else {
			workButton.GetComponent<Image> ().sprite = workingSprite;
		}
	}
	public void changeEquipTool (bool equipped, string toolName) {
		if (rightArm.transform.GetChild (0).childCount > 0) {
			Destroy (rightArm.transform.GetChild (0).GetChild(0).gameObject);
		}
		equipTool = equipped;
		if (equipTool) {
			if (toolName == "flintAxe") { //flint axe
				GameObject insItem = Instantiate (axePrefab, rightArm.transform.GetChild(0).position, rightArm.transform.rotation, rightArm.transform.GetChild(0));
				insItem.transform.Rotate (new Vector3 (90, 0, 0), Space.Self);
			} else if (toolName == "flintPickaxe") { //flint pickaxe
				GameObject insItem = Instantiate (pickaxePrefab, rightArm.transform.GetChild(0).position, rightArm.transform.rotation, rightArm.transform.GetChild(0));
				insItem.transform.Rotate (new Vector3 (90, 0, 0), Space.Self);
			}
		}
	}
	public void updateCamera () {
		//keep camera focused on player
		mainCamera.transform.position = transform.position;

		//update construction hover
		updateConstructionHover();
	}
	Text sleepText;
	public void toggleSleep (Text changeText) {
		sleepText = changeText;
		if (!sleeping && energy < maxEnergy / 2) {
			moving = false;
			sleeping = true;
			changeText.text = "Wake Up";
			transform.Rotate (new Vector3(270, 0, 0));
		} else if (sleeping) {
			sleeping = false;
			changeText.text = "Sleep";
			transform.Rotate (new Vector3(-270, 0, 0));
		}
	}
	private bool firstFrame = true;
	void Update () {
		if (firstFrame) {
			firstFrame = false;
			while (true) {
				bool hasThing = false;
				Collider[] colls = Physics.OverlapBox (transform.position, new Vector3 (1, 2, 1)); 
				foreach (Collider i in colls) {
					if (i.gameObject.GetComponent<TerrainScript> () != null && i.gameObject.GetComponent<TerrainScript>().terrainName == "water") {
						hasThing = true;
						break;
					}
				}
				if (!hasThing) {
					break;
				} else {
					transform.Translate (1, 0, 0);
				}
			}
		}
		if (sleeping) {
			if (Time.timeScale != 10) {
				Time.timeScale = 10;
			}
			energy += Time.deltaTime / 2;
			if (energy >= maxEnergy - 1) {
				toggleSleep (sleepText);
			}
		} else if (Time.timeScale != 1) {
			Time.timeScale = 1;
		}
		//lose hunger, update hunger bar
		if (hunger > maxHunger)
			hunger = maxHunger;
		if (health > maxHealth)
			health = maxHealth;
		if (happiness > maxHappiness)
			happiness = maxHappiness;
		if (energy > maxEnergy)
			energy = maxEnergy;
		
		if (hunger > 0) {
			if (working) {
				hunger -= Time.deltaTime / 2.5f;
			} else
				hunger -= Time.deltaTime / 6f;
		} else if (health > 0) {
			health -= Time.deltaTime * 2;
		} else {
			//die
		}
		if (energy > 0) {
			if (working) {
				energy -= Time.deltaTime / 1.5f;
			} else
				energy -= Time.deltaTime / 4;
		} else if (health > 0) {
			health -= Time.deltaTime / 3f;
		} else {
			//die
		}
			
		hungerBar.transform.localScale = new Vector3 (hungerBar.transform.localScale.x, hungerBar.transform.localScale.x * (hunger / maxHunger), hungerBar.transform.localScale.z);
		healthBar.transform.localScale = new Vector3 (healthBar.transform.localScale.x, healthBar.transform.localScale.x * (health / maxHealth), healthBar.transform.localScale.z);
		energyBar.transform.localScale = new Vector3 (energyBar.transform.localScale.x, energyBar.transform.localScale.x * (energy / maxEnergy), energyBar.transform.localScale.z);

		//check tools
		PlayerInventoryScript s = GetComponent<PlayerInventoryScript> ();

		rightArmRotationEuler = rightArm.transform.eulerAngles.x;
		if (prevPos != transform.position) {
			updateCamera ();
			prevPos = transform.position;
		}
		if (!equipTool && s.determineWhetherItemIsToolWithName(s.playerInventory[s.playerInventory.Length - 1].name) && s.playerInventory[s.playerInventory.Length - 1].quantity > 0) {
			changeEquipTool (true, s.playerInventory[s.playerInventory.Length - 1].name);
		} else if (equipTool && (!s.determineWhetherItemIsToolWithName(s.playerInventory[s.playerInventory.Length - 1].name) || s.playerInventory[s.playerInventory.Length - 1].quantity <= 0)) {
			changeEquipTool (false, s.playerInventory[s.playerInventory.Length - 1].name);
		}
		//construction
		if (s.determineWhetherItemIsBuildingWithName (s.playerInventory [s.playerInventory.Length - 1].name) && constructionHover == null) {
			GameObject buildPrefab = s.findBuildingPrefabWithName (s.playerInventory [s.playerInventory.Length - 1].name);
			GameObject insItem = Instantiate(constructionHoverPrefab, new Vector3(transform.position.x, -0.5f, transform.position.z), transform.rotation);
			constructionHover = insItem;
			constructionHover.transform.localScale = new Vector3(buildPrefab.GetComponent<BoxCollider>().size.x, 1, buildPrefab.GetComponent<BoxCollider>().size.z);
			updateConstructionHover ();
		}
		if (!s.determineWhetherItemIsBuildingWithName (s.playerInventory [s.playerInventory.Length - 1].name) || s.playerInventory [s.playerInventory.Length - 1].quantity <= 0) {
			if (constructionHover != null) {
				Destroy (constructionHover);
				constructionHover = null;
				updateConstructionHover ();
			}
		}
		//builds the building here; refer back when creating save method
		if (working && canBuildBuilding && constructionHover != null) {
			GameObject insItem = Instantiate (s.findBuildingPrefabWithName(s.playerInventory [s.playerInventory.Length - 1].name), new Vector3(constructionHover.transform.position.x, s.findBuildingPrefabWithName(s.playerInventory [s.playerInventory.Length - 1].name).transform.position.y, constructionHover.transform.position.z), constructionHover.transform.rotation);
			GetComponent<PlayerInventoryScript> ().addGameObjectToSaves (insItem);
			s.playerInventory [s.playerInventory.Length - 1].quantity--;
			s.updateAllSprites ();
		}
		Rect rect = new Rect (new Vector2(workButton.GetComponent<RectTransform> ().position.x - workButton.GetComponent<RectTransform> ().rect.size.x / 2, workButton.GetComponent<RectTransform> ().position.y - workButton.GetComponent<RectTransform> ().rect.size.y / 2), workButton.GetComponent<RectTransform> ().rect.size);
		if (rect.Contains (Input.mousePosition) && Input.GetMouseButton(0) || Input.GetKey(KeyCode.G)) {
			working = true;
		} else
			working = false;
		
		//check fps
		checkInterval -= Time.deltaTime;
		if (checkInterval <= 0) {
			checkInterval = 1;
			framesPerSecond = 1 / Time.deltaTime;
		}

		//determine swing animation
		if (!sleeping) {
			if (prevWorking && !working) {
				rightArmStop = true;
			}
			if (rightArmStop && (leftArm.transform.eulerAngles.x > 350 + framesPerSecond / 10 || leftArm.transform.eulerAngles.x < 10 - framesPerSecond / 10) && (rightArm.transform.eulerAngles.x > 350 + framesPerSecond / 10 || rightArm.transform.eulerAngles.x < 10 - framesPerSecond / 10)) {
				rightArmStop = false;
			}
			if (working) {
				moving = false;
				if (swingRightWork) { 
					rightArm.transform.Rotate (new Vector3 (-Time.deltaTime * 180, 0, 0), Space.Self);
				} else {
					rightArm.transform.Rotate (new Vector3 (Time.deltaTime * 360, 0, 0), Space.Self);
				}
			}
			if (!working && (equipTool || !moving || masterControllerScript.touchContainsUI || masterControllerScript.UIBusy || rightArmStop)) {
				if (rightArm.transform.eulerAngles.x < 350 + (framesPerSecond / 10) && rightArm.transform.eulerAngles.x > 200)
					rightArm.transform.Rotate (new Vector3 (Time.deltaTime * 170, 0, 0), Space.Self);
				if (rightArm.transform.eulerAngles.x > 10 - (framesPerSecond / 10) && rightArm.transform.eulerAngles.x < 100)
					rightArm.transform.Rotate (new Vector3 (-Time.deltaTime * 170, 0, 0), Space.Self);
			}
			if (moving && !masterControllerScript.touchContainsUI && !masterControllerScript.UIBusy) {
				if (swingRightMove) {
					if (!equipTool && !rightArmStop)
						rightArm.transform.Rotate (new Vector3 (-Time.deltaTime * 54 * moveSpeed, 0, 0), Space.Self);
					leftArm.transform.Rotate (new Vector3 (Time.deltaTime * 54 * moveSpeed, 0, 0), Space.Self);
					rightLeg.transform.Rotate (new Vector3 (Time.deltaTime * 54 * moveSpeed, 0, 0), Space.Self);
					leftLeg.transform.Rotate (new Vector3 (-Time.deltaTime * 54 * moveSpeed, 0, 0), Space.Self);
				} else {
					if (!equipTool && !rightArmStop)
						rightArm.transform.Rotate (new Vector3 (Time.deltaTime * 54 * moveSpeed, 0, 0), Space.Self);
					leftArm.transform.Rotate (new Vector3 (-Time.deltaTime * 54 * moveSpeed, 0, 0), Space.Self);
					rightLeg.transform.Rotate (new Vector3 (-Time.deltaTime * 54 * moveSpeed, 0, 0), Space.Self);
					leftLeg.transform.Rotate (new Vector3 (Time.deltaTime * 54 * moveSpeed, 0, 0), Space.Self);
				}
			} else {
				if (leftLeg.transform.eulerAngles.x < 350 + (framesPerSecond / 10) && leftLeg.transform.eulerAngles.x > 200) {
					if (!working && !equipTool && !rightArmStop)
						rightArm.transform.Rotate (new Vector3 (Time.deltaTime * 54 * moveSpeed, 0, 0), Space.Self);
					leftArm.transform.Rotate (new Vector3 (-Time.deltaTime * 54 * moveSpeed, 0, 0), Space.Self);
					rightLeg.transform.Rotate (new Vector3 (-Time.deltaTime * 54 * moveSpeed, 0, 0), Space.Self);
					leftLeg.transform.Rotate (new Vector3 (Time.deltaTime * 54 * moveSpeed, 0, 0), Space.Self);
				} else if (leftLeg.transform.eulerAngles.x > 10 - (framesPerSecond / 10) && leftLeg.transform.eulerAngles.x < 100) {
					if (!working && !equipTool && !rightArmStop)
						rightArm.transform.Rotate (new Vector3 (-Time.deltaTime * 54 * moveSpeed, 0, 0), Space.Self);
					leftArm.transform.Rotate (new Vector3 (Time.deltaTime * 54 * moveSpeed, 0, 0), Space.Self);
					rightLeg.transform.Rotate (new Vector3 (Time.deltaTime * 54 * moveSpeed, 0, 0), Space.Self);
					leftLeg.transform.Rotate (new Vector3 (-Time.deltaTime * 54 * moveSpeed, 0, 0), Space.Self);
				}
			}

			//determine swing direction
			if (leftLeg.transform.eulerAngles.x < 330 && leftLeg.transform.eulerAngles.x > 200 && swingRightMove)
				swingRightMove = false;
			if (leftLeg.transform.eulerAngles.x > 30 && leftLeg.transform.eulerAngles.x < 100 && !swingRightMove)
				swingRightMove = true;
			if (rightArm.transform.eulerAngles.x < 330 && rightArm.transform.eulerAngles.x > 200 && swingRightMove)
				swingRightMove = false;
			if (rightArm.transform.eulerAngles.x > 30 && rightArm.transform.eulerAngles.x < 100 && !swingRightMove)
				swingRightMove = true;
			if (working) {
				if (rightArm.transform.eulerAngles.x < 290 && rightArm.transform.eulerAngles.x > 250 && swingRightWork) {
					swingRightWork = false;
					if (prevSwingRight) {
						if (equipTool) {
							rightArm.transform.GetChild (0).GetChild (0).GetComponent<ToolScript> ().doDamage = true;
						} 
						transform.Translate (Vector3.forward, Space.Self);
						Collider[] coll = Physics.OverlapSphere (transform.position, 0.8f);
						transform.Translate (Vector3.back, Space.Self);
						if (coll.Length > 0) {
							foreach (Collider i in coll) {
								if (i.GetComponent<GrassScript> () != null) {
									i.GetComponent<GrassScript> ().harvestItem ();
								} else if (i.GetComponent<BushScript> () != null) {
									i.GetComponent<BushScript> ().harvestItem ();
								} else if (i.GetComponent<BerryBushScript> () != null) {
									i.GetComponent<BerryBushScript> ().harvestItem ();
								} else if (i.GetComponent<GroundFoodScript> () != null) {
									i.GetComponent<GroundFoodScript> ().harvestItem ();
								}
							}
						}
					}
					prevSwingRight = false;
				}
				if (rightArm.transform.eulerAngles.x > 30 && rightArm.transform.eulerAngles.x < 100 && !swingRightWork) {
					swingRightWork = true;
					prevSwingRight = true;
				}
			}
			prevWorking = working;
		}
	}
}
