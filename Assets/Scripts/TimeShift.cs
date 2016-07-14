using UnityEngine;
using System.Collections;

public class TimeShift : MonoBehaviour {

	Material materialForTest;
	public float currentTime;
	public float targetRangeBottom, targetRangeTop;
	delegate void UpdateFunction();
	UpdateFunction updateFunction;
	int[] triangleArray;
	Mesh mesh;
	// Use this for initialization
	void Start () {
		materialForTest = GetComponent<Renderer>().material;
		mesh = GetComponent<MeshFilter>().sharedMesh;
		switch(gameObject.name) {
			case "wholepaper":
			case "tornpaper":
				updateFunction = PaperUpdate;
				triangleArray = mesh.triangles;
				break;
			case "rose":
				updateFunction = TestCubeUpdate;
				break;
			default:
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
		Debug.Log(triangleArray.Length);
		mesh.SetTriangles(tempArray, 0);
	}

	public bool withinTargetRange() {
		return currentTime < targetRangeTop && currentTime > targetRangeBottom;
	}

	void OnApplicationQuit() {
		if(triangleArray != null) {
			mesh.SetTriangles(triangleArray, 0);
		}
	}
}
