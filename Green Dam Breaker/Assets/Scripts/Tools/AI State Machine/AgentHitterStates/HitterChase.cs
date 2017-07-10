using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HitterChase : HitterStateBase
{
	public float updateInterval = 0.25f;

	private float chaseTimer;

	public override void OnEnter ()
	{
		Debug.Log("Enter chase state!");
		chaseTimer = 0.0f;
	}

	public override void OnExit ()
	{
	}

	public override void StateUpdate ()
	{
		chaseTimer += Time.deltaTime;

		if((sfsm.transform.position - sfsm.player.transform.position).sqrMagnitude <= Mathf.Pow(sfsm.agent.stoppingDistance, 2))
		{
			sfsm.TransitState(sfsm.attackState);
		}

		if(chaseTimer > updateInterval)
		{
			chaseTimer = 0.0f;

			if(!sfsm.hp.IsDead && sfsm.agent.enabled)
			{
				sfsm.agent.SetDestination(sfsm.player.transform.position);
			}
		}
	}
}
