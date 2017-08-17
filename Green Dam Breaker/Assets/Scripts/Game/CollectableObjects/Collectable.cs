using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour 
{
	public Transform parent;
	public Transform minimapMark;
 	public float spinSpeed = 60f;

	protected virtual void Update()
	{
		transform.Rotate(new Vector3(spinSpeed, spinSpeed, spinSpeed) * Time.deltaTime);
		minimapMark.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
	}
}
