using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolmanFSM : AIStateMachine
{
	public Vector3[] patrolPoints;
	public FPSCharacterController player;

	[Space(10)]
	[Header("States")]
	public MoveTo moveToState = new MoveTo();
	public LookAround lookAroundState = new LookAround();
	public Alert alertState = new Alert();

	void Start()
	{
		moveToState.Initialize(this);
		lookAroundState.Initialize(this);
		alertState.Initialize(this);

		TransitState(lookAroundState);
	}

	void Update()
	{
		currentState.StateUpdate();
	}

	void OnTriggerStay(Collider other)
	{
		if(other.GetComponent<FPSCharacterController>())
		{
			if(player == null)
				player = other.GetComponent<FPSCharacterController>();

			TransitState(alertState);
		}
	}

	//test line
	void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 10f);

		Gizmos.color = Color.cyan;
		Vector2 rotatedLeft = GetRotationVector(new Vector2(transform.forward.x, transform.forward.z), alertState.sightAngle / 2f);
		Vector3 left = (new Vector3(rotatedLeft.x, 0f, rotatedLeft.y)).normalized;
		Gizmos.DrawLine(this.transform.position,  this.transform.position + left * 10f);
	}

	Vector2 GetRotationVector(Vector2 start, float untiClockwiseAngle)
	{
		float x = start.x * Mathf.Cos(untiClockwiseAngle) - start.y * Mathf.Sin(untiClockwiseAngle);
		float y = start.x * Mathf.Sin(untiClockwiseAngle) + start.y * Mathf.Cos(untiClockwiseAngle);

		return new Vector2(x, y);
	}
}
