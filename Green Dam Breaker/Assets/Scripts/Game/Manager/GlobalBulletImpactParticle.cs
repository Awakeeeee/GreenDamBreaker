using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBulletImpactParticle : SingletonBase<GlobalBulletImpactParticle>
{
	public ObjectPool particles;

	public void CreateBulletImpactAt(Vector3 pos)
	{
		GameObject bulletImpact = particles.GetPooledObj();
		bulletImpact.transform.position = pos;
	}
}
