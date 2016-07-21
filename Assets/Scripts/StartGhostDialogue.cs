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
								audio.clip = dialogue[0];
								audio.Play();
								spiritGuideController.flags["bed"] = true;
								}
								break;
						case "theater":
							if(!spiritGuideController.flags["theater"]){
								audio.clip = dialogue[2];
								spiritGuideController.flags["theater"] = true;
								}
							break;
							
					}

					spiritGuideController.Appear(gameObject.transform.position + new Vector3(0, 0, 0.5f), audio.clip.length);
				}
				else {
					lookedAtObjectForTime = 0;
				}
			}
			else {
				lookedAtObjectForTime = 0;
			}
		}
	}
}
