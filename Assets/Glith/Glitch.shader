Shader "Hidden/Custom/Glitch"
{
    HLSLINCLUDE

    #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

    TEXTURE2D_SAMPLER2D(_MainTex,sampler_MainTex);
    float _Blend;
    sampler2D _RandomNoiseTexture;
    float _BlockSize;
    float _FloatGeneralValue0;
    float _LineColorShiftValueRed;
    float _LineColorShiftValueGreen;
    float _LineColorShiftValueBlue;
    float _BlockThrethIntensity;
    float _LineThrethIntensity;

    float4 Frag(VaryingsDefault i) : SV_Target
    {
        float2 uv = i.texcoord;
        //return float4(uv,0,1);
        float2 block = floor( (uv * _ScreenParams) / _BlockSize);

        int maxBlockNumX = floor(_ScreenParams.x / _BlockSize);
        int maxBlockNumY = floor(_ScreenParams.y / _BlockSize);
        //return float4(block,0,1);
        float2 uv_noise =
            float2(
                block.x / maxBlockNumX,
                block.y / maxBlockNumY);
        uv_noise = frac( uv_noise + _Time );
        // return float4(uv_noise,0,1); 

        //TODO: 生数値が適当かどうか調べる
        float line_thresh = pow(frac(_Time * 1236.0453), 3.0) * _LineThrethIntensity;
        float block_thresh = pow(frac(_Time * 2236.0453), 3.0) * _BlockThrethIntensity;
        float textureIntensity = 5;
        float block_threshhold_from_texture = tex2D(_RandomNoiseTexture,uv_noise).g * textureIntensity;
        float line_threshhold_from_texture = tex2D(_RandomNoiseTexture,double2(uv_noise.y,0.0)).g * textureIntensity;

        // return block_threshhold_from_texture;
        // return line_threshhold_from_texture;

        float2 uv_r,uv_g,uv_b;
        uv_r = uv_g = uv_b = uv;

        int enableLineGlitch =
            step(line_threshhold_from_texture,line_thresh)
         || step(block_threshhold_from_texture,block_thresh);
        //return enableLineGlitch;
        if(enableLineGlitch)
        {
            float2 distortion_power = (frac(uv_noise) - 0.5) * 0.3;
            // return float4(distortion_power,0,1);
   
            uv_r += distortion_power * _LineColorShiftValueRed;
            uv_g += distortion_power * _LineColorShiftValueGreen;
            uv_b += distortion_power * _LineColorShiftValueBlue;
        }
        
        float r = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,uv_r).r;;
        float g = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,uv_g).g;
        float b = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,uv_b).b;
        return float4(r,g,b,1);
    }

    ENDHLSL
    
    SubShader
    {
        Cull Off ZWrite Off ZTest
    Always
        Pass
        {
            HLSLPROGRAM
                #pragma vertex VertDefault
                #pragma fragment Frag
            ENDHLSL
            
        }
    }
}