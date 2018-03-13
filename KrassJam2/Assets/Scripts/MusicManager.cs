using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour {
	public AudioSource mainTrack;
	public Sprite audioOnSprite, audioOffSprite;

	public static MusicManager instance;

	void Awake(){
		if (instance == null){
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy(gameObject);
		}

		mainTrack = GetComponent<AudioSource> ();
	}

	void Start () {
		AudioListener.volume = 0.35f;
		mainTrack.volume = PlayerPrefController.GetVolume ();
	}

	public void SetVolume(float volume){
		PlayerPrefController.SetVolume(volume);
		mainTrack.volume = PlayerPrefController.GetVolume ();

		if (volume == 0) {
			GameObject.Find ("VolumeImage").GetComponent<Image> ().sprite = audioOffSprite;
		} else {
			GameObject.Find ("VolumeImage").GetComponent<Image> ().sprite = audioOnSprite;
		}
	}
}