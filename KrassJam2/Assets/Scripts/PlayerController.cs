using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public int moveCount;
	public bool moveLocked;
	public List<GameObject> tilesTakenThisTurn;
	public Vector3 startPos;
	public GameObject movementArrow;

	private Vector3 screenPoint, offset, oldPos;
	private Vector3 currentPos, enemyPos, curScreenPoint;
	private EnemyController enemy;
	private GameController game;
	private bool hitEnemy;
	private List<Vector3> movePositions;
	private bool isBacktracking;
	private Vector3 posBeforeMove;
	private float spriteRotation;

	void Start(){
		movePositions = new List<Vector3> ();
		GetStartPosition ();

		enemy = GameObject.FindObjectOfType<EnemyController> ();
		game = GameObject.FindObjectOfType<GameController> ();

		moveLocked = false;
		tilesTakenThisTurn = new List<GameObject> ();
	}

	void OnMouseDown(){
		screenPoint = Camera.main.WorldToScreenPoint(transform.position);
		offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

		if (PlayerPrefController.GetDragHold () == 1) {
			oldPos = transform.position;
		}
	}

	void OnMouseUp(){
		if (moveLocked && !enemy.turnInProgress) {
			moveLocked = false;

			if (!hitEnemy) {
				moveCount--;
				UIController.instance.SetMoveCounter (moveCount);

				if (moveCount <= 0) {
					EndOfPlayerTurn ();
				}
			}
		}
	}

	// The position the player is in at the start of the turn.
	public void GetStartPosition(){
		startPos = transform.position;
		movePositions.Add (startPos);
	}

	public void EndOfPlayerTurn(){
		movePositions.Clear ();

		foreach (GameObject arrow in game.movementArrows) {
			Animator arrowAnim = arrow.GetComponent<Animator> ();
			arrowAnim.Play ("Arrow_FadeOut");
			Destroy (arrow, 1f);
		}

		game.movementArrows.Clear ();

		if (enemy.firstTurn) {
			game.IncrementTurnCount ();
		}


		foreach (GameObject tile in tilesTakenThisTurn) {
			Destroy (tile);
		}

		tilesTakenThisTurn.Clear ();

		enemy.PlayEnemyTurn ();
		moveCount = 5;
	}

	void FixedUpdate(){
		//		RaycastHit hit;
		//
		//		if (Physics.Raycast (transform.position, -Vector3.up, out hit)) {
		//			print ("Found an object - distance: " + hit.distance);
		//		}
	}

	void OnMouseDrag(){
		switch (PlayerPrefController.GetDragHold ()) {
		case 0:

			if (!moveLocked && !enemy.turnInProgress) {
				SetNewPosVectors ();

				// Block any movement beyond single N/E/S/W movement
				if (currentPos.x == transform.position.x || currentPos.y == transform.position.y) {
					currentPos = new Vector3 (Mathf.Clamp (currentPos.x, oldPos.x - 1, oldPos.x + 1),
											  Mathf.Clamp (currentPos.y, oldPos.y - 1, oldPos.y + 1), currentPos.z);

					if (enemyPos == currentPos) {
						hitEnemy = true;
					} else {
						hitEnemy = false;
						transform.position = currentPos;

						if (transform.position != oldPos) {
							moveLocked = true;
						}
					}
				}
			}

			break;
		case 1:
			if (!moveLocked && !enemy.turnInProgress) {
				SetNewPosVectors ();

				// Block any movement beyond single N/E/S/W movement
				if (currentPos.x == transform.position.x || currentPos.y == transform.position.y) {
					currentPos = new Vector3 (Mathf.Clamp (currentPos.x, currentPos.x - 1, currentPos.x + 1),
						Mathf.Clamp (currentPos.y, currentPos.y - 1, currentPos.y + 1), currentPos.z);

					if (enemyPos == currentPos) {
						hitEnemy = true;
					} else {
						hitEnemy = false;

						if (transform.position != currentPos) {
							posBeforeMove = transform.position;
							transform.position = currentPos;
							CheckDistanceToOriginalPos ();
						}

						if (moveCount <= 0) {
							moveLocked = true;
						}
					}
				}
			}

			break;
		}
	}

	private void CheckDistanceToOriginalPos(){
		isBacktracking = false;

		foreach (Vector3 previousMove in movePositions) {
			if (previousMove == transform.position) {
				foreach (GameObject tile in tilesTakenThisTurn) {
					if (tile.transform.position == posBeforeMove) {
						GetComponent<DigitController> ().ChangeDigitAmount (-tile.GetComponent<TileController> ().value, false);
						tile.SetActive (true);
						tilesTakenThisTurn.Remove (tile);
						break;
					}
				}

				movePositions.Remove (posBeforeMove);

				foreach (GameObject arrow in game.movementArrows) {
					if (arrow.transform.position == transform.position) {
						Destroy (arrow);
						game.movementArrows.Remove (arrow);
						break;
					}
				}

				moveCount++;
				isBacktracking = true;
				break;
			}
		}

		if (!isBacktracking) {
			moveCount--;
			movePositions.Add (transform.position);

			AddMovementArrow ();
		}

		UIController.instance.SetMoveCounter (moveCount);
	}

	private void AddMovementArrow(){
		// Work out direction of player to know what the arrow sprite's angle should be
		if (transform.position.x > posBeforeMove.x) {
			spriteRotation = 0f;
		} else if (transform.position.x < posBeforeMove.x) {
			spriteRotation = 180f;
		} else if (transform.position.y > posBeforeMove.y) {
			spriteRotation = 90f;
		} else if (transform.position.y < posBeforeMove.y) {
			spriteRotation = -90f;
		}

		GameObject newMovementArrow = Instantiate (movementArrow, posBeforeMove, Quaternion.Euler(0, 0, spriteRotation)) as GameObject;
		game.movementArrows.Add (newMovementArrow);
	}

	private void SetNewPosVectors(){
		if (PlayerPrefController.GetDragHold () == 0) {
			oldPos = transform.position;
		}

		enemyPos = enemy.gameObject.transform.position;

		curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

		currentPos = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
		currentPos.x = Mathf.Round (Mathf.Clamp (currentPos.x, 1, 16));
		currentPos.y = Mathf.Round (Mathf.Clamp (currentPos.y, 1, 16));
	}
}