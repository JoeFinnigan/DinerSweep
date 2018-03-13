using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RestaurantDropper : MonoBehaviour {
	public Sprite validSprite, invalidSprite;
	public GameObject currentTile, lastTile, radiusObj;
	public AudioClip placementSound;
	public GameObject groundEffect;

	private bool canBePlaced;
	private GameObject tutorialButton;
	private int selectedColor, radiusColor, unselectedColor;

	private Vector3 screenPoint, offset;
	private Vector3 roundedPos;

	private Vector2 minBounds, maxBounds;

	private RestaurantActivity restaurantActivity;
	private RestaurantController restaurantController;
	private PlayerController player;
	private GameController game;
	private TutorialController tutorial;

	private Sprite originalSprite;

	void Start(){
		originalSprite = GetComponent<SpriteRenderer> ().sprite;

		restaurantActivity = GetComponent<RestaurantActivity> ();
		player = GameObject.FindObjectOfType<PlayerController> ();
		restaurantController = GameObject.FindObjectOfType<RestaurantController> ();
		tutorial = GameObject.FindObjectOfType<TutorialController> ();
		game = GameObject.FindObjectOfType<GameController> ();

		screenPoint = Camera.main.WorldToScreenPoint(transform.position);
		offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

		selectedColor = 105;
		radiusColor = 157;
		unselectedColor = 255;
	}

	void Update(){
		if (tutorial.isEnabled && tutorial.tutorialTriggered [5]) {
			tutorial.isActive = false;
		}

		if (!tutorial.isActive || !tutorial.isEnabled) {
			Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

			Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
			curPosition.x = Mathf.Clamp (curPosition.x, 1, 16);
			curPosition.y = Mathf.Clamp (curPosition.y, 1, 16);

			roundedPos = new Vector3 (Mathf.Round (curPosition.x), Mathf.Round (curPosition.y), -0.5f);
			transform.position = roundedPos;

			if (currentTile != null) {
				// Placement is valid
				GetPlayerRadius ();
				canBePlaced = false;

				for (int x = (int)minBounds.x; x <= (int)maxBounds.x; x++) {
						for (int y = (int)minBounds.y; y <= (int)maxBounds.y; y++) {
						Vector3 radiusPos = new Vector3 (x, y, -0.5f);

						if (roundedPos == radiusPos && player.transform.position != radiusPos) {
							if (GameObject.Find ("PlayableTile (" + x + ", " + y + ")")) {
								//SetColorToTile (radiusColor, GameObject.Find ("PlayableTile (" + x + ", " + y + ")"));
							}
							canBePlaced = true;
						}
					}
				}

				if (canBePlaced) {
					GetComponent<SpriteRenderer> ().sprite = validSprite;
				} else {
					GetComponent<SpriteRenderer> ().sprite = invalidSprite;
				}
			} else {
				canBePlaced = false;
				GetComponent<SpriteRenderer> ().sprite = invalidSprite;
			}

			// Right-click to cancel restaurant selection
			if (Input.GetMouseButtonDown (1)) {
				restaurantController.RefundRestaurantCost ();
				player.gameObject.SetActive (true);
				Destroy (gameObject);
			}

			if (Input.GetMouseButtonDown (0) && canBePlaced) {
				if (UIController.instance.tutorialPanel.activeSelf) {
					tutorialButton = UIController.instance.tutorialPanel.transform.Find ("OKButton").gameObject;
				}

				if (EventSystem.current.currentSelectedGameObject != tutorialButton || EventSystem.current.currentSelectedGameObject == null) {
					GetComponent<SpriteRenderer> ().sprite = originalSprite;
					player.EndOfPlayerTurn ();
					Instantiate (groundEffect, transform.position, Quaternion.identity);
					AudioSource.PlayClipAtPoint (placementSound, transform.position);
					GameObject newRadius = Instantiate (radiusObj, transform.position, Quaternion.identity) as GameObject;
					newRadius.transform.SetParent (transform);

					game.inactiveTiles.Remove(new GameController.Coordinate(transform.position));
					restaurantActivity.active = true;
					player.gameObject.SetActive (true);

					// Destroy tile where resturant is being placed.
					//Destroy(currentTile, 0.01f);

					// Remove this script from the restaurant object
					Destroy (this, 0.01f);
				}
			}
		}
	}

	private void GetPlayerRadius(){
		Vector2 playerPos = player.transform.position;

		minBounds = new Vector2 (playerPos.x - 2, playerPos.y - 2);
		maxBounds = new Vector2 (playerPos.x + 2, playerPos.y + 2);
	}

	void OnTriggerEnter2D(Collider2D collider){
		//Weird bug - if you move across 3 or 4 tiles quickly into a blank tile, it will show as invalid.
		if (currentTile != null) {
			//SetColorToTile (unselectedColor, currentTile);
			currentTile = null;
		}

		if (collider.tag == "PositiveTile" || collider.tag == "NegativeTile" || collider.tag == "Player") {
			currentTile = collider.gameObject;
			//SetColorToTile (selectedColor, currentTile);

			if (currentTile.tag != "Player" && !restaurantActivity.active) {
				currentTile.GetComponent<TileController> ().notSelected = false;
			}
		}
	}

	void OnTriggerExit2D(Collider2D collider){
		if ((collider.tag == "PositiveTile" || collider.tag == "NegativeTile") && !restaurantActivity.active) {
			//SetColorToTile (selectedColor, currentTile);
			collider.gameObject.GetComponent<TileController> ().notSelected = true;
		}
	}

	private void SetColorToTile(int color, GameObject tile){
		SpriteRenderer tileSprite = tile.GetComponent<SpriteRenderer> ();
		tileSprite.color = new Color (color, color, color);
	}

}
