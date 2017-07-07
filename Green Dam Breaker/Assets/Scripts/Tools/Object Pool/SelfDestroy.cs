using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour 
{
	public float delay;

	void OnEnable()
	{
		Invoke("DestroySelf", delay);
	}

	void DestroySelf()
	{
		Destroy(this.gameObject);
	}
}
