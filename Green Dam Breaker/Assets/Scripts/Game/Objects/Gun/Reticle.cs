using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reticle : MonoBehaviour 
{
	[Header("RayCast")]
	public bool toggle;
	public LayerMask eyeDetectLayer;
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
		wposCanvas.transform.localPosition = new Vector3(wposCanvas.transform.localPosition.x, wposCanvas.transform.localPosition.y, defaultReticleDist);
		//eyeDetectLayer = ~LayerMask.GetMask("Ignore Raycast");
	}

	/// <summary>
	/// Raycast of sight, this is not gun shot raycast.
	/// </summary>
	void EyeRaycast()
	{
		RaycastHit hitInfo;

		if(Physics.Raycast(this.transform.position, this.transform.forward, out hitInfo, detectLength, eyeDetectLayer))
		{
			FPSCharacterController.Instance.lookingAtObject = hitInfo.transform.gameObject;
		}else{
			FPSCharacterController.Instance.lookingAtObject = null;
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * detectLength);
	}
}
