using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DressingRoomDirector : MonoBehaviour {

	Animator anna;
	Animator sara;

	AudioSource audio;
	int beatNumber;

	delegate IEnumerator Beat();
	List<Beat> beats; 
	Beat currentBeat;
	bool switchingBeat;
	// Use this for initialization
	void Start () {
		beatNumber = 0;
		anna = GameObject.Find("Anna").GetComponent<Animator>();
		beats = new List<Beat>();
		beats.Add(Beat1);
	}
	
	// Update is called once per frame
	void Update () {
		if(switchingBeat) {
			currentBeat = beats[beatNumber];
			StartCoroutine(currentBeat());
			switchingBeat = false;
		}
	}

	IEnumerator Beat1() {
		// Do stuff
		yield return null;
	}
}
