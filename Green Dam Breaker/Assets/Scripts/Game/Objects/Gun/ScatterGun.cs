using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScatterGun : Gun
{
	public float scatterRange = 0.3f;

	protected override void Fire ()
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
		Ray[] shootRays = new Ray[9];
		shootRays[0].origin = reticle.transform.position;
		shootRays[0].direction = reticle.transform.forward + reticle.transform.right * Random.Range(0, scatterRange);
		shootRays[1].origin = reticle.transform.position;
		shootRays[1].direction = reticle.transform.forward - reticle.transform.right * Random.Range(0, scatterRange);
		shootRays[2].origin = reticle.transform.position;
		shootRays[2].direction = reticle.transform.forward + reticle.transform.up * Random.Range(0, scatterRange);
		shootRays[3].origin = reticle.transform.position;
		shootRays[3].direction = reticle.transform.forward - reticle.transform.up * Random.Range(0, scatterRange);
		shootRays[4].origin = reticle.transform.position;
		shootRays[4].direction = reticle.transform.forward;
		shootRays[5].origin = reticle.transform.position;
		shootRays[5].direction = reticle.transform.forward + reticle.transform.right * Random.Range(0, scatterRange) + reticle.transform.up * Random.Range(0, scatterRange);
		shootRays[6].origin = reticle.transform.position;
		shootRays[6].direction = reticle.transform.forward + reticle.transform.right * Random.Range(0, scatterRange) - reticle.transform.up * Random.Range(0, scatterRange);
		shootRays[7].origin = reticle.transform.position;
		shootRays[7].direction = reticle.transform.forward - reticle.transform.right * Random.Range(0, scatterRange) + reticle.transform.up * Random.Range(0, scatterRange);
		shootRays[8].origin = reticle.transform.position;
		shootRays[8].direction = reticle.transform.forward - reticle.transform.right * Random.Range(0, scatterRange) - reticle.transform.up * Random.Range(0, scatterRange);

		for(int i = 0; i < shootRays.Length; i++)
		{
			RaycastHit hitInfo;
			if(Physics.Raycast(shootRays[i], out hitInfo, effectiveRange, shootableLayer, QueryTriggerInteraction.Ignore))
			{
				Health target = hitInfo.transform.GetComponent<Health>();

				if(target != null)
				{
					target.TakeDamage(damage, hitInfo);
				}
			}
		}
	}
}
