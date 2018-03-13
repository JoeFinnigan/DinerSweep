using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDetector : MonoBehaviour {
	public List<GameObject> tilesInRadius = new List<GameObject> ();

	private void OnTriggerEnter2D(Collider2D collider){
		if ((collider.tag == "PositiveTile" || collider.tag == "NegativeTile" || collider.tag == "Powerup") || 
			(collider.tag == "PlayerRestaurant" && tag == "Enemy")){
			if (collider.tag == "PlayerRestaurant" && collider.GetComponent<RestaurantActivity> ().starRating == 0) {
				// Do nothing
			} else {
				tilesInRadius.Add (collider.gameObject);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collider){
		if ((collider.tag == "PositiveTile" || collider.tag == "NegativeTile" || collider.tag == "Powerup") ||
			(collider.tag == "PlayerRestaurant" && tag == "Enemy")){
			tilesInRadius.Remove (collider.gameObject);
		}
	}
}
