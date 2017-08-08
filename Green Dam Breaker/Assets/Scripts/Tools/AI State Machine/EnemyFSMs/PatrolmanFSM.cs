using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Think enemy FSM as FPS player controller, it is a manager of a character, enemy is controlled by AI player is controlled by player.
public class PatrolmanFSM : AIStateMachine
{
	[Header("Models")]
	public Transform head;
	public Transform eye;
	public Transform body;
	public Transform weapon;
	public Transform barrelEnd;

	[Space(10)]
	[Header("Patrol")]
	public Vector3[] patrolPoints;
	public FPSCharacterController player;

	[Space(10)]
	[Header("Attack")]
	//these params determine a random shoot direction
	public float damage = 10f;
	public float fireRange = 100f;
	public LayerMask attackLayer;
	public float aimPivotDist = 2f;
	public float aimMaxRadius = 2f;
	private float aimRadius = 0f;
	private Vector3 animDir;
	private Ray fireRay;

	[Space(10)]
	[Header("States")]
	public MoveTo moveToState = new MoveTo();
	public LookAround lookAroundState = new LookAround();
	public Alert alertState = new Alert();
	public Attack attackState = new Attack();

	private LineRenderer gunLine;

	void Start()
	{
		gunLine = weapon.GetComponent<LineRenderer>();
		TestSetGunLine();

		moveToState.Initialize(this);
		lookAroundState.Initialize(this);
		alertState.Initialize(this);
		attackState.Initialize(this);

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

			if(currentState != alertState && currentState != attackState)
				TransitState(alertState);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.GetComponent<FPSCharacterController>())
		{
			TransitState(lookAroundState);

			if(player != null)
				player = null;
		}
	}

	public void Attack()
	{
		aimRadius = Random.Range(0f, aimMaxRadius);
		animDir = Random.insideUnitCircle;
		Vector3 origin = eye.position + transform.forward * aimPivotDist;
		Vector3 aim = origin + animDir * aimRadius;
		fireRay = new Ray(eye.position, (aim - eye.position).normalized);

		RaycastHit fireHit;
		if(Physics.Raycast(fireRay, out fireHit, fireRange, attackLayer, QueryTriggerInteraction.Ignore))
		{
			Health targetHP = fireHit.transform.GetComponent<Health>();
			if(targetHP != null)
			{
				targetHP.TakeDamage(damage, fireHit);
			}
		}

		gunLine.SetPosition(0, barrelEnd.position);
		if(fireHit.transform != null)
		{
			gunLine.SetPosition(1, fireHit.point);
		}else{
			gunLine.SetPosition(1, barrelEnd.position + barrelEnd.forward * 10f);
		}
		gunLine.enabled = true;
	}

	void TestSetGunLine()
	{
		gunLine.startWidth = 0.05f;
		gunLine.endWidth = 0.05f;
		gunLine.startColor = Color.yellow;
		gunLine.endColor = Color.yellow;
		gunLine.useWorldSpace = true;
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
