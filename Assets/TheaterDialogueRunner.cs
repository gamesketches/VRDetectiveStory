using UnityEngine;
using System.Collections;

public class TheaterDialogueRunner : MonoBehaviour {

	AudioSource audio;
	AudioClip[] audioClips;
	public float timeBetweenClips;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
		audioClips = Resources.LoadAll<AudioClip>("theater");
		StartCoroutine(PlayAudio());
	}
	
	IEnumerator PlayAudio() {
		yield return new WaitForSeconds(1.0f);
		foreach(AudioClip clip in audioClips) {
			audio.clip = clip;
			audio.Play();
			yield return new WaitForSeconds(timeBetweenClips + clip.length);
		}
		
	}
}
