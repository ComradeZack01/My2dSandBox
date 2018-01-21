using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobScript : MonoBehaviour {
	//script must be attached to mob with one of the following scripts
	public void takeDamage (float damage) {
		if (GetComponent<PassiveFourLegs> () != null) {
			GetComponent<PassiveFourLegs> ().takeDamage (damage);
		}
	}
}
