using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SniperRifle : Gun
{
	private bool inSniperSight = false;
	private CanvasGroup reticlePrompt;

	protected override void OnEnable ()
	{
		base.OnEnable ();
		reticlePrompt = reticle.wposCanvas.GetComponent<CanvasGroup>();
	}

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

		if(Input.GetButtonDown("Fire2"))
		{
			IntoSniperSight();
		}

		if(Input.GetButton("Fire2"))
		{
			//if for any reason, it comes here with out ButtonDown event, reset sniper sight state
			if(!inSniperSight)
			{
				IntoSniperSight();
			}

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
			float newFOV = Mathf.MoveTowards(cam.fieldOfView, originFOV, zoomSpeed * 2f * Time.deltaTime);
			cam.fieldOfView = newFOV;
			GUIManager.Instance.sniperSightMask.alpha = Mathf.MoveTowards(1f, 0f, zoomSpeed * 2f * Time.deltaTime);

			//fully zoomed back
			if(Mathf.Abs(cam.fieldOfView - originFOV) <= 0.01f)
			{
				OutOfSniperSight();
			}
		}
	}

	void IntoSniperSight()
	{
		inSniperSight = true;
		reticlePrompt.alpha = 0.0f;
		FPSCharacterController.Instance.SetRotateSensitivity(zoomScale, zoomScale);	//I think it makes sense that rotate slow down depends on zoomscale
	}
	void OutOfSniperSight()
	{
		cam.fieldOfView = originFOV;
		OutOfSiperSightWithoutCam();
	}
	void OutOfSiperSightWithoutCam()
	{
		GUIManager.Instance.sniperSightMask.alpha = 0f;
		reticlePrompt.alpha = 1.0f;
		inSniperSight = false;
		FPSCharacterController.Instance.ResetRotateSensityvity();
	}
}
