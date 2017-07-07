using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour 
{
	public enum MiniMapType
	{
		Whole,
		Track,
		TrackUnderPlayerView
	}

	public MiniMapType miniMapType;

	private FPSCharacterController player;

	void Start()
	{
		player = FindObjectOfType<FPSCharacterController>();
	}

	void Update()
	{
		if(miniMapType == MiniMapType.Track)
		{
			this.transform.position = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z);
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
