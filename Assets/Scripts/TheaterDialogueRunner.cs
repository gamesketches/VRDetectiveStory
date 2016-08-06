using UnityEngine;
using System.Collections;

public class TheaterDialogueRunner : MonoBehaviour {

	AudioSource audio;
	AudioClip[] audioClips;
	public float timeBeforeDialogueStart;
	public float timeBetweenClips;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
		audioClips = Resources.LoadAll<AudioClip>("theater");
		StartCoroutine(PlayAudio());
	}
	
	IEnumerator PlayAudio() {
		yield return new WaitForSeconds(timeBeforeDialogueStart);
		for(int i = 1; i < audioClips.Length; i++) {
			audio.clip = audioClips[i];
			audio.Play();
			yield return new WaitForSeconds(timeBetweenClips + audio.clip.length);
		}
		
	}
}
