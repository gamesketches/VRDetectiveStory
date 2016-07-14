using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TransferBack : MonoBehaviour {


	int controllerIndex;
	public string sceneName;
	// Use this for initialization
	void Start () {
		controllerIndex = (int)gameObject.transform.parent.gameObject.GetComponent<SteamVR_TrackedObject>().index;
	}
	
	// Update is called once per frame
	void Update () {
		var device = SteamVR_Controller.Input(controllerIndex);
		if(device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger)) {
			SceneManager.LoadScene(sceneName);
			}
	}
}
