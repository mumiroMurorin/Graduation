// Not for redistribution without the author's express written permission
#ifndef MMDLIT_SURFACE_LIGHTING_INCLUDED
#define MMDLIT_SURFACE_LIGHTING_INCLUDED
//#include "MMD4Mecanim-MMDLit-Lighting.cginc"

#include "MMD4Mecanim-MMDLit-Surface-Tessellation.cginc"

#ifndef TEXTURE2D
#define TEXTURE2D(textureName) sampler2D textureName
#endif
#ifndef TEXTURECUBE
#define TEXTURECUBE(textureName) samplerCUBE textureName
#endif
#ifndef SAMPLER
#define SAMPLER(samplerName)
#endif
#ifndef SAMPLE_TEXTURE2D
#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2) tex2D(textureName, coord2)
#endif
#ifndef SAMPLE_TEXTURECUBE
#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3) texCUBE(textureName, coord3)
#endif

#ifdef MMD4MECANIM_USE_BASEMAP
#define _MMDLit_Albedo _BaseColorMap
#define _MMDLit_Sampler_Albedo sampler_BaseColorMap
#else
#define _MMDLit_Albedo _MainTex
#define _MMDLit_Sampler_Albedo sampler_MainTex
#endif

#ifdef MMD4MECANIM_USE_CUBUFFER
CBUFFER_START(MMD4Mecanim)
#endif

#ifdef MMD4MECANIM_USE_DIFFUSE
_mmdlit_float4 _Diffuse;
#define _MMDLit_Diffuse _Diffuse
#elif defined(MMD4MECANIM_USE_BASECOLOR) // Deprecated
#define _MMDLit_Diffuse _BaseColor
#else
#define _MMDLit_Diffuse _Color
#endif

#ifndef MMD4MECANIM_SKIPDEFINE_UNITYVARS
_mmdlit_float4 _Color;
#endif
_mmdlit_float4 _Specular;
_mmdlit_float4 _Ambient;
_mmdlit_float _Shininess;
_mmdlit_float _ShadowLum;
_mmdlit_float _AmbientToDiffuse;

#ifndef MMD4MECANIM_SKIPDEFINE_UNITYVARS
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
#endif
TEXTURE2D(_ToonTex);
SAMPLER(sampler_ToonTex);

#ifdef UNITY_PASS_FORWARDADD
_mmdlit_float _AddLightToonCen;
_mmdlit_float _AddLightToonMin;
#endif

_mmdlit_float4 _ToonTone;

#ifdef EMISSIVE_ON
_mmdlit_float4 _Emissive;
#endif

#if defined(SPHEREMAP_MUL) || defined(SPHEREMAP_ADD)
TEXTURECUBE(_SphereCube);
SAMPLER(sampler_SphereCube);
#endif

#ifdef MMD4MECANIM_USE_CUBUFFER
CBUFFER_END
#endif

#define MMDLIT_GLOBALLIGHTING		_mmdlit_float3(0.6, 0.6, 0.6)
#define MMDLIT_CENTERAMBIENT		_mmdlit_float3(0.5, 0.5, 0.5)
#define MMDLIT_CENTERAMBIENT_INV	_mmdlit_float3(1.0 / 0.5, 1.0 / 0.5, 1.0 / 0.5)
#define MMDLIT_DIFFUSECLIPPING		_mmdlit_float3(0.5, 0.5, 0.5)

// Ambient Feedback Rate from Unity.
inline _mmdlit_float3 MMDLit_GetTempAmbientL()
{
	return max(MMDLIT_CENTERAMBIENT - (_mmdlit_float3)_Ambient, _mmdlit_float3(0,0,0)) * MMDLIT_CENTERAMBIENT_INV;
}

inline _mmdlit_float3 MMDLit_GetAmbientRate()
{
	return _mmdlit_float3(1.0, 1.0, 1.0) - MMDLit_GetTempAmbientL();
}

inline _mmdlit_float3 MMDLit_GetTempAmbient( _mmdlit_float3 globalAmbient )
{
	return globalAmbient * MMDLit_GetAmbientRate();
}

inline _mmdlit_float3 MMDLit_GetTempDiffuse( _mmdlit_float3 globalAmbient )
{
	_mmdlit_float3 tempColor = min((_mmdlit_float3)_Ambient + (_mmdlit_float3)_MMDLit_Diffuse * MMDLIT_GLOBALLIGHTING, _mmdlit_float3(1,1,1));
	tempColor = max(tempColor - MMDLit_GetTempAmbient(globalAmbient), _mmdlit_float3(0,0,0));
	#ifdef AMB2DIFF_ON // Passed in Forward Add
	tempColor *= min(globalAmbient * _AmbientToDiffuse, _mmdlit_float3(1,1,1)); // Feedback ambient for Unity5.
	#endif
	return tempColor;
}

