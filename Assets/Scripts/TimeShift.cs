using UnityEngine;
using System.Collections;

public class TimeShift : MonoBehaviour {

	Material materialForTest;
	public float currentTime;
	public float targetRangeBottom, targetRangeTop;
	delegate void UpdateFunction();
	UpdateFunction updateFunction;
	Material[] roseMaterials;
	public Color decayedRoseColor;
	int[] triangleArray;
	Mesh mesh;
	// Use this for initialization
	void Start () {
		switch(gameObject.name) {
			case "wholepaper":
			case "tornpaper":
				mesh = GetComponent<MeshFilter>().sharedMesh;
				updateFunction = PaperUpdate;
				triangleArray = mesh.triangles;
				break;
			case "rose":
			case "brokenrose":
				updateFunction = RoseUpdate;
				MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
				roseMaterials = new Material[renderers.Length - 1];
				for(int i = 0; i < roseMaterials.Length - 1; i++) {
					roseMaterials[i] = renderers[i].material;
					roseMaterials[i].color = decayedRoseColor;
				}
				updateFunction = RoseUpdate;
				break;
			case "pillCup":
				updateFunction = PillBottleUpdate;
				break;
			// Test Cube handler
			default:
				materialForTest = GetComponent<Renderer>().material;
				updateFunction = TestCubeUpdate;
				break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		updateFunction();
	}

	void TestCubeUpdate() {
		Quaternion currentAngles = transform.rotation;
		materialForTest.color = Color.Lerp(Color.blue, Color.red, currentTime);
		//materialForTest.color = new Color(Mathf.Abs(currentAngles.x), Mathf.Abs(currentAngles.y), Mathf.Abs(currentAngles.z));

	}

	void PaperUpdate() {
		int numTriangles = (int)(triangleArray.Length * currentTime);
		while(numTriangles % 3 != 0) {
			numTriangles++;
		}
		int[] tempArray = new int[numTriangles];
		for(int i = 0; i < tempArray.Length; i++) {
			tempArray[i] = triangleArray[i];
		} 
		mesh.SetTriangles(tempArray, 0);
	}

	public bool withinTargetRange() {
		return currentTime < targetRangeTop && currentTime > targetRangeBottom;
	}

	void RoseUpdate() {
		foreach(Material mat in roseMaterials) {
			mat.color = Color.Lerp(decayedRoseColor, Color.white, currentTime);
		}
	}

	void PillBottleUpdate() {
		
	}

	void OnApplicationQuit() {
		if(triangleArray != null) {
			mesh.SetTriangles(triangleArray, 0);
		}
	}
}
