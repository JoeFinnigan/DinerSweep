using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {
	public bool[] tutorialTriggered;
	public bool multipleMessages;
	public bool isActive, isEnabled;
	public int currentID;

	private GameController game;
	private string title, description;

	void Awake(){
		// If tutorials are disabled we don't need this game object
		if (PlayerPrefController.GetTutorialToggle () != 1) {
			isEnabled = false;
		} else {
			isEnabled = true;
		}
	}

	void Start () {
		if (isEnabled) {

			tutorialTriggered = new bool[8];

			UIController.instance.StopTime ();
			TriggerTutorialMessage (0);
		}

	}

	public void TriggerTutorialMessage(int id){
		isActive = true;

		if (isEnabled){
		currentID = id;

			if (!tutorialTriggered [id]) {
				tutorialTriggered [id] = true;

				switch (id) {
				case 0:
					multipleMessages = true;
					title = "Welcome!";
					description = "The town is split into different districts, each with their own numeric value. This value represents the wealth in that area - by travelling around the map, " +
					"you advertise your new business venture to potential customers. The higher the wealth, the greater the reward (or cost).\n\n" +
					"Rich (green) areas are more likely to gain customers and subsequently 'Digits' (the currency used locally).\n" +
					"Poor (red) areas will cause you to lose ground on your rival.";
					break;
				case 1:
					multipleMessages = true;
					title = "Man Vs Food";
					description = "An extremely wealthy and influencial businessman is unhappy with your endeavours " +
					"and seeks to end your career in the food industry before it even gets started.\n\nThe progress bar at the top represents your current approval rating. " +
					"This changes based on the Digits you and your opponent pick up. " +
					"Reach 100% and the businessman will reluctantly give up his efforts - lower yourself to 0%, however, and you will be forced out of town.";
					break;
				case 2:
					title = "Player Movement";
					description = "First things first - let's get moving. Click and hold the player (light blue square) to select it, then drag it to any adjacent tile to gain (or lose) the specified Digits.\n\n" +
					"You can move a total of 5 times before control is handed to your opponent.";
					break;
				case 3:
					title = "Enemy Movement";
					description = "Your rival (dark blue square) moves in the same way as you do, visiting every internet cafe in town to spread negative reviews about your business. \n" +
					"By visiting rich areas, townspeople are more likely to be bribed into never visiting your restaurants. " +
					"In poor areas, the reviews have no impact and your business continues to thrive!";
					break;
				case 4:
					title = "Selecting Restaurants";
					description = "You now have enough Digits to introduce the world to your restaurant chain!\n" +
					"To start the process, click the restaurant icon on the left-side bar at any time during your turn.";
					break;
				case 5:
					multipleMessages = true;
					title = "Placing Restaurants 1";
					description = "Restaurants are placed on any empty tile in a limited radius around your position.\n" +
					"The restaurant is given a star rating out of 5 based on the immediate surrounding area. The wealthier the area, the better the rating. " +
					"Your final restaurant count and their respective ratings impact your final score.\n\n" + 
					"Think tactically whenever you move - walking into wealthy areas may give you a short-term boost to your income and allow you to purchase " +
					"extra restaurants, but being left with 'poor' area tiles will result in restaurants with lower star ratings, giving a low final score.";
					break;
				case 6:
					title = "Placing Restaurants 2";
					description = "Empty areas will be repopulated with either rich or poor people periodically.\n\n" +
					"Left-click on any valid tile to place the restaurant on the grid. Right-click to cancel.\n\n" +
					"Note that placing a restaurant will end your turn regardless of how many moves you have left.";
					break;
				case 7:
					title = "An Unwelcomed Guest";
					description = "Sometimes your rival will bring the fight to your own turf, causing a scene at your restaurants in front of your customers.\n\n" +
					"This has a change of lowering your star rating - if the star rating is already low, it could be shut down completely!";
					break;
				}

				UIController.instance.DisplayTutorialText (title, description);
			}

			if (!isEnabled) {
				SetInactive ();
			}
		}
	}

	public void SetInactive(){
		isActive = false;
	}

}
