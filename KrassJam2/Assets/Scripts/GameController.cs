using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public GameObject player, enemy, baseTile, powerup, groundEffect;
	public GameObject[] playableTiles;
	public Vector2 mapSize;
	public Transform gridTileParent;
	public AudioClip placementSound;

	public List<Coordinate> activeTiles, inactiveTiles;

	public int totalTurnLimit, currentTurn, tileRespawnRate;
	private bool gameOverTriggered;
	private static GameStatus gameStatus = new GameStatus();

	private int turnsUntilRespawn, tileRespawnAmount;
	private string endTitle, endDescription;
	private bool gameEnded;
	//private int powerupSpawnRate, powerupSpawnCounter;

	void Start(){
		gameEnded = false;
		gameOverTriggered = false;
		currentTurn = 1;
		turnsUntilRespawn = tileRespawnRate;
		totalTurnLimit = PlayerPrefController.GetTurnLimit ();

		GenerateMap();
		DecideFirstTurn ();
	
		//SetPowerupSpawnRate ();
	}

	private void DecideFirstTurn(){
		int random = Random.Range (0, 2);

		if (random == 0) {
			enemy.GetComponent<EnemyController>().firstTurn = true;
			enemy.GetComponent<EnemyController> ().PlayEnemyTurn ();
		}
		
	}

	private void Update(){
		if (turnsUntilRespawn <= 0) {
			RespawnTiles ();
			turnsUntilRespawn = tileRespawnRate;
		}

		//if (powerupSpawnCounter <= 0) {
		//	SpawnPowerup ();
		//	SetPowerupSpawnRate ();
		//}

		if (currentTurn > totalTurnLimit && !gameOverTriggered) {
			gameOverTriggered = true;
			gameStatus.status = GameStatus.Status.COMPLETED_TURNCOUNT;
			EndGame ();
		}

		if (player.GetComponent<DigitController> ().startingDigits <= -250 && !gameOverTriggered) {
			gameOverTriggered = true;
			gameStatus.status = GameStatus.Status.COMPLETED_DEBT;
			EndGame ();
		}
	}

	private void GenerateMap(){
		activeTiles = new List<Coordinate> ();
		inactiveTiles = new List<Coordinate> ();

		Vector3 playerPos = player.transform.position;
		Vector3 enemyPos = enemy.transform.position;

		// Setting up main tile grid
		for (int x = 1; x <= mapSize.x; x++) {
			for (int y = 1; y <= mapSize.y; y++) {
				Vector3 finalTilePos = new Vector3 (x, y, -0.5f); 

				GameObject newBaseTile = Instantiate (baseTile, finalTilePos, Quaternion.identity) as GameObject;
				newBaseTile.transform.SetParent (gridTileParent);
				newBaseTile.transform.localScale *= 0.96f;

				if (finalTilePos != enemyPos && finalTilePos != playerPos) {
					Instantiate (playableTiles [Random.Range (0, playableTiles.Length)], finalTilePos, Quaternion.identity);
				}

				activeTiles.Add (new Coordinate (finalTilePos));
			}
		}
	}

	private void RespawnTiles(){
		int i = 0;
		tileRespawnAmount = inactiveTiles.Count / 4;

		while (i < tileRespawnAmount) {
			int random = Random.Range (0, inactiveTiles.Count);
			Vector3 inactiveTilePos = inactiveTiles [random].position;

			if (inactiveTilePos != player.transform.position && inactiveTilePos != enemy.transform.position) {
				GameObject newTile = Instantiate (playableTiles [Random.Range (0, playableTiles.Length)], inactiveTilePos, Quaternion.identity) as GameObject;
				GameObject newGroundEffect = Instantiate (groundEffect, newTile.transform.position, Quaternion.identity) as GameObject;
				AudioSource.PlayClipAtPoint (placementSound, transform.position);

				inactiveTiles.Remove (inactiveTiles [random]);
				activeTiles.Add (new Coordinate (inactiveTilePos));
				i++;
			}
		}
	}

//	private void SetPowerupSpawnRate(){
//		powerupSpawnRate = Random.Range (4, 8);
//		powerupSpawnCounter = powerupSpawnRate;
//	}
//
//	private void SpawnPowerup(){
//		Vector3 randomPos = new Vector3 ((int)Random.Range (1, mapSize.x), (int)Random.Range (1, mapSize.y), -0.5f);
//		string tileName = "PlayableTile (" + randomPos.x + ", " + randomPos.y + ")";
//
//		GameObject tileToDestroy = GameObject.Find (tileName);
//		Destroy (tileToDestroy);
//
//		GameObject newPowerup = Instantiate (powerup, randomPos, Quaternion.identity) as GameObject;
//		newPowerup.name = tileName;
//	}

	public void MakeTileInactiveInList(Vector3 pos){
		foreach (Coordinate tile in activeTiles) {
			if (tile.position == pos) {
				activeTiles.Remove (tile);
				inactiveTiles.Add (new Coordinate (pos));
				break;
			}
		}
	}

	public void IncrementTurnCount(){
		currentTurn++;
		//powerupSpawnCounter--;
		turnsUntilRespawn--;

		UIController.instance.SetTurnText (currentTurn);
	}

	public void FullBarReached(bool reachedByPlayer){
		if (!gameOverTriggered) {
			gameOverTriggered = true;

			if (reachedByPlayer) {
				gameStatus.status = GameStatus.Status.COMPLETED_HUNDREDPERCENT;
			} else {
				gameStatus.status = GameStatus.Status.COMPLETED_ZEROPERCENT;
			}
		}

		EndGame ();
	}

	public void EndGame(){
		if (!gameEnded) {
			switch (gameStatus.status) {
			case GameStatus.Status.COMPLETED_TURNCOUNT:
				endTitle = "Time's Up!";
				endDescription = "Despite valiant efforts made by both yourself and your rival, neither of you were able to establish complete dominance this time.";
				break;
			case GameStatus.Status.COMPLETED_DEBT:
				endTitle = "Erm...";
				endDescription = "So, yeah. Somehow, you lost so much money that you were forced to sell your remaining assets and live out the rest of your days " +
				"as an internet streamer playing whatever is popular right now.\n\n(Developer Note: Honestly I'm pretty sure you did this on purpose)";
				break;
			case GameStatus.Status.COMPLETED_ZEROPERCENT:
				endTitle = "Most Unpopular Guy In Town";
				endDescription = "All of your customers left you. Yep, literally every single one.\n\nAfter a suspiciously detailed anonymous report of a salmonella outbreak, " +
				"along with a heavily photoshopped picture of a menu displaying the word aubergine as 'aborgine' going viral, protests were made and you were forced " +
				"to leave with your tail between your legs. Better luck next time!";
				break;
			case GameStatus.Status.COMPLETED_HUNDREDPERCENT:
				endTitle = "Who Needs Competition?";
				endDescription = "With the entire town showing you their support (and money), the mystery businessman had no choice but to end " +
				"his attempt at ending your reign in the world of dining.\n\nIt was also revealed that he is not actually a businessman, but rather a fast food worker with rich parents.";
				break;
			default:
				Debug.LogError ("Shouldn't be here! Investigate");
				break;
			}

			UIController.instance.EndGame (endTitle, endDescription);
			gameEnded = true;
		}
	}

	public struct Coordinate{
		public Vector3 position;

		public Coordinate(Vector3 _position){
			position = _position;
		}
	}
}
