using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour 
{
	public float oneDayTime;

	private float revolutionSpeed;

	void Start()
	{
		revolutionSpeed = 360f / oneDayTime;
	}

	void FixedUpdate()
	{
		this.transform.Rotate(Vector3.right, revolutionSpeed * Time.deltaTime);
	}
}
