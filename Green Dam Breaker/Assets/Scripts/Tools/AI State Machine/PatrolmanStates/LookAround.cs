using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LookAround : PatrolmanStateBase
{
	public float lookTime = 3.0f;

	float timer;
	int patrolCounter;

	public override void OnEnter ()
	{
		timer = 0.0f;
		patrolCounter++;
		if(patrolCounter >= sfsm.patrolPoints.Length)
		{
			patrolCounter = 0;
		}
	}

	public override void OnExit ()
	{
		
	}

	public override void StateUpdate ()
	{
		//Debug.Log("Updateing look around state.");

		timer += Time.deltaTime;
		if(timer >= lookTime)
		{
			sfsm.moveToState.SetDestination(sfsm.patrolPoints[patrolCounter]);
			sfsm.TransitState(sfsm.moveToState);
		}
	}
}
