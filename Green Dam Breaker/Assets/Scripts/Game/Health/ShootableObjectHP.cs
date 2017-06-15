using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Shootable Object inherit from health, but just use as an interface. It doesnot decrease Hp nor die.
public class ShootableObjectHP : Health
{
	public override void TakeDamage(RaycastHit hit, float damage)
	{
		//sfx
		if(getShootSound != null && audioSource != null)
		{
			audioSource.PlayOneShot(getShootSound);	
		}

		//particle hit effet
		GlobalBulletImpactParticle.Instance.CreateBulletImpactAt(hit.point);
		GameObject bulletHole = GlobalBulletImpactParticle.Instance.GetPooledObject(GlobalImpactType.BulletHoleTexture);
		bulletHole.transform.position = hit.point;
		bulletHole.transform.rotation = Quaternion.LookRotation(hit.normal);
	}
}
