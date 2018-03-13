using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitController : MonoBehaviour {
	public int score, startingDigits;
	public RestaurantController baseRestaurant;

	private GameController gameController;
	private int difficultyLevel;
	private TutorialController tutorial;

	void Start () {
		//TODO DEBUG
		if (tag == "Player") {
			startingDigits = 40;
		} else {
			startingDigits = 0;
		}
		gameController = GameObject.FindObjectOfType<GameController> ();
		tutorial = GameObject.FindObjectOfType<TutorialController> ();
		difficultyLevel = PlayerPrefController.GetDifficultyLevel ();

		if (tag == "Player") {
			UIController.instance.SetStandaloneDigitTotal (startingDigits);
		}
	}

	public void ChangeDigitAmount(int digitChange, bool restaurantCreated){
		// Hard mode - enemies get 150% from positive digit tiles
		if (difficultyLevel == 3 && tag == "Enemy" && digitChange > 0) {
			digitChange = Mathf.RoundToInt (digitChange * 1.5f);
		}

		score += digitChange;
		startingDigits += digitChange;

		if (!restaurantCreated) {
			UIController.instance.UpdateProgressBar (digitChange, tag);
		}

		if (tag == "Player") {
			UIController.instance.SetStandaloneDigitTotal (startingDigits);

			if (startingDigits >= baseRestaurant.digitValue && tutorial.isEnabled) {
					tutorial.TriggerTutorialMessage (4);
				}
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.tag == "PositiveTile" || collider.tag == "NegativeTile") {
			ChangeDigitAmount(collider.GetComponent<TileController> ().value, false);

			Vector3 tilePos = collider.gameObject.transform.position;
		
			gameController.MakeTileInactiveInList (tilePos);

			if (PlayerPrefController.GetDragHold () == 0 || tag != "Player") {
				Destroy (collider.gameObject);
			} else {
				collider.gameObject.SetActive (false);
				GetComponent<PlayerController>().tilesTakenThisTurn.Add (collider.gameObject);
			}
		}
	}
}
