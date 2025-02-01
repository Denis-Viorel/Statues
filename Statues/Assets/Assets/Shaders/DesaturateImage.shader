Shader "Custom/DesaturateImage"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DesaturateAmount ("Desaturate Amount", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata_t
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float _DesaturateAmount;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

            half3 ConvertToGrayscale(half3 color)
            {
                return dot(color, half3(0.299, 0.587, 0.114));
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                
                // Convert to grayscale
                half grayscale = dot(col.rgb, half3(0.299, 0.587, 0.114));
                
                // Interpolate only the green and blue channels towards grayscale
                col.g = lerp(col.g, grayscale, _DesaturateAmount);
                col.b = lerp(col.b, grayscale, _DesaturateAmount);

                return col;
            }
            ENDHLSL
        }
    }
}
