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
float PingPong(float value)
{
    value %= 1;
    if (value < 0)
        value += 1;

    if (value >= 0.5)
        return 2 - value * 2;

    return value * 2;
}
float2 Rotate(float2 uv, float amount)
{
    float2 uv2 = uv;
    float s = sin(amount);
    float c = cos(amount);
    uv2.x = (uv.x * c) + (uv.y * -s);
    uv2.y = (uv.x * s) + (uv.y * c);

    return uv2;
    
}
float sdCylinder(float3 p, float3 a, float3 b, float r)
{
    float3 ba = b - a;
    float3 pa = p - a;
    float baba = dot(ba, ba);
    float paba = dot(pa, ba);
    float x = length(pa * baba - ba * paba) - r * baba;
    float y = abs(paba - baba * 0.5) - baba * 0.5;
    float x2 = x * x;
    float y2 = y * y * baba;
    
    float d = (max(x, y) < 0.0) ? -min(x2, y2) : (((x > 0.0) ? x2 : 0.0) + ((y > 0.0) ? y2 : 0.0));
    
    return sign(d) * sqrt(abs(d)) / baba;
}

float sdVesica(float2 p, float w, float h)
{
    float3 d = 0.5 * (w * w - h * h) / h;
    p = abs(p);
    float3 c = (w * p.y < d * (p.x - w)) ? float3(0.0, w, 0.0) : float3(-d.x, 0.0, (d.x) + h);
    return length(p - c.yx) - c.z;
}
float sdCone(float3 p, float2 c, float h)
{

    float2 q = h * float2(c.x / c.y, -1.0);
    
    float2 w = float2(length(p.xz), p.y);
    float2 a = w - q * clamp(dot(w, q) / dot(q, q), 0.0, 1.0);
    float2 b = w - q * float2(clamp(w.x / q.x, 0.0, 1.0), 1.0);
    float k = sign(q.y);
    float d = min(dot(a, a), dot(b, b));
    float s = max(k * (w.x * q.y - w.y * q.x), k * (w.y - q.y));
    return sqrt(d) * sign(s);
}
float3 Rotate(float3 p, float3 axis, float angle)
{
    return lerp(dot(axis, p) * axis, p, cos(angle)) + cross(axis, p) * sin(angle);
}

float sdSphere(float3 p, float s)
{
    return (length(p) - s);
}

float sdPlane(float3 p, float3 normlized, float h)
{
    return dot(p, normlized) + h;
}
float3 rotateZ(float3 p, float angle)
{
    float s = sin(angle);
    float c = cos(angle);
    float3 p2 = p;
    p2.y = (p.x * c) + (p.y * -s);
    p2.x = (p.x * s) + (p.y * c);

    return p2;

}
float3 rotateX(float3 p, float angle)
{
    float s = sin(angle);
    float c = cos(angle);
    float3 p2 = p;
    p2.y = (p.z * c) + (p.y * -s);
    p2.z = (p.z * s) + (p.y * c);

    return p2;

}

float3 mutliLerp(float3 value1, float3 value2, float3 value3, float t)
{
    float3 value = 0;
    if (t < 0.5)
    {
        value = lerp(value1, value2, (t) * 2);

    }
    else
    {
        value = lerp(value2, value3, (t - 0.5) * 2);

    }
    return value;

}
float map(float3 p, out int ID)
{

    
    
    float plane = sdSphere(p, 1);

    float d = plane;
    if (d == plane)
    {
        ID = 0;
    }
    else if (d == 1)
    {
        ID = 1;
    }


    return (d);
}

float map2(float3 p)
{
    return sdSphere(p, 16);

}

// soo cool 

//float2 centerUV = (texCoords * 2 - 1);
//float2 cylinderUV = float2(pow(centerUV.x, 2), centerUV.y);
//float4 tornado = tex2D(image1, centerUV + float2(time - centerUV.y * 5, 0));
//    tornado.rgba *= lerp(1,0,cylinderUV.x);
//    tornado.rgb *= lerp(color + float3(0,0,1), float3(1, 0, 0), tornado.r - centerUV.x);
//    tornado /= tex2D(image1,centerUV + time).r - cylinderUV.x * 5;
//    return
//tornado;

float4 ShaderPS(float4 vertexColor : COLOR0, float2 texCoords : TEXCOORD0) : COLOR0
{
    float2 centerUV = (texCoords * 2 - 1) + float2(sin(texCoords.y * 7 + time * 3) * 0.1,0);
    float2 cylinderUV = float2(pow(centerUV.x, 2), centerUV.y);
    float2 windDir = Rotate(centerUV, 3.141518 / 4);
    float4 tornado = tex2D(image1, cylinderUV * float2(2, 3) - float2(time * 4, 0) - centerUV.y * 5);
    float cone = abs(1 / centerUV.x) * pow(texCoords.y - 1,1) - 1;
    float tighterCone = abs(1 / centerUV.x) * pow(texCoords.y - 1, 2) - 1;

    float4 edgeTexture = lerp(1, 0, tex2D(image1, texCoords + float2(0, time * 2 + centerUV.y)) / tighterCone);
    float4 final = float4(tornado.rgb * tighterCone, smoothstep(0, 0.2, cone));
    final = saturate(final.r) * 2 * float4(lerp(color, float3(0, 0.5, 1), centerUV.x), final.a) * tex2D(image3, texCoords).r * step(.01, texCoords.y);
    
    final.rgba *= lerp(0, 1, texCoords.y - shaderData.y);
    return final;

}

technique t0
{
    pass DukeTornado
    {
        PixelShader = compile ps_3_0 ShaderPS();
    }
}