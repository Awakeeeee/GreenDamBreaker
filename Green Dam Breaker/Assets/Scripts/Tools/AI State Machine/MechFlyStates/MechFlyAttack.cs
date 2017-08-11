using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MechFlyAttack : MechFlyStateBase
{
	public int fireTime;
	public float fireInterval;
	public float damage;
	public float bulletSpeed;

	private float timer;
	private int fireCount;
	private Transform playerTrans;

	public override void OnEnter ()
	{
		timer = 0.0f;
		fireCount = 0;

		sfsm.modelAnim.SetTrigger("toAttack");
		playerTrans = sfsm.Player.transform;
	}

	public override void StateUpdate ()
	{
		sfsm.model.LookAt(playerTrans.position);

		if(fireCount >= fireTime)
		{
			sfsm.TransitState(sfsm.wait);
			return;
		}

		timer += Time.deltaTime;
		if(timer > fireInterval)
		{
			timer = 0.0f;
			EnemyFire();
		}
	}

	public override void OnExit ()
	{
		sfsm.modelAnim.SetTrigger("toIdle");
	}

	void EnemyFire()
	{
		sfsm.ShowFireFlare();

		Vector3 playerPos = Camera.main.transform.position;	//hit on face
		Vector3 bulletDir = (playerPos - sfsm.transform.position).normalized;
		EnemyBullet eb = GlobalBulletImpactParticle.Instance.enemyBulletPool.GetPooledObj(true).GetComponent<EnemyBullet>();
		eb.transform.position = sfsm.muzzlePos.position;
		eb.gameObject.SetActive(true);

		if(eb != null)
		{
			eb.SetBullet(damage, bulletDir, bulletSpeed);
		}
		fireCount++;
	}
}
