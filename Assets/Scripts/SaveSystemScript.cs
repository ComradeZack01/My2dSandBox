using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystemScript : MonoBehaviour {
	//save system arrays, terrain generated via saved seed
	[HideInInspector]
	public GameObject[] buildings;
	[HideInInspector]
	public GameObject[] terrains;
	[HideInInspector]
	public GameObject[] items;
	[HideInInspector]
	public GameObject[] mobs;
	void Awake () {
		mobs = new GameObject[30000];
		buildings = new GameObject[30000];
		terrains = new GameObject[30000];
		items = new GameObject[30000];
	}
	public void addObjectToBuildingsArray (GameObject building) {
		for (int i = 0; i < buildings.Length; i++) {
			if (buildings [i] == null) {
				buildings [i] = building;
				break;
			}
		}
	}
	public void addObjectToMobsArray (GameObject mob) {
		for (int i = 0; i < mobs.Length; i++) {
			if (mobs [i] == null) {
				mobs [i] = mob;
				break;
			}
		}
	}
	public void addObjectToItemsArray (GameObject item) {
		for (int i = 0; i < items.Length; i++) {
			if (items [i] == null) {
				items [i] = item;
				break;
			}
		}
	}
	public void addObjectToTerrainsArray (GameObject terrain) {
		for (int i = 0; i < terrains.Length; i++) {
			if (terrains [i] == null) {
				terrains [i] = terrain;
				break;
			}
		}
	}
}

//use for saving
[Serializable]
public class MyVector3 {
	float x;
	float y;
	float z;
	public MyVector3 (float x, float y, float z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}
	public MyVector3 (Vector3 position) {
		this.x = position.x;
		this.y = position.y;
		this.z = position.z;
	}
	public Vector3 toVec3 () {
		return new Vector3 (x, y, z);
	}
	public Quaternion toRotation () {
		return Quaternion.Euler (toVec3());
	}
}

//manually load all scripts
[Serializable]
public class SaveSystem {
	[Serializable]
	public class Buildings {
		[Serializable]
		public class BasicBuildingSaveProperties {
			//basic attributes
			public string name;
			public MyVector3 position;
			public MyVector3 rotation;

			//NaturalBarriers
			public int health;

			//NaturalCrops
			public float growthTime;
			public bool cut;

			//Campfire
			public float fuel;

			//basic constructor
			public BasicBuildingSaveProperties (string name, Vector3 position, Vector3 rotation) {
				this.name = name;
				this.position = new MyVector3(position);
				this.rotation = new MyVector3(rotation);
			}
			public BasicBuildingSaveProperties () {}
		}
		[Serializable]
		public class NaturalBarriers : BasicBuildingSaveProperties {
			public NaturalBarriers (string name, Vector3 position, Vector3 rotation, int health) {
				this.name = name;
				this.position = new MyVector3(position);
				this.rotation = new MyVector3(rotation);
				this.health = health; 
			}
		}
		[Serializable]
		public class NaturalCrops : BasicBuildingSaveProperties {
			public NaturalCrops (string name, Vector3 position, Vector3 rotation, bool cut, float growthTime) {
				this.name = name;
				this.position = new MyVector3(position);
				this.rotation = new MyVector3(rotation);
				this.cut = cut;
				this.growthTime = growthTime;
			}
		}
		[Serializable]
		public class Campfire : BasicBuildingSaveProperties {
			public Campfire (string name, Vector3 position, Vector3 rotation, float fuel) {
				this.name = name; 
				this.position = new MyVector3(position);
				this.rotation = new MyVector3(rotation);
				this.fuel = fuel;
			}
		}
	}
	[Serializable]
	public class MyTerrain {
		public string terrainType;
		public MyVector3 position;
		public MyTerrain (string terrainType, Vector3 position) {
			this.terrainType = terrainType;
			this.position = new MyVector3(position);
		}
	}
	[Serializable]
	public class MyDroppedItem {
		public string itemName;
		public int itemQuantity;
		public MyVector3 position;
		public MyDroppedItem (string itemName, int itemQuantity, MyVector3 position) {
			this.itemName = itemName;
			this.itemQuantity = itemQuantity;
			this.position = position;
		}
	}
	[Serializable]
	public class Mobs {
		[Serializable]
		public class BasicMobAttributes {
			//basic attributes
			public string name;
			public float health;
			public float maxHealth;
			public MyVector3 position;
			public MyVector3 rotation;

