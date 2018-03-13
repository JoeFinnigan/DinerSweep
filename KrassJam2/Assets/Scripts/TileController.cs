using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour{
	public int value;
	public bool notSelected;

	private TextMesh valueText;

	private void Start(){
		transform.SetParent (GameObject.Find ("PlayableTiles").transform);
		transform.localScale = new Vector3 (transform.localScale.x * 0.96f, transform.localScale.y * 0.96f, 1f);
		name = "PlayableTile (" + transform.position.x + ", " + transform.position.y + ")";
		valueText = GetComponentInChildren<TextMesh> ();

		value = Random.Range (1, 10);

		valueText.text = value.ToString ();

		if (tag == "NegativeTile") {
			value = -value;
		}

		notSelected = true;
	}

	private void Update(){
		transform.Find ("Text").gameObject.SetActive (notSelected);
	}
}
