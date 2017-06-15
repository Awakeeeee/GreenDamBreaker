using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDisableComponent : MonoBehaviour 
{
	public LineRenderer targetRenderer;
	public float disableInTime;

	private float timer;

	void OnEnable()
	{
		timer = 0.0f;
	}

	void Update()
	{
		if(!targetRenderer)
			return;
		
		if(targetRenderer.enabled == false)
			return;

		timer += Time.deltaTime;
		if(timer >= disableInTime)
		{
			timer = 0.0f;
			targetRenderer.enabled = false;
		}
	}
}
