using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : Health
{
	public GameObject[] bodyParts;	//optional
	public Transform miniMapMark;

	public float maxHP;
	public ParticleSystem deathEffectPrefab;
	private ParticleSystem deathEffect;
	public Collider nonTriggerCollider;

	public enum DieEffect
	{
		Sink,
		Crush
	}
	public DieEffect dieEffect;

	[Header("If Die to Sink")]
	public float deathHitBackForce;
	public float startToSinkAfter;
	public float sinkTime;
	public float sinkSpeed;

	float currentHP;
	AIStateMachine enemyAI;
	Rigidbody body;

	private bool isDead;
	public bool IsDead { get {return isDead; }}

	public event System.Action OnDeath;	//a non-static event which belongs to each enemy instance

	void Start()
	{
		if(deathEffectPrefab != null)
		{
			deathEffect = Instantiate(deathEffectPrefab, this.transform);
			deathEffect.transform.localPosition = Vector3.zero;
			deathEffect.gameObject.SetActive(false);
		}
	}

	protected override void OnEnable ()
	{
		base.OnEnable ();
		currentHP = maxHP;
		isDead = false;
		body = GetComponent<Rigidbody>();
		enemyAI = GetComponent<AIStateMachine>();
	}

	public override void TakeDamage(float damage, RaycastHit hit = default(RaycastHit))
	{
		currentHP -= damage;

		//sfx
		if(getShootSound != null && audioSource != null)
		{
			audioSource.PlayOneShot(getShootSound);	
		}

		//particle hit effet
		GlobalBulletImpactParticle.Instance.CreateBulletImpactAt(hit.point);

		//die
		if(currentHP <= 0f && !isDead)
		{
			currentHP = 0f;
			CommonDie();
			//die effect
			switch(dieEffect)
			{
				case DieEffect.Sink:
					StartCoroutine(SinkDieCo(hit));
					break;
				case DieEffect.Crush:
					CrushDie();
					break;
				default:
					break;
			}
		}
	}
		
	void CrushDie()
	{
		Renderer rr = deathEffect.GetComponent<Renderer>();
		if(rr != null)
		{
			Material newMat = new Material(rr.sharedMaterial);
			newMat.color = this.bodyParts[0].GetComponent<Renderer>().sharedMaterial.color;
			rr.material = newMat;
		}

		SelfDestroy sd = deathEffect.GetComponent<SelfDestroy>();
		if(sd != null)
		{
			sd.delay = deathEffect.main.startLifetimeMultiplier + 0.1f;
			sd.enabled = true;
		}

		deathEffect.transform.SetParent(null);
		deathEffect.gameObject.SetActive(true);

		Destroy(this.gameObject);
	}

	IEnumerator SinkDieCo(RaycastHit hit)
	{
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

	void CommonDie()
	{
		isDead = true;
		if(miniMapMark != null)
		{
			miniMapMark.gameObject.SetActive(false);
		}

		if(dieSound != null)
		{
			SoundManager.Instance.PlayClip2D(dieSound);
		}

		if(OnDeath != null)
		{
			OnDeath();
		}

		if(enemyAI != null)
		{
			enemyAI.DeactiveSelf();
		}
	}
}
