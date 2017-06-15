using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDeactivate : MonoBehaviour 
{
	public float time;

	void Start()
	{
		Invoke("Deactivate", time);
	}

	void Deactivate()
	{
		//TODO
	}
}
