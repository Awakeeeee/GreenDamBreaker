using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HitterAttack : HitterStateBase 
{
	public float atkSpeed = 3f;
	public float atkInterval = 1f;
	public float atkDamage = 10f;

	float atkTimer = 0.0f;

	public override void OnEnter ()
	{
		Debug.Log("Enter atk state!");
		atkTimer = atkInterval;
		sfsm.agent.enabled = false;	//take over the control
	}

	public override void OnExit ()
	{
		sfsm.agent.enabled = true;
	}

	public override void StateUpdate ()
	{
		atkTimer += Time.deltaTime;

		if(atkTimer > atkInterval)
		{
			sfsm.player.GetComponent<CharacterHP>().TakeDamage(atkDamage);
			sfsm.transform.rotation = Quaternion.LookRotation((new Vector3(sfsm.player.transform.position.x, sfsm.transform.position.y, sfsm.player.transform.position.z) - sfsm.transform.transform.position).normalized);
			sfsm.Attack(atkSpeed);
			atkTimer = 0f;
		}

		if((sfsm.transform.position - sfsm.player.transform.position).sqrMagnitude > Mathf.Pow(sfsm.agent.stoppingDistance, 2))
		{
			sfsm.TransitState(sfsm.chaseState);
		}
	}
}
