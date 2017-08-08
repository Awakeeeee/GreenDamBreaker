using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour 
{
	public AudioClip getShootSound;
	public AudioClip dieSound;
	protected AudioSource audioSource;

	protected virtual void OnEnable()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public virtual void TakeDamage(float damage, RaycastHit hit = default(RaycastHit))
	{
		
	}
}
