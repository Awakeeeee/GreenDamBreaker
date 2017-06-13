using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveTo : PatrolmanStateBase
{
	public float moveSpeed;
	public Vector3 destination;
	public float stopDistance;

	Quaternion faceDir;

	public override void OnEnter()
	{
		Debug.Log("Enter move to state.");
		faceDir = Quaternion.LookRotation(destination - sfsm.transform.position);
	}

	public override void OnExit()
	{
		Debug.Log("Exit move to state.");
	}

	public override void StateUpdate()
	{
		//Debug.Log("Updating move to state.");

		//turn face
		sfsm.transform.rotation = Quaternion.Slerp(sfsm.transform.rotation, faceDir, Time.deltaTime * 2);

		//move
		Vector3 newPos = Vector3.MoveTowards(sfsm.transform.position, destination, moveSpeed * Time.deltaTime);
		sfsm.transform.position = newPos;

		//check finish
		if(Vector3.SqrMagnitude(sfsm.transform.position - destination) < stopDistance)
		{
			sfsm.TransitState(sfsm.lookAroundState);
		}
	}

	public void SetDestination(Vector3 _destination)
	{
		destination = _destination;
	}

	public void SetMoveSpeed(float _speed)
	{
		moveSpeed = _speed;
	}
}
