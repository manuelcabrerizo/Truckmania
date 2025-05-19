Shader "Custom/PixelShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Dimension ("Dimension", Float) = 8.0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Dimension;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 resolution = _ScreenParams.xy;
                // Pixelation
                float ratio = resolution.x / resolution.y; 
                float resIndDim = (resolution.y * _Dimension) / 1080.0f;
                float2 dims = float2(resIndDim * ratio, resIndDim);
                float2 coord = floor(i.uv * dims) / dims;
                
                fixed4 col = tex2D(_MainTex, coord);
                return col;
            }
            ENDCG
        }
    }
}
