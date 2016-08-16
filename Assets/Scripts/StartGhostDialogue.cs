using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class StartGhostDialogue : MonoBehaviour {

	AudioClip[] dialogue;
	public float baseLookTime;
	public float bedAdditionalTime;
	public float roseAdditionalTime;
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
			StartCoroutine (FadeOutTransition ());
		}
		else if(spiritGuideController.flags["outside"] && spiritGuideController.flags["theater"]) {
			audio.clip = Resources.Load<AudioClip>("Apartment/Sound/OnYourBed");
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
				if(lookedAtObjectForTime > baseLookTime) {
					switch(hit.collider.tag) {
						case "bed": 
							if(lookedAtObjectForTime > baseLookTime + bedAdditionalTime && 
												!spiritGuideController.flags["bed"]) {
								audio.clip = Resources.Load<AudioClip>("Apartment/Sound/GuideIntro");//dialogue[0];
								audio.Play();
								spiritGuideController.flags["bed"] = true;
								spiritGuideController.Appear(gameObject.transform.position, audio.clip.length);
								}
								break;
						case "theater":
							if(lookedAtObjectForTime > baseLookTime + bedAdditionalTime &&
								!spiritGuideController.flags["theater"]){
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

	IEnumerator FadeOutTransition() {
		yield return new WaitForSeconds (audio.clip.length);
		GameObject head = GameObject.Find ("Camera (head)");//gameObject.transform.parent.parent.GetChild(2).gameObject;
		Vortex theVortex = head.GetComponentInChildren<Vortex>();
		float t = 0f;
		audio.clip = Resources.Load<AudioClip>("Apartment/Sound/clockbell");
		audio.Play();
		SteamVR_Fade.View(new Color(110f / 255f, 101f / 255f, 212f / 255f, 1f), audio.clip.length / 2);
		while(t < audio.clip.length) {
			float newRad = Mathf.SmoothStep(0f, 0.5f, t / audio.clip.length);
			theVortex.radius = new Vector2(newRad, newRad);
			t += Time.deltaTime;
			yield return null;
		}
	}
}
