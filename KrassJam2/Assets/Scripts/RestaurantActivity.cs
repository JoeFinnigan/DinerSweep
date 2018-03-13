using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantActivity : MonoBehaviour {
	public GameObject[] stars;
	public int starRating;
	public bool active;
	public List<GameObject> tilesInRadius;

	private bool objectInitialised, enemyFirstHit;
	private int baseScore;
	private TutorialController tutorial;

	void Start(){
		active = false;
		enemyFirstHit = true;

		tutorial = GameObject.FindObjectOfType<TutorialController> ();
	}

	void Update () {
		if (active & !objectInitialised) {
			StartCoroutine (InitialisePlacement());
			//UpdateStarDisplay ();
			objectInitialised = true;
		}
	}

	private IEnumerator InitialisePlacement(){
		yield return new WaitForSeconds (0.01f);
		UpdateStarDisplay ();
	}

	public void UpdateStarDisplay(){	
		starRating = SetStarRating (GetComponentInChildren<TileDetector> ().tilesInRadius);

		AddStarsToImage (starRating);
	}

	private void AddStarsToImage(int starRating){
		for (int i = 0; i < stars.Length; i++) {
			if (starRating >= i + 1) {
				stars [i].SetActive (true);
			} else {
				stars [i].SetActive (false);
			}
		}
	}

	private int SetStarRating(List<GameObject> tiles){
		int score = 0;

		foreach (GameObject tile in tiles) {
			score += tile.GetComponent<TileController> ().value;
		}

		int difficultyLevel = PlayerPrefController.GetDifficultyLevel ();

		// Star rating requirements go up depending on difficulty selected
		if (difficultyLevel == 1) {
			baseScore = 2;
		} else if (difficultyLevel == 2) {
			baseScore = 3;
		} else if (difficultyLevel == 3) {
			baseScore = 4;
		}

		if (score <= (1 * baseScore)) {
			return 1;
		} else if (score <= (2 * baseScore)) {
			return 2;
		} else if (score <= (3 * baseScore)) {
			return 3;
		} else if (score <= (4 * baseScore)) {
			return 4;
		} else {
			return 5;
		}
	}

	private void OnTriggerEnter2D(Collider2D collider){
		print (collider.tag);

		if (collider.tag == "Enemy" & objectInitialised) {
			if (enemyFirstHit){
				enemyFirstHit = false;
				tutorial.TriggerTutorialMessage (7);
			}

			starRating--;

			if (starRating > 0) {
				AddStarsToImage (starRating);
			} else {
				if (gameObject != null) {
					List<GameObject> tileRadius = collider.GetComponentInChildren<TileDetector> ().tilesInRadius;
					tileRadius.Remove (gameObject);
				}

				Destroy (gameObject, 0.1f);
			}

			print ("Enemy has flooded internet forums with negative reviews of one of your restaurants, forcing it to be shut down!");


		}
	}
}
