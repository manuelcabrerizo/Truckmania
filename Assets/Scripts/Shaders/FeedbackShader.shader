Shader "Custom/FeebackShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Tint ("Tint", Color) = (0, 0, 0, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert
        #pragma target 3.0

        sampler2D _MainTex;
        float4 _Color;
        float4 _Tint;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldNormal;
            float3 worldPos;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            float3 n = normalize(IN.worldNormal);
            float3 v = normalize(_WorldSpaceCameraPos - IN.worldPos);

            float fresnel = saturate(dot(n, v));

            float4 col = tex2D(_MainTex, IN.uv_MainTex);
            //col.rgb *= 3.0f;
            col.rgb *= _Color.rgb;
            col.rgb += _Tint.rgb * (1.0f - fresnel);

            o.Albedo = col.rgb;
            o.Alpha = 1.0;
        }
        ENDCG
    }

    FallBack "Diffuse"
}