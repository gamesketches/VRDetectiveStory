using UnityEngine;
using System.Collections;

public class StairBehavior : MonoBehaviour {

	private GameObject[] sceneObjects;
	private float targetY;
	// Use this for initialization
	void Start () {
		var goList = new System.Collections.Generic.List<GameObject>();
		GameObject[] allObjects = FindObjectsOfType<GameObject>();
		foreach(GameObject obj in allObjects){
			if(obj.tag != "Player" && obj.activeInHierarchy) {
				goList.Add(obj);
			}
		}
		sceneObjects = goList.ToArray();

		targetY = (gameObject.transform.GetComponent<BoxCollider>().size.y / 2) + transform.position.y;
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player") {
			foreach(GameObject obj in sceneObjects) {
				Vector3 temp = obj.transform.position;
				temp.y = temp.y - targetY;
				obj.transform.position = temp;
			}
		}
	}
}
