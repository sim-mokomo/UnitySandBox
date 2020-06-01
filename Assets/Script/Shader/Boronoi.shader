Shader "Unlit/Boronoi"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [Toggle(CONTRAST)] _Contrastable ("Contrastable",float) = 0
        _CircleMaxScale ("_CircleMaxScale",float) = 0.4
        _Step ("_Step",float) = 0.4
        _Tiling ("Tiling",int) = 1
        _GirdWidth ("_GirdWidth",float) = 0.99
        _EdgeWidth ("_EdgeWidth",int) = 1
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
            float _ContrastThreshold;
            float _CircleMaxScale;
            int _Tiling;
            float _GirdWidth;
            float _Step;
            int _EdgeWidth;

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
                
                float min_dist = _CircleMaxScale;
                float2 min_point;
                
                for(int y = -1; y <= 1; y++)
                {
                    for(int x = -1; x <= 1; x++)
                    {
                        float2 neighbor = float2(x,y);
                        float2 neighborGirdRandomPoint = random2(i_tilingUv + neighbor);
                        neighborGirdRandomPoint = 0.5 + 0.5 * sin(_Time + 6.2831 * neighborGirdRandomPoint);
                        float2 neighborPoint = neighbor + neighborGirdRandomPoint - f_tilingUv;
                        float neighborDistance = length(neighborPoint);
                        if(neighborDistance < min_dist)
                        {
                            min_dist = neighborDistance;
                            min_point = neighborPoint;
                        }
                    }
                }
                              
                float4 col = float4(0,0,0,1);
                col += 1 - step(0.05,length(min_point));
                col += float2ToColor(min_point);
                col += drawGrid(_GirdWidth,tilingUv);
                return col;
            }
            
            ENDCG
        }
    }
}
