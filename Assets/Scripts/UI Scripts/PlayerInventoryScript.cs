using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolProperties {
	public string name;
	public float damage;
	public float useDamage;
	public ToolProperties () {}
	public ToolProperties (string name, float damage, float useDamage) {
		this.name = name;
		this.damage = damage;
		this.useDamage = useDamage;
	}
}
public class InventoryItem {
	public int quantity;
	public string name;
	public string displayName;
	public Sprite texture;
	public InventoryItem (int quantity, string name, string displayName) {
		this.quantity = quantity;
		this.name = name;
		this.displayName = displayName;
	}
	public SerializedInventoryItem convertToSave () {
		return new SerializedInventoryItem (quantity, name, displayName);
	}
}
//this one does not contain sprite because sprites don't support serialization
[System.Serializable]
public class SerializedInventoryItem {
	public int quantity;
	public string name;
	public string displayName;
	public SerializedInventoryItem (int quantity, string name, string displayName) {
		this.quantity = quantity;
		this.name = name;
		this.displayName = displayName;
	}
	public InventoryItem convertToNormal () {
		return new InventoryItem (quantity, name, displayName);
	}
}

[System.Serializable]
public class ConsumedFoodValue {
	public string name;
	public float hunger;
	public float health;
	public float happiness;
	public ConsumedFoodValue (float hunger, float health, float happiness, string name) {
		this.hunger = hunger;
		this.health = health;
		this.happiness = happiness;
		this.name = name;
	}
}
[System.Serializable]
public class Fuel {
	public string name;
	public float fuelValue;
	public Fuel (string name, float fuelValue) {
		this.name = name;
		this.fuelValue = fuelValue;
	}
}
[System.Serializable]
public class CraftingRecipe {
	public string[] materialNames;
	public string[] materialDisplayNames;
	public int[] materialQuantities;
	public Sprite[] materialSprites;
	public string productName;
	public string productDisplayName;
	public int productQuantity;
	public Sprite productSprite;

	public CraftingRecipe (string[] materialNames, string[] materialDisplayNames, int[] materialQuantities, string productName, string productDisplayName, int productQuantity, Sprite productSprite) {
		this.materialNames = materialNames;
		this.materialDisplayNames = materialDisplayNames;
		this.materialQuantities = materialQuantities;
		this.productName = productName;
		this.productDisplayName = productDisplayName;
		this.productQuantity = productQuantity;
		this.productSprite = productSprite;
	}
	public CraftingRecipe () {}
}

public class PlayerInventoryScript : MonoBehaviour {
	[HideInInspector]
	public InventoryItem[] itemIdentityStorage;
	[HideInInspector]
	public CraftingRecipe[] recipeIdentityStorage;
	public InventoryItem[] playerInventory;
	[HideInInspector]
	public string[] terrainNames;
	[HideInInspector]
	public ToolProperties[] tools;
	[HideInInspector]
	public string[] itemBuildingNames;
	[HideInInspector]
	public string[] buildingPrefabNames;
	[HideInInspector]
	public string[] mobPrefabNames;
	[HideInInspector]
	public ConsumedFoodValue[] foodValues;
	[HideInInspector]
	public Fuel[] fuelValues;

	//input via editor
	public GameObject[] terrainPrefabs;
	public GameObject[] mobPrefabs;
	public GameObject[] buildingPrefabs;
	public Sprite[] itemIdentityStorageSprites;
	public GameObject droppedItemPrefab;

	public ControllerScript masterControllerScript;

