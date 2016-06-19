using UnityEngine;
using System.Collections;

public class StampingScript : MonoBehaviour {

	public bool stamped;
	// Use this for initialization
	void Start () {
		stamped = false;
	}
	
	void OnTriggerEnter(Collider other) {
		if(!stamped && other.gameObject.name == "Stamp") {
			// replace this with actual stuff
			GetComponent<Renderer>().material.color = Color.red;
		}
	}
}
