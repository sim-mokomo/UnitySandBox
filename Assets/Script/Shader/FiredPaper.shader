Shader "Unlit/FiredPaper"
{
    Properties
    {
        _NoiseTexture ("Texture", 2D) = "white" {}
        _Alpha ("Alpha",Range(0,1)) = 1.0
        _TextureWidth ("TextureWidth",float) = 1
        _TextureHeight ("TextureHeight",float) = 1
        _EdgeThrethhold ("EdgeThrethhold",float) = 0
        _EdgeColor ("EdgeColor",Color) = (0,0,0,0)
        _BaseColor ("BaseColor",Color) = (0,0,0,0)
        _EdgeThickness ("EdgeThickness",float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        
        GrabPass{}

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
                float4 grabPos : TEXCOORD1;
            };

            float4 _NoiseTexture_ST;
            sampler2D _NoiseTexture;
            float _Alpha;
            float4 _BaseColor;
            sampler2D _GrabTexture;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _NoiseTexture);
                o.grabPos = ComputeGrabScreenPos(o.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            float gray (fixed4 c) {
                return 0.2126 * c.r + 0.7152 * c.g + 0.0722 * c.b;
            }

            fixed4 frag (v2f i) : SV_TARGET
            {
                float noise = tex2D(_NoiseTexture,i.uv);
                float visibleRange = step(noise,_Alpha);
                
                float4 color = visibleRange;
                return color;
            }
            ENDCG
        }
        
        GrabPass{}
        
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
                float4 vertex : SV_POSITION;
                float4 grabPos : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };
            
            sampler2D _GrabTexture;
            sampler2D _NoiseTexture;
            float4 _GrabTexture_ST;
            float _TextureWidth;
            float _TextureHeight;
            float _EdgeThrethhold;
            float4 _EdgeColor;
            float4 _BaseColor;
            float _EdgeThickness;
            float _Alpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.grabPos = ComputeGrabScreenPos(o.vertex);
                o.uv = v.uv;
                return o;
            }
            
            float gray (fixed4 c) {
                return 0.2126 * c.r + 0.7152 * c.g + 0.0722 * c.b;
            }

            fixed4 frag (v2f i) : SV_TARGET
            {
                float dx = _EdgeThickness / _TextureWidth;
                float dy = _EdgeThickness / _TextureHeight;
                float f = step(tex2D(_NoiseTexture,i.uv),_Alpha);
                
                float c00 = gray(tex2Dproj(_GrabTexture, i.grabPos + float4(-dx, -dy,0,0))); 
                float c01 = gray(tex2Dproj(_GrabTexture, i.grabPos + float4(-dx, 0.0,0,0))); 
                float c02 = gray(tex2Dproj(_GrabTexture, i.grabPos + float4(-dx, dy,0,0))); 
                float c10 = gray(tex2Dproj(_GrabTexture, i.grabPos + float4(0, -dy,0,0))); 
                float c12 = gray(tex2Dproj(_GrabTexture, i.grabPos + float4(0, dy,0,0))); 
                float c20 = gray(tex2Dproj(_GrabTexture, i.grabPos + float4(dx, -dy,0,0))); 
                float c21 = gray(tex2Dproj(_GrabTexture, i.grabPos + float4(dx, 0.0,0,0))); 
                float c22 = gray(tex2Dproj(_GrabTexture, i.grabPos + float4(dx, dy,0,0))); 

                float sx = -1.0 * c00 + -2.0 * c10 + -1.0 * c20 + 1.0 * c02 + 2.0 * c12 + 1.0 * c22;
                float sy = -1.0 * c00 + -2.0 * c01 + -1.0 * c02 + 1.0 * c20 + 2.0 * c21 + 1.0 * c22;

                float g = sqrt(sx * sx + sy * sy);
                float edge = g > _EdgeThrethhold ? 1 : 0;
                float4 edgeColor = edge * _EdgeColor;
                return edgeColor * f;
            }
            ENDCG
        }
    }
}
