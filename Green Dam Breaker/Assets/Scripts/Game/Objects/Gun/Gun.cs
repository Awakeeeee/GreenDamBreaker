using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
	public enum MachineType
	{
		Automatic,
		Semi_Automatic,
		OldStyle
	}

	[Header("Category")]
	public MachineType machineType;

	[Header("Infomation")]
	public string gunName;
	[Multiline]
	public string description;
	public int ID;	//ID is the key to distinguish different guns

	[Header("Parameter: Ammo")]
	public int magazineSize;
	public int ammoLeft;

	[Header("Parameter: Shoot")]
	public int fireSpeed;	//bullets per minute
	public float damage;
	public float initialVelocity;
	public float effectiveRange;
	public LayerMask shootableLayer;

	[Header("Parameter: Aim")]
	public float zoomScale = 0.5f;
	public float zoomTime = 1.0f;

	[Header("Model")]
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
	public ParticleSystem gunFlarePrefab;
	protected ParticleSystem gunFlare;
	public ObjectPool shells;
	public float outSpeed;
	public Vector3 outDirMins;
	public Vector3 outDirMaxs;
	public Light fireLight;

	protected Ray shootRay;
	protected Reticle reticle;

	//Migrate from 'WeaponAbility'
	protected FPSCharacterController character;
	protected Camera cam;
	protected float originFOV;
	protected float zoomSpeed;

	protected virtual void Start()
	{
		character = FPSCharacterController.Instance;
		cam = character.cam;
		if(cam)
		{
			originFOV = cam.fieldOfView;
		}

		if(gunFlarePrefab != null)
		{
			gunFlare = Instantiate(gunFlarePrefab, this.transform);
			gunFlare.transform.localPosition = Vector3.zero;
			gunFlare.gameObject.SetActive(false);
		}
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
		UpdateAmmoUI();

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

		ZoomView();
	}

	protected virtual void Fire()
	{
		if(EventSystem.current.IsPointerOverGameObject())
			return;
		
		//fire sfx
		gunAudio.PlayOneShot(fireSFX);
		//gun flare
		gunFlare.gameObject.transform.position = muzzle.position;
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
				target.TakeDamage(damage, hitInfo);
			}
		}
	}

	protected virtual void ZoomView()
	{
		if(character.fovManipulater.isChangingFOV)
			return;

		if(cam.fieldOfView == character.fovManipulater.originalFOV + character.fovManipulater.changeAmount)
			return;

		zoomSpeed = Mathf.Abs(originFOV - originFOV * zoomScale) / zoomTime;

		if(Input.GetButton("Fire2"))
		{
			//when fully complete zoom
			if(Mathf.Abs(cam.fieldOfView - originFOV * zoomScale) <= 0.01f)
			{
				cam.fieldOfView = originFOV * zoomScale;

				return;
			}

			float newFOV = Mathf.MoveTowards(cam.fieldOfView, originFOV * zoomScale, zoomSpeed * Time.deltaTime);
			cam.fieldOfView = newFOV;

			return;
		}

		//if not holding zoom button
		if(cam.fieldOfView != originFOV)
		{
			float newFOV = Mathf.MoveTowards(cam.fieldOfView, originFOV, zoomSpeed * Time.deltaTime);
			cam.fieldOfView = newFOV;

			//fully zoomed back
			if(Mathf.Abs(cam.fieldOfView - originFOV) <= 0.01f)
			{
				cam.fieldOfView = originFOV;
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
		UpdateAmmoUI(false);

		if(currentAmmo <= 0 && ammoLeft > 0)
		{
			currentAmmo = 0;
			Reload();
		}
	}

	protected virtual void Reload()
	{
		isReloading = true;
		modelAnimator.SetTrigger("reload");
	}

	public virtual void CollectAmmo(int amount)
	{
		ammoLeft += amount;
		UpdateAmmoUI();
	}

	protected virtual void UpdateAmmoUI(bool updateLeft = true)
	{
		GUIManager.Instance.UpdateText(GUIManager.Instance.CurrentAmmoText, currentAmmo.ToString());
		if(updateLeft)
		{
			GUIManager.Instance.UpdateText(GUIManager.Instance.FullAmmoText, " / " + ammoLeft.ToString());
		}
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

		UpdateAmmoUI();
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
