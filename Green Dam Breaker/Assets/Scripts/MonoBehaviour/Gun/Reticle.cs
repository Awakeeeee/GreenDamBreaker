using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reticle : MonoBehaviour 
{
	[Header("RayCast")]
	public bool toggle;
	public LayerMask detectLayer;
	public float detectLength;

	[Header("Visual")]
	public Canvas wposCanvas;
	public Image reticleUI;
	public float defaultReticleDist = 5.0f;

	void Start()
	{
		toggle = true;
		InitReticle();
	}

	void Update()
	{
		if(!toggle)
			return;

		EyeRaycast();
	}

	void InitReticle()
	{
		wposCanvas.transform.position = new Vector3(wposCanvas.transform.position.x, wposCanvas.transform.position.y, defaultReticleDist);
	}

	void EyeRaycast()
	{
		RaycastHit hitInfo;

		if(Physics.Raycast(this.transform.position, this.transform.forward, out hitInfo, detectLength, detectLayer))
		{
			FPSCharacterController.Instance.animingTarget = hitInfo.transform.gameObject;
			FPSCharacterController.Instance.targetInfo = hitInfo;
		}else{
			FPSCharacterController.Instance.animingTarget = null;
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * detectLength);
	}
}
