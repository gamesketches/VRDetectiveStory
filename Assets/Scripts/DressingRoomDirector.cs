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
	GameObject pillBottle;
	public Transform pillBottlePlacement;
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
		pillBottle = GameObject.Find("pillbottle");
		pillBottle.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(switchingBeat) {
			Debug.Log("switched to beat" + (beatNumber + 1).ToString());
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
			t += Time.deltaTime / 3;
			yield return null;
		}
	}

	IEnumerator RotateCharacter(Animator character, Vector3 rotation) {
		float t = 0;
		while(t < 1) {
			character.gameObject.transform.Rotate(rotation * Time.deltaTime);
			t += Time.deltaTime;
			yield return null;
		}
	}

	// Sara walks in
	IEnumerator Beat1() {
		//yield return new WaitForSeconds(8f);
		yield return StartCoroutine(moveCharacter(sara, new Vector3(-4.194f, 7.24f, -8.53f)));
		yield return StartCoroutine(RotateCharacter(sara, new Vector3(0f, -90f, 0f)));
		yield return StartCoroutine(moveCharacter(sara, new Vector3(-3.788f, 7.25f, -5.066f)));

		// Sara looks around
		sara.SetInteger("animationId", 1);

		// Sound of anna coming
		yield return StartCoroutine(PlayAudioFile(Resources.Load<AudioClip>("Dressing/Sound/ThatsWeird")));

		nextBeat();
	}

	IEnumerator Beat2() {
		yield return StartCoroutine(RotateCharacter(sara, new Vector3(0f, -30f, 0f)));
		sara.SetInteger("animationId", 2);
		yield return StartCoroutine(moveCharacter(sara, new Vector3(-4.174f, 7.23f, -4.346f)));
		sara.SetInteger("animationId", 3);
		yield return StartCoroutine(moveCharacter(anna, new Vector3(-4.194f, 7.24f, -8.53f)));
		yield return StartCoroutine(RotateCharacter(anna, new Vector3(0f, -90f, 0f)));
		yield return StartCoroutine(moveCharacter(anna, new Vector3(-4.384f, 7.25f, -5.448f)));
		sara.gameObject.transform.Rotate(new Vector3(0, -140, 0));
		Debug.Log("rotated");
		anna.SetInteger("animationId", 1);
		yield return new WaitForSeconds(anna.GetCurrentAnimatorStateInfo(0).length);
		nextBeat();
	}

	// Sara does her shifty pill replacement
	// Start scene dialogue here?
	IEnumerator Beat3() {

		AudioClip[] dialogueClips = Resources.LoadAll<AudioClip>("Dressing/Sound");

		yield return StartCoroutine(PlayDialogue());
		anna.SetInteger("animationId", 2);
		pillBottle.SetActive(true);
		pillBottle.transform.parent = anna.GetBoneTransform(HumanBodyBones.RightHand);
		pillBottle.transform.localPosition = new Vector3(-0.059f, 0.073f, 0.001f);
		pillBottle.transform.localRotation = Quaternion.Euler(new Vector3(286.4384f, 291.4244f, 232.6425f));
		yield return new WaitForSeconds(1);//anna.GetCurrentAnimatorStateInfo(0).length);
		Debug.Log("initial dialogue over");
		anna.SetInteger("animationId", 3);
		sara.SetInteger("animationId", 4);
		yield return new WaitForSeconds(1);//sara.GetCurrentAnimatorStateInfo(0).length);
		pillBottle.transform.parent = sara.GetBoneTransform(HumanBodyBones.RightHand);
		pillBottle.transform.localPosition = new Vector3(-0.0488f, 0.0281f, 0.0294f);
		pillBottle.transform.localRotation = Quaternion.Euler(new Vector3(72.52068f, 225.0405f, 46.9778f));
		sara.SetInteger("animationId", 5);
		yield return StartCoroutine(PlayAudioFile(dialogueClips[3]));
		yield return StartCoroutine(PlayAudioFile(dialogueClips[10]));
		audio.clip = dialogueClips[4];
		audio.Play();
		yield return new WaitForSeconds(sara.GetCurrentAnimatorStateInfo(0).length);
		sara.SetInteger("animationId", 6);
		float t = 0;
		while(t < 1) {
			sara.gameObject.transform.Rotate(new Vector3(0, -180, 0) * Time.deltaTime);
			t += Time.deltaTime;
			yield return null;
		}

		Debug.Log(sara.GetCurrentAnimatorStateInfo(0).length);
		yield return new WaitForSeconds(sara.GetCurrentAnimatorStateInfo(0).length);
		sara.SetInteger("animationId", 7);
		Debug.Log(sara.GetCurrentAnimatorStateInfo(0).length);
		t = 0;
		while(t < 1) {
			sara.gameObject.transform.Rotate(new Vector3(0, 180, 0) * Time.deltaTime);
			t += Time.deltaTime;
			yield return null;
		}
		yield return new WaitForSeconds(sara.GetCurrentAnimatorStateInfo(0).length);

		nextBeat();
	}

	IEnumerator Beat4() {
		/*audio.clip = Resources.Load<AudioClip>("Dressing/Sound/spiritvoice10");
		audio.Play();*/
		sara.SetInteger("animationId", 8);
		yield return new WaitForSeconds(sara.GetCurrentAnimatorStateInfo(0).length / 3);
		pillBottle.transform.parent = null;
		pillBottle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
		sara.SetInteger("animationId", 9);
		yield return StartCoroutine(moveCharacter(sara, new Vector3(-4.194f, 7.24f, -8.53f)));
		yield return StartCoroutine(RotateCharacter(sara, new Vector3(0f, 70f, 0f)));
		yield return StartCoroutine(moveCharacter(sara, new Vector3(-6.87f, 7.24f, -8.53f)));
	}

	IEnumerator PlayDialogue() {
		AudioClip[] temp = Resources.LoadAll<AudioClip>("Dressing/Sound");
		AudioClip[] dialogue = new AudioClip[] {temp[8], temp[0], temp[9], temp[2]};
		foreach(AudioClip line in dialogue) {
			audio.clip = line;
			audio.Play();
			while(audio.isPlaying) {
				yield return null;
			}
		}
	}

	IEnumerator PlayAudioFile(AudioClip clip) {
		audio.clip = clip;
		audio.Play();
		while(audio.isPlaying) {
			yield return null;
		}
	}
}
