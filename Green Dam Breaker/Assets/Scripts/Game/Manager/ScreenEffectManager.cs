using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEffectManager : SingletonBase<ScreenEffectManager>
{
	[Header("Effect Materials")]
	public Material boxBlurMat;
	public Material distortionMat;

	public enum EffectType
	{
		BoxBlur,
		Distortion
	}
	public EffectType eType;

	[Header("Box Blur")]
	public int srcShiftDownScaler = 1;
	[Range(0, 20)]public int blurIterration = 1;

	[Header("Distortion")]
	public float magnitude = 0.05f;
	public Texture noiseTex;

	private Material effectMat;

	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		switch(eType)
		{
		case EffectType.BoxBlur:
			BoxBlurEffect(src, dst);
			break;
		case EffectType.Distortion:
			DistortionEffect(src, dst);
			break;
		default:
			break;
		}
	}

	void BoxBlurEffect(RenderTexture src, RenderTexture dst)
	{
		effectMat = boxBlurMat;

		int width = src.width >> srcShiftDownScaler;	//bit wise calculation, scale down the image size so it becomes fuzzy
		int height = src.height >> srcShiftDownScaler;

		RenderTexture rt = RenderTexture.GetTemporary(width, height);
		Graphics.Blit(src, rt);	//now rt = src with a lower size

		for(int i = 0; i < blurIterration; i++)
		{
			RenderTexture rt2 = RenderTexture.GetTemporary(rt.width, rt.height);
			Graphics.Blit(rt, rt2, effectMat);	//now rt2 = rt + shader blur effect

			RenderTexture.ReleaseTemporary(rt);
			rt = rt2;
		}

		Graphics.Blit(rt, dst, effectMat);
		RenderTexture.ReleaseTemporary(rt);
	}

	void DistortionEffect(RenderTexture src, RenderTexture dst)
	{
//		distortionMat.SetTexture("NoiseTex", noiseTex);
//		distortionMat.SetFloat("_NoiseMagnitude", magnitude);	//TODO doesnt work
		effectMat = distortionMat;

		Graphics.Blit(src, dst, effectMat);
	}
}
