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

	public bool isAimingTarget;

	public GunType guntype;
	public MachineType machineType;
	public int magazineSize;
	public int fireSpeed;	//bullets per minute
	public float initialVelocity;
	public float effectiveRange;
	public Transform model;
	public Animator modelAnimator;
	public Transform muzzle;
	public Transform shellOut;

	[SerializeField]protected float fireRate;
	protected float fireTimer;
	protected int currentAmmo;
	protected bool isReloading;

	[Space(10)]
	[Header("Audio")]
	public AudioClip fireSFX;
	public AudioClip reloadSFX;
	protected AudioSource gunAudio;

	[Space(10)]
	[Header("Other")]
	public bool useScriptedRecoil;
	public RecoilEffect recoilEffect = new RecoilEffect();

	[Space(10)]
	[Header("Effect")]
	public ObjectPool gunFlares;
	public ObjectPool shells;
	public float outSpeed;
	public Vector3 outDirMins;
	public Vector3 outDirMaxs;
	public GameObject pointLight;

	protected virtual void Start()
	{
		fireRate = 1f / ((float)fireSpeed / 60f);
		recoilEffect.SetUp(this.transform.localRotation);

		gunAudio = GetComponent<AudioSource>();
	}

	protected virtual void OnEnable()
	{
		fireTimer = 0f;
		currentAmmo = magazineSize;
		isReloading = false;
		GUIManager.Instance.UpdateText(GUIManager.Instance.FullAmmoText, " / " + magazineSize.ToString());
		GUIManager.Instance.UpdateText(GUIManager.Instance.CurrentAmmoText, currentAmmo.ToString());
	}

	protected virtual void Update()
	{
		if(isReloading)	//if is reloading, pause the timer
			return;
		
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
		//fire sfx
		gunAudio.PlayOneShot(fireSFX);
		//gun flare
		GameObject gunFlare = gunFlares.GetPooledObj();
		gunFlare.transform.position = muzzle.position;
		//lighting
		pointLight.SetActive(true);
		//bullet shell pop out
		ShellPopOut();
		//update ammo
		UpdateAmmo();
		//recoil effect
		if(useScriptedRecoil)
		{
			StartCoroutine(recoilEffect.DoRecoil(model.transform));	
		}else{
			modelAnimator.SetTrigger("recoil");
		}

		//notify potential target that I am shooting you
		GameObject target = FPSCharacterController.Instance.animingTarget;
		if(target != null)
		{
			RaycastHit targetInfo = FPSCharacterController.Instance.targetInfo;

			if(target.GetComponent(typeof(DestroyableObject)))	//if this is a real target
			{
				DestroyableObject destroyableTarget = target.GetComponent(typeof(DestroyableObject)) as DestroyableObject;
				destroyableTarget.GetShoot(targetInfo.point);
			}else{												//if this is just some block
				
			}
		}
	}

	protected virtual void ShellPopOut()
	{
		GameObject shell = shells.GetPooledObj();
		shell.transform.position = shellOut.position;

		float changerX = Random.Range(outDirMins.x, outDirMaxs.x);
		float changerY = Random.Range(outDirMins.y, outDirMaxs.y);
		float changerZ = Random.Range(outDirMins.z, outDirMaxs.z);
		Vector3 outDir = (shellOut.forward * changerZ + shellOut.up * changerY + shellOut.right * changerX).normalized;

		shell.GetComponent<Rigidbody>().velocity = outDir * outSpeed;
	}

	protected virtual void UpdateAmmo()
	{
		currentAmmo -= 1;
		GUIManager.Instance.UpdateText(GUIManager.Instance.CurrentAmmoText, currentAmmo.ToString());

		if(currentAmmo <= 0)
		{
			currentAmmo = 0;
			isReloading = true;
			modelAnimator.SetTrigger("reload");
		}
	}

	//animation event
	public void ReloadingFinish()
	{
		currentAmmo = magazineSize;
		GUIManager.Instance.UpdateText(GUIManager.Instance.CurrentAmmoText, currentAmmo.ToString());
		fireTimer = fireRate;
		isReloading = false;
	}
	public void ReloadingSFX()
	{
		gunAudio.PlayOneShot(reloadSFX);
	}

	//test
	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(shellOut.position, shellOut.position + shellOut.forward * outDirMaxs.z + shellOut.up * outDirMaxs.y + shellOut.right * outDirMaxs.x);
	}
}
