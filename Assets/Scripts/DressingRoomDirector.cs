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
		audio = GetComponent<AudioSource>();
		anna = GameObject.Find("Anna").GetComponent<Animator>();
		sara = GameObject.Find("Sara").GetComponent<Animator>();
		beats = new List<Beat>();
		beats.Add(Beat1);
		beats.Add(Beat2);
		beats.Add(Beat3);
		beats.Add(Beat4);
		switchingBeat = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(switchingBeat) {
			Debug.Log("switched to beat" + beatNumber.ToString());
			currentBeat = beats[beatNumber];
			StartCoroutine(currentBeat());
			switchingBeat = false;
		}
	}

	void nextBeat() {
		beatNumber++;
		switchingBeat = true;
	}

	IEnumerator moveCharacter(Animator character, Vector3 endPos) {
		Transform charTransform = character.gameObject.transform;
		Vector3 startPos = charTransform.position;
		float t = 0;
		while(t < 1) {
			charTransform.position = Vector3.Lerp(startPos, endPos, t);
			t += Time.deltaTime;
			yield return null;
		}
	}

	// Sara walks in
	IEnumerator Beat1() {
		yield return StartCoroutine(moveCharacter(sara, new Vector3(-3.788f, 7.25f, -5.066f)));

		// Sara looks around
		sara.SetInteger("animationId", 1);

		// Sound of anna coming
		audio.clip = Resources.Load<AudioClip>("Dressing/Sound/annasstepsdressingroom");
		Debug.Log(audio.clip.name);
		audio.Play();

		while(audio.isPlaying) {
			yield return null;
		}
		nextBeat();
	}

	IEnumerator Beat2() {
		sara.SetInteger("animationId", 2);
		moveCharacter(sara, new Vector3(4.194f, 7.25f, -4.745f));
		yield return moveCharacter(anna, new Vector3(-3.788f, 7.25f, -5.066f));
		anna.SetInteger("animationId", 1);
		yield return new WaitForSeconds(anna.GetCurrentAnimatorStateInfo(0).length);
		nextBeat();
	}

	// Sara does her shifty pill replacement
	IEnumerator Beat3() {

		anna.SetInteger("animationId", 2);
		yield return new WaitForSeconds(anna.GetCurrentAnimatorStateInfo(0).length);
		anna.SetInteger("animationId", 3);
		sara.SetInteger("animationId", 3);
		yield return new WaitForSeconds(sara.GetCurrentAnimatorStateInfo(0).length);
		sara.SetInteger("animationId", 4);
		yield return new WaitForSeconds(sara.GetCurrentAnimatorStateInfo(0).length);
		sara.SetInteger("animationId", 5);
		yield return new WaitForSeconds(sara.GetCurrentAnimatorStateInfo(0).length);
		sara.SetInteger("animatinoId", 6);
		yield return new WaitForSeconds(sara.GetCurrentAnimatorStateInfo(0).length);

		nextBeat();
	}

	IEnumerator Beat4() {
		// Play dialogue
		sara.SetInteger("animationId", 7);
		yield return new WaitForSeconds(sara.GetCurrentAnimatorStateInfo(0).length);
		sara.SetInteger("animationId", 8);
		yield return moveCharacter(sara,  new Vector3(-4.21f, 7.21f, -7.585f));
	}
}
