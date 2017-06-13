using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Alert : PatrolmanStateBase 
{
	public float sightAngle = 60f;

	private float toPlayerAngle;
	private Vector3 toPlayerVec;

	public override void OnEnter ()
	{
		Debug.Log("Enter Alert state.");
	}

	public override void OnExit ()
	{
		Debug.Log("Exit Alert state.");
	}

	public override void StateUpdate ()
	{
		toPlayerVec = (sfsm.player.transform.position - sfsm.transform.position).normalized;
		toPlayerAngle = Vector3.Angle(sfsm.transform.forward, toPlayerVec);
		Debug.Log(toPlayerAngle);
		if(toPlayerAngle <= sightAngle / 2f)
		{
			Debug.Log("See player!");
		}
	}
}
