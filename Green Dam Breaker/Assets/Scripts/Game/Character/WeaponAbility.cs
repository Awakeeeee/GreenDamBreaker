using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The ability to use weapon
public class WeaponAbility : CharacterAbility
{
	//- what happens when you click fire and hit stuff?
	//gun recoil shakes ~
	//gun flare & lighting ~
	//fire sound effect ~
	//(optional)create bullet
	//show a bullet trace
	//update armo consumption ~
	//bullet shell pop out ~
	//-------
	//particle effect on target
	//sound effect on hiting target ~
	//other effect on target
	//target receive damage

	[Header("Right click to Zoom")]
	public float zoomScale = 0.5f;
	public float zoomTime = 1.0f;

	Camera cam;
	FPSCharacterController character;

	float originFOV;
	float zoomSpeed;

	protected override void Start()
	{
		character = GetComponent<FPSCharacterController>();
		cam = GetComponentInChildren<Camera>();

		if(cam)
		{
			originFOV = cam.fieldOfView;
		}
	}

	protected override void Update()
	{
		if(!toggle)
			return;
		
		ZoomView();
	}

	void ZoomView()
	{
		if(character.fovManipulater.isChangingFOV)
			return;

		if(cam.fieldOfView == character.fovManipulater.originalFOV + character.fovManipulater.changeAmount)
			return;

		zoomSpeed = Mathf.Abs(originFOV - originFOV * zoomScale) / zoomTime;

		if(Input.GetButton("Fire2"))
		{
			//when fully complete zoom
			if(Mathf.Abs(cam.fieldOfView - originFOV * zoomScale) <= 0.01f)
			{
				cam.fieldOfView = originFOV * zoomScale;
				return;
			}

			float newFOV = Mathf.MoveTowards(cam.fieldOfView, originFOV * zoomScale, zoomSpeed * Time.deltaTime);
			cam.fieldOfView = newFOV;

			return;
		}

		//if not holding zoom button
		if(cam.fieldOfView != originFOV)
		{
			float newFOV = Mathf.MoveTowards(cam.fieldOfView, originFOV, zoomSpeed * Time.deltaTime);
			cam.fieldOfView = newFOV;

			//fully zoomed back
			if(Mathf.Abs(cam.fieldOfView - originFOV) <= 0.01f)
			{
				cam.fieldOfView = originFOV;
			}
		}
	}
}
