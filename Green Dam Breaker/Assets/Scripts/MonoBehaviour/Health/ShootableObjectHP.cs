using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Shootable Object inherit from health, but just use as an interface. It doesnot decrease Hp nor die.
public class ShootableObjectHP : Health
{
	public override void TakeDamage(Vector3 hitPos)
	{
		//sfx
		if(getShootSound != null && audioSource != null)
		{
			audioSource.PlayOneShot(getShootSound);	
		}

		//particle hit effet
		GlobalBulletImpactParticle.Instance.CreateBulletImpactAt(hitPos);
	}
}
