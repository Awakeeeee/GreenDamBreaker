using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Shootable Object inherit from health, but just use as an interface. It doesnot decrease Hp nor die.
public class ShootableObjectHP : Health
{
	public override void TakeDamage(float damage, RaycastHit hit = default(RaycastHit))
	{
		//sfx
		if(getShootSound != null && audioSource != null)
		{
			audioSource.PlayOneShot(getShootSound);	
		}

		if(hit.transform != null)
		{
			//particle hit effet
			GlobalBulletImpactParticle.Instance.CreateBulletImpactAt(hit.point);
			GameObject bulletHole = GlobalBulletImpactParticle.Instance.bulletHolePool.GetPooledObj();
			bulletHole.transform.position = hit.point;
			bulletHole.transform.rotation = Quaternion.LookRotation(hit.normal);
		}
	}
		
	/// Without ray and damage
	public void TakeDamageAsBlock(Vector3 pos)
	{
		if(getShootSound != null && audioSource != null)
		{
			audioSource.PlayOneShot(getShootSound);	
		}
		GlobalBulletImpactParticle.Instance.CreateBulletImpactAt(pos);
	}
}
