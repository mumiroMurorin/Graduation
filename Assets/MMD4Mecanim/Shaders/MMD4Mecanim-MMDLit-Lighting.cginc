// Not for redistribution without the author's express written permission
// UNITY_SHADER_NO_UPGRADE
#ifndef MMDLIT_LIGHTING_INCLUDED
#define MMDLIT_LIGHTING_INCLUDED

#include "MMD4Mecanim-MMDLit-Compatible.cginc"

#ifdef MMD4MECANIM_USE_HIGHPREC
#define _mmdlit_float		float
#define _mmdlit_float2		float2
#define _mmdlit_float3		float3
#define _mmdlit_float4		float4
#define _mmdlit_float3x3	float3x3
#else
#define _mmdlit_float		half
#define _mmdlit_float2		half2
#define _mmdlit_float3		half3
#define _mmdlit_float4		half4
#define _mmdlit_float3x3	half3x3
#endif

#if UNITY_VERSION >= 500
#define MMDLIT_ATTEN(ATTEN_)	(ATTEN_)
#define MMDLIT_SV_TARGET		SV_Target
#else
#define MMDLIT_ATTEN(ATTEN_)	(ATTEN_ * 2)
#define MMDLIT_SV_TARGET		COLOR
#endif

#if defined(SPHEREMAP_MUL) || defined(SPHEREMAP_ADD)
#define SPHEREMAP_ON
#endif

// UnityCG.cginc
inline _mmdlit_float3 MMDLit_DecodeLightmap(_mmdlit_float4 color)
{
#if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) && defined(SHADER_API_MOBILE)
	return (2.0 * (_mmdlit_float3)color);
#else
	// potentially faster to do the scalar multiplication
	// in parenthesis for scalar GPUs
	return (8.0 * color.a) * (_mmdlit_float3)color;
#endif
}

// Lighting.cginc
inline _mmdlit_float3 MMDLit_DirLightmapDiffuse(in _mmdlit_float3x3 dirBasis, _mmdlit_float4 color, _mmdlit_float4 scale, _mmdlit_float3 normal, bool surfFuncWritesNormal, out _mmdlit_float3 scalePerBasisVector)
{
	_mmdlit_float3 lm = MMDLit_DecodeLightmap(color);
	
	// will be compiled out (and so will the texture sample providing the value)
	// if it's not used in the lighting function, like in LightingLambert
	scalePerBasisVector = MMDLit_DecodeLightmap(scale);

	// will be compiled out when surface function does not write into o.Normal
	if (surfFuncWritesNormal)
	{
		_mmdlit_float3 normalInRnmBasis = saturate(mul(dirBasis, normal));
		lm *= dot (normalInRnmBasis, scalePerBasisVector);
	}

	return lm;
}

// UnityCG.cginc
inline _mmdlit_float MMDLit_Luminance( _mmdlit_float3 c )
{
	return dot( c, _mmdlit_float3(0.22, 0.707, 0.071) );
}

inline _mmdlit_float MMDLit_SpecularRefl( _mmdlit_float3 normal, _mmdlit_float3 lightDir, _mmdlit_float3 viewDir, _mmdlit_float s )
{
	//return saturate(pow(saturate(dot(normal, normalize(lightDir + viewDir))), s));
	return pow(saturate(dot(normal, normalize(lightDir + viewDir))), s); // Optimized.
	// (Memo: pow(x,0) as Depends on the particular graphics processor 0 or NaN)
}

// Platform limitation. (Must be unsupported without permission.)
#if defined(SHADER_API_PSSL) || defined(SHADER_API_XBOXONE) || defined(SHADER_API_XBOX360) || defined(SHADER_API_PSP2) || defined(SHADER_API_WIIU)
#define _SHADER_API_CONSOLE
#endif

#ifdef _SHADER_API_CONSOLE
#define MMDLIT_CLIP(A_) clip((A_) - (1.0 / 255.0));
#define MMDLIT_CLIP_FAST(A_)
#define MMDLIT_CLIP_CUTOFF(A_, B_) clip((A_) - (B_));
#define MMDLIT_CLIP_FAST_CUTOFF(A_, B_)
#define MMDLIT_CLIP_VAL_ALPHA(A_) (A_)
#define MMDLIT_CLIP_VAL_CUTOFF(C_) max((C_), (1.0 / 255.0))
#else
_mmdlit_float ___Eliminate; // Please observe terms of use. (Don't modify this code)
#define MMDLIT_CLIP(A_) clip((A_) * ___Eliminate - (1.0 / 255.0));
#define MMDLIT_CLIP_FAST(A_) MMDLIT_CLIP((A_))
#define MMDLIT_CLIP_CUTOFF(A_, B_) clip((A_) * ___Eliminate - (B_));
#define MMDLIT_CLIP_FAST_CUTOFF(A_, B_) MMDLIT_CLIP_CUTOFF((A_), (B_))
#define MMDLIT_CLIP_VAL_ALPHA(A_) ((A_) * ___Eliminate)
#define MMDLIT_CLIP_VAL_CUTOFF(C_) max((C_), (1.0 / 255.0))
#endif

#endif
