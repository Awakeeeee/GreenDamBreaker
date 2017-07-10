using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitterStateBase : AIState
{
	protected NavAgentHitterFSM sfsm;

	public override void Initialize (AIStateMachine _fsm)
	{
		base.Initialize (_fsm);
		sfsm = (NavAgentHitterFSM)fsm;
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
