Shader "My/Distortion" 
{
	Properties 
	{
		_MainTex("Main Texture", 2D) = "white"{}
		_NoiseTex("Noise Texture", 2D) = "white"{}
		_NoiseMagnitude("Noise Magnitude", Range(0, 1)) = 0
		_Color("Color", Color) = (1, 1, 1, 1)
	}

	SubShader 
	{
		Tags{ "Queue" = "Transparent" }
		
		pass
		{
		blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _NoiseTex;
			float _NoiseMagnitude;
			float4 _Color;

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

			float4 frag(v2f IN) : SV_TARGET
			{
				float2 offset = tex2D(_NoiseTex, IN.uv).xy;	//TODO why use xy from the sampled color?
				offset = offset * 2 - 1;
				float mag = _NoiseMagnitude * sin(_Time.x * 20);
				offset = offset * mag;

				float4 col = tex2D(_MainTex, IN.uv + float2(offset.x, offset.y));
				return col;
			}

			ENDCG
		}
	}

	FallBack "Diffuse"
}
