Shader "My/BoxBlurScreen" 
{
	Properties 
	{
		_MainTex("Main Texture", 2D) = "white"{}
		_Color("Color", Color) = (1, 1, 1, 1)
	}

	SubShader 
	{
		Tags{ "Queue" = "Transparent" }	//TODO compulsory?
		
		pass
		{
		blend SrcAlpha OneMinusSrcAlpha	//TODO compulsory?

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _Color;
			float4 _MainTex_TexelSize;

			struct v2f
			{
				float4 wpos : SV_POSITION;
				float2 uv : TEXCOORD1;
			};

			v2f vert(appdata_full af)
			{
				v2f o;

				o.wpos = UnityObjectToClipPos(af.vertex);
				o.uv = af.texcoord;

				return o;
			}

			float4 boxBlur(sampler2D tex, float2 uv, float2 size)
			{
				float4 thisPixel = 
				  tex2D(tex, uv + float2(-size.x, -size.y))
				+ tex2D(tex, uv + float2(-size.x, 0))
				+ tex2D(tex, uv + float2(-size.x, size.y))
				+ tex2D(tex, uv + float2(0, size.y))
				+ tex2D(tex, uv + float2(size.x, size.y))
				+ tex2D(tex, uv + float2(size.x, 0))
				+ tex2D(tex, uv + float2(size.x, -size.y))
				+ tex2D(tex, uv + float2(0, -size.y))
				+ tex2D(tex, uv + float2(0, 0));

				thisPixel = thisPixel / 9;

				return thisPixel;
			}

			float4 frag(v2f IN) : SV_TARGET
			{
				float4 col = boxBlur(_MainTex, IN.uv, _MainTex_TexelSize.xy);

				return col;
			}

			ENDCG
		}
	}

	FallBack "Diffuse"
}
