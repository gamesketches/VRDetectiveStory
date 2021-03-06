﻿using UnityEngine;
using System.Collections;

public class AnnaOutsideIKController : MonoBehaviour {

	protected Animator animator;
	public Transform targetObject;
	public bool IKActive = false;
	float IKTime;
	// Use this for initialization
	void Start () {
		IKTime = 0;
		animator = GetComponent<Animator>();
		targetObject = GameObject.Find("wholepaper").transform;
	}

	void OnAnimatorIK() {
		if(animator) {
			if(IKActive) {
				if(targetObject != null) {
					animator.SetLookAtWeight(1);
					animator.SetLookAtPosition(targetObject.position);

					animator.SetIKPositionWeight(AvatarIKGoal.RightHand,IKTime);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand,IKTime);  
                    animator.SetIKPosition(AvatarIKGoal.RightHand,targetObject.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand,targetObject.rotation);
                    IKTime += Time.deltaTime;
				}

			}
			else {          
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand,0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand,0); 
                animator.SetLookAtWeight(0);
            }
			
		}
	}
}
