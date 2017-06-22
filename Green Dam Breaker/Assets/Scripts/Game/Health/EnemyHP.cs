using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIStateMachine))]
public class EnemyHP : Health
{
	public GameObject[] bodyParts;	//optional

	public float maxHP;
	public float deathHitBackForce;
	public float startToSinkAfter;
	public float sinkTime;
	public float sinkSpeed;
	public ParticleSystem deathEffect;
	public Collider nonTriggerCollider;

	private float currentHP;
	bool isDead;
	AIStateMachine enemyAI;
	Rigidbody body;

	protected override void OnEnable ()
	{
		base.OnEnable ();
		currentHP = maxHP;
		isDead = false;
		deathEffect.gameObject.SetActive(false);
		body = GetComponent<Rigidbody>();
		enemyAI = GetComponent<AIStateMachine>();
	}

	public override void TakeDamage(RaycastHit hit, float damage)
	{
		currentHP -= damage;
		if(currentHP <= 0f && !isDead)
		{
			currentHP = 0f;
			StartCoroutine(DieCo(hit));
		}

		//sfx
		if(getShootSound != null && audioSource != null)
		{
			audioSource.PlayOneShot(getShootSound);	
		}

		//particle hit effet
		GlobalBulletImpactParticle.Instance.CreateBulletImpactAt(hit.point);
	}

	IEnumerator DieCo(RaycastHit hit)
	{
		isDead = true;
		enemyAI.enabled = false;

		body.useGravity = true;
		body.AddForce(-hit.normal * deathHitBackForce);

		deathEffect.transform.rotation = Quaternion.LookRotation(-hit.normal);
		deathEffect.transform.position = hit.point;
		deathEffect.gameObject.SetActive(true);

		yield return new WaitForSeconds(startToSinkAfter);

		nonTriggerCollider.enabled = false;
		float timer = 0.0f;
		while(timer < sinkTime)
		{
			transform.Translate(Vector3.down * sinkSpeed * Time.deltaTime);
			timer += Time.deltaTime;
			yield return null;
		}

		Destroy(this.gameObject);
	}
}
