using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class Director : MonoBehaviour {

	public class Beat {
		public string character;
		public string motion;
		public string audioClip;
		public float x;
		public float y;
	}

	public class Scene { 
		[XmlAttribute("Beat")]
		public Beat[] beats;
	}

	public int layer;
	private GameObject[] allObjects;
	// Use this for initialization
	void Start () {
		layer = 0;
		allObjects = FindObjectsOfType<GameObject>();
		ChangeScene(0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeScene(int sceneLayer) {
		layer = sceneLayer;
		foreach(GameObject obj in allObjects){
			if(obj.layer == layer || obj.tag == "Player") {
				obj.SetActive(true);
			}
			else {
				obj.SetActive(false);
				Debug.Log("set " + obj.ToString() + " to false");
			}
		}
	}

}
