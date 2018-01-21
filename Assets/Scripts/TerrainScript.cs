using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScript : MonoBehaviour {
	public string terrainName;

	public float spawnTreesPercentage;
	public float spawnFlintPercentage;
	public float spawnGrassPercentage;
	public float spawnBushPercentage;
	public float spawnBerryBushPercentage;
	public float spawnBoulderPercentage;
	public float spawnCarrotPercentage;
	public float spawnTomatoPercentage;
	public float spawnSheepPercentage;
	public float spawnPigPercentage;

	public GameObject droppedItemPrefab;

	public PlayerInventoryScript playerInventoryScript;

	void Awake () {
		playerInventoryScript = GameObject.Find ("Player").GetComponent<PlayerInventoryScript> ();

		if (PlayerPrefs.GetInt ("saveGame") != 1) {
			//generate stuff on terrain
			Vector3 offset = transform.position;
			for (float i = -3 + (int)offset.x; i < 4 + (int)offset.x; i += 2) {
				for (float j = -3 + (int)offset.z; j < 4 + (int)offset.z; j += 2) {
					float rand = Random.Range (0f, 100f);
					if (rand < spawnTreesPercentage) {
						//spawn trees
						GameObject insItem = Instantiate (playerInventoryScript.findBuildingPrefabWithName("tree"), new Vector3 (i, -1, j), Quaternion.identity);
						playerInventoryScript.addGameObjectToSaves (insItem);
					} else if (rand < spawnFlintPercentage + spawnTreesPercentage) {
						//spawn flint
						GameObject insItem = Instantiate (droppedItemPrefab, new Vector3 (i, -0.7f, j), droppedItemPrefab.transform.rotation) as GameObject;
						insItem.transform.GetChild (0).GetComponent<DroppedItemScript> ().myValue = new InventoryItem (1, "flint", "Flint");
						playerInventoryScript.addDroppedItemToSaves (insItem);
					} else if (rand < spawnGrassPercentage + spawnFlintPercentage + spawnTreesPercentage) {
						//spawn grass
						GameObject insItem = Instantiate (playerInventoryScript.findBuildingPrefabWithName("grassPatch"), new Vector3 (i + Random.Range (-0.5f, 0.5f), 0, j + Random.Range (-0.5f, 0.5f)), Quaternion.identity);
						playerInventoryScript.addGameObjectToSaves (insItem);
					} else if (rand < spawnBushPercentage + spawnGrassPercentage + spawnFlintPercentage + spawnTreesPercentage) {
						//spawn bush
						GameObject insItem = Instantiate (playerInventoryScript.findBuildingPrefabWithName("bush"), new Vector3 (i + Random.Range (-0.5f, 0.5f), 0, j + Random.Range (-0.5f, 0.5f)), Quaternion.identity);
						playerInventoryScript.addGameObjectToSaves (insItem);
					} else if (rand < spawnBerryBushPercentage + spawnBushPercentage + spawnGrassPercentage + spawnFlintPercentage + spawnTreesPercentage) {
						//spawn berry bush
						GameObject insItem = Instantiate (playerInventoryScript.findBuildingPrefabWithName("berryBush"), new Vector3 (i + Random.Range (-0.5f, 0.5f), 0, j + Random.Range (-0.5f, 0.5f)), Quaternion.identity);
						playerInventoryScript.addGameObjectToSaves (insItem);
					} else if (rand < spawnBerryBushPercentage + spawnBushPercentage + spawnGrassPercentage + spawnFlintPercentage + spawnTreesPercentage + spawnBoulderPercentage) {
						//spawn boulder
						GameObject insItem = Instantiate (playerInventoryScript.findBuildingPrefabWithName("boulder"), new Vector3 (i + Random.Range (-0.5f, 0.5f), 0, j + Random.Range (-0.5f, 0.5f)), playerInventoryScript.findBuildingPrefabWithName("boulder").transform.rotation);
						playerInventoryScript.addGameObjectToSaves (insItem);
					} else if (rand < spawnBerryBushPercentage + spawnBushPercentage + spawnGrassPercentage + spawnFlintPercentage + spawnTreesPercentage + spawnBoulderPercentage + spawnCarrotPercentage) {
						//spawn carrot
						GameObject insItem = Instantiate (playerInventoryScript.findBuildingPrefabWithName("naturalCarrot"), new Vector3 (i + Random.Range (-0.5f, 0.5f), -0.9f, j + Random.Range (-0.5f, 0.5f)), playerInventoryScript.findBuildingPrefabWithName("naturalCarrot").transform.rotation);
						playerInventoryScript.addGameObjectToSaves (insItem);
					} else if (rand < spawnBerryBushPercentage + spawnBushPercentage + spawnGrassPercentage + spawnFlintPercentage + spawnTreesPercentage + spawnBoulderPercentage + spawnCarrotPercentage + spawnTomatoPercentage) {
						//spawn tomato
						GameObject insItem = Instantiate (playerInventoryScript.findBuildingPrefabWithName("naturalTomato"), new Vector3 (i + Random.Range (-0.5f, 0.5f), -0.9f, j + Random.Range (-0.5f, 0.5f)), playerInventoryScript.findBuildingPrefabWithName("naturalTomato").transform.rotation);
						playerInventoryScript.addGameObjectToSaves (insItem);
					} else if (rand < spawnBerryBushPercentage + spawnBushPercentage + spawnGrassPercentage + spawnFlintPercentage + spawnTreesPercentage + spawnBoulderPercentage + spawnCarrotPercentage + spawnTomatoPercentage + spawnSheepPercentage) {
						//spawn sheep
						GameObject insItem = Instantiate (playerInventoryScript.findMobPrefabWithName("sheep"), new Vector3 (i, -0.2f, j), Quaternion.identity);
						playerInventoryScript.addMobToSaves (insItem);
					} else if (rand < spawnBerryBushPercentage + spawnBushPercentage + spawnGrassPercentage + spawnFlintPercentage + spawnTreesPercentage + spawnBoulderPercentage + spawnCarrotPercentage + spawnTomatoPercentage + spawnSheepPercentage + spawnPigPercentage) {
						//spawn pig
						GameObject insItem = Instantiate (playerInventoryScript.findMobPrefabWithName("pig"), new Vector3 (i, -0.2f, j), Quaternion.identity);
						playerInventoryScript.addMobToSaves (insItem);
					}
				}
			}
		}
	}

	void Update () {
		
	}
}
