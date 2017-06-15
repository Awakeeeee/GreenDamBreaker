using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack : PatrolmanStateBase
{
	public float turnFaceSpeed = 5f;
	public float sightRange = 100f;
	public LayerMask sightTarget;

	public float fireRate = 1.0f;
	private float fireTimer;

	public override void OnEnter ()
	{
		fireTimer = fireRate;
	}

	public override void OnExit ()
	{

	}

	public override void StateUpdate ()
	{
		//in attack state, enemy sight tracks player
		Quaternion faceRotation = Quaternion.LookRotation(sfsm.player.transform.position - sfsm.transform.position);
		sfsm.transform.rotation = Quaternion.Slerp(sfsm.transform.rotation, faceRotation, turnFaceSpeed * Time.deltaTime);

		//cast ray to check if attack
		RaycastHit hit;
		if(Physics.Raycast(sfsm.transform.position, sfsm.transform.forward, out hit, sightRange, sightTarget, QueryTriggerInteraction.Ignore))
		{
			if(hit.transform.CompareTag("Character"))
			{
				fireTimer += Time.deltaTime;

				if(fireTimer >= fireRate)	//ready to fire
				{
					fireTimer = 0.0f;
					sfsm.Attack();
				}else						//fire in cd
				{
					//anything to do?
				}
			}else{
				Debug.Log("Enemy sight is blocked!!");
			}
		}
	}
}
