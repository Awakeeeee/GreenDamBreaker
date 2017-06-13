using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for any specific AI fsm
[RequireComponent(typeof(Animator))]
public class AIStateMachine : MonoBehaviour 
{
	public AIState currentState;
	public AIState previousState;

	public virtual void TransitState(AIState toState)
	{
		if(currentState == toState)
		{
			Debug.LogWarning("AI fsm trying to change to the same state, cancel transition.");
			return;
		}

		if(toState == null)
		{
			Debug.LogWarning("AI fsm target transit state is null, cancel transition.");
		}
		
		previousState = currentState;
		currentState = toState;

		if(previousState != null)
		{
			previousState.OnExit();
		}
		currentState.OnEnter();
	}
}
