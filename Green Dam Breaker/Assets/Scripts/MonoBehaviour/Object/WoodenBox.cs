using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenBox : MonoBehaviour, DestroyableObject
{
	public AudioClip getShootSound;
	AudioSource audioSource;

	void OnEnable()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void GetShoot(Vector3 hitPos)
	{
		//sfx
		audioSource.PlayOneShot(getShootSound);

		//particle hit effet
		GlobalBulletImpactParticle.Instance.CreateBulletImpactAt(hitPos);
	}
}
