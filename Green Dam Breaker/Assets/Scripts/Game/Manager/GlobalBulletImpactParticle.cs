using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBulletImpactParticle : SingletonBase<GlobalBulletImpactParticle>
{
	public ObjectPool bulletImapctPool;
	public ObjectPool bulletHolePool;

	public void CreateBulletImpactAt(Vector3 pos)
	{
		GameObject bulletImpact = bulletImapctPool.GetPooledObj();
		bulletImpact.transform.position = pos;
	}

	public GameObject GetPooledObject(GlobalImpactType type)
	{
		switch(type)
		{
		case GlobalImpactType.BulletImpactParticle:
			return null;
		case GlobalImpactType.BulletHoleTexture:
			GameObject bulletHole = bulletHolePool.GetPooledObj();
			return bulletHole;
		default:
			return null;
		}
	}
}

public enum GlobalImpactType
{
	BulletImpactParticle,
	BulletHoleTexture,
}

