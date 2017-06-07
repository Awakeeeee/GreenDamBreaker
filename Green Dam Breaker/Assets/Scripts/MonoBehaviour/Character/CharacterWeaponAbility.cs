using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The ability to use weapon
public class CharacterWeaponAbility : CharacterAbility
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

	float defaultFOV;
	float zoomSpeed;

	void Start()
	{
		character = GetComponent<FPSCharacterController>();
		cam = GetComponentInChildren<Camera>();

		if(cam)
		{
			defaultFOV = cam.fieldOfView;
		}
	}

	void Update()
	{
		ZoomView();
	}

	void ZoomView()
	{
		if(character.fovManipulater.isChangingFOV)
			return;

		zoomSpeed = Mathf.Abs(defaultFOV - defaultFOV * zoomScale) / zoomTime;

		if(Input.GetButton("Fire2"))
		{
			//when fully complete zoom
			if(Mathf.Abs(cam.fieldOfView - defaultFOV * zoomScale) <= 0.01f)
			{
				cam.fieldOfView = defaultFOV * zoomScale;
				return;
			}

			float newFOV = Mathf.MoveTowards(cam.fieldOfView, defaultFOV * zoomScale, zoomSpeed * Time.deltaTime);
			cam.fieldOfView = newFOV;

			return;
		}

		//if not holding zoom button
		if(cam.fieldOfView != defaultFOV)
		{
			float newFOV = Mathf.MoveTowards(cam.fieldOfView, defaultFOV, zoomSpeed * Time.deltaTime);
			cam.fieldOfView = newFOV;

			//fully zoomed back
			if(Mathf.Abs(cam.fieldOfView - defaultFOV) <= 0.01f)
			{
				cam.fieldOfView = defaultFOV;
			}
		}
	}
}
