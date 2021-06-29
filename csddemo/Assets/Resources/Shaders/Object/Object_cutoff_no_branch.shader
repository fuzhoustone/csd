// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: commented out 'half4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "RPG/Object/Object cutoff no branch(Transparent)" {
	Properties 
	{
		_MainColor ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGBA)", 2D) = "white" {}
		_CutOffRange ("Cut Off Range", Range (0, 1)) = 0.5
		_LightmapIntensity ("Lightmap Intensity", Float) = 1
	}
		
	SubShader
	{	
		//Tags { "RenderType"="Opaque" }
		Tags { "Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Opaque"}
		LOD 80

		/*
		// ------------------------------------------------------------------
		// Extracts information for lightmapping, GI (emission, albedo, ...)
		// This pass it not used during regular rendering.
		Pass
		{
			Name "META" 
			Tags { "LightMode"="Meta" }

			Cull Off

			CGPROGRAM
			#include "ObjectFunctions.cginc"

			#pragma vertex vert_meta
			#pragma fragment frag_meta
			
			struct VS_IN
			{
				float4 vertex		: POSITION;
			};
		
			struct VS_OUT 
			{
				float4 pos			: SV_POSITION;
			};

			VS_OUT vert_meta (VS_IN v)
			{
				VS_OUT o;
				float4 pos = GetLocalVertex(v.vertex);
				o.pos = mul (UNITY_MATRIX_MVP, pos);
				return o;
			}

			float4 frag_meta (VS_OUT i) : SV_Target
			{
				return 0.5;
			}

			ENDCG
		}
		*/

		// Non-lightmapped	
		Pass 
		{
			Tags { "LightMode" = "Vertex" }		
			Fog {Mode Off}

			CGPROGRAM
			#include "UnityCG.cginc"
			#include "../UtilFunctionsCG.cginc"
			#include "../LightFunctions.cginc"
			#include "ObjectFunctions.cginc"

			#pragma target 3.0

			#pragma vertex vert		//vertex shader naming
			#pragma fragment frag	//fragment shader naming
			#pragma fragmentoption ARB_precision_hint_fastest

			sampler2D _MainTex;
			float4 _MainTex_ST;

			fixed4 _MainColor;
			half _CutOffRange;

			uniform half4 unity_FogStart;
			uniform half4 unity_FogEnd;

			struct VS_IN
			{
				float4 vertex		: POSITION;
				float2 texcoord		: TEXCOORD0;
				float3 normal		: NORMAL;
			};
		
			struct VS_OUT 
			{
				float4 pos			: SV_POSITION;
				half2 uv_MainTex	: TEXCOORD0;	
				fixed4 diff			: COLOR;				
				half fogFactor		: TEXCOORD1;
			};
		
			VS_OUT vert (VS_IN v)
			{
				VS_OUT o;

				float4 pos = GetLocalVertex(v.vertex);
				o.pos = UnityObjectToClipPos (pos);
				
				o.uv_MainTex = TRANSFORM_TEX (v.texcoord, _MainTex);
				o.fogFactor = GetFogFactor(o.pos, unity_FogStart, unity_FogEnd);

				float3 viewpos = ViewPos(v.vertex);
				half3 viewnormal = NormalizeViewNormal(v.normal);
				
				o.diff=1;			
				o.diff.rgb = VsDiffuseLighting(viewpos, viewnormal);
				
				o.diff.rgb += UNITY_LIGHTMODEL_AMBIENT.xyz;
				o.diff *= _MainColor;
				return o;
			}

			fixed4 frag(VS_OUT i) : COLOR 
			{
				fixed4 c;

				fixed4 mainTex = tex2D (_MainTex, i.uv_MainTex);
				clip(mainTex.a - _CutOffRange);

				c = mainTex;
				c.rgb = c.rgb * i.diff;

				//c*=_MainColor;

				ApplyFog(c, i.fogFactor, unity_FogColor);

				return c;
			}

			ENDCG
		}

		// Lightmapped, encoded as dLDR
		Pass 
		{
			Tags { "LightMode" = "VertexLM" }
			Fog {Mode Off}

			CGPROGRAM
			#include "UnityCG.cginc"
			#include "../UtilFunctionsCG.cginc"
			#include "../LightFunctions.cginc"
			#include "ObjectFunctions.cginc"

			#pragma vertex vert		//vertex shader naming
			#pragma fragment frag	//fragment shader naming
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma target 3.0

			sampler2D _MainTex;
			half4 _MainTex_ST;

			fixed4 _MainColor;

			// sampler2D unity_Lightmap;
			// half4 unity_LightmapST;
			
			half _LightmapIntensity;

			half _CutOffRange;

			uniform half4 unity_FogStart;
			uniform half4 unity_FogEnd;

			struct VS_IN
			{
				float4 vertex		: POSITION;
				float2 texcoord		: TEXCOORD0;
				float2 texcoord1	: TEXCOORD1;
				float3 normal	: NORMAL;
			};

			struct VS_OUT 
			{
				float4 pos			: SV_POSITION;
				half2 uv_MainTex	: TEXCOORD0;		
				half2 lmap			: TEXCOORD1;	
			
				fixed3 diff			: COLOR;
				half fogFactor		: TEXCOORD2;
			};

			VS_OUT vert (VS_IN v)
			{
				VS_OUT o;
				
				float4 pos = GetLocalVertex(v.vertex);
				o.pos = UnityObjectToClipPos (pos);
				
				o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				o.uv_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.fogFactor = GetFogFactor(o.pos, unity_FogStart, unity_FogEnd);
								
				half3 viewnormal = NormalizeViewNormal(v.normal);

				float3 viewpos = ViewPos(v.vertex);
				o.diff = VsDiffuseLighting(viewpos, viewnormal);

				return o;
			}

			fixed4 frag(VS_OUT i) : COLOR 
			{
				fixed4 c;
			
				fixed4 mainTex = tex2D (_MainTex, i.uv_MainTex);
				clip(mainTex.a - _CutOffRange);

				fixed4 lmtex = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap.xy) * _LightmapIntensity;

				c = fixed4(lmtex.rgb*2+ i.diff, 1);
				c *= mainTex*_MainColor;			

				ApplyFog(c, i.fogFactor, unity_FogColor);

				return c;
			}

			ENDCG 
		}

		//Lightmap pass, RGBM;
		Pass 
		{
			Tags { "LightMode" = "VertexLMRGBM" }
			Fog {Mode Off}

			CGPROGRAM
			#include "UnityCG.cginc"
			#include "../UtilFunctionsCG.cginc"
			#include "../LightFunctions.cginc"
			#include "ObjectFunctions.cginc"

			#pragma vertex vert		//vertex shader naming
			#pragma fragment frag	//fragment shader naming
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma target 3.0

			sampler2D _MainTex;
			half4 _MainTex_ST;

			fixed4 _MainColor;
			
			// sampler2D unity_Lightmap;
			// half4 unity_LightmapST;

			half _LightmapIntensity;
			
			half _CutOffRange;

			uniform half4 unity_FogStart;
			uniform half4 unity_FogEnd;
			
			struct VS_IN
			{
				float4 vertex		: POSITION;
				float2 texcoord		: TEXCOORD0;
				float2 texcoord1	: TEXCOORD1;

				float3 normal	: NORMAL;
			};

			struct VS_OUT 
			{
				float4 pos			: SV_POSITION;
				half2 uv_MainTex	: TEXCOORD0;		
				half2 lmap			: TEXCOORD1;	
				fixed3 diff			: COLOR;

				half fogFactor		: TEXCOORD2;
			};

			VS_OUT vert (VS_IN v)
			{
				VS_OUT o;
				
				float4 pos = GetLocalVertex(v.vertex);
				o.pos = UnityObjectToClipPos (pos);

				o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				o.uv_MainTex = TRANSFORM_TEX (v.texcoord, _MainTex);
				o.fogFactor = GetFogFactor(o.pos, unity_FogStart, unity_FogEnd);
	
				half3 viewnormal = NormalizeViewNormal(v.normal);

				float3 viewpos = ViewPos(v.vertex);
				o.diff = VsDiffuseLighting(viewpos, viewnormal);
				
				return o;
			}

			fixed4 frag(VS_OUT i) : COLOR 
			{
				fixed4 c;
			
				fixed4 mainTex = tex2D (_MainTex, i.uv_MainTex);
				clip(mainTex.a - _CutOffRange);

				fixed4 lmtex = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap.xy);
				lmtex.rgb *= _LightmapIntensity;

				c = fixed4(lmtex.rgb * lmtex.a * 8 + i.diff, 1);
				c *= mainTex*_MainColor;		

				ApplyFog(c, i.fogFactor, unity_FogColor);
				return c;
			}

			ENDCG
		}
	}
	
//Fallback "Transparent/VertexLit"
//CustomEditor "ObjectMaterialInspector"
}