			public BasicMobAttributes (string name, float health, float maxHealth, Vector3 position, Vector3 rotation) {
				this.name = name;
				this.health = health;
				this.maxHealth = maxHealth;
				this.position = new MyVector3(position);
				this.rotation = new MyVector3(rotation);
			}
			public BasicMobAttributes () {}
		}
		[Serializable]
		public class Sheep : BasicMobAttributes {
			public Sheep (string name, float health, float maxHealth, Vector3 position, Vector3 rotation) {
				this.name = name;
				this.health = health;
				this.maxHealth = maxHealth;
				this.position = new MyVector3(position);
				this.rotation = new MyVector3(rotation);
			}
		}
	}
}

public static class SaveAndLoad {
	public static void SavePlayer(ControllerScript controller) {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream stream = new FileStream(Application.persistentDataPath + "/wa.sav", FileMode.Create);

		WorldData data = new WorldData(controller);

		bf.Serialize(stream, data);
		stream.Close();

	}
	public static void LoadPlayer(ControllerScript controller) {
		if (File.Exists(Application.persistentDataPath + "/wa.sav")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/wa.sav", FileMode.Open);
			WorldData data = bf.Deserialize(stream) as WorldData;
			for (int i = 0; i < controller.playerInventoryScript.playerInventory.Length; i++) {
				controller.playerInventoryScript.playerInventory [i] = data.playerInventory [i].convertToNormal ();
			}
			controller.playerInventoryScript.transform.position = data.playerPosition.toVec3 ();

			controller.currentDay = data.currentDay - 1;
			controller.currentSeason = data.currentSeason;
			controller.currentDayInSeason = data.currentDayInSeason - 1;
			controller.newDay ();
			controller.time = data.time;
			controller.ambientTemperature = data.ambientTemperature;
			controller.playerTemperature = data.playerTemperature;

			controller.playerInventoryScript.GetComponent<PlayerScript> ().health = data.health;
			controller.playerInventoryScript.GetComponent<PlayerScript> ().hunger = data.hunger;
			controller.playerInventoryScript.GetComponent<PlayerScript> ().happiness = data.happiness;

			for (int i = 0; i < data.mobs.Length; i++) {
				if (data.mobs [i] != null) {
					GameObject prefab = controller.playerInventoryScript.findMobPrefabWithName (data.mobs [i].name);
					GameObject insItem = GameObject.Instantiate (prefab, data.mobs [i].position.toVec3 (), data.mobs [i].rotation.toRotation ()) as GameObject;
					controller.GetComponent<SaveSystemScript> ().addObjectToMobsArray (insItem);

					string targetName = data.mobs [i].name;

					//for pigs
					if (insItem.GetComponent<PassiveFourLegs> () != false) {
						insItem.GetComponent<PassiveFourLegs> ().health = data.mobs [i].health;
						insItem.GetComponent<PassiveFourLegs> ().maxHealth = data.mobs [i].maxHealth;
					} else if (targetName == "sheep") {
						insItem.GetComponent<Sheep> ().health = data.mobs [i].health;
						insItem.GetComponent<Sheep> ().maxHealth = data.mobs [i].maxHealth;
					}
				}
			}
			for (int i = 0; i < data.buildings.Length; i++) {
				if (data.buildings[i] != null) {
					GameObject prefab = controller.playerInventoryScript.findBuildingPrefabWithName (data.buildings [i].name);
					GameObject insItem = GameObject.Instantiate (prefab, data.buildings[i].position.toVec3(), data.buildings[i].rotation.toRotation()) as GameObject;
					controller.GetComponent<SaveSystemScript> ().addObjectToBuildingsArray (insItem);

					string targetName = data.buildings [i].name;

					if (targetName == "campfire") {
						//float
						insItem.GetComponent<FireScript> ().fuel = data.buildings [i].fuel;
					} else if (targetName == "berryBush" || targetName == "grassPatch" || targetName == "bush") {
						//bool, float
						if (insItem.GetComponent<BerryBushScript> () != null) {
							insItem.GetComponent<BerryBushScript> ().cut = data.buildings [i].cut;
							insItem.GetComponent<BerryBushScript> ().growTimer = data.buildings [i].growthTime;
						} else if (insItem.GetComponent<BushScript> () != null) {
							insItem.GetComponent<BushScript> ().cut = data.buildings [i].cut;
							insItem.GetComponent<BushScript> ().growTimer = data.buildings [i].growthTime;
						} else if (insItem.GetComponent<GrassScript> () != null) {
							insItem.GetComponent<GrassScript> ().cut = data.buildings [i].cut;
							insItem.GetComponent<GrassScript> ().growTimer = data.buildings [i].growthTime;
						}
					} else if (targetName == "boulder" || targetName == "tree") {
						//int
						if (insItem.GetComponent<BoulderScript> () != null) {
							insItem.GetComponent<BoulderScript> ().health = data.buildings [i].health;
						} else if (insItem.GetComponent<TreeScript> () != null) {
							insItem.GetComponent<TreeScript> ().health = data.buildings [i].health;
						}
					}
				}
			}
			for (int i = 0; i < data.terrains.Length; i++) {
				if (data.terrains[i] != null) {
					GameObject prefab = controller.playerInventoryScript.findTerrainPrefabWithName (data.terrains [i].terrainType);
					GameObject insItem = GameObject.Instantiate (prefab, data.terrains[i].position.toVec3(), Quaternion.identity) as GameObject;
					controller.GetComponent<SaveSystemScript> ().addObjectToTerrainsArray (insItem);
				}
			}
			for (int i = 0; i < data.droppedItems.Length; i++) {
				if (data.droppedItems[i] != null) {
					GameObject prefab = controller.playerInventoryScript.droppedItemPrefab;
					GameObject insItem = GameObject.Instantiate (prefab, data.droppedItems[i].position.toVec3(), prefab.transform.rotation);
					insItem.transform.GetChild (0).GetComponent<DroppedItemScript> ().myValue = new InventoryItem (data.droppedItems[i].itemQuantity, data.droppedItems[i].itemName, controller.playerInventoryScript.findItemDisplayNameWithName(data.droppedItems[i].itemName));
					controller.GetComponent<SaveSystemScript> ().addObjectToItemsArray (insItem);
				}
			}

			stream.Close();
		}
	}
}
[Serializable]
public class WorldData {
	public SaveSystem.MyDroppedItem[] droppedItems;
	public SaveSystem.Buildings.BasicBuildingSaveProperties[] buildings;
	public SaveSystem.MyTerrain[] terrains;
	public SaveSystem.Mobs.BasicMobAttributes[] mobs;
	public SerializedInventoryItem[] playerInventory;
	public MyVector3 playerPosition;
	public int currentSeason; 
	public int currentDay; 
	public int currentDayInSeason;
	public float ambientTemperature;
	public float playerTemperature;
	public float time;
	public float health;
	public float hunger;
	public float happiness;

