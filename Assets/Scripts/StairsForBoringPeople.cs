using UnityEngine;
using System.Collections;

public class StairsForBoringPeople : MonoBehaviour {

	public Vector3 targetPosition;
	public float changeInY;
	private GameObject[] sceneObjects;
	void Start() {
		var goList = new System.Collections.Generic.List<GameObject>();
		GameObject[] allObjects = FindObjectsOfType<GameObject>();
		foreach(GameObject obj in allObjects){
			if(obj.tag != "Player" && obj.activeInHierarchy && obj.transform.parent == null) {
				goList.Add(obj);
			}
		}
		sceneObjects = goList.ToArray();
	}

	void OnTriggerEnter(Collider other) {
		//StartCoroutine(travelUpStairs(other.gameObject.transform));
		Vector3 deltaPosition = other.gameObject.transform.position - targetPosition;
		foreach(GameObject obj in sceneObjects) {
			Vector3 newPosition = obj.transform.position;
			newPosition.y -= changeInY;
			obj.transform.position = newPosition;
			//StartCoroutine(travelUpStairs(obj.transform, deltaPosition));
		}
	}

	IEnumerator travelUpStairs(Transform obj, Vector3 changeInPosition) {
		float t = 0;
		Vector3 startPos = obj.position;
		Vector3 target = startPos + changeInPosition;
		while(t < 1) {
			obj.position = new Vector3(Mathf.SmoothStep(startPos.x, target.x, t),
										Mathf.SmoothStep(startPos.x, target.y, t),
										Mathf.SmoothStep(startPos.z, target.z, t));
			t += Time.deltaTime;
			yield return null;
		}
	}
}
