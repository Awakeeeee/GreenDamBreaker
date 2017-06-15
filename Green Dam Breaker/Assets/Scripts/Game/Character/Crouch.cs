using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : CharacterAbility
{
	[Header("Collider Height")]
	public float colliderSizeLower;

	CharacterController cc;
	float originalColliderHeight;
	Vector3 originalColliderCenter;

	[Header("View Height")]
	public float viewHorizonLower;
	float originalViewHorinzon;	//which is camera transform.localposition.y when character is standing on ground
	float targetViewHorizon;

	[Header("Setting")]
	public float crouchTime;
	public float crouchMoveSpeed;
	float timer;
	bool isCrouching;	//is in the process of stand -> crouch
	Transform view;

	protected override void Start()
	{
		cc = GetComponent<CharacterController>();
		view = FPSCharacterController.Instance.cam.transform;
		originalColliderHeight = cc.height;
		originalColliderCenter = cc.center;
		timer = 0f;
		isCrouching = false;

		originalViewHorinzon = view.localPosition.y;
		targetViewHorizon = originalViewHorinzon - viewHorizonLower;
	}

	protected override void Update()
	{
		if(!toggle)
			return;

		//TODO for now character cannot space+ctrl, there is conflict with state
		if(FPSCharacterController.Instance.characterState == FPSCharacterController.CharacterState.Jumping)
			return;

		//collider size down
		if(Input.GetButtonDown("Crouch"))
		{
			FPSCharacterController.Instance.characterState = FPSCharacterController.CharacterState.Crouching;
			FPSCharacterController.Instance.SetMoveSpeed(crouchMoveSpeed, crouchMoveSpeed);
			isCrouching = true;

			cc.height -= colliderSizeLower;
			cc.center = new Vector3(originalColliderCenter.x, originalColliderCenter.y - colliderSizeLower / 2f, originalColliderCenter.z);
		}

		//transform down
		if(isCrouching)
		{
			float viewHorizon = Mathf.Lerp(view.localPosition.y, targetViewHorizon, timer / crouchTime);
			timer += Time.deltaTime;
			if(Mathf.Abs(view.localPosition.y - targetViewHorizon) < 0.01f)
			{
				view.localPosition = new Vector3(view.localPosition.x, targetViewHorizon, view.localPosition.z);
				timer = crouchTime;
				isCrouching = false;
			}else{
				view.localPosition = new Vector3(view.localPosition.x, viewHorizon, view.localPosition.z);
			}
		}

		if(Input.GetButtonUp("Crouch"))
		{
			FPSCharacterController.Instance.characterState = FPSCharacterController.CharacterState.Idle;
			FPSCharacterController.Instance.ResetMoveSpeed();
			ResetViewHeight();

			cc.center = originalColliderCenter;
			cc.height = originalColliderHeight;
			isCrouching = false;
			timer = 0.0f;
		}
	}

	void ResetViewHeight()
	{
		view.localPosition = new Vector3(view.localPosition.x, originalViewHorinzon, view.localPosition.z);
	}

	/*
	 idea:
	when stand : !isCrouching + !CrouchState
	when stand -> crouch : isCrouching + CrouchState
	when crouch : !isCrouching + CrouchState
	*/
}
