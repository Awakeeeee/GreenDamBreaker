using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// A tool class used to change camera FOV.
/// </summary>
[System.Serializable]
public class FOVManipulater
{
	[HideInInspector] public bool isChangingFOV;

	public Camera targetCamera;
	public float changeAmount = 3f;
	public float increaseTime = 1f;
	public float decreaseTime = 1f;
	//x = already used time / total increase or decreaase time, x max is always 1, meaning fov change is over
	//y = the proportion of changeAmount that has been applied, y max is always 1
	//so this is a (0,0) ~ (1,1) curve
	public AnimationCurve changeCurve;

	[HideInInspector] public float originalFOV;

	//Some setting are meant to be set in Unity Inspector so here is no constructor to set them

	public FOVManipulater()
	{}

	public void Setup(Camera camera)
	{
		isChangingFOV = false;

		targetCamera = camera;

		CheckState();

		originalFOV = targetCamera.fieldOfView;
	}

	public void CheckState()
	{
		if(targetCamera == null)
		{
			targetCamera = Camera.main;
			if(targetCamera == null)
			{
				throw new Exception("No camera set on FOVManipulator."); //throw exception will let program progress halt
			}
		}

		if(changeCurve == null)
		{
			throw new Exception("Change curve is not specified on FOVManipulator.");
		}
	}

	public IEnumerator IncreaseFOV()
	{
		isChangingFOV = true;
		//Start time is not always 0, in case FOV starts to increase when half way it is decreasing, and vice versa
		//this is appoximately the start time(when curve is straight line this should the exact time)
		float timer = Mathf.Abs((targetCamera.fieldOfView - originalFOV)/changeAmount) * increaseTime;

		while(timer < increaseTime)
		{
			targetCamera.fieldOfView = originalFOV + (timer / increaseTime) * changeAmount;
			timer += Time.deltaTime;
			yield return null;
		}

		targetCamera.fieldOfView = originalFOV + changeAmount;
		isChangingFOV = false;
	}

	public IEnumerator DecreaseFOV()
	{
		isChangingFOV = true;
		float timer = Mathf.Abs((targetCamera.fieldOfView - originalFOV)/changeAmount) * decreaseTime;

		while(timer > 0)
		{
			targetCamera.fieldOfView = originalFOV + (timer / decreaseTime) * changeAmount;
			timer -= Time.deltaTime;
			yield return null;
		}

		targetCamera.fieldOfView = originalFOV;
		isChangingFOV = false;
	}
}
