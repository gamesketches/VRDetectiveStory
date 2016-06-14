using UnityEngine;
using System.Collections;

public class TimeShift : MonoBehaviour {

	Material materialForTest;
	// Use this for initialization
	void Start () {
		materialForTest = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion currentAngles = transform.rotation;
		materialForTest.color = new Color(Mathf.Abs(currentAngles.x), Mathf.Abs(currentAngles.y), Mathf.Abs(currentAngles.z));
	}
}
