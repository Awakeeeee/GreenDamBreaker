using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MechFlyWait : MechFlyStateBase
{
	public float minWaitTime = 0.5f;
	public float maxWaitTime = 2.0f;

	private float waitTime;
	private float timer;

	public override void OnEnter ()
	{
		timer = 0.0f;
		waitTime = Random.Range(minWaitTime, maxWaitTime);
	}

	public override void StateUpdate ()
	{
		timer += Time.deltaTime;
		if(timer >= waitTime)
		{
			float chance = Random.value;
			if(chance > 0.5f)
			{
				sfsm.TransitState(sfsm.attack);
			}else
			{
				sfsm.TransitState(sfsm.move);
			}
		}
	}

	public override void OnExit ()
	{
		
	}
}
