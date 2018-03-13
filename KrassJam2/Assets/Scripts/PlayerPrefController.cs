using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefController : MonoBehaviour {
	const string DIFFICULTY_KEY = "difficulty";
	const string TURN_KEY = "turnlimit";
	const string TUTORIAL_KEY = "tutorial";
	const string INITIAL_KEY = "initial";
	const string VOLUME_KEY = "volume";
	const string DRAGHOLD_KEY = "dragHold";

	public static void SetDifficultyLevel(int difficulty){
		PlayerPrefs.SetInt (DIFFICULTY_KEY, difficulty);
	}

	public static int GetDifficultyLevel(){
		return PlayerPrefs.GetInt (DIFFICULTY_KEY);
	}

	public static void SetTurnLimit(int turns){
		PlayerPrefs.SetInt (TURN_KEY, turns);
	}

	public static int GetTurnLimit(){
		return PlayerPrefs.GetInt (TURN_KEY);
	}

	public static void SetTutorialToggle(int enabled){
		PlayerPrefs.SetInt(TUTORIAL_KEY, enabled);
	}

	public static int GetTutorialToggle(){
		return PlayerPrefs.GetInt(TUTORIAL_KEY);
	}

	public static void SetInitialGameStatus(int firstTimeLoad){
		PlayerPrefs.SetInt (INITIAL_KEY, firstTimeLoad);
	}

	public static int GetInitialGameStatus(){
		return PlayerPrefs.GetInt (INITIAL_KEY);
	}

	public static void SetVolume(float volume){
		PlayerPrefs.SetFloat (VOLUME_KEY, volume);
	}

	public static float GetVolume(){
		return PlayerPrefs.GetFloat (VOLUME_KEY);
	}

	public static void SetDragHold(int dragHold){
		PlayerPrefs.SetInt (DRAGHOLD_KEY, dragHold);
	}

	public static int GetDragHold(){
		return PlayerPrefs.GetInt (DRAGHOLD_KEY);
	}
}
