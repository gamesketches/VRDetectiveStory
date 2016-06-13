using UnityEngine;
using System.Collections;

public class ObjectHandling : MonoBehaviour {

	GameObject intersectingObject;
	int controllerIndex;
	// Use this for initialization
	void Start () {
		intersectingObject = null;
		controllerIndex = (int)gameObject.transform.parent.gameObject.GetComponent<SteamVR_TrackedObject>().index;
	}
	
	// Update is called once per frame
	void Update () {
		var device = SteamVR_Controller.Input(controllerIndex);
		if(device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger)) {
			intersectingObject.transform.parent = gameObject.transform.parent;
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "holdable") {
			intersectingObject = other.gameObject;
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.gameObject.tag == "holdable") {
			intersectingObject = null;
		}
	}
}
