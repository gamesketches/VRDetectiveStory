using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpiritGuideController : MonoBehaviour {

	public Dictionary<string,bool> flags;
	// Use this for initialization
	void Start () {
		StartCoroutine(FadeOut(0f));
		flags = new Dictionary<string,bool>();
		flags.Add("bed", false);
		flags.Add("theater", false);
		flags.Add("outside", false);
		DontDestroyOnLoad(transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Appear(Vector3 position, float time) {
		transform.position = position;
		StartCoroutine(FadeIn());
		StartCoroutine(FadeOut(time));
	}

	IEnumerator FadeIn() {
		float t = 0;
		SkinnedMeshRenderer[] meshes = GetComponentsInChildren<SkinnedMeshRenderer>();

		while(t < 1) {
			foreach(SkinnedMeshRenderer mesh in meshes) {
				Color temp = mesh.material.GetColor("_Color");
				temp.a = Mathf.SmoothStep(0, 1, t);
				mesh.material.SetColor("_Color", temp);
				Debug.Log(mesh.material.GetFloat("_Mode"));
			}
			t += Time.deltaTime;
			yield return null;
		}
	}

	IEnumerator FadeOut(float lastingTime) {
		yield return new WaitForSeconds(lastingTime);
		float t = 0;
		SkinnedMeshRenderer[] meshes = GetComponentsInChildren<SkinnedMeshRenderer>();
		while(t < 1) {
			foreach(SkinnedMeshRenderer mesh in meshes) {
				mesh.material.SetFloat("_Mode", 2f);
				Color temp = mesh.material.GetColor("_Color");
				temp.a = Mathf.SmoothStep(1, 0, t);
				mesh.material.SetColor("_Color", temp);
	//			mesh.material.color = temp;
			}
			t += Time.deltaTime;
			yield return null;
		}
	}
}
