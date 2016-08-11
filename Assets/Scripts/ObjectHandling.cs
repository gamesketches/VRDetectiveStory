using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ObjectHandling : MonoBehaviour {

	public GameObject intersectingObject;
	private GameObject clock;
	public Vector3 rosePositionWhenHeld;
	public Vector3 paperPositionWhenHeld;
	public Vector3 pillBottlePositionWhenHeld;
	private Dictionary<int, Vector3> objectPositions;
	Director director;
	int controllerIndex;
	float lastTimeAngle;
	// Use this for initialization
	void Start () {
		director = gameObject.transform.parent.transform.parent.GetComponent<Director>();
		clock = gameObject.transform.GetChild(0).gameObject;
		clock.SetActive(false);
		if(SceneManager.GetActiveScene().name == "main") {
			intersectingObject = null;
			}
		objectPositions = new Dictionary<int, Vector3>();
		objectPositions.Add(8, rosePositionWhenHeld);
		objectPositions.Add(9, paperPositionWhenHeld);
		objectPositions.Add(10, pillBottlePositionWhenHeld);
		controllerIndex = (int)gameObject.transform.parent.gameObject.GetComponent<SteamVR_TrackedObject>().index;
	}
	
	// Update is called once per frame
	void Update () {
		if(intersectingObject == null) {
			return;
		}
		var device = SteamVR_Controller.Input(controllerIndex);
		if(intersectingObject.transform.parent == gameObject.transform.parent && 
							ConvertTagToInt(intersectingObject.tag) != 1) {
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
				clock.SetActive(false);
				intersectingObject.GetComponent<Rigidbody>().useGravity = true;
				StartCoroutine(ClockSummoning(5, 1));
				if(ObjectHasSceneChangeTag()){//&& ConvertTagToInt(intersectingObject.tag) != director.layer) {
					if(intersectingObject.tag == SceneManager.GetActiveScene().name) {
					StartCoroutine(ChangeScene("main"));//SceneManager.LoadScene("main");//director.ChangeScene(0);//ConvertTagToInt(intersectingObject.tag));
					}
				}
			}
			else {
				intersectingObject.transform.parent = gameObject.transform.parent;
				int objectKey = ConvertTagToInt(intersectingObject.tag);
				intersectingObject.transform.localPosition = objectPositions[objectKey];
				intersectingObject.GetComponent<Rigidbody>().useGravity = false;
				if(objectKey != 1) {
					clock.SetActive(true);
					StartCoroutine(ClockSummoning(1, 5));
				}
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

	IEnumerator ClockSummoning(float baseSize, float targetSize) {
		float t = 0;
		while(t < 1) {
			float temp = Mathf.SmoothStep(baseSize, targetSize, t);
			Vector3 tempScale = new Vector3(temp, temp, temp);
			clock.transform.localScale = tempScale;
			t += Time.deltaTime;
			t += 1;
			yield return null;
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
		if(Mathf.Abs((newTime / 360) - timeShift.currentTime) > 0.5f) {
			return timeShift.currentTime;
		}
		timeShift.currentTime = newTime / 360;
		if(timeShift.withinTargetRange()) {
			//SceneManager.LoadScene(intersectingObject.tag);//director.ChangeScene(ConvertTagToInt("theater"));//intersectingObject.tag));
			StartCoroutine(ChangeScene(intersectingObject.tag));
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
			case "outside":
			case "dressing room":
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
			case "outside":
				return 9;
			case "dressing room":
				return 10;
		}
		return 0;
	}

	IEnumerator ChangeScene(string sceneName) {
		float t = 0f;
		SteamVR_Fade.Start(Color.black, 0f);
		while(t < 1) {
			// Apply image effect
			t += Time.deltaTime;
			yield return null;
		}
		SceneManager.LoadScene(sceneName);
	}
}
