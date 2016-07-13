using UnityEngine;
using System.Collections;

public class ObjectHandling : MonoBehaviour {

	GameObject intersectingObject;
	Director director;
	int controllerIndex;
	float lastTimeAngle;
	// Use this for initialization
	void Start () {
		director = gameObject.transform.parent.transform.parent.GetComponent<Director>();
		intersectingObject = null;
		controllerIndex = (int)gameObject.transform.parent.gameObject.GetComponent<SteamVR_TrackedObject>().index;
	}
	
	// Update is called once per frame
	void Update () {
		if(intersectingObject == null) {
			return;
		}
		var device = SteamVR_Controller.Input(controllerIndex);
		if(intersectingObject.transform.parent == gameObject.transform.parent) {
			if(device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad)) {
				lastTimeAngle = -1;
			}
			else if(device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad)) {
				Vector2 coordinates = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
				lastTimeAngle = AdjustCurrentTime(coordinates.x, coordinates.y);
			}
		}
		if(device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger)) {
			if(intersectingObject.transform.parent != null) {
				intersectingObject.transform.parent = null;
				intersectingObject.GetComponent<Rigidbody>().useGravity = true;
				if(ObjectHasSceneChangeTag() && ConvertTagToInt(intersectingObject.tag) != director.layer) {
					director.ChangeScene(0);//ConvertTagToInt(intersectingObject.tag));
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

	float AdjustCurrentTime(float xCoord, float yCoord) {
		float newTime = Mathf.Rad2Deg * Mathf.Atan2(yCoord, xCoord);
		if(yCoord < 0) {
		newTime += 360f;
		}
		if(lastTimeAngle == -1) {
			return newTime;
		}
		TimeShift timeShift = intersectingObject.GetComponent<TimeShift>();
		timeShift.currentTime = newTime / 360;
		if(timeShift.withinTargetRange()) {
			director.ChangeScene(ConvertTagToInt("theater"));//intersectingObject.tag));
		}
		return newTime;
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
