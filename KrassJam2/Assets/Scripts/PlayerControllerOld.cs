using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOld : MonoBehaviour {
	public int moveCount;
	public bool moveLocked;
	public List<GameObject> tilesTakenThisTurn;

	private Vector3 screenPoint, offset, oldPos;
	private EnemyController enemy;
	private GameController game;
	private bool hitEnemy;

	void Start(){
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

	public void EndOfPlayerTurn(){
		if (enemy.firstTurn) {
			game.IncrementTurnCount ();
		}

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
				oldPos = transform.position;
				Vector3 enemyPos = enemy.gameObject.transform.position;

				Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

				Vector3 currentPos = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
				currentPos.x = Mathf.Round (Mathf.Clamp (currentPos.x, 1, 16));
				currentPos.y = Mathf.Round (Mathf.Clamp (currentPos.y, 1, 16));

				// Block any movement beyond single N/E/S/W movement
				if (currentPos.x == transform.position.x || currentPos.y == transform.position.y) {
					currentPos = new Vector3 (Mathf.Clamp (currentPos.x, oldPos.x - 1, oldPos.x + 1),
						Mathf.Clamp (currentPos.y, oldPos.y - 1, oldPos.y + 1), 
						currentPos.z);
				
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
				Vector3 enemyPos = enemy.gameObject.transform.position;

				Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

				Vector3 currentPos = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
				currentPos.x = Mathf.Round (Mathf.Clamp (currentPos.x, 1, 16));
				currentPos.y = Mathf.Round (Mathf.Clamp (currentPos.y, 1, 16));

				// Block any movement beyond single N/E/S/W movement
				if (currentPos.x == transform.position.x || currentPos.y == transform.position.y) {
					currentPos = new Vector3 (Mathf.Clamp (currentPos.x, oldPos.x - 5, oldPos.x + 5),
						Mathf.Clamp (currentPos.y, oldPos.y - 5, oldPos.y + 5), 
						currentPos.z);

					if (enemyPos == currentPos) {
						hitEnemy = true;
					} else {
						hitEnemy = false;
						transform.position = currentPos;

						if (transform.position != oldPos) {
							int xDistance = (int) Mathf.Abs(transform.position.x - oldPos.x);
							int yDistance = (int) Mathf.Abs(transform.position.y - oldPos.y);

							moveCount = 5 - (xDistance + yDistance);
							UIController.instance.SetMoveCounter (moveCount);

							if (moveCount <= 0) {
								moveLocked = true;
							}
						}
					}
				}
			}

			break;
		}
	}

	private void HoldAndDragChecks(){
		
	}
}
