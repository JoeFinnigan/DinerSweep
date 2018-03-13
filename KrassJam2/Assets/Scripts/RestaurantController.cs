using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestaurantController : MonoBehaviour {
	public int digitValue;
	public string description;

	private DigitController playerDigits;
	private TutorialController tutorial;
	private EnemyController enemy;
	private int difficultyLevel;
	private Text digitText;

	public GameObject restaurantObj;

	void Start () {
		enemy = GameObject.FindObjectOfType<EnemyController> ();
		playerDigits = GameObject.FindGameObjectWithTag ("Player").GetComponent<DigitController> ();
		tutorial = GameObject.FindObjectOfType<TutorialController> ();

		description = gameObject.name;

		digitText = GetComponentInChildren<Text> ();
		difficultyLevel = PlayerPrefController.GetDifficultyLevel ();

		if (difficultyLevel == 1) {
			digitText.text = "20";
		} else if (difficultyLevel == 2) {
			digitText.text = "30";
		} else if (difficultyLevel == 3) {
			digitText.text = "50";
		}

		digitValue = int.Parse (digitText.text);
	}

	public void CreateRestaurant(){
		if (!enemy.turnInProgress) {
			if (tutorial.isEnabled) {
				tutorial.TriggerTutorialMessage (5);
			}

			if (playerDigits.startingDigits >= digitValue) {
				playerDigits.ChangeDigitAmount (-digitValue, true);

				GameObject newRestaurant = Instantiate (restaurantObj, transform.position, Quaternion.identity) as GameObject;
				newRestaurant.transform.SetParent (GameObject.Find ("PlayableTiles").transform);
			} else {
				print ("Cannot currently buy");
			}
		}
	}

	public void RefundRestaurantCost(){
		playerDigits.ChangeDigitAmount (digitValue, true);
	}
}
