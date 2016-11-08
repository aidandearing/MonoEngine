sampler TextureSampler : register(s0);

float N; // Noisiness
float S; // External Noise seed

float PI2 = 3.14159 * 2;
float2 hash = float2(12.9898, 78.233)*2.0;

// Random function using irrational numbers to generate a 0-0.9999 'random' number based on input float2, say coordinates
float rand(in float2 uv)
{
	float2 noise = frac(sin(dot(uv, hash) * 43758.5453));
	return abs(noise.x + noise.y) * 0.5;
}

// Combines 2 random numbers, used for combined seeds
float RWS(in float2 uv, in float2 xy)
{
	return rand(uv) * rand(xy);
}

// Radial Coordinate Displacing Seed Sample Base
float4 NoiseRCDSSBFunction(float4 texPosition : SV_position, float4 texColor : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
	float random = rand(float2(S, S));

	float retx = cos(random * PI2) * (random * N);
	float rety = sin(random * PI2) * (random * N);

	float2 offset = float2(retx, rety);

	float4 ret = tex2D(TextureSampler, texCoord + offset);

	return ret;
}

// Radial Coordinate Displacing Coord Seed Sample Base
float4 NoiseRCDCSSBFunction(float4 texPosition : SV_position, float4 texColor : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
	float random = RWS(texCoord,float2(S,S));

	float retx = cos(random * PI2) * (random * N);
	float rety = sin(random * PI2) * (random * N);

	float2 offset = float2(retx, rety);

	float4 ret = tex2D(TextureSampler, texCoord + offset);

	return ret;
}

// Radial Coordinate Displacing Coord Sample Base
float4 NoiseRCDCSBFunction(float4 texPosition : SV_position, float4 texColor : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
	float random = rand(texCoord);
	float retx = cos(random * PI2) * (random * N);
	float rety = sin(random * PI2) * (random * N);

	float2 offset = float2(retx, rety);

	float4 ret = tex2D(TextureSampler, texCoord + offset);

	return ret;
}

// Radial Coordinate Displacing Luminosity Seed Sample Base
float4 NoiseRCDLSSBFunction(float4 texPosition : SV_position, float4 texColor : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
	float4 ret = tex2D(TextureSampler, texCoord);
	float y = dot(ret.rgb, float3(0.3,0.59,0.11));
	float random = RWS(float2(y, y), float2(S, S));
	float lum_random = rand(float2(y, y));

	float retx = cos(random * PI2) * (lum_random * N);
	float rety = sin(random * PI2) * (lum_random * N);

	float2 offset = float2(retx, rety);

	ret = tex2D(TextureSampler, texCoord + offset);

	return ret;
}

// Radial Coordinate Displacing Luminosity Sample Base
float4 NoiseRCDLSBFunction(float4 texPosition : SV_position, float4 texColor : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
	float4 ret = tex2D(TextureSampler, texCoord);
	float y = dot(ret.rgb, float3(0.3,0.59,0.11));
	float random = rand(float2(y, y));

	float retx = cos(random * PI2) * (random * N);
	float rety = sin(random * PI2) * (random * N);

	float2 offset = float2(retx, rety);

	ret = tex2D(TextureSampler, texCoord + offset);

	return ret;
}

// Radial Coordinate Displacing Coord Luminosity Sample Base
float4 NoiseRCDCLSBFunction(float4 texPosition : SV_position, float4 texColor : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
	float4 ret = tex2D(TextureSampler, texCoord);
	float y = dot(ret.rgb, float3(0.3,0.59,0.11));
	float random = RWS(texCoord, float2(y, y));

	float retx = cos(random * PI2) * (random * N);
	float rety = sin(random * PI2) * (random * N);

	float2 offset = float2(retx, rety);

	ret = tex2D(TextureSampler, texCoord + offset);

	return ret;
}

// Colour Changing Coord Seed Sample Base
float4 NoiseCCCSSBFunction(float4 texPosition : SV_position, float4 texColor : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
	float s = RWS(texCoord,float2(S,S));
float random = RWS(texCoord, float2(S, S));

	float4 ret = tex2D(TextureSampler, texCoord);

	ret = lerp(0, ret, random * N);

	return ret;
}

technique RCDSSB
{
	pass Pass1
	{
		PixelShader = compile ps_4_0_level_9_1 NoiseRCDSSBFunction();
	}
}

technique RCDCSSB
{
	pass Pass1
	{
		PixelShader = compile ps_4_0_level_9_1 NoiseRCDCSSBFunction();
	}
}

technique RCDCSB
{
	pass Pass1
	{
		PixelShader = compile ps_4_0_level_9_1 NoiseRCDCSBFunction();
	}
}

technique RCDLSSB
{
	pass Pass1
	{
		PixelShader = compile ps_4_0_level_9_1 NoiseRCDLSSBFunction();
	}
}

technique RCDLSB
{
	pass Pass1
	{
		PixelShader = compile ps_4_0_level_9_1 NoiseRCDLSBFunction();
	}
}

technique RCDCLSB
{
	pass Pass1
	{
		PixelShader = compile ps_4_0_level_9_1 NoiseRCDCLSBFunction();
	}
}

technique CCCSSB
{
	pass Pass1
	{
		PixelShader = compile ps_4_0_level_9_1 NoiseCCCSSBFunction();
	}
}
