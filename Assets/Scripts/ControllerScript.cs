using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerScript : MonoBehaviour {
	//control UI size
	public float UIScale;

	public PlayerInventoryScript playerInventoryScript;
	public Text ambientTemperatureDisplay;
	public Text playerTemperatureDisplay;

	//time and season clock
	//x (4) for seasons, y (20) for days, z (4) for day, dusk, night, dawn
	public int[][][] daylightCycleClock;

	//0 for autumn, 1 for winter, 2 for spring, 3 for summer
	public int currentSeason; 
	public int currentDay; 
	public int currentDayInSeason;
	public GameObject displayClock;

	int duskBeginTime;
	int nightBeginTime;
	int dawnBeginTime;

	//disable renderer at this value
	public float changeRendererRange;

	public float targetPlayerTemp;
	public float targetTemp;
	public float ambientTemperature;
	public float playerTemperature;

	Color targetSunColor = new Color(1, 1, 1);

	//also a rotation value for clock, 0 >= x > 16 (time * 22.5)
	public float time;

	public bool touchContainsUI;
	public bool UIBusy;

	public Rect[] onScreenUI;
	public Rect[] inventoryUI;
	public GameObject[] inventoryUIObject;

	public void newDay () {
		currentDay++;
		currentDayInSeason++;
		if (currentDayInSeason > daylightCycleClock[currentSeason].Length) {
			currentDayInSeason = 1;
			if (currentSeason < daylightCycleClock.Length - 1) {
				currentSeason++;
			} else
				currentSeason = 0;
		}
		time = 0;
		int i = 0;
		while (i < daylightCycleClock[currentSeason][currentDayInSeason - 1][0]) {
			displayClock.transform.GetChild (i).GetComponent<Image> ().color = new Color (1, 1, 0);
			i++;
		}
		duskBeginTime = i;
		while (i < daylightCycleClock[currentSeason][currentDayInSeason - 1][0] + daylightCycleClock[currentSeason][currentDayInSeason - 1][1]) {
			displayClock.transform.GetChild (i).GetComponent<Image> ().color = new Color (1, 0.1f, 0.1f);
			i++;
		}
		nightBeginTime = i;
		while (i < daylightCycleClock[currentSeason][currentDayInSeason - 1][0] + daylightCycleClock[currentSeason][currentDayInSeason - 1][1] + daylightCycleClock[currentSeason][currentDayInSeason - 1][2]) {
			displayClock.transform.GetChild (i).GetComponent<Image> ().color = new Color (0.1f, 0.1f, 1);
			i++;
		}
		dawnBeginTime = i;
		while (i < daylightCycleClock[currentSeason][currentDayInSeason - 1][0] + daylightCycleClock[currentSeason][currentDayInSeason - 1][1] + daylightCycleClock[currentSeason][currentDayInSeason - 1][2] + daylightCycleClock[currentSeason][currentDayInSeason - 1][3]) {
			displayClock.transform.GetChild (i).GetComponent<Image> ().color = new Color (1, 0.6f, 0.6f);
			i++;
		}
	}
	void Awake () {
		playerInventoryScript = GameObject.Find ("Player").GetComponent<PlayerInventoryScript> ();

		daylightCycleClock = new int[][][] {
			//fall
			new int[][] {
				new int[] {
					9, 3, 2, 2
				},
				new int[] {
					9, 3, 2, 2
				},
				new int[] {
					9, 3, 2, 2
				},
				new int[] {
					8, 3, 2, 3
				},
				new int[] {
					8, 3, 2, 3
				},
				new int[] {
					7, 4, 2, 3
				},
				new int[] {
					7, 4, 2, 3
				},
				new int[] {
					6, 4, 3, 3
				},
				new int[] {
					6, 4, 3, 3
				},
				new int[] {
					5, 4, 4, 3
				}
			},
			//winter
			new int[][] {
				new int[] {
					5, 4, 4, 3
				},
				new int[] {
					5, 3, 5, 3
				},
				new int[] {
					5, 3, 5, 3
				},
				new int[] {
					4, 4, 5, 3
				},
				new int[] {
					4, 3, 6, 3
				},
				new int[] {
					4, 3, 6, 3
				},
				new int[] {
					3, 4, 6, 3
				},
				new int[] {
					3, 4, 5, 4
				},
				new int[] {
					4, 3, 5, 4
				},
				new int[] {
					4, 4, 4, 4
				}
			},
			//spring
			new int[][] {
				new int[] {
					5, 4, 4, 3
				},
				new int[] {
					5, 5, 3, 3
				},
				new int[] {
					5, 5, 3, 3
				},
				new int[] {
					6, 4, 3, 3
				},
				new int[] {
					6, 4, 3, 3
				},
				new int[] {
					6, 3, 3, 4
				},
				new int[] {
					6, 3, 3, 4
				},
				new int[] {
					7, 3, 3, 3
				},
				new int[] {
					7, 4, 3, 3
				},
				new int[] {
					8, 3, 2, 3
				}
			},
			//summer
			new int[][] {
				new int[] {
					8, 2, 2, 4
				},
				new int[] {
					9, 2, 2, 3
				},
				new int[] {
					10, 2, 2, 2
				},
				new int[] {
					11, 2, 1, 2
				},
				new int[] {
					12, 1, 1, 2
				},
				new int[] {
					12, 1, 1, 2
				},
				new int[] {
					12, 1, 2, 1
				},
				new int[] {
					11, 2, 2, 1
				},
				new int[] {
					10, 2, 2, 2
				},
				new int[] {
					9, 3, 2, 2
				}
			}
		};
		if (PlayerPrefs.GetInt("saveGame") == 0) {
			newDay ();
		}
	}
	void Start () {
		if (PlayerPrefs.GetInt ("saveGame") != 1) {
			//generate world
			float seed = Random.Range (-1000, 1000);
			for (int i = -270; i <= 270; i += 9) {
				for (int j = -270; j <= 270; j += 9) {
					if (i == -270 || i == -261 || i == 270 || i == 261 || j == -270 || j == 270 || j == -261 || j == 261) {
						//map borders
						GameObject insItem = Instantiate (playerInventoryScript.findTerrainPrefabWithName("water"), new Vector3 (i, -2f, j), Quaternion.identity);
						playerInventoryScript.addTerrainToSaves (insItem);
					} else {
						float amount = Mathf.PerlinNoise (((float)i + seed) / 70, ((float)j + seed) / 70);
						if (amount >= 0.84) {
							GameObject insItem = Instantiate (playerInventoryScript.findTerrainPrefabWithName("rocky"), new Vector3 (i, -1.5f, j), Quaternion.identity);
							playerInventoryScript.addTerrainToSaves (insItem);
						} else if (amount >= 0.76f) {
							GameObject insItem = Instantiate (playerInventoryScript.findTerrainPrefabWithName("desert"), new Vector3 (i, -1.5f, j), Quaternion.identity);
							playerInventoryScript.addTerrainToSaves (insItem);
						} else if (amount >= 0.60f) {
							GameObject insItem = Instantiate (playerInventoryScript.findTerrainPrefabWithName("savanna"), new Vector3 (i, -1.5f, j), Quaternion.identity);
							playerInventoryScript.addTerrainToSaves (insItem);
						} else if (amount >= 0.47f) {
							GameObject insItem = Instantiate (playerInventoryScript.findTerrainPrefabWithName("forest"), new Vector3 (i, -1.5f, j), Quaternion.identity);
							playerInventoryScript.addTerrainToSaves (insItem);
						} else if (amount >= 0.37f) {
							GameObject insItem = Instantiate (playerInventoryScript.findTerrainPrefabWithName("plains"), new Vector3 (i, -1.5f, j), Quaternion.identity);
							playerInventoryScript.addTerrainToSaves (insItem);
						} else {
							GameObject insItem = Instantiate (playerInventoryScript.findTerrainPrefabWithName("water"), new Vector3 (i, -2f, j), Quaternion.identity);
							playerInventoryScript.addTerrainToSaves (insItem);
						}
					}
				}
			}
		} else {
			//load save
			SaveAndLoad.LoadPlayer(this);
		}
	}

	public int saved;
	void Update () {
		saved = PlayerPrefs.GetInt ("saveGame");

		//lose health in extreme temperatures
		if (playerTemperature <= 0)
			playerInventoryScript.GetComponent<PlayerScript> ().health -= -playerTemperature * Time.deltaTime * 0.2f;
		if (playerTemperature > 35)
			playerInventoryScript.GetComponent<PlayerScript> ().health -= (playerTemperature - 35) * Time.deltaTime * 0.2f;

		targetTemp = 20;

		//calculate temperature repeatedly
		if (currentSeason == 0) {
			//autumn
			targetTemp -= (float)currentDayInSeason;
		} else if (currentSeason == 1) { 
			//winter
			if (currentDay < 6) {
				targetTemp -= (float)currentDayInSeason + 12;
			} else
				targetTemp -= -(float)currentDayInSeason + 17;
		} else if (currentSeason == 2) {
			//spring
			targetTemp += (float)currentDayInSeason - 12;
		} else {
			//summer
			if (currentDay < 6) {
				targetTemp += (float)currentDayInSeason + 2;
			} else
				targetTemp += -(float)currentDayInSeason + 7;
		}
		if (time < nightBeginTime && time > duskBeginTime || time > dawnBeginTime) {
			targetTemp -= 6;
		} else if (time > nightBeginTime) {
			targetTemp -= 12;
		}
		//slowly change ambient to target
		if (ambientTemperature < targetTemp) {
			ambientTemperature += Time.deltaTime / 6;
		} else if (ambientTemperature > targetTemp)
			ambientTemperature -= Time.deltaTime / 6;
		if (Mathf.Abs (ambientTemperature - targetTemp) < 0.5f) {
			ambientTemperature = targetTemp;
		}

		//clothing, fires, and character type will affect player warmth
		targetPlayerTemp = ambientTemperature + 3;
		if (playerInventoryScript.findClosestFireToPlayer (4) != null)
			targetPlayerTemp += (4 - Vector3.Distance (playerInventoryScript.findClosestFireToPlayer (4).transform.position, playerInventoryScript.gameObject.transform.position)) * 6f;

		//slowly change playerTemp to target
		if (playerTemperature < targetPlayerTemp) {
			playerTemperature += Time.deltaTime / 6;
		} else if (playerTemperature > targetPlayerTemp)
			playerTemperature -= Time.deltaTime / 6;
		if (Mathf.Abs (playerTemperature - targetPlayerTemp) < 0.5f) {
			playerTemperature = targetPlayerTemp;
		}

		ambientTemperatureDisplay.text = ((int)ambientTemperature).ToString() + " ºc";
		playerTemperatureDisplay.text = ((int)playerTemperature).ToString() + " ºc";
		
		Light sunlight = GameObject.Find ("Directional Light").GetComponent<Light>();
		if (time < duskBeginTime) {
			targetSunColor = new Color (1, 1, 0.9f);
		} else if (time < nightBeginTime) {
			targetSunColor = new Color (1, 0.42f, 0.42f);
		} else if (time < dawnBeginTime) {
			targetSunColor = new Color (0, 0, 0); 
		} else {
			targetSunColor = new Color (1, 0.6f, 0.6f);
		}
		if (Mathf.Abs (targetSunColor.r - sunlight.color.r) > 0.06f) {
			if (sunlight.color.r < targetSunColor.r) {
				sunlight.color = new Color (sunlight.color.r + Time.deltaTime * 0.16f, sunlight.color.g, sunlight.color.b);
			} else
				sunlight.color = new Color (sunlight.color.r - Time.deltaTime * 0.16f, sunlight.color.g, sunlight.color.b);
		}
		if (Mathf.Abs (targetSunColor.g - sunlight.color.g) > 0.06f) {
			if (sunlight.color.g < targetSunColor.g) {
				sunlight.color = new Color (sunlight.color.r, sunlight.color.g + Time.deltaTime * 0.16f, sunlight.color.b);
			} else
				sunlight.color = new Color (sunlight.color.r, sunlight.color.g - Time.deltaTime * 0.16f, sunlight.color.b);
		}
		if (Mathf.Abs (targetSunColor.b - sunlight.color.b) > 0.06f) {
			if (sunlight.color.b < targetSunColor.b) {
				sunlight.color = new Color (sunlight.color.r, sunlight.color.g, sunlight.color.b + Time.deltaTime * 0.16f);
			} else
				sunlight.color = new Color (sunlight.color.r, sunlight.color.g, sunlight.color.b - Time.deltaTime * 0.16f);
		}
		//change dayCount display
		displayClock.transform.GetChild (17).GetComponent<Text> ().text = "Day " + currentDay.ToString();
		time += Time.deltaTime / 30;
		if (time >= 16) {
			newDay ();
		}
		displayClock.transform.GetChild (16).eulerAngles = new Vector3 (0, 0, -time * 22.5f + 90);
		touchContainsUI = false;
		foreach (Rect rect in onScreenUI) {
			if (rect.Contains(Input.mousePosition)) {
				touchContainsUI = true;
				break;
			}
		}
	}
}
