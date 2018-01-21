using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
	public GameObject loadingScreen;
	int clearStage;
	string[] loadingTips;
	void Start () {
		loadingTips = new string[] {
			"Passive animals will run away from the player after being attacked",
			"Berries, grass and twigs regenerate and can be dug for transplant"
		};
		if (loadingScreen != null) {
			loadingScreen.transform.GetChild (0).GetComponent<Text>().text = loadingTips[Random.Range(0, loadingTips.Length)];
		}
	}
	public void loadScene (int index) {
		Time.timeScale = 1;
		StartCoroutine (LoadAsync (index));
	}
	IEnumerator LoadAsync (int index) {
		if (loadingScreen != null) {
			loadingScreen.transform.position = GameObject.Find ("Canvas").transform.position;
		}
		AsyncOperation operation = SceneManager.LoadSceneAsync (index);
		yield return null;
	}
	public void clearSave () {
		transform.GetChild(0).GetComponent<Text> ().text = "Confirm?";
		if (clearStage == 1) {
			PlayerPrefs.SetInt ("saveGame", 0);
			clearStage = 0;
			transform.GetChild (0).GetComponent<Text> ().text = "Reset Save";
		} else {
			clearStage = 1;
		}
	}
}

