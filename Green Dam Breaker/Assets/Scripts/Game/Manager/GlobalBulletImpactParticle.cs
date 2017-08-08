using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBulletImpactParticle : SingletonBase<GlobalBulletImpactParticle>
{
	public ObjectPool bulletImapctPool;
	public ObjectPool bulletHolePool;
	public ObjectPool enemyBulletPool;

	public void CreateBulletImpactAt(Vector3 pos)
	{
		GameObject bulletImpact = bulletImapctPool.GetPooledObj();
		bulletImpact.transform.position = pos;
	}
}


