using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for any AI state
[System.Serializable]
public abstract class AIState 
{
	protected AIStateMachine fsm;

	public virtual void Initialize(AIStateMachine _fsm)
	{
		fsm = _fsm;
	}

	public abstract void OnEnter();

	public abstract void OnExit();

	public abstract void StateUpdate();
}
