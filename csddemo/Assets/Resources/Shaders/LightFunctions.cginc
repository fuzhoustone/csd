#ifndef LIGHT_FUNCTION_INCLUDED
#pragma exclude_renderers d3d11 xbox360 gles
#define LIGHT_FUNCTION_INCLUDED

#include "UnityShaderVariables.cginc"

#define MANY_PIXEL_LIGHT_COUNT 3

inline void VsGetAttenAndToLight(in float3 viewpos, out float4 atten4, out float3 toLight[4])
{
	//int i;
	toLight[0] = unity_LightPosition[0].xyz - viewpos * unity_LightPosition[0].w;
	toLight[1] = unity_LightPosition[1].xyz - viewpos * unity_LightPosition[1].w;
	toLight[2] = unity_LightPosition[2].xyz - viewpos * unity_LightPosition[2].w;
	toLight[3] = unity_LightPosition[3].xyz - viewpos * unity_LightPosition[3].w;

	half4 lengthSq4 = LengthSq4(toLight[0], toLight[1], toLight[2], toLight[3]);
	Normalize4(toLight[0], toLight[1], toLight[2], toLight[3], lengthSq4);
	
	//customzied attenuation tern
	
	float4 rangeSq4 = float4(unity_LightAtten[0].w, unity_LightAtten[1].w, unity_LightAtten[2].w, unity_LightAtten[3].w);
	float4 invRangeSq = 1.0/rangeSq4;
	atten4 = saturate(1 - lengthSq4 * invRangeSq);
	atten4 *= atten4;
	
	/*
	float4 attenSq4 = float4(unity_LightAtten[0].z, unity_LightAtten[1].z, unity_LightAtten[2].z, unity_LightAtten[3].z);
	atten4 = 1.0 / (1.0 + lengthSq4 * attenSq4);
	*/
}

inline fixed3 VsGetDiffuseLighting(	in float3 viewpos, 
									in half3 viewnormal, 
									in float4 atten4,
									in float3 toLight[4],
									uniform int pixelLightCount)
{		
	fixed3 diffuse=0;
	half4 ndotL4=0;

	if(pixelLightCount<1) ndotL4[0] = dot(toLight[0], viewnormal);
	if(pixelLightCount<2) ndotL4[1] = dot(toLight[1], viewnormal);
	if(pixelLightCount<3) ndotL4[2] = dot(toLight[2], viewnormal);
	if(pixelLightCount<4) ndotL4[3] = dot(toLight[3], viewnormal);

	ndotL4 = saturate(ndotL4) * atten4;
	
	if(pixelLightCount<1) diffuse += ndotL4[0] * unity_LightColor[0].rgb;
	if(pixelLightCount<2) diffuse += ndotL4[1] * unity_LightColor[1].rgb;
	if(pixelLightCount<3) diffuse += ndotL4[2] * unity_LightColor[2].rgb;
	if(pixelLightCount<4) diffuse += ndotL4[3] * unity_LightColor[3].rgb;
		
	return diffuse;	
}

inline half SpecularFactor(in half3 viewnormal, in half3 sight, in float3 toLight)
{
	half3 lightReflect = (toLight + sight) * 0.5f; 
	lightReflect = normalize(lightReflect);
	return dot(lightReflect, viewnormal);
}

inline fixed3 VsGetSpecularLighting(in float3 viewpos,
									in half3 viewnormal, 
									in half3 sight,
									in half shininess, 
									in half specIntensity,
									in float4 atten4,
									in float3 toLight[4],
									uniform int pixelLightCount)
{
	fixed3 specular=0;		
	half4 ndotH4=0;
	
	if(pixelLightCount<1) ndotH4[0] = SpecularFactor(viewnormal, sight, toLight[0]);
	if(pixelLightCount<2) ndotH4[1] = SpecularFactor(viewnormal, sight, toLight[1]);
	if(pixelLightCount<3) ndotH4[2] = SpecularFactor(viewnormal, sight, toLight[2]);
	if(pixelLightCount<4) ndotH4[3] = SpecularFactor(viewnormal, sight, toLight[3]);
	
	ndotH4 = pow(saturate(ndotH4), shininess.xxxx) * atten4;

	if(pixelLightCount<1) specular += ndotH4[0] * unity_LightColor[0].rgb;
	if(pixelLightCount<2) specular += ndotH4[1] * unity_LightColor[1].rgb;
	if(pixelLightCount<3) specular += ndotH4[2] * unity_LightColor[2].rgb;
	if(pixelLightCount<4) specular += ndotH4[3] * unity_LightColor[3].rgb;
	
	specular *= specIntensity;

	return specular;
}


inline fixed3 VsDiffuseLighting(in float3 viewpos, 
								in half3 viewnormal, 
								out float4 atten4,
								out float3 toLight[4],
								uniform int pixelLightCount)
{
	VsGetAttenAndToLight(viewpos, atten4, toLight);
	return VsGetDiffuseLighting(viewpos, viewnormal, atten4, toLight, pixelLightCount);
}

inline fixed3 VsDiffuseLighting(in float3 viewpos, in half3 viewnormal)
{
	float4 atten4;
	float3 toLight[4];

	return VsDiffuseLighting(viewpos, viewnormal, atten4, toLight, 0);
}

