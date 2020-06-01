#ifndef SHADER_UTILITY_INCLUDED
#define SHADER_UTILITY_INCLUDED

#include "UnityCG.cginc"

float2 random2(float2 p) 
{
    return frac(sin(float2(dot(p,float2(127.1,311.7)),dot(p,float2(269.5,183.3))))*43758.5453);
}

float random(float2 uv)
{
    return frac(sin(dot(uv,float2(12.9898,78.233)))*43758.5453123);
}

float4 drawGrid(float width,float2 uv)
{
    float2 fracUv = frac(uv);
    float grid = 
        step(width,fracUv.x) + 
        step(width,fracUv.y);
    return float4(grid,0,0,1); 
}

void TilingSplitFloorAndFrac(float2 uv,float tilingNum,out float2 tilingUv,out float2 floorUv,out float2 fracUv)
{
    float2 multiplyUv = uv * tilingNum;
    tilingUv = fmod(multiplyUv,1);
    floorUv = floor(multiplyUv);
    fracUv = frac(multiplyUv);
}

float4 float2ToColor(float2 value)
{
    return float4(value.x,value.y,0,1);
}

#endif