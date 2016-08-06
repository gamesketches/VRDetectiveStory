using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OutsideSceneDirector : MonoBehaviour {

	Animator anna;
	Animator manager;
	Animator stalker;
	public AudioSource managerAudio;
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
		manager = GameObject.Find("Manager").GetComponent<Animator>();
		stalker = GameObject.Find("Stalker").GetComponent<Animator>();
		beats = new List<Beat>();
		beats.Add(Beat1);
		beats.Add(Beat2);
		beats.Add(Beat3);
		beats.Add(Beat4);
		beats.Add(Beat5);
		switchingBeat = true;
		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(switchingBeat) {
			currentBeat = beats[beatNumber];
			StartCoroutine(currentBeat());
			switchingBeat = false;
		}
	}

	IEnumerator PlayDialogue(AudioSource source, string path) {
		source.clip = Resources.Load<AudioClip>(path);
		source.Play();
		while(source.isPlaying) {
		yield return null;
		}

	}

	// Anna and Manager Talk
	// Play Audio of their conversation
	IEnumerator Beat1(){
		yield return StartCoroutine(PlayDialogue(managerAudio, "outside/Sound/manager1"));
		yield return StartCoroutine(PlayDialogue(managerAudio, "outside/Sound/ring"));
		manager.SetInteger("animationId", 1);
		yield return StartCoroutine(PlayDialogue(managerAudio, "outside/Sound/manager2"));
		managerAudio.clip = Resources.Load<AudioClip>("outside/Sound/manager3");
		managerAudio.Play();
		Debug.Log(anna.GetCurrentAnimatorStateInfo(0).length);
		//yield return new WaitForSeconds(anna.GetCurrentAnimatorStateInfo(0).length);
		beatNumber++;
		switchingBeat = true;
	}

	IEnumerator Beat2() {
		float t = 0;
		// Rotate towards car
		while(t < 1) {
			anna.gameObject.transform.Rotate(new Vector3(0, 60, 0) * Time.deltaTime);
			t += Time.deltaTime;
			yield return null;
		}
		// Begin walk animation
		anna.SetInteger("animationId", 1);
		anna.SetBool("finished", true);
		yield return null;
		Vector3 startPosition = new Vector3(4.52f, 0.6f, -1.5f);
		Vector3 endPosition = new Vector3(-8.32f, 0.6f, -6.08f);
		t = 0;
		// Actually move model
		while(t < 1) {
			anna.gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, t);
			t += (Time.deltaTime / 3);
			yield return null;
		}
		beatNumber++;
		switchingBeat = true;
	}

	IEnumerator Beat3() {
		float t = 0;
		// Start Reaching Animation
		anna.SetInteger("animationId", 2);
		while(t < 1) {
			anna.gameObject.transform.Rotate(new Vector3(0, -60, 0) * Time.deltaTime);
			t += Time.deltaTime;
			yield return null;
		}
		yield return new WaitForSeconds(anna.GetCurrentAnimatorStateInfo(0).length);
		beatNumber++;
		switchingBeat = true;
	}

	IEnumerator Beat4() {
		// Read animation would go here
		anna.SetInteger("animationId", 3);
		yield return new WaitForSeconds(anna.GetCurrentAnimatorStateInfo(0).length);
		beatNumber++;
		switchingBeat = true;
	}

	IEnumerator Beat5() {
		float t = 0;
		anna.SetInteger("animationId", 4);
		stalker.SetInteger("animationId", 1);
		stalker.gameObject.transform.Rotate(new Vector3(0, 53, 0));
		Vector3 startPosition = stalker.gameObject.transform.position;
		Vector3 endPosition = new Vector3 (-25.1f, 0.6f, -15.42f);
		yield return StartCoroutine(PlayDialogue(audio, "outside/Sound/spirit8"));
		while(t < 1) {
			stalker.gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, t);
			t += (Time.deltaTime / 3);
			yield return null;
		}

		stalker.SetInteger("animationId", 2);
		audio.clip = Resources.Load<AudioClip>("outside/Sound/spirit9");
		audio.Play();
		Debug.Log("spirit9");
	}
}
