Shader "Custom/FadingOut"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_MainColor("_MainColor", Color) = (1,1,1,1)
		_Dissolution("TextureDissolution", 2D) = "white" {}
		_BurnColor("Burn Color", Color) = (1,0,0,1)
		_BurnLength("Burn Length", Range(0, 1)) = 0.1
		_Didi("Dissolution", Range(0, 1)) = 0.5
		_Scale("Scale", vector) = (1, 1, 1, 1)
		_EtirementScale("EtirementScale",float) = 2
		_IlluminationTexture("IllumTex",float) = 0.7
	}
		SubShader
	{
		//		Tags { "RenderType"="Opaque" }
		Tags{					//Importe la possibilité de jouer avec l'alpha
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
	}
		LOD 100

		Pass
	{
		Blend SrcAlpha OneMinusSrcAlpha	
		cull off	//afficher les faces intérieures et exterieures ("cull back" pour cacher les faces intérieures, "cull front" pour n'afficher que les faces intérieures)

		CGPROGRAM
#pragma vertex vert			
#pragma fragment frag		


#include "UnityCG.cginc"


		struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
		float3 normal : NORMAL;
		float3 viewDir : TEXCOORD1;
		float3 vertexWorld : TEXCOORD2;
		float4 screenUV : TEXCOORD3;
	};

	sampler2D _MainTex;
	sampler2D _Dissolution;
	float4 _MainTex_ST;
	float _Didi;
	float3 _BurnColor;
	float _BurnLength;
	float4 _MainColor;
	float4 _Scale;
	float _EtirementScale;
	float _IlluminationTexture;


	v2f vert(appdata_full v)
	{
		v2f o;

		o.vertexWorld = mul(unity_ObjectToWorld, v.vertex);	
		o.vertex = UnityObjectToClipPos(v.vertex);	
		o.screenUV = ComputeScreenPos(o.vertex);	
		o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);		
		o.viewDir = WorldSpaceViewDir(v.vertex);	
		o.normal = v.normal;
		return o;
	}

	
	fixed4 frag(v2f i) : SV_Target
	{
		//fixed4 col = tex2D(_MainTex, i.uv);		
		fixed4 col = _MainColor;

		col.rgb *= dot(normalize(i.normal), normalize(i.viewDir)) * _IlluminationTexture + 0.5;	

		float2 screenUV = i.screenUV.xy / i.screenUV.w ;	
		float lum = Luminance(tex2D(_Dissolution, i.uv*	(_Scale.x + _Scale.y + _Scale.z)/_EtirementScale));

		col.a *= lerp(1, 0, step(_Didi, lum));	
	
		fixed3 col3 = _BurnColor;	

		col.rgb = lerp(col.rgb, col3, step(_Didi - _BurnLength, lum));		
		return col;
	}
		ENDCG
	}
	}
}
