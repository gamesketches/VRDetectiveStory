using UnityEngine;
using System.Collections;

public class SpiritGuideIKController : MonoBehaviour {

	Animator animator;
	Transform target;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		target = GameObject.Find("Camera (eye)");
	}

	void OnAnimatorIK() {
		animator.SetLookAtWeight(1);
		animator.SetLookAtPosition(target.position);
	}
}
