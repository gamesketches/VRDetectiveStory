using UnityEngine;
using System.Collections;

public class TimeShift : MonoBehaviour {

	Material materialForTest;
	public float currentTime;
	// Use this for initialization
	void Start () {
		currentTime = 1.0f;
		materialForTest = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion currentAngles = transform.rotation;
		materialForTest.color = Color.Lerp(Color.blue, Color.red, currentTime);
		//materialForTest.color = new Color(Mathf.Abs(currentAngles.x), Mathf.Abs(currentAngles.y), Mathf.Abs(currentAngles.z));
	}
}
