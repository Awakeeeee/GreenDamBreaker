using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableGun : MonoBehaviour, ICollectable
{
	public Gun gun;

	void OnEnable()
	{
	}

	void Update()
	{
		transform.Rotate(new Vector3(45f, 45, 45f) * Time.deltaTime);
	}

	public void Collect()
	{
		PersonalIntelligentMachine.Instance.AddGunToCollection(gun);
		Destroy(this.gameObject);
	}
}
