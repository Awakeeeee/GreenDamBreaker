using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO limit of my current FSM: states have to belong to specific fsm, in order to know how to transit betwwen states

public class PatrolmanStateBase : AIState
{
	protected PatrolmanFSM sfsm;

	public override void Initialize (AIStateMachine _fsm)
	{
		base.Initialize (_fsm);
		sfsm = (PatrolmanFSM)fsm;
	}

	public override void OnEnter ()
	{
	}

	public override void OnExit ()
	{
	}

	public override void StateUpdate ()
	{
	}
}
