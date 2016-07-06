﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class Director : MonoBehaviour {

	public Script screenplay;

	public class Beat {
		public string character;
		public string motion;
		public string audioClip;
		public float x;
		public float y;
	}

	public class Scene { 
		[XmlAttribute("name")]
		public string sceneName;
		[XmlElement("Beat")]
		public Beat[] beats;
	}

	[XmlRoot("Root")]
	public class Script {
		[XmlArray("Script")]
		[XmlArrayItem("Scene")]
		public List<Scene> scenes = new List<Scene>();
	}

	public int layer;
	private GameObject[] allObjects;
	// Use this for initialization
	void Start () {
		layer = 0;
		screenplay = new Script();
		var serializer = new XmlSerializer(typeof(Script));
		TextAsset sceneData = Resources.Load("playbook") as TextAsset;
		TextReader reader = new StringReader(sceneData.text);
		screenplay = (Script)serializer.Deserialize(reader);
		allObjects = FindObjectsOfType<GameObject>();
		ChangeScene(0);

		XmlDocument xmlDoc = new XmlDocument();

		xmlDoc.LoadXml(sceneData.text);

		XmlNode node = xmlDoc.ChildNodes[1].FirstChild.FirstChild.FirstChild;

		Debug.Log(xmlDoc.ChildNodes[1].FirstChild.OuterXml);

		foreach(XmlNode child in node.ChildNodes) {
			Debug.Log(child.InnerXml);
		}
		Debug.Log(node.InnerXml);

		Debug.Log(screenplay.scenes[0].beats[0]);
		PlayScene(xmlDoc.ChildNodes[1].FirstChild.FirstChild);
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

	void PlayScene(XmlNode theScene) {
		//create characters here
		GameObject character = GameObject.Find("walking");

		StartCoroutine(PlayBeat(theScene.FirstChild));

	}

	IEnumerator PlayBeat(XmlNode Beat) {
		GameObject character = GameObject.Find("walking");
		Animator anim = character.GetComponent<Animator>();
		anim.SetBool("finished", false);
		AudioSource audio = GetComponent<AudioSource>();
		AudioClip voiceClip = Resources.Load<AudioClip>("VoiceClips/" + Beat.ChildNodes[2].InnerText);
		audio.clip = voiceClip;
		audio.Play();
		anim.SetInteger("animationId", 2);
		float t = 0;
		Vector3 startPosition = character.transform.position;
		Vector3 targetPosition = startPosition;
		targetPosition.x += int.Parse(Beat.ChildNodes[3].InnerText);
		targetPosition.z += int.Parse(Beat.ChildNodes[4].InnerText);
		while(t <= 0) {
			character.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
			t += Time.deltaTime;
			yield return null;
		}
		anim.SetInteger("animationId", 0);
		anim.SetBool("finished", true);
	}

}
