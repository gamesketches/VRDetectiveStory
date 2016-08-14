using UnityEngine;
using System.Collections;

public class DressingRoomIKController : MonoBehaviour {

	Animator sara;
	public Transform pillBottlePlacement;
	float t;
	float timeElapsed;
	// Use this for initialization
	void Start () {
		t = 0;
		timeElapsed = 0;
		sara = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnAnimatorIK() {
		if(sara.GetInteger("animationId") == 8) {
			sara.SetIKPositionWeight(AvatarIKGoal.RightHand,t / (sara.GetCurrentAnimatorStateInfo(0).length / 2));
			sara.SetIKRotationWeight(AvatarIKGoal.RightHand,t / (sara.GetCurrentAnimatorStateInfo(0).length / 2));  
   		    sara.SetIKPosition(AvatarIKGoal.RightHand,pillBottlePlacement.position);
			sara.SetIKRotation(AvatarIKGoal.RightHand,pillBottlePlacement.rotation);
			if(timeElapsed > sara.GetCurrentAnimatorStateInfo(0).length / 2) {
				t -= Time.deltaTime;
			}
			else {
				t += Time.deltaTime;
			}
			timeElapsed += Time.deltaTime;
		}
		else {
			sara.SetIKPositionWeight(AvatarIKGoal.RightHand,0);
   		    sara.SetIKRotationWeight(AvatarIKGoal.RightHand,0);
		}
	}
}
