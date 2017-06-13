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
		Debug.Log("Enter look around state.");

		timer = 0.0f;
		patrolCounter++;
		if(patrolCounter >= sfsm.patrolPoints.Length)
		{
			patrolCounter = 0;
		}
	}

	public override void OnExit ()
	{
		Debug.Log("Exit look around state.");
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
