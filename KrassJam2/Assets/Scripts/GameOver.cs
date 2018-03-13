using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {
	public Text movesLeftBaseText, movesLeftTotalText;
	public Text digitCountText, grandTotalText;
	public GameObject[] starRatingObjs;
	public GameObject gameOverPanel;
	public int[] starRatingCounts;

	private GameController game;
	private GameObject player;
	private bool unlimitedMode;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		game = GameObject.FindObjectOfType<GameController> ();

		if (game.totalTurnLimit > 100){
			unlimitedMode = true;
		} else {
			unlimitedMode = false; 
		}
	}

	public void CalculateTotalText(){
		starRatingCounts = new int[5];
		int grandTotal = 0;

		if (!unlimitedMode) {
			int movesLeft = game.totalTurnLimit - game.currentTurn;
			movesLeftBaseText.text = movesLeft + " x 100";
			movesLeftTotalText.text = (movesLeft * 100).ToString ();
			grandTotal += movesLeft * 100;
		}

		int playerDigits = player.GetComponent<DigitController> ().score;
		digitCountText.text = playerDigits.ToString ();
		grandTotal += playerDigits;

		foreach (RestaurantActivity restaurant in GameObject.FindObjectsOfType<RestaurantActivity>()) {
			int rating = restaurant.starRating;
			starRatingCounts [rating - 1]++;
		}
			
		for (int i = 0; i < starRatingObjs.Length; i++) {
			if (starRatingCounts [i] == 0) {
				starRatingObjs [i].SetActive (false);
			} else {
				starRatingObjs [i].SetActive (true);
				starRatingObjs [i].transform.Find ("Amount").GetComponent<Text> ().text = starRatingCounts [i].ToString ();
				starRatingObjs [i].transform.Find ("Score").GetComponent<Text> ().text = (starRatingCounts [i] * 20).ToString();
				grandTotal += (starRatingCounts [i] * 20);
			}
		}

		grandTotalText.text = grandTotal.ToString ();

		gameOverPanel.SetActive (true);

	}
}
