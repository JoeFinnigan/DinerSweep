using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour {
	public float destroyTime;

	void Update () {
		destroyTime -= Time.deltaTime;

		if (destroyTime <= 0) {
			print ("Object destroyed");
			Destroy (gameObject);
		}
	}
}
