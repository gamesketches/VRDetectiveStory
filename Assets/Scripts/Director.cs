using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.SceneManagement;

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
	private string[] animations;
	private int beatNum;
	// Use this for initialization
	void Start () {
		layer = 0;
		beatNum = 0;
		List<string> tempList = new List<string>();
/*		foreach(AnimationClip clip in GameObject.Find("walking").GetComponent<Animator>().runtimeAnimatorController.animationClips) {
			tempList.Add(clip.name);
			Debug.Log(clip.name);
		}*/
		animations = tempList.ToArray();
		screenplay = new Script();
		var serializer = new XmlSerializer(typeof(Script));
		string scenePath = SceneManager.GetActiveScene().name;
		TextAsset sceneData = Resources.Load(scenePath + "/playbook") as TextAsset;
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
		//GameObject character = GameObject.Find("walking");

	//	foreach(XmlNode beat in theScene.ChildNodes) {
			StartCoroutine(PlayBeat(theScene.ChildNodes[0]));
	//	}

	}

	IEnumerator PlayBeat(XmlNode Beat) {
		GameObject character = GameObject.Find(Beat.ChildNodes[0].InnerText);
		Debug.Log(character.name);
		Animator anim = character.GetComponent<Animator>();
		anim.SetBool("finished", false);
		/*AudioSource audio = GetComponent<AudioSource>();
		AudioClip voiceClip = Resources.Load<AudioClip>("VoiceClips/" + Beat.ChildNodes[2].InnerText);
		audio.clip = voiceClip;
		audio.Play();
		*/
		anim.SetInteger("animationId", anim.GetInteger("animationId") + 1);//System.Array.IndexOf(animations, Beat.ChildNodes[1].InnerText));
		float t = 0;
		float animationTime = anim.GetCurrentAnimatorStateInfo(0).length;
		Vector3 startPosition = character.transform.position;
		Vector3 targetPosition = startPosition;
		targetPosition.x += float.Parse(Beat.ChildNodes[3].InnerText);
		targetPosition.z += float.Parse(Beat.ChildNodes[4].InnerText);
		while(t <= animationTime) {
			character.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
			t += Time.deltaTime;
			yield return null;
		}
		anim.SetInteger("animationId", 0);
		anim.SetBool("finished", true);
	}

}
