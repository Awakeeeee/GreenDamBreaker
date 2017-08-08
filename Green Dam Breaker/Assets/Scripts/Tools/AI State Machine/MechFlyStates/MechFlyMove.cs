using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MechFlyMove : MechFlyStateBase
{
	public float moveSpeed;
	Vector3 destination;

	public override void OnEnter ()
	{
		sfsm.modelAnim.SetBool("isMoving", true);

		float randomX = Random.Range(sfsm.RangeX.x, sfsm.RangeX.y);
		float randomY = Random.Range(sfsm.RangeY.x, sfsm.RangeY.y);
		float randomZ = Random.Range(sfsm.RangeZ.x, sfsm.RangeZ.y);
		destination = new Vector3(randomX, randomY, randomZ);

		sfsm.model.transform.localPosition = Vector3.zero;
		sfsm.model.localRotation = Quaternion.Euler(Vector3.zero);
		sfsm.transform.LookAt(destination);
	}

	public override void StateUpdate ()
	{
		float distance = (sfsm.transform.position - destination).sqrMagnitude;
		if(distance > 0.1f)
		{
			Vector3 newPos = Vector3.MoveTowards(sfsm.transform.position, destination, moveSpeed * Time.deltaTime);
			sfsm.transform.position = newPos;
		}else
		{
			sfsm.TransitState(sfsm.wait);
		}
	}

	public override void OnExit ()
	{
		sfsm.modelAnim.SetBool("isMoving", false);
	}
}
