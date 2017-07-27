using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour 
{
	public bool trackPlayer;
	public Vector3 trackOffset;

	private FPSCharacterController player;

	void Start()
	{
		player = FindObjectOfType<FPSCharacterController>();
	}

	void Update()
	{
		if(trackPlayer)
		{
			this.transform.position = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z) + trackOffset;
		}
	}
//	public Material miniMapMat;
//
//	void OnRenderImage(RenderTexture src, RenderTexture dst)
//	{
//		Graphics.Blit(src, dst, miniMapMat);
//	}

//	public List<Light> lights;
//
//	void OnPreRender()
//	{
//		foreach(Light l in lights)
//		{
//			l.enabled = false;
//		}
//	}
}