	public WorldData (ControllerScript controller) {
		time = controller.time;
		ambientTemperature = controller.ambientTemperature;
		playerTemperature = controller.playerTemperature;
		currentSeason = controller.currentSeason;
		currentDay = controller.currentDay;
		currentDayInSeason = controller.currentDayInSeason;
		health = controller.playerInventoryScript.GetComponent<PlayerScript> ().health;
		hunger = controller.playerInventoryScript.GetComponent<PlayerScript> ().hunger;
		happiness = controller.playerInventoryScript.GetComponent<PlayerScript> ().happiness;

		droppedItems = new SaveSystem.MyDroppedItem[controller.GetComponent<SaveSystemScript>().items.Length];
		terrains = new SaveSystem.MyTerrain[controller.GetComponent<SaveSystemScript> ().terrains.Length];
		buildings = new SaveSystem.Buildings.BasicBuildingSaveProperties[controller.GetComponent<SaveSystemScript>().buildings.Length];
		mobs = new SaveSystem.Mobs.BasicMobAttributes[controller.GetComponent<SaveSystemScript>().mobs.Length];
		playerPosition = new MyVector3 (controller.playerInventoryScript.transform.position);

		playerInventory = new SerializedInventoryItem[controller.playerInventoryScript.playerInventory.Length];
		for (int i = 0; i < playerInventory.Length; i++) {
			playerInventory [i] = controller.playerInventoryScript.playerInventory [i].convertToSave ();
		}
		for (int i = 0; i < droppedItems.Length; i++) {
			if (controller.GetComponent<SaveSystemScript> ().items [i] != null) {
				droppedItems [i] = new SaveSystem.MyDroppedItem (controller.GetComponent<SaveSystemScript> ().items [i].transform.GetChild(0).GetComponent<DroppedItemScript> ().myValue.name, controller.GetComponent<SaveSystemScript> ().items [i].transform.GetChild(0).GetComponent<DroppedItemScript> ().myValue.quantity, new MyVector3 (controller.GetComponent<SaveSystemScript> ().items [i].transform.position));
			}
		}

		for (int i = 0; i < terrains.Length; i++) {
			if (controller.GetComponent<SaveSystemScript> ().terrains [i] != null) {
				GameObject terrain = controller.GetComponent<SaveSystemScript> ().terrains [i];
				terrains [i] = new SaveSystem.MyTerrain (terrain.GetComponent<TerrainScript> ().terrainName, terrain.transform.position);
			}
		}
		for (int i = 0; i < mobs.Length; i++) {
			if (controller.GetComponent<SaveSystemScript> ().mobs [i] != null) {
				GameObject mob = controller.GetComponent<SaveSystemScript> ().mobs [i];
				if (mob.GetComponent<PassiveFourLegs> () != null) {
					mobs [i] = new SaveSystem.Mobs.BasicMobAttributes (mob.GetComponent<PassiveFourLegs>().mobName, mob.GetComponent<PassiveFourLegs> ().health, mob.GetComponent<PassiveFourLegs> ().maxHealth, mob.transform.position, mob.transform.eulerAngles); 
				} else if (mob.GetComponent<Sheep> () != null) {
					mobs [i] = new SaveSystem.Mobs.Sheep (mob.GetComponent<Sheep>().mobName, mob.GetComponent<Sheep> ().health, mob.GetComponent<Sheep> ().maxHealth, mob.transform.position, mob.transform.eulerAngles); 
				}
			}
		}
		for (int i = 0; i < buildings.Length; i++) {
			if (controller.GetComponent<SaveSystemScript> ().buildings [i] != null) {
				GameObject building = controller.GetComponent<SaveSystemScript> ().buildings [i];

				TreeScript treeScript = null;
				BoulderScript boulderScript = null;
				GrassScript grassScript = null;
				BushScript bushScript = null;
				BerryBushScript berryBushScript = null;
				FireScript fireScript = null;
				GroundFoodScript naturalFoodScript = null;

				if (building.GetComponent<TreeScript> () != null)
					treeScript = building.GetComponent<TreeScript> ();
				if (building.GetComponent<BoulderScript> () != null)
					boulderScript = building.GetComponent<BoulderScript> (); 
				if (building.GetComponent<GrassScript> () != null)
					grassScript = building.GetComponent<GrassScript> ();
				if (building.GetComponent<BushScript> () != null)
					bushScript = building.GetComponent<BushScript> ();
				if (building.GetComponent<BerryBushScript> () != null)
					berryBushScript = building.GetComponent<BerryBushScript> ();
				if (building.GetComponent<FireScript> () != null)
					fireScript = building.GetComponent<FireScript> ();
				if (building.GetComponent<GroundFoodScript> () != null)
					naturalFoodScript = building.GetComponent<GroundFoodScript> ();
				
				if (treeScript != null || boulderScript != null) {
					//natural barriers
					if (treeScript != null) {
						buildings [i] = new SaveSystem.Buildings.NaturalBarriers ("tree", treeScript.transform.position, treeScript.transform.eulerAngles, treeScript.health);
					} else if (boulderScript != null)
						buildings [i] = new SaveSystem.Buildings.NaturalBarriers ("boulder", boulderScript.transform.position, boulderScript.transform.eulerAngles, boulderScript.health);
				} else if (grassScript != null || bushScript != null || berryBushScript != null) {
					if (grassScript != null) {
						buildings [i] = new SaveSystem.Buildings.NaturalCrops ("grassPatch", grassScript.transform.position, grassScript.transform.eulerAngles, grassScript.cut, grassScript.growTimer);
					}
					if (bushScript != null) { 
						buildings [i] = new SaveSystem.Buildings.NaturalCrops ("bush", bushScript.transform.position, bushScript.transform.eulerAngles, bushScript.cut, bushScript.growTimer);
					} else if (berryBushScript != null)
						buildings [i] = new SaveSystem.Buildings.NaturalCrops ("berryBush", berryBushScript.transform.position, berryBushScript.transform.eulerAngles, berryBushScript.cut, berryBushScript.growTimer);
				} else if (fireScript != null) {
					buildings [i] = new SaveSystem.Buildings.Campfire ("campfire", fireScript.transform.position, fireScript.transform.eulerAngles, fireScript.fuel);
				} else if (naturalFoodScript != null)
					buildings [i] = new SaveSystem.Buildings.BasicBuildingSaveProperties (naturalFoodScript.buildingIDName, naturalFoodScript.transform.position, naturalFoodScript.transform.eulerAngles);
			}
		}
	}
}