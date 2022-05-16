Shader "Unlit/ModelLineNew"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Diffuse("Diffuse", COLOR) = (1,1,1,1)
		_OutlineColor("Outline Color", COLOR) = (0,0,0,1)
		_OutlineScale("Outline Scale", Range(0,10)) = 0.001
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Opaque" }
		LOD 100

		Pass{
			Name "Outline"
			ZWrite Off
			Cull Front
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 _OutlineColor;
			float _OutlineScale;

			struct v2f {
				float4 vertex : SV_POSITION;
			};

			float random(float3 st, float n) {
				st = floor(st * n);
				float aaa = frac(sin(dot(st.xyz, float2(12.9898,78.233))) * 100.5453123);
				return float3(aaa,aaa,aaa);
			}

			v2f vert(appdata_base v) {
				v2f o;
				v.vertex.xyz += v.normal + random(v.normal,0.001) * _OutlineScale;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) :SV_Target{

				return _OutlineColor;
			}

			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag


			#include "UnityCG.cginc"
			#include "Lighting.cginc"



			struct v2f
			{

				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNormal:TEXCOORD1;
				float3 worldPos:TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Diffuse;

			v2f vert(appdata_base v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld,v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float3 ambient = UNITY_LIGHTMODEL_AMBIENT;
				fixed3 albedo = tex2D(_MainTex, i.uv).rgb;
				fixed3 worldLightDir = UnityWorldSpaceLightDir(i.worldPos);
				float halfLambert = dot(worldLightDir,i.worldNormal) * 0.5 + 0.5;
				fixed3 diffuse = albedo * _Diffuse.rgb * halfLambert;
				return fixed4(diffuse,1);
			}
			ENDCG
		}
	}

	FallBack "DIFFUSE"
}