	bool firstFrame;
	void lateStart () {
		updateAllSprites ();
	}
	void Start () {}
	void Update () {
		if (!firstFrame) {
			firstFrame = true;
			lateStart ();
		}
	}
	void Awake () {
		masterControllerScript = GameObject.Find ("EventSystem").GetComponent<ControllerScript> ();

		//mob id (tag name)
		mobPrefabNames = new string[] {
			"sheep",
			"pig"
		};
		//terrain id (tag name)
		terrainNames = new string[] {
			"savanna",
			"water",
			"forest",
			"desert",
			"rocky",
			"plains"
		};

		//building id (must be equivalent to item id name for prefabs crafted by player), used to find building prefab
		buildingPrefabNames = new string[] {
			"campfire",
			"berryBush",
			"tree",
			"boulder",
			"grassPatch",
			"bush",
			"naturalCarrot",
			"naturalTomato"
		};
		//fuel
		fuelValues = new Fuel[] {
			//name, fuel
			new Fuel("sticks", 10),
			new Fuel("wood", 30),
			new Fuel("coal", 100),
			new Fuel("grass", 12),
			new Fuel("manure", 30)
		};
		//food
		foodValues = new ConsumedFoodValue[] {
			//hunger, health, happiness
			new ConsumedFoodValue(10, 1, 0, "berries"),
			new ConsumedFoodValue(15, 2, 0, "apple"),
			new ConsumedFoodValue(10, 1, 0, "orange"),
			new ConsumedFoodValue(10, 3, 0, "tomato"),
			new ConsumedFoodValue(12, 1, 0, "banana"),
			new ConsumedFoodValue(12, 1, 0, "corn"),
			new ConsumedFoodValue(20, 1, 0, "meat"),
			new ConsumedFoodValue(10, 0, 0, "mushroom"),
			new ConsumedFoodValue(12, 0, 0, "carrot")
		};

		//quantity is for maximum per stack in this array
		itemIdentityStorage = new InventoryItem[23];

		//item id (tag name)
		itemIdentityStorage [0] = new InventoryItem(20, "flint", "Flint");
		itemIdentityStorage [1] = new InventoryItem(40, "sticks", "Sticks");
		itemIdentityStorage [2] = new InventoryItem(40, "grass", "Grass");
		itemIdentityStorage [3] = new InventoryItem(20, "rope", "Rope");
		itemIdentityStorage [4] = new InventoryItem(100, "flintAxe", "Flint Axe");
		itemIdentityStorage [5] = new InventoryItem(100, "flintPickaxe", "Flint Pickaxe");
		itemIdentityStorage [6] = new InventoryItem(40, "rocks", "Rocks");
		itemIdentityStorage [7] = new InventoryItem(40, "wood", "Wood");
		itemIdentityStorage [8] = new InventoryItem(20, "sapling", "Sapling");
		itemIdentityStorage [9] = new InventoryItem(1, "campfire", "Campfire");
		itemIdentityStorage [10] = new InventoryItem(40, "berries", "Berries");
		itemIdentityStorage [11] = new InventoryItem(40, "corn", "Corn");
		itemIdentityStorage [12] = new InventoryItem(40, "carrot", "Carrot");
		itemIdentityStorage [13] = new InventoryItem(40, "tomato", "Tomato");
		itemIdentityStorage [14] = new InventoryItem(40, "apple", "Apple");
		itemIdentityStorage [15] = new InventoryItem(20, "bone", "Bone");
		itemIdentityStorage [16] = new InventoryItem(40, "banana", "Banana");
		itemIdentityStorage [17] = new InventoryItem(20, "coal", "Coal");
		itemIdentityStorage [18] = new InventoryItem(40, "leather", "Leather");
		itemIdentityStorage [19] = new InventoryItem(20, "manure", "Manure");
		itemIdentityStorage [20] = new InventoryItem(40, "meat", "Meat");
		itemIdentityStorage [21] = new InventoryItem(40, "mushroom", "Mushroom");
		itemIdentityStorage [22] = new InventoryItem(40, "orange", "Orange");

		for (int i = 0; i < itemIdentityStorage.Length; i++) {
			if (itemIdentityStorageSprites [i] != null) {
				itemIdentityStorage [i].texture = itemIdentityStorageSprites [i];
			}
		}

		tools = new ToolProperties[] {
			new ToolProperties("flintAxe", 10, 1),
			new ToolProperties("flintPickaxe", 12, 2)
		};

		itemBuildingNames = new string[1];
		itemBuildingNames [0] = "campfire";

		//**control recipes here**

		recipeIdentityStorage = new CraftingRecipe[4];

		recipeIdentityStorage[0] = new CraftingRecipe(new string [] {"sticks", "flint", "rope"}, new string[] {"Sticks", "Flint", "Rope"}, new int [] {0, 0, 0}, "flintAxe", "Flint Axe", 100, findItemSpriteWithName("flintAxe"));
		recipeIdentityStorage[1] = new CraftingRecipe(new string [] {"sticks", "flint", "rope"}, new string[] {"Sticks", "Flint", "Rope"}, new int [] {3, 2, 1}, "flintPickaxe", "Flint Pickaxe", 100, findItemSpriteWithName("flintPickaxe"));
		recipeIdentityStorage[2] = new CraftingRecipe(new string [] {"grass"}, new string[] {"Grass"}, new int [] {4}, "rope", "Rope", 1, findItemSpriteWithName("rope"));
		recipeIdentityStorage[3] = new CraftingRecipe(new string [] {"grass", "sticks", "wood"}, new string[] {"Grass", "Sticks", "Wood"}, new int [] {0, 0, 0}, "campfire", "Campfire", 1, findItemSpriteWithName("campfire"));

		playerInventory = new InventoryItem[13];
		for (int i = 0; i < playerInventory.Length; i++) {
			playerInventory [i] = new InventoryItem (0, "flint", "Flint");
		}
	}
	public string findItemDisplayNameWithName (string name) {
		foreach (InventoryItem i in itemIdentityStorage) {
			if (i.name == name) {
				return i.displayName;
			}
		}
		return "";
	}
	//reference here everytime building is placed, item is dropped or crop is planted
	//complete here
	public void addGameObjectToSaves (GameObject building) {
		masterControllerScript.GetComponent<SaveSystemScript> ().addObjectToBuildingsArray (building);
	}
	public void addDroppedItemToSaves (GameObject item) {
		masterControllerScript.GetComponent<SaveSystemScript> ().addObjectToItemsArray (item);
	}
	public void addTerrainToSaves (GameObject terrain) {
		masterControllerScript.GetComponent<SaveSystemScript> ().addObjectToTerrainsArray (terrain);
	}
	public void addMobToSaves (GameObject mob) {
		masterControllerScript.GetComponent<SaveSystemScript> ().addObjectToMobsArray (mob);
	}
	//returns closest fire
	public GameObject findClosestFireToPlayer (float radius) {
		GameObject currentPrefab = null;
		float closestDistance = radius * 100;
		foreach (Collider i in Physics.OverlapSphere (transform.position, radius)) {
			if (i.GetComponent<FireScript> () != null && Vector3.Distance(transform.position, i.transform.position) < closestDistance) {
				closestDistance = Vector3.Distance (transform.position, i.transform.position);
				currentPrefab = i.gameObject;
			}
		}
		return currentPrefab;
	}
	public ToolProperties findToolWithName (string name) {
		foreach (ToolProperties i in tools) {
			if (i.name == name) {
				return i;
			}
		}
		return null;
	}
	public bool determineWhetherItemIsFoodWithName (string name) {
		foreach (ConsumedFoodValue i in foodValues) {
			if (i.name == name) {
				return true;
			}
		}
		return false;
	}
	public bool determineWhetherItemIsFuelWithName (string name) {
		foreach (Fuel i in fuelValues) {
			if (i.name == name) {
				return true;
			}
		}
		return false;
	}
	public ConsumedFoodValue findFoodValuesWithName (string name) {
		foreach  (ConsumedFoodValue i in foodValues) {
			if (i.name == name) {
				return i;
			}
		}
		return null;
	}
	public Fuel findFuelValuesWithName (string name) {
		foreach  (Fuel i in fuelValues) {
			if (i.name == name) {
				return i;
			}
		}
		return null;
	}
	public GameObject findMobPrefabWithName (string name) {
		for (int i = 0; i < mobPrefabNames.Length; i++) {
			if (mobPrefabNames [i] == name) {
				return mobPrefabs [i];
			}
		}
		return null;
	}
	public GameObject findBuildingPrefabWithName (string name) {
		for (int i = 0; i < buildingPrefabNames.Length; i++) {
			if (buildingPrefabNames [i] == name) {
				return buildingPrefabs [i];
			}
		}
		return null;
	}
	public GameObject findTerrainPrefabWithName (string name) {
		for (int i = 0; i < terrainNames.Length; i++) {
			if (terrainNames [i] == name) {
				return terrainPrefabs [i];
			}
		}
		return null;
	}
	public void takeItemFromPlayerInventory (int quantity, string name) {
		int amountLeft = quantity;
		foreach (InventoryItem i in playerInventory) {
			if (i.name == name && amountLeft > 0) {
				if (i.quantity >= amountLeft) {
					i.quantity -= amountLeft;
					break;
				} else {
					amountLeft -= i.quantity;
					i.quantity = 0;
				}
			} else if (amountLeft <= 0) {
				break;
			}
		} 
		updateAllSprites ();
	}
	public int findItemQuantityInPlayerInventory (string name) {
		int totalQuantity = 0;
		foreach (InventoryItem i in playerInventory) {
			if (i.name == name) {
				totalQuantity += i.quantity;
			}
		}
		return totalQuantity;
	}
	public CraftingRecipe findRecipeWithProductName (string name) {
		foreach (CraftingRecipe i in recipeIdentityStorage) {
			if (i.productName == name) {
				return i;
			}
		}
		return null;
	}
	public Sprite findItemSpriteWithName (string name) {
		foreach (InventoryItem i in itemIdentityStorage) {
			if (i.name == name) {
				return i.texture;
			}
		}
		return null;
	}
	public int findItemStackCapacityWithName (string name) {
		foreach (InventoryItem i in itemIdentityStorage) {
			if (i.name == name) {
				return i.quantity;
			}
		}
		return 0;
	}
	public bool determineWhetherItemIsToolWithName (string name) {
		foreach (ToolProperties i in tools) {
			if (i.name == name) {
				return true;
			}
		}
		return false;
	}
	public bool determineWhetherItemIsBuildingWithName (string name) {
		foreach (string i in itemBuildingNames) {
			if (i == name) {
				return true;
			}
		}
		return false;
	}
	public void updateAllSprites () {
		for (int i = 0; i < masterControllerScript.inventoryUIObject.Length; i++) {
			if (masterControllerScript.inventoryUIObject [i] != null) {
				masterControllerScript.inventoryUIObject [i].transform.GetChild (0).GetComponent<InventoryItemScript> ().updateSprite ();
			}
		}
	}
	public void addObjectToPlayerInventory (int quantity, string name, string displayName, bool fromDroppedItem, Vector3 originalPos) {
		//if not added, instantiates returns droppedItem
		bool added = false;

		//adds object to player inventory; if full, drops item
		for (int i = 0; i < playerInventory.Length; i++) {
			if (i == playerInventory.Length - 1 && !determineWhetherItemIsToolWithName(name)) {
				break;
			}
			if (playerInventory [i].quantity <= 0) {
				playerInventory [i].quantity = quantity;
				playerInventory [i].name = name;
				added = true;
				break;
			} else if (playerInventory [i].name == name && playerInventory [i].quantity < findItemStackCapacityWithName (name)) {
				if (playerInventory [i].quantity + quantity <= findItemStackCapacityWithName (name)) {
					playerInventory [i].quantity += quantity;
					added = true;
					break;
				} else {
					playerInventory [i].quantity = findItemStackCapacityWithName (name);
					addObjectToPlayerInventory (playerInventory [i].quantity + quantity - findItemStackCapacityWithName (name), name, displayName, fromDroppedItem, originalPos);
					return;
				}
			}
		}
		updateAllSprites ();
		if (!added) {
			Vector3 pos = new Vector3 (transform.position.x, -0.7f, transform.position.z);
			if (fromDroppedItem) {
				pos = originalPos;
			}
			GameObject insItem = Instantiate (droppedItemPrefab, pos, droppedItemPrefab.transform.rotation);
			insItem.transform.GetChild(0).GetComponent<DroppedItemScript> ().myValue = new InventoryItem (quantity, name, displayName);
			addDroppedItemToSaves (insItem);
		}
	}
}
