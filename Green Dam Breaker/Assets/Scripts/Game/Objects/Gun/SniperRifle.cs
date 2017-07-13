using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SniperRifle : Gun
{
	private bool inSniperSight = false;

	protected override void Update ()
	{
		if(isReloading)	//if is reloading, pause the timer
		{
			if(inSniperSight)
				OutOfSniperSight();

			return;
		}

		if(Input.GetButtonDown("Reload") && ammoLeft > 0)
		{
			Reload();
		}

		if(ammoLeft + currentAmmo <= 0)	//no ammo, stop here
		{
			if(Input.GetButtonDown("Fire1")) 
			{
				gunAudio.PlayOneShot(fireNoAmmoSFX);
			}
			return;
		}

		if(fireTimer < fireRate)	//fire in CD
		{
			fireTimer += Time.deltaTime;
		}else{						//ready to fire
			if(Input.GetButton("Fire1"))
			{
				fireTimer = 0.0f;
				Fire();
			}
		}

		ZoomView();
	}

	protected override void ZoomView ()
	{
		if(character.fovManipulater.isChangingFOV)
		{
			OutOfSiperSightWithoutCam();
			return;
		}

		if(cam.fieldOfView == character.fovManipulater.originalFOV + character.fovManipulater.changeAmount)
		{
			OutOfSiperSightWithoutCam();
			return;
		}

		zoomSpeed = Mathf.Abs(originFOV - originFOV * zoomScale) / zoomTime;

		if(Input.GetButton("Fire2"))
		{
			inSniperSight = true;

			//when fully complete zoom
			if(Mathf.Abs(cam.fieldOfView - originFOV * zoomScale) <= 0.01f)
			{
				cam.fieldOfView = originFOV * zoomScale;
				GUIManager.Instance.sniperSightMask.alpha = 1.0f;

				return;
			}

			float newFOV = Mathf.MoveTowards(cam.fieldOfView, originFOV * zoomScale, zoomSpeed * Time.deltaTime);
			cam.fieldOfView = newFOV;
			GUIManager.Instance.sniperSightMask.alpha = Mathf.MoveTowards(0f, 1f, zoomSpeed * Time.deltaTime);

			return;
		}

		//if not holding zoom button
		if(cam.fieldOfView != originFOV)
		{
			float newFOV = Mathf.MoveTowards(cam.fieldOfView, originFOV, zoomSpeed * Time.deltaTime);
			cam.fieldOfView = newFOV;
			GUIManager.Instance.sniperSightMask.alpha = Mathf.MoveTowards(1f, 0f, zoomSpeed * Time.deltaTime);

			//fully zoomed back
			if(Mathf.Abs(cam.fieldOfView - originFOV) <= 0.01f)
			{
				cam.fieldOfView = originFOV;
				GUIManager.Instance.sniperSightMask.alpha = 0f;
				inSniperSight = false;
			}
		}
	}

	void OutOfSniperSight()
	{
		cam.fieldOfView = originFOV;
		OutOfSiperSightWithoutCam();
	}

	void OutOfSiperSightWithoutCam()
	{
		GUIManager.Instance.sniperSightMask.alpha = 0f;
		inSniperSight = false;
	}
}
