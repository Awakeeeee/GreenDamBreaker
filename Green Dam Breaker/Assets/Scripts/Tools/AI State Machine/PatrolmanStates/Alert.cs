using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Alert : PatrolmanStateBase 
{
	public float sightAngle = 60f;
	public float searchRotateSpeed = 30f;

	private float toPlayerAngle;
	private Vector3 toPlayerVec;

	public override void OnEnter ()
	{

	}

	public override void OnExit ()
	{
		
	}

	public override void StateUpdate ()
	{
		toPlayerVec = (sfsm.player.transform.position - sfsm.transform.position).normalized;
		toPlayerAngle = Vector3.Angle(sfsm.transform.forward, toPlayerVec);

		if(toPlayerAngle <= sightAngle / 2f)	//with in view of sight
		{
			//into trying to attack state
			sfsm.TransitState(sfsm.attackState);
		}else
		{
			//into searching state
			sfsm.transform.Rotate(Vector3.up, searchRotateSpeed * Time.deltaTime);
		}
	}
}
