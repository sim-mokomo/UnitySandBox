Shader "Dithered Transparent/Dithered"
{
    Properties 
    {
        _Alpha ("Alpha", Range(0,1)) = 1.0
        _MainTex ("Main Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags{ "RenderType" = "Opaque" "Queue" = "Geometry" }
        
        GrabPass{}

        Pass
        {

            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag

            uniform fixed4 _LightColor0;
            float _Alpha;
            float4 _MainTex_ST;         // For the Main Tex UV transform
            sampler2D _MainTex;         // Texture used for the line
            sampler2D _GrabTexture;
            
            struct v2f
            {
                float4 pos      : POSITION;
                float4 col      : COLOR;
                float2 uv       : TEXCOORD0;
                float4 screenPos     : TEXCOORD1;
            };
            
            float dether(float2 pos, float alpha) 
            {
                pos *= _ScreenParams.xy;
            
                // Define a dither threshold matrix which can
                // be used to define how a 4x4 set of pixels
                // will be dithered
                float DITHER_THRESHOLDS[16] =
                {
                    1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
                    13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
                    4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
                    16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
                };
            
                int index = (int(pos.x) % 4) * 4 + int(pos.y) % 4;
                return alpha - DITHER_THRESHOLDS[index];
            }
            
            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

                float4 norm = mul(unity_ObjectToWorld, v.normal);
                float3 normalDirection = normalize(norm.xyz);
                float4 LightDirection = normalize(_WorldSpaceLightPos0);
                float4 DiffuseLight = saturate(dot(LightDirection, -normalDirection))*_LightColor0;
                o.col = float4(UNITY_LIGHTMODEL_AMBIENT + DiffuseLight);
                o.screenPos = ComputeScreenPos(o.pos);
                return o;
            }

            float4 frag(v2f i) : COLOR
            {
                float4 col = tex2D(_MainTex, i.uv);
                
                float4 viewPos = i.screenPos / i.screenPos.w;
                clip(dether(viewPos,_Alpha));
                // BaseColor * Lighting Value
                return col;
            }

            ENDCG
        }
    }

    SubShader
    {
        Tags { "RenderType" = "ShadowCaster" }
        UsePass "Hidden/Dithered Transparent/Shadow/SHADOW"
    }
}
