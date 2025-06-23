Shader "Custom/AimBarShader"
{
    Properties
    {
		_HorizontalScale ("HorizontalScale", Float) = 10.0
		_VertialScale ("VertialScale", Float) = 1.0

		_MarkerPosition ("MarkerPosition", Float) = 0.0
		_MarkerWidth ("MarkerWidth", Float) = 0.5
		_MarkerHeight ("MarkerHeight", Float) = 0.75
		_MarkerThickness ("MarkerThickness", Float) = 0.25
		_MarkerTex ("MarkerTex", 2D) = "white" {}
		_MarkerTexScale ("MarkerTexScale", Float) = 1.0

		_BorderThickness ("BorderThickness", Float) = 0.25
		_BorderTex ("BorderTex", 2D) = "white" {}
		_BorderTexScale ("BorderTexScale", Float) = 1.0

		_NoiseTex ("Noise", 2D) = "black" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque"}
        LOD 100

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
				float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

			float _HorizontalScale;
			float _VertialScale;

			float _MarkerPosition;
			float _MarkerWidth;
			float _MarkerHeight;
			float _MarkerThickness;
			sampler2D _MarkerTex; 
			float _MarkerTexScale;

			float _BorderThickness;
			sampler2D _BorderTex; 
			float _BorderTexScale;

			sampler2D _NoiseTex;


			float IntersectRect(float2 p, float2 min, float2 max)
			{
				if (p.x >= min.x && p.x <= max.x &&
					p.y >= min.y && p.y <= max.y)	
				{
					return 1.0f;
				}
				return 0.0f;
			}

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float2 worldUv = i.uv;
				
				// Center the uv
				worldUv.x -= 0.5f;
				worldUv.y -= 0.5f;
				
				// Applay scale
				worldUv.x *= _HorizontalScale;
				worldUv.y *= _VertialScale;

				float3 blackColor = float3(0.0f, 0.0f, 0.0f);
				float3 whiteColor = float3(1.0f, 1.0f, 1.0f);
				float3 greenColor = float3(0.0f, 1.0f, 0.0f);
				float3 redColor = float3(1.0f,  0.0f, 0.0f);
				float3 yellowColor = float3(1.0f, 1.0f, 0.0f);

				float2 markerMin = float2(-_MarkerWidth*0.5f, -_MarkerHeight*0.5f);
				float2 markerMax = float2(_MarkerWidth*0.5f, _MarkerHeight*0.5f);
				float2 markerInnerMin = markerMin + float2(_MarkerThickness, _MarkerThickness);
				float2 markerInnerMax = markerMax - float2(_MarkerThickness, _MarkerThickness);
				
				// Apply translation
				markerMin.x += _MarkerPosition;
				markerMax.x += _MarkerPosition;
				markerInnerMin.x += _MarkerPosition;
				markerInnerMax.x += _MarkerPosition;

				float markerInnerMask = IntersectRect(worldUv, markerInnerMin, markerInnerMax);
				float markerMask = saturate(IntersectRect(worldUv, markerMin, markerMax) - markerInnerMask);
				
				float2 noiseUv = i.uv;
				noiseUv.x -= _MarkerPosition / _HorizontalScale;
				noiseUv.y += _Time.x;
				float4 noise = tex2D(_NoiseTex, noiseUv*0.125);

				float2 markerUv = i.uv + noise.xy;
				markerUv.x -= _MarkerPosition / _HorizontalScale;
				float3 markerColor = tex2D(_MarkerTex, markerUv * _MarkerTexScale).xyz * 4.0f;

				float2 borderMin = float2(-5.0f, -0.75f);
				float2 borderMax = float2(5.0f, 0.75f);
				float2 borderInnerMin = borderMin + float2(_BorderThickness, _BorderThickness);
				float2 borderInnerMax = borderMax - float2(_BorderThickness, _BorderThickness);
				float borderInnerMask = IntersectRect(worldUv, borderInnerMin, borderInnerMax);
				float borderMask = saturate(IntersectRect(worldUv, borderMin, borderMax) - borderInnerMask);
				borderMask = saturate(borderMask - markerMask);

				float2 borderUv = i.uv;
				float3 borderColor = tex2D(_BorderTex, borderUv * _BorderTexScale).xyz * 4.0f;

				float distToCenter = abs((i.uv.x - 0.5f) * 2.0f);
				float colorMask = step(0.5f, distToCenter);
				float3 greenToYellowColor = lerp(greenColor, yellowColor, distToCenter*2.0f);
				float3 yellowToRedColor = lerp(yellowColor, redColor, (distToCenter - 0.5f) * 2.0f);
				float3 barColor = lerp(greenToYellowColor, yellowToRedColor, colorMask);

				float barMask = saturate(IntersectRect(worldUv, float2(-5.0f, -0.75f), float2(5.0f, 0.75f)) - markerMask);
				barMask = saturate(barMask - borderMask);

				float3 finalColor = float3(0.0f, 0.0f, 0.0f);
				finalColor += markerColor * markerMask;
				finalColor += borderColor * borderMask;
				finalColor += barColor * barMask;
				float alpha = saturate(markerMask + borderMask + barMask);
				if(alpha < 0.5f)
				{
					discard;	
				}
                return float4(finalColor, alpha);
            }
            ENDCG
        }
    }
}
