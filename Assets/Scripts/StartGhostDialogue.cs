using UnityEngine;
using System.Collections;

public class StartGhostDialogue : MonoBehaviour {

	public AudioClip dialogue;
	public float lookTime;
	private float lookedAtBedForTime;
	AudioSource audio;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
		lookedAtBedForTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, 5f)) {
			if(hit.collider.tag == "bed") {
				lookedAtBedForTime += Time.deltaTime;
				if(lookedAtBedForTime > lookTime) {
				audio.clip = dialogue;
				audio.Play();
				}
			}
		}
		else {
			lookedAtBedForTime = 0;
		}
	}
}
