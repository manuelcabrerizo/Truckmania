Shader "Custom/ToxicShader"
{
	Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_NoiseTex ("Noise", 2D) = "black" {}
		_Alpha ("Alpha", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "QUEUE"="Transparent" }
        LOD 100

        Pass
        {
			Blend SrcAlpha One
			Cull Off
			ZWrite Off

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

            sampler2D _MainTex;
			sampler2D _NoiseTex;
            float4 _MainTex_ST;
			float _Alpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
				float2 noiseUv = i.uv;
				noiseUv.y += _Time.x;
				float4 noise = tex2D(_NoiseTex, noiseUv*0.25);

				float2 textureUv = i.uv + noise.xy;
				float4 col = tex2D(_MainTex, textureUv);
				col.w = _Alpha;
				return col;
            }

            ENDCG
        }
    }
}
