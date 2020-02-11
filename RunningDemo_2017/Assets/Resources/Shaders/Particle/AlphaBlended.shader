// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Simplified Alpha Blended Particle shader. Differences from regular Alpha Blended Particle one:
// - no Tint color
// - no Smooth particle support
// - no AlphaTest
// - no ColorMask

Shader "RPG/Particles/Alpha Blended" 
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (1,1,1,0.5)
		_ColorIntensity ("Color Intensity", Float) = 1
		_MainTex ("Particle Texture", 2D) = "white" {}
		_DistortionTex ("Distortion Texture", 2D) = "0.5, 0.5, 0.5, 0.5" {}
		_CutOffRange ("Cut Off Range", Range (0, 1)) = 0.5
		_DistortionRate ("Distortion Rate", Float) = 0
		_FlowSpeed ("Flow Speed(XY:Main ZW:Distortion)", Vector) = (0,0,0,0)
		[HideInInspector]_ControlAlpha ("Control Alpha", Float) = 1

		[HideInInspector]_BillboardMat0 ("BillboardMat0", Vector) = (1,0,0,0)
		[HideInInspector]_BillboardMat1 ("BillboardMat1", Vector) = (0,1,0,0)
		[HideInInspector]_BillboardMat2 ("BillboardMat2", Vector) = (0,0,1,0)
	}

	Subshader 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Pass
        {
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off 
			ZWrite Off 
			Fog {Mode Off}

			CGPROGRAM

			#pragma multi_compile NO_CUTOFF CUTOFF_WITH_ALPHA CUTOFF_ONLY
			#pragma multi_compile UVCUT_OFF UVCUT_ON
			#pragma multi_compile DISTORTION_OFF DISTORTION_ON
			#pragma multi_compile SECOND_UV_OFF SECOND_UV_ON
			#pragma multi_compile BILLBOARD_OFF BILLBOARD_ON

			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "../UtilFunctionsCG.cginc"

			struct VS_IN
			{
				float4 vertex		: POSITION;			
				float4 color		: COLOR;

				#ifdef SECOND_UV_ON
					float2 texcoord		: TEXCOORD1;
				#else
					float2 texcoord		: TEXCOORD0;
				#endif
			};

			struct v2f
			{
				float4 pos		: SV_POSITION;
				fixed4 color	: COLOR;

				float3 uv_ff	: TEXCOORD0;

				#ifdef DISTORTION_ON
					float2 distortion_uv	: TEXCOORD1;
				#endif
			};
 
			fixed4 _TintColor;
			fixed _ControlAlpha;
			sampler2D _MainTex;
			half4 _MainTex_ST;
			half4 _FlowSpeed;
			half _ColorIntensity;

			#if defined(CUTOFF_WITH_ALPHA) || defined(CUTOFF_ONLY) 
				half _CutOffRange;
			#endif

			#ifdef DISTORTION_ON
				sampler2D _DistortionTex;
				half4 _DistortionTex_ST;
				half _DistortionRate;
			#endif
			
			#ifdef BILLBOARD_ON
				uniform float4 _BillboardMat0;
				uniform float4 _BillboardMat1;
				uniform float4 _BillboardMat2;
			#endif

			
			uniform half4 unity_FogStart;
			uniform half4 unity_FogEnd;

			v2f vert(VS_IN v)
			{
				v2f o;

				#ifdef BILLBOARD_ON				
					float4 p = v.vertex;
					v.vertex.x = dot(_BillboardMat0, p);
					v.vertex.y = dot(_BillboardMat1, p);
					v.vertex.z = dot(_BillboardMat2, p);
					o.pos = mul (UNITY_MATRIX_VP, v.vertex);
				#else
					o.pos = UnityObjectToClipPos (v.vertex);
				#endif
				
				o.uv_ff.xy = TRANSFORM_TEX (v.texcoord, _MainTex) + _FlowSpeed.xy * _Time.y;  
				o.color = saturate(v.color * _TintColor * 2);
				o.color.rgb *= _ColorIntensity;
				o.color.a *= _ControlAlpha;

				#ifdef DISTORTION_ON
					o.distortion_uv = TRANSFORM_TEX (v.texcoord, _DistortionTex) + _FlowSpeed.zw * _Time.y;  
				#endif

				o.uv_ff.z = GetFogFactor(o.pos, unity_FogStart, unity_FogEnd);
				return o;
			}
 
			fixed4 frag (v2f i) : COLOR
			{
				float2 uv = i.uv_ff.xy;

				#ifdef DISTORTION_ON
					half2 distortionOffset = tex2D(_DistortionTex, i.distortion_uv).xy * 2-1;
					uv += distortionOffset*_DistortionRate;
				#endif

				#ifdef UVCUT_ON
					clip(float4(uv, 1-uv));
				#endif

				fixed4 tex = tex2D(_MainTex, uv) * i.color;

				#if defined(CUTOFF_WITH_ALPHA) || defined(CUTOFF_ONLY) 
					clip(tex.a-_CutOffRange);
				#endif

				#ifdef CUTOFF_ONLY
					tex.a=1;
				#endif

				half ff = i.uv_ff.z;
				ApplyFog(tex, ff, unity_FogColor);

				return tex;
			}
			ENDCG		
		}
	}
CustomEditor "ParticleMaterialInspector"
}