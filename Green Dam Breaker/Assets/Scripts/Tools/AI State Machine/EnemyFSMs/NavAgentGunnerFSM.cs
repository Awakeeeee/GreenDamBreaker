using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentGunnerFSM : AIStateMachine
{
	NavMeshAgent agent;
	FPSCharacterController player;

	void Start()
	{
		player = FindObjectOfType<FPSCharacterController>();
		agent = GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		agent.SetDestination(player.transform.position);
	}

	public override void DeactiveSelf ()
	{
		agent.enabled = false;
		this.enabled = false;
	}
}
