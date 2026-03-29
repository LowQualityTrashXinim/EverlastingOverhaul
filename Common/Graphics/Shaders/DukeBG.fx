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
    float2 uv = texCoords * float2(1,1 / exp(texCoords.y) * 0.5);
    uv.x = sin(texCoords.x * 2 - 1);
    float4 tex1 = tex2D(image1, uv + shaderData.xy / 5120 + time * 0.05) * float4(0.5, 0.25, 1, 1);
    float4 texClouds = tex2D(image1, uv * float2(1, 3) + shaderData.xy / 3840 + time * 0.01) * float4(uv.y, uv.y, uv.y, 1) * float4(0, 0.2, 2, 1);
    return tex1 * tex2D(image1, uv + shaderData.xy / 1920 + time * 0.03) * float4(1, 0.25, 1, 1) + texClouds;

}

technique t0
{
    pass DukeBG
    {
        PixelShader = compile ps_3_0 ShaderPS();
    }
}