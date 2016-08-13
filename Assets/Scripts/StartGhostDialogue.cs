using UnityEngine;
using System.Collections;

public class StartGhostDialogue : MonoBehaviour {

	AudioClip[] dialogue;
	public float lookTime;
	private float lookedAtObjectForTime;
	SpiritGuideController spiritGuideController;
	AudioSource audio;
	// Use this for initialization
	void Start () {
		dialogue = Resources.LoadAll<AudioClip>("Apartment/Sound");
		audio = GetComponent<AudioSource>();
		lookedAtObjectForTime = 0;
		spiritGuideController = GameObject.Find("SpiritGuide").GetComponent<SpiritGuideController>();
		if(spiritGuideController.CheckGameOverState()){
			audio.clip = Resources.Load<AudioClip>("Apartment/Sound/endingSpeech");//dialogue[dialogue.Length - 1];
			audio.Play();
		}
		else if(spiritGuideController.flags["theater"]){
			audio.clip = Resources.Load<AudioClip>("Apartment/Sound/StartExploring");
			audio.Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, 5f)) {
			if(hit.collider.gameObject.layer == 9) {
				lookedAtObjectForTime += Time.deltaTime;
				if(lookedAtObjectForTime > lookTime) {
					switch(hit.collider.tag) {
						case "bed": 
							if(!spiritGuideController.flags["bed"]) {
								audio.clip = Resources.Load<AudioClip>("Apartment/Sound/GuideIntro");//dialogue[0];
								audio.Play();
								spiritGuideController.flags["bed"] = true;
								spiritGuideController.Appear(gameObject.transform.position, audio.clip.length);
								}
								break;
						case "theater":
							if(!spiritGuideController.flags["theater"]){
								audio.clip = Resources.Load<AudioClip>("Apartment/Sound/ControllerInstruction");//dialogue[2];
								audio.Play();
								spiritGuideController.flags["theater"] = true;
								spiritGuideController.Appear(gameObject.transform.position, audio.clip.length);
								}
							break;
						case "guide":
							spiritGuideController.FlyAway();
							break;
					}

				}
			}
			else {
				lookedAtObjectForTime = 0;
			}
		}
	}
}
