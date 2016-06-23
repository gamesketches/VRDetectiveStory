using UnityEngine;
using System.Collections;

public class ObjectHandling : MonoBehaviour {

	GameObject intersectingObject;
	Director director;
	int controllerIndex;
	// Use this for initialization
	void Start () {
		director = gameObject.transform.parent.transform.parent.GetComponent<Director>();
		intersectingObject = null;
		controllerIndex = (int)gameObject.transform.parent.gameObject.GetComponent<SteamVR_TrackedObject>().index;
	}
	
	// Update is called once per frame
	void Update () {
		var device = SteamVR_Controller.Input(controllerIndex);
		if(device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger)) {
			if(intersectingObject.transform.parent != null) {
				intersectingObject.transform.parent = null;
				intersectingObject.GetComponent<Rigidbody>().useGravity = true;
				if(ObjectHasSceneChangeTag() && ConvertTagToInt(intersectingObject.tag) != director.layer) {
					director.ChangeScene(ConvertTagToInt(intersectingObject.tag));
				}
			}
			else {
				intersectingObject.transform.parent = gameObject.transform.parent;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if(ConvertTagToInt(other.gameObject.tag) > 0) {
			intersectingObject = other.gameObject;
		}
	}

	void OnTriggerExit(Collider other) {
		if(ConvertTagToInt(other.gameObject.tag) > 0) {
			intersectingObject = null;
		}
	}

	bool ObjectHasSceneChangeTag() {
		if(intersectingObject == null) {
			return false;
		}
		switch(intersectingObject.tag) {
			case "Untagged":
			case "Respawn":
			case "Finish":
			case "EditorOnly":
			case "MainCamera":
			case "Player":
			case "GameController":
				return false;
			case "holdable":
			case "theater":
				return true;
		}
		return false;
	}

	int ConvertTagToInt(string tag) {
		switch(tag) {
			case "Untagged":
			case "Respawn":
			case "Finish":
			case "EditorOnly":
			case "MainCamera":
			case "Player":
			case "GameController":
				return 0;
			case "holdable":
				return 1;
			case "theater":
				return 8;
		}
		return 0;
	}
}
