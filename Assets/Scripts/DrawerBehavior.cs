using UnityEngine;
using System.Collections;

public class DrawerBehavior : MonoBehaviour {

	Vector3 startPosition;
	Vector3 lastFramePosition;
	public float maxZChange;
	// Use this for initialization
	void Start () {
		startPosition = gameObject.transform.position;
		lastFramePosition = startPosition;
	}

	void LateUpate() {
		Vector3 temp = startPosition;
		if(Mathf.Abs(gameObject.transform.position.z - startPosition.z) < maxZChange) {
			temp.z = gameObject.transform.position.z;
			gameObject.transform.position = temp;
			lastFramePosition = temp;
		}
		else {
			gameObject.transform.position = lastFramePosition;
		}
	}
}
