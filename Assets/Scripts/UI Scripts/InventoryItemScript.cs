using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemScript : MonoBehaviour {
	Rect rect;
	Rect parentRect;
	InventoryItem[] playerInv;
	public Vector3 previousPosition;
	bool followMouse = false;
	ControllerScript masterControllerScript;
	bool clickedOnce = false;
	float clickedTimer = 2;
	void Start () {
		masterControllerScript = GameObject.Find ("EventSystem").GetComponent<ControllerScript> ();
		previousPosition = transform.position;
		updateSprite ();
		rect = new Rect (new Vector2 (GetComponent<RectTransform>().position.x - GetComponent<RectTransform>().rect.width / 2, GetComponent<RectTransform>().position.y - GetComponent<RectTransform>().rect.height / 2), new Vector2 (GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height));
		parentRect = new Rect (new Vector2 (transform.parent.GetComponent<RectTransform>().position.x - transform.parent.GetComponent<RectTransform>().rect.width / 2, transform.parent.GetComponent<RectTransform>().position.y - transform.parent.GetComponent<RectTransform>().rect.height / 2), new Vector2 (transform.parent.GetComponent<RectTransform>().rect.width, transform.parent.GetComponent<RectTransform>().rect.height));
		playerInv = masterControllerScript.playerInventoryScript.playerInventory;
	}
	public void updateSprite () {
		clickedOnce = false;
		GetComponent <Image> ().sprite = masterControllerScript.playerInventoryScript.findItemSpriteWithName(masterControllerScript.playerInventoryScript.playerInventory [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].name);
		if (masterControllerScript.playerInventoryScript.playerInventory [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].quantity <= 0) {
			GetComponent <Image> ().enabled = false;
			transform.GetChild (0).GetComponent<Text> ().text = "";
		} else {
			GetComponent <Image> ().enabled = true;
			transform.GetChild (0).GetComponent<Text> ().text = masterControllerScript.playerInventoryScript.playerInventory [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].quantity.ToString ();
		}
	}
	bool isFood () {
		return masterControllerScript.playerInventoryScript.determineWhetherItemIsFoodWithName (playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].name);
	}
	bool isFuel () {
		return masterControllerScript.playerInventoryScript.determineWhetherItemIsFuelWithName (playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].name);
	}
	GameObject closeToFire (float radius) {
		return masterControllerScript.playerInventoryScript.findClosestFireToPlayer (radius);
	}
	void Update () {
		rect = new Rect (new Vector2 (GetComponent<RectTransform>().position.x - GetComponent<RectTransform>().rect.width / 2, GetComponent<RectTransform>().position.y - GetComponent<RectTransform>().rect.height / 2), new Vector2 (GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height));
		parentRect = new Rect (new Vector2 (transform.parent.GetComponent<RectTransform>().position.x - transform.parent.GetComponent<RectTransform>().rect.width / 2, transform.parent.GetComponent<RectTransform>().position.y - transform.parent.GetComponent<RectTransform>().rect.height / 2), new Vector2 (transform.parent.GetComponent<RectTransform>().rect.width, transform.parent.GetComponent<RectTransform>().rect.height));
		if ((isFood() || isFuel()) && playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].quantity > 0) {
			if (clickedOnce) {
				if (isFood () && transform.GetChild (0).gameObject.GetComponent<Text> ().text != "Eat")
					transform.GetChild (0).gameObject.GetComponent<Text> ().text = "Eat";
				if (isFuel () && transform.GetChild (0).gameObject.GetComponent<Text> ().text != "Add" && closeToFire (2) != null)
					transform.GetChild (0).gameObject.GetComponent<Text> ().text = "Add";
				clickedTimer -= Time.deltaTime;
				if (clickedTimer <= 0) {
					clickedOnce = false;
					updateSprite ();
				}
			}
			if (Input.GetMouseButtonUp(0) && rect.Contains(new Vector2(Input.mousePosition.x, Input.mousePosition.y)) && parentRect.Contains(new Vector2(Input.mousePosition.x, Input.mousePosition.y))) {
				if (!clickedOnce) {
					clickedOnce = true;
					clickedTimer = 2;
				} else if (isFood ()) {
					masterControllerScript.playerInventoryScript.GetComponent<PlayerScript> ().hunger += masterControllerScript.playerInventoryScript.findFoodValuesWithName (playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].name).hunger;
					masterControllerScript.playerInventoryScript.GetComponent<PlayerScript> ().health += masterControllerScript.playerInventoryScript.findFoodValuesWithName (playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].name).health;
					masterControllerScript.playerInventoryScript.GetComponent<PlayerScript> ().happiness += masterControllerScript.playerInventoryScript.findFoodValuesWithName (playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].name).happiness;
					playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].quantity--;
					updateSprite ();
				} else if (isFuel ()) {
					if (closeToFire (2) != null) {
						playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].quantity--;
						closeToFire (2).GetComponent<FireScript> ().fuel += masterControllerScript.playerInventoryScript.findFuelValuesWithName (playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].name).fuelValue;
						updateSprite ();
					}
				}
			}
		}

		if (transform.localPosition != Vector3.zero && !followMouse) {
			transform.localPosition = Vector3.zero;
		}
			
		if (Input.GetMouseButtonDown(0) && rect.Contains(Input.mousePosition) && GetComponent <Image> ().enabled) {
			followMouse = true;
		}
		if (followMouse) {
			if (transform.parent.parent != GameObject.Find ("Front").transform) {
				transform.parent.SetParent (GameObject.Find ("Front").transform);
			}
			transform.position = Input.mousePosition;
			masterControllerScript.UIBusy = true;
		} else {
			if (transform.parent.parent != GameObject.Find ("Back").transform) {
				transform.parent.SetParent (GameObject.Find ("Back").transform);
			}
		}
		if (Input.GetMouseButtonUp(0) && followMouse) {
			masterControllerScript.UIBusy = false;
			followMouse = false;
			transform.position = previousPosition;
			bool change = false;
			GameObject swapGameObject = null; 
			float distance = 1000;
			int a = 0;

			foreach (Rect i in masterControllerScript.inventoryUI) {
				if (i.Overlaps(rect) && i != new Rect(0, 0, 0, 0) && Vector2.Distance(new Vector2(i.x, i.y), new Vector2(rect.x, rect.y)) < distance) {
					swapGameObject = masterControllerScript.inventoryUIObject[a].transform.GetChild(0).gameObject;
					GetComponent<RectTransform>().position = new Vector2 (i.x + i.width / 2, i.y + i.height / 2);
					distance = Vector2.Distance (new Vector2 (i.x, i.y), new Vector2 (rect.x, rect.y));
					change = true;
				}
				a++;
			}

			if (change && swapGameObject != gameObject && (swapGameObject.transform.parent.GetComponent<InventoryUIScript> ().rectIndex != 12 || masterControllerScript.playerInventoryScript.determineWhetherItemIsToolWithName (playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].name) || masterControllerScript.playerInventoryScript.determineWhetherItemIsBuildingWithName (playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].name))) {
				if (playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].name == playerInv [swapGameObject.transform.parent.GetComponent<InventoryUIScript> ().rectIndex].name) {
					if (playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].quantity + playerInv [swapGameObject.transform.parent.GetComponent<InventoryUIScript> ().rectIndex].quantity <= masterControllerScript.playerInventoryScript.findItemStackCapacityWithName (playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].name)) {
						playerInv [swapGameObject.transform.parent.GetComponent<InventoryUIScript> ().rectIndex].quantity = playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].quantity + playerInv [swapGameObject.transform.parent.GetComponent<InventoryUIScript> ().rectIndex].quantity;
						playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].quantity = 0;
					} else {
						int val = masterControllerScript.playerInventoryScript.findItemStackCapacityWithName (playerInv [swapGameObject.transform.parent.GetComponent<InventoryUIScript> ().rectIndex].name);
						playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].quantity = playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].quantity + playerInv [swapGameObject.transform.parent.GetComponent<InventoryUIScript> ().rectIndex].quantity - masterControllerScript.playerInventoryScript.findItemStackCapacityWithName (playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].name);
						playerInv [swapGameObject.transform.parent.GetComponent<InventoryUIScript> ().rectIndex].quantity = val;
					}
				} else {
					InventoryItem intersectStorage = masterControllerScript.playerInventoryScript.playerInventory [transform.parent.GetComponent<InventoryUIScript> ().rectIndex];
					masterControllerScript.playerInventoryScript.playerInventory [transform.parent.GetComponent<InventoryUIScript> ().rectIndex] = masterControllerScript.playerInventoryScript.playerInventory [swapGameObject.transform.parent.GetComponent<InventoryUIScript> ().rectIndex];
					masterControllerScript.playerInventoryScript.playerInventory [swapGameObject.transform.parent.GetComponent<InventoryUIScript> ().rectIndex] = intersectStorage;
				}
				Transform target = swapGameObject.transform.parent;
				swapGameObject.transform.SetParent (transform.parent);
				transform.SetParent (target);

				GetComponent<RectTransform> ().localPosition = Vector3.zero;
				swapGameObject.GetComponent<RectTransform> ().localPosition = Vector3.zero;

				previousPosition = transform.position;
				swapGameObject.GetComponent<InventoryItemScript> ().previousPosition = swapGameObject.transform.position;

				updateSprite ();
				swapGameObject.GetComponent<InventoryItemScript> ().updateSprite ();
			} else if (swapGameObject != gameObject) {
				PlayerInventoryScript script = masterControllerScript.playerInventoryScript;
				GameObject insItem = Instantiate (script.droppedItemPrefab, new Vector3 (script.transform.position.x, -0.7f, script.transform.position.z), script.droppedItemPrefab.transform.rotation);
				insItem.transform.GetChild(0).GetComponent<DroppedItemScript> ().myValue = new InventoryItem (playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].quantity, playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].name, playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].displayName);
				masterControllerScript.playerInventoryScript.addDroppedItemToSaves (insItem);
				playerInv [transform.parent.GetComponent<InventoryUIScript> ().rectIndex].quantity = 0;
				updateSprite ();
			} else {
				transform.position = previousPosition;
			}
			if (masterControllerScript.playerInventoryScript.determineWhetherItemIsToolWithName (playerInv[12].name) && playerInv[12].quantity > 0) {
				GameObject.Find ("Player").GetComponent<PlayerScript> ().changeEquipTool(true, playerInv[12].name);
			}
		}
	}
}