inline _mmdlit_float3 MMDLit_GetTempDiffuse_NoAmbient()
{
	_mmdlit_float3 tempColor = saturate((_mmdlit_float3)_Ambient + (_mmdlit_float3)_MMDLit_Diffuse * MMDLIT_GLOBALLIGHTING - MMDLIT_DIFFUSECLIPPING);
	return tempColor;
}

inline void MMDLit_GetBaseColor(
	_mmdlit_float3 albedo,
	_mmdlit_float3 tempDiffuse,
	_mmdlit_float3 uvw_Sphere,
	out _mmdlit_float3 baseC,
	out _mmdlit_float3 baseD)
{
	#ifdef SPHEREMAP_MUL
	_mmdlit_float3 sph = (_mmdlit_float3)SAMPLE_TEXTURECUBE(_SphereCube, sampler_SphereCube, uvw_Sphere);
	baseC = albedo * sph;
	baseD = baseC * tempDiffuse; // for Diffuse only.
	#elif SPHEREMAP_ADD
	_mmdlit_float3 sph = (_mmdlit_float3)SAMPLE_TEXTURECUBE(_SphereCube, sampler_SphereCube, uvw_Sphere);
	baseC = albedo + sph;
	baseD = albedo * tempDiffuse + sph; // for Diffuse only.
	#else
	baseC = albedo;
	baseD = albedo * tempDiffuse;
	#endif
}

inline _mmdlit_float MMDLit_GetToolRefl(_mmdlit_float NdotL)
{
	return NdotL * _ToonTone.y + _ToonTone.z; // Necesally saturate.
}

inline _mmdlit_float MMDLit_GetShadowAttenToToon(_mmdlit_float shadowAtten)
{
	return ((shadowAtten - 0.5) * _ToonTone.x) + _ToonTone.z; // Necesally saturate.
}

inline _mmdlit_float MMDLit_GetToonShadow(_mmdlit_float toonRefl)
{
	_mmdlit_float toonShadow = toonRefl * 2.0;
	return (_mmdlit_float)saturate(toonShadow * toonShadow - 1.0);
}

#ifdef UNITY_PASS_FORWARDADD
inline _mmdlit_float MMDLit_GetForwardAddStr(_mmdlit_float toonRefl)
{
	_mmdlit_float toonShadow = (toonRefl - _AddLightToonCen) * 2.0;
	return (_mmdlit_float)clamp(toonShadow * toonShadow - 1.0, _AddLightToonMin, 1.0);
}
#endif

// for HDRP
inline _mmdlit_float3 MMDLit_GetRamp_HighPrec(_mmdlit_float luminance)
{
	_mmdlit_float refl = saturate(MMDLit_GetShadowAttenToToon(luminance));

	_mmdlit_float toonRefl = refl;

#ifdef SELFSHADOW_ON
	refl = 0;
#endif

	_mmdlit_float3 ramp = (_mmdlit_float3)SAMPLE_TEXTURE2D(_ToonTex, sampler_ToonTex, _mmdlit_float2(refl, refl));

#ifdef SELFSHADOW_ON
	_mmdlit_float toonShadow = MMDLit_GetToonShadow(toonRefl);
	ramp = lerp(ramp, _mmdlit_float3(1.0, 1.0, 1.0), toonShadow);
#endif

	float lumHigh = step(1.0, luminance);

	ramp = ramp * luminance * lumHigh + saturate(1.0 - (1.0 - ramp) * _ShadowLum) * (1.0 - lumHigh);
	return ramp;
}

// for ForwardBase
inline _mmdlit_float3 MMDLit_GetRamp(_mmdlit_float NdotL, _mmdlit_float shadowAtten)
{
	_mmdlit_float refl = saturate(min(MMDLit_GetToolRefl(NdotL), MMDLit_GetShadowAttenToToon(shadowAtten)));

	_mmdlit_float toonRefl = refl;

	#ifdef SELFSHADOW_ON
	refl = 0;
	#endif
	
	_mmdlit_float3 ramp = (_mmdlit_float3)SAMPLE_TEXTURE2D(_ToonTex, sampler_ToonTex, _mmdlit_float2(refl, refl));

	#ifdef SELFSHADOW_ON
	_mmdlit_float toonShadow = MMDLit_GetToonShadow(toonRefl);
	ramp = lerp(ramp, _mmdlit_float3(1.0, 1.0, 1.0), toonShadow);
	#endif

	ramp = saturate(1.0 - (1.0 - ramp) * _ShadowLum);
	return ramp;
}

