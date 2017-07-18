Shader "My/RoundCubeSplitMeshShader" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		[KeywordEnum(X, Y, Z)]_Faces("Faces", float) = 0
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM

		#pragma multi_compile _FACES_X _FACES_Y _FACES_Z

		#pragma surface surf Standard fullforwardshadows vertex:vert
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input 
		{
			float2 roundCubeUV;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void vert(inout appdata_full IN, out Input o)	//it seems 'inout' of appdata is needed for surface shader
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);

			#if defined(_FACES_X)
			//o.roundCubeUV = IN.vertex.zy;
			o.roundCubeUV = IN.color.zy * 255;
			#elif defined(_FACES_Y)
			//o.roundCubeUV = IN.vertex.xz;
			o.roundCubeUV = IN.color.xz * 255;
			#elif defined(_FACES_Z)
			//o.roundCubeUV = IN.vertex.xy;
			o.roundCubeUV = IN.color.xy * 255;
			#endif
		}

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.roundCubeUV) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	} 

	FallBack "Diffuse"
}