using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectObject : CharacterAbility 
{
	protected override void Start ()
	{
		base.Start ();
	}

	protected override void Update ()
	{
		base.Update ();

		if(FPSCharacterController.Instance.lookingAtObject == null)
			return;

		ICollectable obj = FPSCharacterController.Instance.lookingAtObject.GetComponent(typeof(ICollectable)) as ICollectable;

		if(obj == null)
			return;

		if(Input.GetButtonDown("Collect"))
		{
			obj.Collect();	//TODO the limitation, if collect method needs parameter, I don't know how to pass them properly
		}
	}
}

