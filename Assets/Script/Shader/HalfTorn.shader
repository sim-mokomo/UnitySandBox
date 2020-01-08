Shader "Unlit/HalfTorn"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScaleMax("ScaleMax",Range(0,3)) = 1
        _ScaleMin("ScaleMin",Range(0,3)) = 1
        _Intensity("Intensity",float) = 1
        _BlockIntensity("BlockIntensity",float) = 1
        _Tiling("Tiling",int) = 1
        _MainColor("MainColor",Color) = (0,0,0,0)
        _SubColor("SubColor",Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            float _ScaleMax;
            float _ScaleMin;
            float _Intensity;
            float _BlockIntensity;
            int _Tiling;
            float4 _MainColor;
            float4 _SubColor;
            
            float Diamond(float2 uv,float2 offset)
            {
                float tiling = (_Tiling % 2 == 0) ? _Tiling + 1 : _Tiling;
                uv = fmod((uv + offset) * tiling,1);
                
                float2 centeringUv = abs((uv * 2.0) - 1.0);
                float width = dot(float2(1,0),centeringUv);
                float height = dot(float2(0,1),centeringUv);
                width = width - _Intensity;
                height = height - _Intensity;
                
                float2 blockUv = floor(uv * tiling) / tiling;
                float diamond = step(width * height,lerp(_ScaleMin,_ScaleMax,pow(blockUv.y,_BlockIntensity)));
                return diamond;
            }

            fixed4 frag (v2f i) : COLOR
            {
                float diamond1 = Diamond(i.uv,float2(0,0));
                float diamond2 = Diamond(i.uv,float2(0.5,0.5));
                return diamond1 + diamond2 ? _MainColor : _SubColor;
            }
           
            ENDCG
        }
    }
}
