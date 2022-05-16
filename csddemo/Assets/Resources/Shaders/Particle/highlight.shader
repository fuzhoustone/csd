Shader "Custom2/highlight"
{
	Properties{
		_Diffuse("Diffuse", Color) = (1,1,1,1)
		_OutlineCol("OutlineCol", Color) = (1,0,0,1)
		_OutlineFactor("OutlineFactor", Range(0,1)) = 0.1
		_MainTex("Base 2D", 2D) = "white"{}
	}

		//子着色器    
		SubShader
	{

		//描边使用两个Pass，第一个pass沿法线挤出一点，只输出描边的颜色  
		Pass
		{
		//剔除正面，只渲染背面，对于大多数模型适用，不过如果需要背面的，就有问题了  
		Cull Front

		CGPROGRAM
		#include "UnityCG.cginc"  
		fixed4 _OutlineCol;
		float _OutlineFactor;

		struct v2f
		{
			float4 pos : SV_POSITION;
		};

		v2f vert(appdata_full v)
		{
			v2f o;
			//在vertex阶段，每个顶点按照法线的方向偏移一部分，不过这种会造成近大远小的透视问题  
			//v.vertex.xyz += v.normal * _OutlineFactor;  
			o.pos = UnityObjectToClipPos(v.vertex);
			//将法线方向转换到视空间  
			float3 vnormal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
			//将视空间法线xy坐标转化到投影空间，只有xy需要，z深度不需要了  
			float2 offset = TransformViewToProjection(vnormal.xy);
			//在最终投影阶段输出进行偏移操作  
			o.pos.xy += offset * _OutlineFactor;
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			//这个Pass直接输出描边颜色  
			return _OutlineCol;
		}

			//使用vert函数和frag函数  
			#pragma vertex vert  
			#pragma fragment frag  
			ENDCG
		}
	}
		//前面的Shader失效的话，使用默认的Diffuse  
	FallBack "Diffuse"
}
