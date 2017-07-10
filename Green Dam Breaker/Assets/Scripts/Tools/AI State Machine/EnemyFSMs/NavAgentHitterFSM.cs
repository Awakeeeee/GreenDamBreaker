using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyHP))]
public class NavAgentHitterFSM : AIStateMachine 
{
	[HideInInspector]public NavMeshAgent agent;
	[HideInInspector]public FPSCharacterController player;
	[HideInInspector]public EnemyHP hp;

	public HitterIdle idleState;
	public HitterChase chaseState;
	public HitterAttack attackState;

	float playerRadius;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		player = FindObjectOfType<FPSCharacterController>();
		hp = GetComponent<EnemyHP>();

		playerRadius = player.GetComponent<CharacterController>().radius;

		idleState.Initialize(this);
		chaseState.Initialize(this);
		attackState.Initialize(this);

		TransitState(chaseState);
	}

	void Update()
	{
		currentState.StateUpdate();
	}

	//TODO confusion: I need a coroutine to attack, but AIState is not Monobehaviour, so I put the method in here.
	public void Attack(float atkSpeed)
	{
		StopAllCoroutines();
		StartCoroutine(AttackCo(atkSpeed));
	}

	IEnumerator AttackCo(float atkSpeed)
	{
		float percent = 0.0f;

		Vector3 startPos = this.transform.position;
		Vector3 hitDir = (new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z) - startPos).normalized;
		float hitRange = (new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z) - startPos).magnitude - playerRadius * 2;
		Vector3 endPos = startPos + hitDir * hitRange;

		while(percent < 1f)
		{
			float myInterpolation = 4 * (-Mathf.Pow(percent, 2) + percent);	//y = 4 * (-x^2 + x), so y is 0~1~0

			this.transform.position = Vector3.Lerp(startPos, endPos, myInterpolation);

			percent += Time.deltaTime * atkSpeed;
			yield return null;
		}
	}

	public override void DeactiveSelf ()
	{
		agent.enabled = false;
		this.enabled = false;
	}
}
