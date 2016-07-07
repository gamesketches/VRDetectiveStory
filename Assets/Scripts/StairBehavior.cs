using UnityEngine;
using System.Collections;

public class StairBehavior : MonoBehaviour {

	private GameObject[] sceneObjects;
	private float targetY;
	private bool entered;
	static float deltaY;
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

		targetY = gameObject.transform.GetComponent<BoxCollider>().center.y + transform.position.y;//.size.y / 2) + transform.position.y;
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player" && !entered) {
			entered = true;
			float dif = Mathf.Abs(targetY - deltaY);
			if(targetY > deltaY) {
				deltaY += dif;
				foreach(GameObject obj in sceneObjects) {
				Vector3 temp = obj.transform.position;
				temp.y = temp.y - dif;
				obj.transform.position = temp;
			}
			}
			else if(targetY < deltaY) {
				deltaY -= dif;
				foreach(GameObject obj in sceneObjects) {
				Vector3 temp = obj.transform.position;
				temp.y = temp.y + dif;
				obj.transform.position = temp;
			}
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.gameObject.tag == "Player") {
			entered = false;
		}
	}
}
