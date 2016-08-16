using UnityEngine;
using System.Collections;

public class StairBehavior : MonoBehaviour {

	private GameObject[] sceneObjects;
	public float targetY;
	public float bottomY;
	private bool entered;
	static float deltaY;
	private float centerY;
	// Use this for initialization
	void Start () {
		entered = false;
		deltaY = 0;
		var goList = new System.Collections.Generic.List<GameObject>();
		GameObject[] allObjects = FindObjectsOfType<GameObject>();
		foreach(GameObject obj in allObjects){
			if(obj.tag != "Player" && obj.activeInHierarchy && obj.transform.parent == null) {
				goList.Add(obj);
			}
		}
		sceneObjects = goList.ToArray();

		centerY = gameObject.transform.GetComponent<BoxCollider>().center.y + transform.position.y;//.size.y / 2) + transform.position.y;
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player" && !entered) {
			entered = true;
			float dif = Mathf.Abs(targetY - deltaY);
			if(deltaY < centerY) {//if(targetY > deltaY) {
				deltaY = dif;
				foreach(GameObject obj in sceneObjects) {
				if(obj != null) {
					Vector3 temp = obj.transform.position;
					temp.y = temp.y - dif;
					//obj.transform.position = temp;
					StartCoroutine(StairMovement(obj.transform, temp));
					}
			}
			}
			else {//if(targetY < deltaY) {
				Debug.Log("should be going down");
				dif = deltaY;
				deltaY = 0;
				foreach(GameObject obj in sceneObjects) {
				Vector3 temp = obj.transform.position;
				temp.y = temp.y + dif;
				//obj.transform.position = temp;
				StartCoroutine(StairMovement(obj.transform, temp));
			}
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.gameObject.tag == "Player") {
			entered = false;
		}
	}

	IEnumerator StairMovement(Transform oldPosition, Vector3 newPosition) {
		float t = 0;
		Vector3 oldVec3 = oldPosition.position;
		while(t < 1) {
			oldPosition.position = Vector3.Lerp(oldVec3, newPosition, t);
//										new Vector3(Mathf.SmoothStep(oldVec3.x, newPosition.x, t),
//										Mathf.SmoothStep(oldVec3.y, newPosition.y, t),
//										Mathf.SmoothStep(oldVec3.z, newPosition.z, t));
			t += Time.deltaTime * 3;
			yield return null;
		}
	}
}
