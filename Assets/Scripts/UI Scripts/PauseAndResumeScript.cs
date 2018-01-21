using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAndResumeScript : MonoBehaviour {
	public GameObject pauseUI;
	public bool paused;
	void Awake () {
		pauseUI.GetComponent<RectTransform> ().sizeDelta = new Vector2 (Screen.width, Screen.height);
	}
	public void pause () {
		if (!paused) {
			paused = true;
			Time.timeScale = 0;
			pauseUI.transform.Translate (new Vector3(0, -3000, 0), Space.Self);
		}
	}
	public void resume ()  {
		if (paused) {
			paused = false;
			Time.timeScale = 1;
			pauseUI.transform.Translate (new Vector3(0, 3000, 0), Space.Self);
		}
	}
	public void SaveGame () {
		SaveAndLoad.SavePlayer (GameObject.Find ("EventSystem").GetComponent<ControllerScript> ());
		print ("saved");
		PlayerPrefs.SetInt ("saveGame", 1);

	}
}


