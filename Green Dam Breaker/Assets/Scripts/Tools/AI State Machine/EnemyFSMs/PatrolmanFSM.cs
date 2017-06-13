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
		Vector3 midLineEnd = this.transform.position + this.transform.forward * GetComponent<SphereCollider>().radius;
		Gizmos.DrawLine(this.transform.position, midLineEnd);

		Gizmos.color = Color.cyan;
		Vector2 relativeStart = new Vector2(midLineEnd.x - this.transform.position.x, midLineEnd.z - this.transform.position.z);
		Vector2 rotatedLeft = GetRotationVector(relativeStart, alertState.sightAngle / 2f);
		Vector3 leftLineEnd = new Vector3(rotatedLeft.x + this.transform.position.x, this.transform.position.y, rotatedLeft.y + this.transform.position.z);
		Gizmos.DrawLine(this.transform.position,  leftLineEnd);

		Vector2 rotatedRight = GetRotationVector(relativeStart, 360f - alertState.sightAngle/ 2f);
		Vector3 rightLineEnd = new Vector3(rotatedRight.x + this.transform.position.x, this.transform.position.y, rotatedRight.y + this.transform.position.z);
		Gizmos.DrawLine(this.transform.position, rightLineEnd);
	}

	Vector2 GetRotationVector(Vector2 start, float untiClockwiseAngle)
	{
		untiClockwiseAngle = untiClockwiseAngle * Mathf.Deg2Rad;

		float x = start.x * Mathf.Cos(untiClockwiseAngle) - start.y * Mathf.Sin(untiClockwiseAngle);
		float y = start.x * Mathf.Sin(untiClockwiseAngle) + start.y * Mathf.Cos(untiClockwiseAngle);

		return new Vector2(x, y);
	}
}