#ifdef UNITY_PASS_FORWARDADD
// for ForwardAdd
inline _mmdlit_float3 MMDLit_GetRamp_Add(_mmdlit_float toonRefl, _mmdlit_float toonShadow)
{
	_mmdlit_float refl = saturate(toonRefl);
	
	#ifdef SELFSHADOW_ON
	refl = 0;
	#endif
	
	_mmdlit_float3 ramp = (_mmdlit_float3)SAMPLE_TEXTURE2D(_ToonTex, sampler_ToonTex, _mmdlit_float2(refl, refl));

	#ifdef SELFSHADOW_ON
	_mmdlit_float3 rampSS = (1.0 - toonShadow) * ramp + toonShadow;
	ramp = rampSS;
	#endif
	
	ramp = saturate(1.0 - (1.0 - ramp) * _ShadowLum);
	return ramp;
}
#endif

inline _mmdlit_float MMDLit_MulAtten(_mmdlit_float atten, _mmdlit_float shadowAtten)
{
	return atten * shadowAtten;
}

#ifndef MMD4MECANIM_SKIPDEFINE_LIGHTINGPASS
inline _mmdlit_float4 MMDLit_GetAlbedo(float2 uv_MainTex)
{
	return (_mmdlit_float4)SAMPLE_TEXTURE2D(_MMDLit_Albedo, _MMDLit_Sampler_Albedo, uv_MainTex);
}

// for FORWARD_BASE
inline _mmdlit_float3 MMDLit_Lighting(
	_mmdlit_float3 albedo,
	_mmdlit_float3 uvw_Sphere,
	_mmdlit_float NdotL,
	_mmdlit_float3 normal,
	_mmdlit_float3 lightDir,
	_mmdlit_float3 viewDir,
	_mmdlit_float atten,
	_mmdlit_float shadowAtten,
	out _mmdlit_float3 baseC,
	_mmdlit_float3 globalAmbient)
{
	_mmdlit_float3 ramp = MMDLit_GetRamp(NdotL, shadowAtten);
	_mmdlit_float3 lightColor = (_mmdlit_float3)_LightColor0 * MMDLIT_ATTEN(atten);

	_mmdlit_float3 baseD;
	MMDLit_GetBaseColor(albedo, MMDLit_GetTempDiffuse(globalAmbient), uvw_Sphere, baseC, baseD);
	
	_mmdlit_float3 c = baseD * lightColor * ramp;
	
	#ifdef SPECULAR_ON
	_mmdlit_float refl = MMDLit_SpecularRefl(normal, lightDir, viewDir, _Shininess);
	c += (_mmdlit_float3)_Specular * lightColor * refl;
	#endif

	#ifdef EMISSIVE_ON
	// AutoLuminous
	c += baseC * (_mmdlit_float3)_Emissive;
	#endif
	return c;
}

#ifdef UNITY_PASS_FORWARDADD
// for FORWARD_ADD
inline _mmdlit_float3 MMDLit_Lighting_Add(
	_mmdlit_float3 albedo,
	_mmdlit_float NdotL,
	_mmdlit_float toonRefl,
	_mmdlit_float toonShadow,
	_mmdlit_float3 normal,
	_mmdlit_float3 lightDir,
	_mmdlit_float3 viewDir,
	_mmdlit_float atten)
{
	_mmdlit_float3 ramp = MMDLit_GetRamp_Add(toonRefl, toonShadow);
	_mmdlit_float3 lightColor = (_mmdlit_float3)_LightColor0 * MMDLIT_ATTEN(atten);

	_mmdlit_float3 baseC;
	_mmdlit_float3 baseD;
	MMDLit_GetBaseColor(albedo, MMDLit_GetTempDiffuse_NoAmbient(), _mmdlit_float3(0.0, 0.0, 0.0), baseC, baseD);

	_mmdlit_float3 c = baseD * lightColor * ramp;

	#ifdef SPECULAR_ON
	_mmdlit_float refl = MMDLit_SpecularRefl(normal, lightDir, viewDir, _Shininess);
	c += (_mmdlit_float3)_Specular * lightColor * refl;
	#endif
	
	return c;
}
#endif
#endif // MMD4MECANIM_SKIPDEFINE_LIGHTINGPASS

#endif // MMDLIT_SURFACE_LIGHTING_INCLUDED
