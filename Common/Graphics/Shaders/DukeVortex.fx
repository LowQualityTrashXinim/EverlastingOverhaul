#pragma warning (disable : 4717) 
sampler2D image1 : register(s1);
sampler2D image2 : register(s2);
sampler2D image3 : register(s3);
float4x4 viewWorldProjection;
float time;
float4 shaderData;
float3 color;


struct VertexShaderInput
{
    float4 pos : POSITION0;
    float4 col : COLOR0;
    float2 texCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 pos : SV_POSITION;
    float4 col : COLOR0;
    float2 texCoord : TEXCOORD0;
};
float sinBetween(float a, float b, float v)
{
    float h = (b - a) / 2.;
    return a + h + sin
    (v) * h;
}
float4 ShaderPS(float4 vertexColor : COLOR0, float2 texCoords : TEXCOORD0) : COLOR0
{
    float2 uv = (texCoords * 2 - 1);
    float d = length((uv.y * 10));
    float m = 1 / d;
    float4 fire = m * sin(time * 25) * .025 + .2 * m;
    fire *= smoothstep(0, 0.5, fire);
    fire.r *= (m );
    fire.a = fire.r;
    
    float4 color1 = fire * 1000 * m;
    color1 = floor(color1 * 5) / 5;

    //initial position of the fire
    color1 *= smoothstep(0, 1, uv.x + .5);
    
    //apply le color
    color1.rgb = saturate(color1.rgb);

    //end point of the fire
    color1 *= smoothstep(1, 0, texCoords.x * texCoords.x * .1);
    color1 = lerp(saturate(color1) * 3, float4(0, 0, 0, 0), texCoords.x + (frac(time * 5) * 0.4));
    
    color1 *= m;
    color1 *= 2;


    return color1 * color.rgbr;
}

technique t0
{
    pass DukeBG
    {
        PixelShader = compile ps_3_0 ShaderPS();
    }
}