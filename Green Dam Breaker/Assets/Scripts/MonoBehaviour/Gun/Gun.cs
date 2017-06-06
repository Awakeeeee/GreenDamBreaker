using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Gun parameters:
- origin production area
- gun type
- machine type
- caliber size
- bullet type
- magazine size
- empty gun weight
- full gun length
- fire speed (bullets / min)
- initial velocity (m / s)
- effective range (m)
*/

/// <summary>
/// Base class for weapons.
/// </summary>
public class Gun : MonoBehaviour 
{
	public enum GunType
	{
		Pistol,
		Rifle,
		ScatterGun,
		SniperRifle
	}

	public enum MachineType
	{
		Automatic,
		Semi_Automatic,
		OldStyle
	}

	public GunType guntype;
	public MachineType machineType;
	public int magazineSize;
	public int fireSpeed;	//bullets per minute
	public float initialVelocity;
	public float effectiveRange;

	[SerializeField]protected float fireRate;
	protected float fireTimer;

	[Space(10)]
	[Header("Audio")]
	public AudioClip fireSFX;
	protected AudioSource gunAudio;

	[Space(10)]
	[Header("Other")]
	public RecoilEffect recoilEffect = new RecoilEffect();

	protected virtual void Start()
	{
		fireRate = 1f / ((float)fireSpeed / 60f);
		fireTimer = 0f;
		recoilEffect.SetUp(this.transform.localRotation);

		gunAudio = GetComponent<AudioSource>();
	}

	protected virtual void Update()
	{
		if(fireTimer < fireRate)	//fire in CD
		{
			fireTimer += Time.deltaTime;
		}else{						//ready to fire
			if(Input.GetButton("Fire1"))
			{
				fireTimer = 0.0f;
				Fire();
			}
		}
	}

	protected virtual void Fire()
	{
		Debug.Log("Fire!");
		gunAudio.PlayOneShot(fireSFX);
		StartCoroutine(recoilEffect.DoRecoil(this.transform));
	}
}
