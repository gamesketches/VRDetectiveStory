using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpiritGuideController : MonoBehaviour {

	public Dictionary<string,bool> flags;
	bool[] visitedScenes;
	Vector3[] flyAwayVectors;
	int flyAwayTimes;
	Animator animator;
	// Use this for initialization
	void Start () {
		if(GameObject.FindGameObjectsWithTag("guide").Length > 1) {
			Destroy(gameObject);
		}
		animator = GetComponent<Animator>();
		flyAwayVectors = new Vector3[2] {Vector3.up, Vector3.left};
		flyAwayTimes = 0;
		visitedScenes = new bool[3];
		flags = new Dictionary<string,bool>();
		flags.Add("bed", false);
		flags.Add("theater", false);
		flags.Add("outside", false);
		visitedScenes[0] = false;
		visitedScenes[1] = false;
		visitedScenes[2] = false;
		DontDestroyOnLoad(transform.gameObject);
	}

	void OnLevelWasLoaded() {
		Debug.Log("ON LOAD");
		switch(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name) {
			case "theater":
				visitedScenes[0] = true;
				transform.position -= Vector3.down * 100;
				flags["theater"] = true;
				break;
			case "outside":
				flags["outside"] = true;
				transform.position -= Vector3.down * 100;
				visitedScenes[1] = true;
				break;
			case "dressing room":
				visitedScenes[2] = true;
				transform.position -= Vector3.down * 100;
				break;
			default:
				if(visitedScenes[0] == true && visitedScenes[1] == false) {
					GameObject secondClue = GameObject.Find("SpiritGuideSecondClue");
					transform.position = secondClue.transform.position;
					transform.rotation = secondClue.transform.rotation;
				}
				break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Debug Code for checking if game over works
		if(Input.GetKeyDown(KeyCode.Q)) {
			int sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
			sceneIndex++;
			if(sceneIndex == 4) {
				sceneIndex = 0;
			}
			UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
		}
	}

	public bool CheckGameOverState() {
		bool[] flagValues = new bool[flags.Values.Count];
		flags.Values.CopyTo(flagValues, 0);
		foreach(bool flag in visitedScenes) {
			if(!flag) {
				return false;
			}
		}
		GameOver();
		return true;
	}

	public void Appear(Vector3 position, float time) {
		Vector3 temp = position;
		temp.y -= 1f;
		temp.z += 1f;
		transform.position = temp;
		transform.LookAt(position - new Vector3(0f, 1f, 0f));
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

		transform.position = new Vector3(0, -100, 0);
	}

	public void FlyAway() {
		if(!animator.GetBool("flying") && flyAwayTimes <= flyAwayVectors.Length) {
			StartCoroutine(FlyingAway());
		}
	}

	IEnumerator FlyingAway() {
		animator.SetBool("flying", true);
		float t = 0;
		float f = 0;
		Quaternion startRotation = transform.rotation;
		Quaternion targetRotation = Quaternion.Euler(new Vector3(-90f, 31.2106f, 0f));
		int flyAwayCopy = flyAwayTimes;
		flyAwayTimes = -1;
		Vector3 targetLocation = transform.position + (flyAwayVectors[flyAwayCopy] * 7);
		float startY = transform.position.y;
		while(t < 1) {
			Vector3 temp = transform.position;
			temp.y = Mathf.SmoothStep(startY, targetLocation.y, t);
			transform.position = temp;
			t += Time.deltaTime;
			transform.rotation = Quaternion.Lerp(startRotation, targetRotation, f);
			f += Time.deltaTime;
			yield return null;
		}

		transform.rotation = startRotation;
		animator.SetBool("flying", false);

		flyAwayTimes = flyAwayCopy + 5;
	}

	void GameOver() {

		var goList = new System.Collections.Generic.List<GameObject>();
		GameObject[] allObjects = FindObjectsOfType<GameObject>();
		foreach(GameObject obj in allObjects){
			if(obj.tag != "Player" && obj.activeInHierarchy && obj.transform.parent == null) {
				StartCoroutine(FloatAway(obj.transform));
			}
		}
	}

	IEnumerator FloatAway(Transform objectTransform) {
		Vector3 start = objectTransform.position;
		Vector3 end = start + new Vector3(0f, -100f, 0f);
		float t = 0;
		while(t < 1) {
			objectTransform.position = new Vector3(Mathf.SmoothStep(start.x, end.x, t),
					Mathf.SmoothStep(start.y, end.y, t),
					Mathf.SmoothStep(start.z, end.z, t));
			t += Time.deltaTime / 15;
			yield return null;
		}
	}
}
