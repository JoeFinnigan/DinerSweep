using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	private CameraController cam;
	private GameController gameController;
	private PlayerController player;
	private int directionIndex;
	private Rigidbody2D rb;
	private TileDetector tileDetector;
	private TutorialController tutorial;

	private Vector3 advancedMovePos;
	private Vector2 newPos;
	private int difficultyLevel;
	private bool validDirection, enemyActive;
	private GameObject tileToMoveTo;

	public bool turnInProgress, firstTurn;
	public int moveCount;

	void Start () {
		difficultyLevel = PlayerPrefController.GetDifficultyLevel ();
		cam = GameObject.FindObjectOfType<CameraController> ();
		gameController = GameObject.FindObjectOfType<GameController> ();
		player = GameObject.FindObjectOfType<PlayerController> ();
		tileDetector = GetComponentInChildren<TileDetector> ();
		tutorial = GameObject.FindObjectOfType<TutorialController> ();

		rb = GetComponent<Rigidbody2D> ();
		turnInProgress = false;
	}

	private void Update(){
		if (Time.timeScale != 1) {
			enemyActive = false;
		} else {
			enemyActive = true;
		}
	}

	public void PlayEnemyTurn(){
		StartCoroutine (EnemyTurn ());
	}

	private IEnumerator EnemyTurn(){
		yield return new WaitForSeconds (0.2f);

		UIController.instance.SetMoveCounter (moveCount);
		cam.target = gameObject;
		turnInProgress = true;
		yield return new WaitForSeconds (2f);

		if (tutorial.isEnabled && gameController.currentTurn == 1) {
			tutorial.TriggerTutorialMessage (3);
			UIController.instance.StopTime ();

			while (!enemyActive){
				// Do nothing
			}
		}

		for (int i = 0; i < moveCount; i++) {
			UIController.instance.SetMoveCounter ((moveCount - 1) - i);
			GetDirection ();

			if (difficultyLevel == 1 || advancedMovePos == Vector3.zero) {
				newPos = Vector2.zero;

				switch (directionIndex) {
				case 0: // Up
					newPos = new Vector2 (rb.position.x, rb.position.y + 1);
					break;
				case 1: // Right
					newPos = new Vector2 (rb.position.x + 1, rb.position.y);
					break;
				case 2: // Down
					newPos = new Vector2 (rb.position.x, rb.position.y - 1);
					break;
				case 3: // Left
					newPos = new Vector2 (rb.position.x - 1, rb.position.y);
					break;
				}

				rb.MovePosition (newPos);
			} else if (difficultyLevel == 2) {
				rb.MovePosition (advancedMovePos);
			} else if (difficultyLevel == 3) {
				//Better AI goes here
				rb.MovePosition (advancedMovePos);
			}

			yield return new WaitForSeconds (0.3f);
		}

		UIController.instance.SetMoveCounter (moveCount);
		cam.SetPlayerAsTarget ();
		turnInProgress = false;
		player.GetStartPosition ();

		if (!firstTurn) {
			gameController.IncrementTurnCount ();
		}
	}

	private void GetDirection(){
		validDirection = false;

		while (!validDirection) {
			directionIndex = Random.Range (0, 4);

			if (difficultyLevel == 1) {
				CalculatePositionEasy ();
			} else if (difficultyLevel == 2) {
				advancedMovePos = CalculatePositionNormal ();

				if (advancedMovePos == Vector3.zero) {
					CalculatePositionEasy ();
				} else {
					validDirection = true;
				}
			} else if (difficultyLevel == 3){
				advancedMovePos = CalculatePositionExpert ();

				if (advancedMovePos == Vector3.zero) {
					CalculatePositionEasy ();
				} else {
					validDirection = true;
				}
			}
		}
	}

	private void CalculatePositionEasy(){
		switch (directionIndex) {
		case 0: // Up
			if (transform.position.y != gameController.mapSize.y) {
				validDirection = true;
			}
			break;
		case 1: // Right
			if (transform.position.x != gameController.mapSize.x) {
				validDirection = true;
			}
			break;
		case 2: // Down
			if (transform.position.y != 1) {
				validDirection = true;
			}
			break;
		case 3: // Left				
			if (transform.position.x != 1) {
				validDirection = true;
			}
			break;
		}
	}

	public Vector3 CalculatePositionNormal(){
		advancedMovePos = Vector3.zero;
		List<GameObject> adjacentTiles = GetComponentInChildren<TileDetector> ().tilesInRadius;

		if (adjacentTiles.Count > 0) {
			foreach (GameObject tile in adjacentTiles) {
				if (tile.tag == "PositiveTile") {
					advancedMovePos = tile.transform.position;
					break;
				}
			}

			if (advancedMovePos == Vector3.zero) {
				advancedMovePos = adjacentTiles [Random.Range (0, adjacentTiles.Count)].transform.position;
			}
		}

		return advancedMovePos;
	}

	public Vector3 CalculatePositionExpert(){
		advancedMovePos = Vector3.zero;
		List<GameObject> adjacentTiles = GetComponentInChildren<TileDetector> ().tilesInRadius;
		int digitValue = 0;
		int highestDigit = 0;

		if (adjacentTiles.Count > 0) {
			foreach (GameObject tile in adjacentTiles) {
				if (tile.tag == "PositiveTile" && tile.tag != "PlayerRestaurant") {

					digitValue = tile.GetComponent<TileController> ().value;

					if (digitValue > highestDigit) {
						tileToMoveTo = tile;
						highestDigit = digitValue;
					}
				}
			}

			if (tileToMoveTo == null) {
				advancedMovePos = adjacentTiles [Random.Range (0, adjacentTiles.Count)].transform.position;
			} else {
				advancedMovePos = tileToMoveTo.transform.position;
			}
		}

		return advancedMovePos;
	}
}
