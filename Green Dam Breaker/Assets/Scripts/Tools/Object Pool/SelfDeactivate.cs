using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDeactivate : MonoBehaviour 
{
	public enum DelayMode
	{
		StartToCount,
		CallByAnimation
	}
	public DelayMode mode;

	public float time;

	void Start()
	{
		if(mode == DelayMode.StartToCount)
			Invoke("Deactivate", time);
	}

	public void Deactivate()
	{
		this.gameObject.SetActive(false);
	}
}
