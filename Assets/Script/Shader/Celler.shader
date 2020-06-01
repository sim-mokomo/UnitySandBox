Shader "Unlit/Celler"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PointNum ("PointNum",int) = 5
        [Toggle(CONTRAST)] _Contrastable ("Contrastable",float) = 0
        _CircleMaxScale ("_CircleMaxScale",float) = 0.4
        _Tiling ("Tiling",int) = 1
        _GirdWidth ("_GirdWidth",float) = 0.99
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
            #pragma multi_compile _ CONTRAST

            #include "UnityCG.cginc"
            #include "ShaderUtility.cginc"

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
            int _PointNum;
            float _ContrastThreshold;
            float _CircleMaxScale;
            int _Tiling;
            float _GirdWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 i_tilingUv;
                float2 f_tilingUv;
                float2 tilingUv;
                TilingSplitFloorAndFrac(i.uv,_Tiling,tilingUv,i_tilingUv,f_tilingUv);
                
                float dist = _CircleMaxScale;
                for(int y = -1; y <= 1; y++)
                {
                    for(int x = -1; x <= 1; x++)
                    {
                        float2 neighbor = float2(x,y);
                        float2 neighborPoint = random2(i_tilingUv + neighbor);
                        neighborPoint = 0.5 + 0.5 * sin(_Time + 6.2831 * neighborPoint);
                        neighborPoint = (neighbor + neighborPoint);
                        float neighborDistance = length(neighborPoint - f_tilingUv);
                        dist = min(dist, neighborDistance);
                    }
                }
                
                return float4(dist,dist,dist,1) + drawGrid(_GirdWidth,tilingUv);
            }
            
            ENDCG
        }
    }
}
