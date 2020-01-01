Shader "Unlit/FiredPaper"
{
    Properties
    {
        _NoiseTexture ("Texture", 2D) = "white" {}
        _Alpha ("Alpha",Range(0,1)) = 1.0
        _Intensity ("Intensity",Range(0,5)) = 1.0
        _TextureWidth ("TextureWidth",float) = 1
        _TextureHeight ("TextureHeight",float) = 1
        _EdgeThrethhold ("EdgeThrethhold",float) = 0
        _EdgeColor ("EdgeColor",Color) = (0,0,0,0)
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
            float _EdgeThickness;
            float _Alpha;
            float _Intensity;

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
            
            float4 getTexColor(sampler2D tex,float2 uv)
            {
                return pow(tex2D(tex,uv),_Intensity);
            }

            fixed4 frag (v2f i) : SV_TARGET
            {
                float dx = _EdgeThickness / _TextureWidth;
                float dy = _EdgeThickness / _TextureHeight;
                
                float c00 = step(  getTexColor(_NoiseTexture, i.uv + float2(-dx, -dy)),_Alpha); 
                float c01 = step(  getTexColor(_NoiseTexture, i.uv + float2(-dx, 0)),_Alpha); 
                float c02 = step(  getTexColor(_NoiseTexture, i.uv + float2(-dx, dy)),_Alpha); 
                float c10 = step(  getTexColor(_NoiseTexture, i.uv + float2(0, -dy)),_Alpha); 
                float c12 = step(  getTexColor(_NoiseTexture, i.uv + float2(0, dy)),_Alpha); 
                float c20 = step(  getTexColor(_NoiseTexture, i.uv + float2(dx, -dy)),_Alpha); 
                float c21 = step(  getTexColor(_NoiseTexture, i.uv + float2(dx, 0)),_Alpha); 
                float c22 = step(  getTexColor(_NoiseTexture, i.uv + float2(dx, dy)),_Alpha); 

                float sx = -1.0 * c00 + -2.0 * c10 + -1.0 * c20 + 1.0 * c02 + 2.0 * c12 + 1.0 * c22;
                float sy = -1.0 * c00 + -2.0 * c01 + -1.0 * c02 + 1.0 * c20 + 2.0 * c21 + 1.0 * c22;

                float g = sqrt(sx * sx + sy * sy);
                float edge = g > _EdgeThrethhold ? 1 : 0;
                float4 edgeColor = edge * _EdgeColor;
                edgeColor.a = edge;
                
                float4 grabTextureColor = tex2Dproj(_GrabTexture,i.grabPos);
                float4 grayGrabTextureColor = gray(grabTextureColor);
                grayGrabTextureColor.a = 1.0f;
                float f = step(getTexColor(_NoiseTexture,i.uv),_Alpha);
                return (f > 0 ? grayGrabTextureColor : grabTextureColor) + lerp(edgeColor,edgeColor,edge);
            }
            ENDCG
        }
    }
}
