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
	public int ammoLeft;
	public int fireSpeed;	//bullets per minute
	public float damage;
	public float initialVelocity;
	public float effectiveRange;
	public LayerMask shootableLayer;
	public Transform model;
	public Animator modelAnimator;
	public Transform muzzle;
	public Transform shellOut;

	[SerializeField]protected float recoilAnimTime;
	[SerializeField]protected float fireRate;
	protected float fireTimer;
	protected int currentAmmo;
	protected bool isReloading;

	[Space(10)]
	[Header("Audio")]
	public AudioClip fireSFX;
	public AudioClip reloadSFX;
	public AudioClip fireNoAmmoSFX;
	protected AudioSource gunAudio;

	[Space(10)]
	[Header("Other")]
	public bool useScriptedRecoil;
	public RecoilEffect recoilEffect = new RecoilEffect();

	[Space(10)]
	[Header("Effect")]
	public ParticleSystem gunFlare;
	public ObjectPool shells;
	public float outSpeed;
	public Vector3 outDirMins;
	public Vector3 outDirMaxs;
	public Light fireLight;

	protected Ray shootRay;
	protected Reticle reticle;

	protected virtual void Start()
	{
	}

	protected virtual void OnEnable()
	{
		//get recoil clip length, show it in inspector to compare with fire rate
		RuntimeAnimatorController rac = modelAnimator.runtimeAnimatorController;
		for(int i = 0; i < rac.animationClips.Length; i++)
		{
			if(rac.animationClips[i].name == "Recoil")
			{
				recoilAnimTime = rac.animationClips[i].length;
				break;
			}
		}

		fireRate = 1f / ((float)fireSpeed / 60f);
		recoilEffect.SetUp(this.transform.localRotation);

		gunAudio = GetComponent<AudioSource>();

		fireTimer = 0f;
		currentAmmo = magazineSize;
		//ammoLeft = magazineSize * 4;	//when pick up a new gun, there are 5 cartridge ammo
		isReloading = false;
		GUIManager.Instance.UpdateText(GUIManager.Instance.FullAmmoText, " / " + ammoLeft.ToString());
		GUIManager.Instance.UpdateText(GUIManager.Instance.CurrentAmmoText, currentAmmo.ToString());

		reticle = Camera.main.GetComponent<Reticle>();
	}

	protected virtual void Update()
	{
		if(isReloading)	//if is reloading, pause the timer
			return;

		if(Input.GetButtonDown("Reload") && ammoLeft > 0)
		{
			Reload();
		}

		if(ammoLeft + currentAmmo <= 0)	//no ammo, stop here
		{
			if(Input.GetButtonDown("Fire1"))
			{
				gunAudio.PlayOneShot(fireNoAmmoSFX);
			}
			return;
		}

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
		gunFlare.gameObject.SetActive(true);
		//lighting
		fireLight.gameObject.SetActive(true);
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
		//physics raycast of shooting
		//fow now this is basically same as eye raycasr, however I cast this new ray because I think eye raycast and gun raycast should be different in the future
		shootRay.origin = reticle.transform.position;
		shootRay.direction = reticle.transform.forward;
		RaycastHit hitInfo;
		if(Physics.Raycast(shootRay, out hitInfo, effectiveRange, shootableLayer, QueryTriggerInteraction.Ignore))
		{
			Health target = hitInfo.transform.GetComponent<Health>();

			if(target != null)
			{
				target.TakeDamage(hitInfo, damage);
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

		if(currentAmmo <= 0 && ammoLeft > 0)
		{
			currentAmmo = 0;
			isReloading = true;
			modelAnimator.SetTrigger("reload");
		}
	}

	void Reload()
	{
		isReloading = true;
		modelAnimator.SetTrigger("reload");
	}

	//animation event
	public void ReloadingFinish()
	{
		int ammoInCartridge = currentAmmo;

		if(currentAmmo + ammoLeft >= magazineSize)
		{
			currentAmmo = magazineSize;
		}else{
			currentAmmo = currentAmmo + ammoLeft;
		}

		ammoLeft -= currentAmmo - ammoInCartridge;

		GUIManager.Instance.UpdateText(GUIManager.Instance.CurrentAmmoText, currentAmmo.ToString());
		GUIManager.Instance.UpdateText(GUIManager.Instance.FullAmmoText, " / " + ammoLeft.ToString());
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
