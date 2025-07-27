Shader "Custom/WaterScreen"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Dimension ("Dimension", Float) = 8.0
		_SurfaceNoise("Surface Noise", 2D) = "white" {}
		_SurfaceNoiseStrength("Surface Strength", Float) = 0.1
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
			sampler2D _SurfaceNoise;
			float4 _SurfaceNoise_ST;
			float _SurfaceNoiseStrength;

            fixed4 frag (v2f i) : SV_Target
            {             
				float2 noiseUv = i.uv;
				noiseUv.y += _Time.r * 4.0f;
				float surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUv).r;
				surfaceNoiseSample = saturate(surfaceNoiseSample);
				surfaceNoiseSample -= 0.5f;
				surfaceNoiseSample *= _SurfaceNoiseStrength;
                fixed4 col = tex2D(_MainTex, i.uv + surfaceNoiseSample);
				col.g *= 1.5;
                return col;
            }
            ENDCG
        }
    }
}
