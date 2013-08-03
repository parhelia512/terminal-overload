//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

// These are some simple wrappers for simple 
// HLSL compatibility.

#define float4 vec4
#define float3 vec3
#define float2 vec2

#define half float
#define half2 vec2
#define half3 vec3
#define half4 vec4

#define float4x4 mat4
#define float3x3 mat3
#define float2x2 mat2

#define texCUBE textureCube
#define tex2D texture2D

#define frac fract

vec4 tex2Dlod(sampler2D sampler, vec4 texCoord) { return texture2DLod(sampler, texCoord.xy, texCoord.w); }

#define lerp mix

float saturate( float val ) { return clamp( val, 0.0, 1.0 ); }
vec2 saturate( vec2 val ) { return clamp( val, 0.0, 1.0 ); }
vec3 saturate( vec3 val ) { return clamp( val, 0.0, 1.0 ); }
vec4 saturate( vec4 val ) { return clamp( val, 0.0, 1.0 ); }

float round( float n ) { return sign( n ) * floor( abs( n ) + 0.5 ); }
vec2 round( vec2 n ) { return sign( n ) * floor( abs( n ) + 0.5 ); }
vec3 round( vec3 n ) { return sign( n ) * floor( abs( n ) + 0.5 ); }
vec4 round( vec4 n ) { return sign( n ) * floor( abs( n ) + 0.5 ); }

vec4 mul( mat4 m, vec4 v) { return m*v; }
vec4 mul( vec4 v, mat4 m) { return v*m; }
vec3 mul( vec3 v, mat3 m) { return v*m; }

vec4 rsqrt( vec4 n ){ return inversesqrt( n ); }

#define invertY vec2

void correctSSP(inout vec4 vec) 
{ 
	vec.y *= -1;
}

#ifdef TORQUE_PIXEL_SHADER
	void clip(float a) { if(a < 0) discard;}
#endif
