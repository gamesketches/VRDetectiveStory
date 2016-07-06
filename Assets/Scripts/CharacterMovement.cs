using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

	CharacterController characterController;
	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		float hori = Input.GetAxis("Horizontal");
		float vert = Input.GetAxis("Vertical");

		transform.Translate(new Vector3(0, 0, vert * 0.5f));
		transform.Rotate(new Vector3(0, hori, 0));
	}
}
