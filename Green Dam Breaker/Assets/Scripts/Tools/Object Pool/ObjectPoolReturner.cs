using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolReturner : MonoBehaviour 
{
	public float time;

	void OnEnable()
	{
		CancelInvoke();
		Invoke("Return", time);
	}

	void Return()
	{
		this.gameObject.SetActive(false);
	}
}
