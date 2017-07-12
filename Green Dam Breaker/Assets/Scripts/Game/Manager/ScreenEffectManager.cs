using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEffectManager : MonoBehaviour 
{
	[Range(0, 20)]
	public int blurIterration = 1;
	public Material effectMat;

	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		BoxBlurEffect(src, dst);
	}

	void BoxBlurEffect(RenderTexture src, RenderTexture dst)
	{
		RenderTexture rt = RenderTexture.GetTemporary(src.width, src.height);
		Graphics.Blit(src, rt);	//now rt = src

		for(int i = 0; i < blurIterration; i++)
		{
			RenderTexture rt2 = RenderTexture.GetTemporary(rt.width, rt.height);
			Graphics.Blit(rt, rt2, effectMat);	//now rt2 = rt + shader effect

			RenderTexture.ReleaseTemporary(rt);
			rt = rt2;
		}

		Graphics.Blit(rt, dst, effectMat);
		RenderTexture.ReleaseTemporary(rt);
	}
}
