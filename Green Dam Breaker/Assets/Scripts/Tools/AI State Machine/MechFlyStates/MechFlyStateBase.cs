using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechFlyStateBase : AIState
{
	protected MechFlyFSM sfsm;

	public override void Initialize (AIStateMachine _fsm)
	{
		base.Initialize (_fsm);
		sfsm = (MechFlyFSM)fsm;
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