inline void VsDiffuseSpecularLighting(	in float3 viewpos,
										in half3 viewnormal, 
										in half3 sight,
										in half shininess, 
										in half specIntensity,
										out fixed3 diffuse, 
										out fixed3 specular,
										out float4 atten4,
										out float3 toLight[4],
										uniform int pixelLightCount)
{
	VsGetAttenAndToLight(viewpos, atten4, toLight);

	diffuse = VsGetDiffuseLighting(	viewpos, 
									viewnormal, 
									atten4, 
									toLight, 
									pixelLightCount);

	specular = VsGetSpecularLighting(	viewpos,
										viewnormal,
										sight,
										shininess,
										specIntensity,
										atten4, 
										toLight, 
										pixelLightCount);
}

inline void VsDiffuseSpecularLighting(	in float3 viewpos,
										in half3 viewnormal, 
										in half3 toVert,
										in half shininess, 
										in half specIntensity,
										out fixed3 diffuse, 
										out fixed3 specular)
{
	float4 atten4;
	float3 toLight[4];

	VsDiffuseSpecularLighting(	viewpos, 
								viewnormal, 
								toVert,
								shininess, 
								specIntensity, 
								diffuse, 
								specular, 
								atten4,
								toLight,
								0);
}

inline fixed3 VsSpecularLighting(	in float3 viewpos,
									in half3 viewnormal, 
									in half3 sight,
									in half shininess, 
									in half specIntensity,
									out float4 atten4,
									out float3 toLight[4],
									uniform int pixelLightCount)
{
	VsGetAttenAndToLight(viewpos, atten4, toLight);

	return VsGetSpecularLighting(	viewpos,
									viewnormal,
									sight,
									shininess,
									specIntensity,
									atten4, 
									toLight, 
									pixelLightCount);
}

inline fixed3 VsSpecularLighting(	in float3 viewpos,
									in half3 viewnormal, 
									in half3 sight,
									in half shininess, 
									in half specIntensity)
{
	float4 atten4;
	float3 toLight[4];

	return VsSpecularLighting(viewpos, viewnormal, sight, shininess, specIntensity, atten4, toLight, 0);
}

inline fixed3 PsDiffuseLighting(in half3 lightDir, in half atten, in half3 normal)
{
	half ndotL = dot(lightDir, normal);
	ndotL = saturate(ndotL) * atten;

	return ndotL * unity_LightColor[0].rgb;
}

inline fixed3 PsDiffuseManyLighting(in half3 lightDir[MANY_PIXEL_LIGHT_COUNT], in half4 atten4, in half3 normal)
{
	fixed3 diffuse=0;
	half4 ndotL4;
	int i;
	for(i=0; i<MANY_PIXEL_LIGHT_COUNT; ++i)
	{
		ndotL4[i] = dot(lightDir[i], normal);
	}
	
	ndotL4 = saturate(ndotL4) * atten4;
	
	for(i=0; i<MANY_PIXEL_LIGHT_COUNT; ++i)
	{
		diffuse += ndotL4[i] * unity_LightColor[i].rgb;
	}
	
	return diffuse;
}

inline fixed3 PsDiffuseLightingInView(in float3 viewpos, in half atten, in half3 normal)
{
	float3 lightDir = normalize(unity_LightPosition[0].xyz - viewpos * unity_LightPosition[0].w);

	half ndotL = dot(lightDir, normal);
	ndotL = saturate(ndotL) * atten;

	return ndotL * unity_LightColor[0].rgb;
}

inline fixed3 PsDiffuseManyLightingInView(in float3 viewpos, in half4 atten4, in half3 normal)
{
	fixed3 diffuse=0;
	half4 ndotL4;
	int i;

	float3 lightDir[MANY_PIXEL_LIGHT_COUNT];
	for(i=0; i<MANY_PIXEL_LIGHT_COUNT; ++i)
	{
		lightDir[i] = unity_LightPosition[i].xyz - viewpos * unity_LightPosition[i].w;
	}

	Normalize3(lightDir[0], lightDir[1], lightDir[2]);

	for(i=0; i<MANY_PIXEL_LIGHT_COUNT; ++i)
	{
		ndotL4[i] = dot(lightDir[i], normal);
	}
	
	ndotL4 = saturate(ndotL4) * atten4;
	
	for(i=0; i<MANY_PIXEL_LIGHT_COUNT; ++i)
	{
		diffuse += ndotL4[i] * unity_LightColor[i].rgb;
	}
	
	return diffuse;
}

inline fixed3 PsSpecularLighting(	in half3 lightDir, 
									in half3 sight,
									in half atten,
									in half3 normal, 
									in float shininess, 
									in float specIntensity)
{
	half3 lightReflect = (lightDir + sight) * 0.5f;
	half ndotH = dot(lightReflect, normal);
	ndotH = pow(saturate(ndotH), shininess.xxxx) * specIntensity * atten;
	
	return ndotH * unity_LightColor[0].rgb;
}

inline fixed3 PsSpecularManyLighting(	in half3 ligtDir[MANY_PIXEL_LIGHT_COUNT],
										in half3 sight,
										in half4 atten4,
										in half3 normal,
										in float shininess, 
										in float specIntensity)
{
	fixed3 spec=0;
	half4 ndotH4; 
	int i;
	for(i=0; i<MANY_PIXEL_LIGHT_COUNT; ++i)
	{
		half3 lightReflect = (ligtDir[i] + sight) * 0.5f;
		//lightReflect = normalize(lightReflect);

		ndotH4[i] = dot(lightReflect, normal);
	}
	
	ndotH4 = pow(saturate(ndotH4), shininess.xxxx) * atten4;
	
	for(i=0; i<MANY_PIXEL_LIGHT_COUNT; ++i)
	{
		spec += ndotH4[i] * unity_LightColor[i].rgb;
	}
	
	return spec * specIntensity;
}

#endif