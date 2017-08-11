using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour 
{
	private float damage;
	public float Damage {get{return damage;}}
	private Vector3 direction;
	private float speed;
	private TrailRenderer tr;

	void Awake()
	{
		tr = GetComponent<TrailRenderer>();
	}

	void OnEnable()
	{
		
	}

	void OnDisable()
	{
		tr.Clear();
	}

	void Update()
	{
		Vector3 newPos = transform.position + direction * speed * Time.deltaTime;
		transform.position = newPos;
	}

	void OnTriggerEnter(Collider other)
	{
		ShootableObjectHP shp = other.GetComponent<ShootableObjectHP>();
		if(shp != null)
		{
			shp.TakeDamageAsBlock(this.transform.position);
			this.gameObject.SetActive(false);
			return;
		}

		CharacterHP chp = other.GetComponent<CharacterHP>();
		if(chp != null)
		{
			chp.TakeDamage(damage);
		}
	}

	public void SetBullet(float _damage, Vector3 _direction, float _speed)
	{
		damage = _damage;
		direction = _direction;
		speed = _speed;
	}
}
