using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour 
{
	public float copterSpeed = 50f;	//one 'knot' is approximated in 51m/s

	Vector3 destination;
	bool isOperating;

	void Start()
	{
		destination = Vector3.zero;
		isOperating = false;
	}
		
	void Update()
	{
		if(isOperating)
		{
			float remainingDist = (this.transform.position - destination).sqrMagnitude;
			if(remainingDist <= 1.0f)
			{
				GetComponent<Rigidbody>().velocity = Vector3.zero;
				destination = Vector3.zero;
				isOperating = false;
			}
		}
	}

	public void OnDispatchCopter()
	{
		destination = FindObjectOfType<FPSCharacterController>().transform.position;	//TODO better solution?
		isOperating = true;

		SendMessageUpwards("OnCopterReply");

		Vector3 direction = (destination - this.transform.position).normalized;

		GetComponent<Rigidbody>().velocity = direction * copterSpeed;
	}
}
