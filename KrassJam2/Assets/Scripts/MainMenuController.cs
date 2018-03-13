using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {
	public Text difficultyText, turnsText;
	public Text versionText;
	public Slider difficultySlider, turnsSlider, volumeSlider;
	public Toggle tutorialToggle;
	public GameObject playPanel, mainPanel, creditsPanel, jamPanel, optionsPanel;

	private int turnMultiplier;
	private float volumeBeforeMute;
	private bool volumeMute;

	private void Awake(){
		if (PlayerPrefController.GetInitialGameStatus () != 1) {
			ApplyDefaultSettings ();
		}
	}

	private void Start(){
		volumeMute = false;
		versionText.text = "v" + Application.version;
		turnMultiplier = 20;

		volumeSlider.value = PlayerPrefController.GetVolume ();
		difficultySlider.value = PlayerPrefController.GetDifficultyLevel ();
		turnsSlider.value = PlayerPrefController.GetTurnLimit () / turnMultiplier;
		if (PlayerPrefController.GetTutorialToggle () != 1) {
			tutorialToggle.isOn = false;
		} else {
			tutorialToggle.isOn = true;
		}
	}

	private void ApplyDefaultSettings(){
		PlayerPrefController.SetTurnLimit (40);
		PlayerPrefController.SetDifficultyLevel (2);
		PlayerPrefController.SetTutorialToggle (1);
		PlayerPrefController.SetVolume (1);
		PlayerPrefController.SetInitialGameStatus (1);
	}

	public void SetDifficulty(){
		int difficultyLevel = (int)difficultySlider.value;

		switch (difficultyLevel) {
		case 1: // Easy
			difficultyText.text = "Easy";
			break;
		case 2: // Normal
			difficultyText.text = "Normal";
			break;
		case 3: // Hard
			difficultyText.text = "Hard";
			break;
		}

		PlayerPrefController.SetDifficultyLevel (difficultyLevel);
	}

	public void SetTurnLimit(){
		int turnLimit = (int)turnsSlider.value * turnMultiplier;


		if (turnsSlider.value == turnsSlider.maxValue) {
			turnsText.text = "Unlimited";
			PlayerPrefController.SetTurnLimit (100000);
		} else {
			turnsText.text = turnLimit.ToString ();
			PlayerPrefController.SetTurnLimit (turnLimit);
		}

	}

	public void ToggleTutorials(bool active){
		if (active) {
			PlayerPrefController.SetTutorialToggle (1);
		} else {
			PlayerPrefController.SetTutorialToggle (0);
		}
	}

	public void ToggleHoldToDrag(bool holdToDrag){
		if (holdToDrag){
			PlayerPrefController.SetDragHold (1);
		} else {
			PlayerPrefController.SetDragHold(0);
		}
	}

	public void ShowTutorialPanel(){
		mainPanel.SetActive (false);
		playPanel.SetActive (true);
	}

	public void HideTutorialPanel(){
		playPanel.SetActive (false);
		mainPanel.SetActive (true);
	}

	public void ShowCredits(){
		mainPanel.SetActive (false);
		creditsPanel.SetActive (true);
	}

	public void HideCredits(){
		creditsPanel.SetActive (false);
		mainPanel.SetActive (true);
	}

	public void ShowOptions(){
		mainPanel.SetActive(false);
		optionsPanel.SetActive(true);
	}

	public void HideOptions(){
		optionsPanel.SetActive (false);
		mainPanel.SetActive (true);
	}

	public void ShowJamPanel(){
		jamPanel.SetActive (true);
	}

	public void HideJamPanel(){
		jamPanel.SetActive (false);
	}

	public void QuitGame(){
		Application.Quit ();
	}

	public void ToggleVolumeMute(){
		volumeMute = !volumeMute;

		if (volumeMute) {
			volumeBeforeMute = MusicManager.instance.mainTrack.volume;
			MusicManager.instance.mainTrack.volume = 0f;
			volumeSlider.value = 0f;

		} else {
			MusicManager.instance.mainTrack.volume = volumeBeforeMute;
			volumeSlider.value = volumeBeforeMute;
		}
	}
}
