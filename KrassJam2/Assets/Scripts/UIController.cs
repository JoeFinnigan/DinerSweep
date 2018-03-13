using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	public Text playerText, turnText;
	public Image playerBar, enemyBar;
	public Text percentageText;
	public GameObject[] moveIcons;
	public Sprite moveFull, moveEmpty;
	public Animator fadeAnim;
	public Sprite audioOff, audioOn;
	public Slider volumeSlider;

	public GameObject tutorialPanel, endMessagePanel, quitPanel;
	public Text tutorialTitleText, tutorialDescriptionText;
	public Text endTitleText, endDescriptionText;

	private TutorialController tutorial;
	private GameOver gameOver;
	private GameController game;

	private Animator anim;
	public float percentage;

	public static UIController instance;

	private void Awake(){
	//	if (instance == null) {
			instance = this;
			//DontDestroyOnLoad (gameObject);
		//} else {
		//	Destroy (gameObject);
		//}
	}

	private void Start(){
		UpdateProgressBar (0, "Player");
		tutorial = GameObject.FindObjectOfType<TutorialController> ();
		game = GameObject.FindObjectOfType<GameController> ();
		gameOver = GameObject.FindObjectOfType<GameOver> ();

		anim = GetComponent<Animator> ();
		volumeSlider.value = PlayerPrefController.GetVolume ();
	}

	private void Update(){
		if (percentage <= 0) {
			game.FullBarReached (false);
		} else if (percentage >= 100) {
			game.FullBarReached (true);
		}
	}

	public void SetStandaloneDigitTotal(int digits){
		playerText.text = "Digits: " + digits;
	}

	public void UpdateProgressBar(int digitChange, string objName){
		if (objName == "Player") {
			playerBar.rectTransform.sizeDelta = new Vector2 (playerBar.rectTransform.sizeDelta.x + (digitChange * 2), playerBar.rectTransform.sizeDelta.y);
			enemyBar.rectTransform.sizeDelta = new Vector2 (enemyBar.rectTransform.sizeDelta.x - (digitChange * 2), enemyBar.rectTransform.sizeDelta.y);
		} else {
			playerBar.rectTransform.sizeDelta = new Vector2 (playerBar.rectTransform.sizeDelta.x - (digitChange * 2), playerBar.rectTransform.sizeDelta.y);
			enemyBar.rectTransform.sizeDelta = new Vector2 (enemyBar.rectTransform.sizeDelta.x + (digitChange * 2), enemyBar.rectTransform.sizeDelta.y);
		}

	    percentage = (playerBar.rectTransform.sizeDelta.x / 1000) * 100;
		percentageText.text = percentage + "%";
	}

	public void SetMoveCounter(int moves){
		for (int i = 0; i < moveIcons.Length; i++) {
			if (moves >= i + 1) {
				moveIcons [i].GetComponent<Image> ().sprite = moveFull;
			} else {
				moveIcons [i].GetComponent<Image> ().sprite = moveEmpty;
			}
		}
	}

	public void SetTurnText(int turn){
		turnText.text = "Turn " + turn;
		anim.Play ("TurnCounter");
	}

	public void ShowQuitPrompt(){
		quitPanel.SetActive (true);
		Time.timeScale = 0;
	}

	public void HandleQuitResponse(bool quit){
		if (quit) {
			LevelManager.instance.LoadLevel ("MainMenu");
		} else {
			quitPanel.SetActive (false);
		}

		Time.timeScale = 1;
	}

	public void EndGame(string title, string description){
		endTitleText.text = title;
		endDescriptionText.text = description;

		endMessagePanel.SetActive (true);
		print ("Hello");
	}

	public void CalculateEndTotal(){
		endMessagePanel.SetActive (false);
		print ("end message going inactive...");
		gameOver.CalculateTotalText ();
	}

	public void FadeOutScreen(bool fadeOut){
		if (fadeOut) {
			fadeAnim.Play ("PartialFadeOut");
		} else {
			fadeAnim.Play ("PartialFadeIn");
		}
	}

	public void SetVolume(float volume){
		PlayerPrefController.SetVolume(volume);
		MusicManager.instance.mainTrack.volume = PlayerPrefController.GetVolume ();

		if (volume == 0) {
			GameObject.Find ("VolumeImage").GetComponent<Image> ().sprite = audioOff;
		} else {
			GameObject.Find ("VolumeImage").GetComponent<Image> ().sprite = audioOn;
		}

	}

	public void DisplayTutorialText(string title, string description){
		StopTime ();
		tutorialPanel.SetActive (true);
		tutorialTitleText.text = title;
		tutorialDescriptionText.text = description;
	}

	public void HideTutorial(){
		if (tutorial.multipleMessages) {
			tutorial.multipleMessages = false;
			tutorial.TriggerTutorialMessage (tutorial.currentID + 1);
		} else {
			tutorialPanel.SetActive (false);
			ResumeTime ();
			tutorial.SetInactive ();
		}
	}

	public void StopTime(){
		FadeOutScreen (true);
		Time.timeScale = 0;
	}

	public void ResumeTime(){
		FadeOutScreen (false);
		Time.timeScale = 1;
	}
}
