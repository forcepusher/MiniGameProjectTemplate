Shader "Custom/StandardOverrideAmbient" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        [Toggle] _UseAdditiveAmbient ("Use Additive Ambient", Float) = 0
        _AdditiveAmbient ("Additive Ambient Color", Color) = (0.2,0.2,0.2,1)
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf CustomAmbient fullforwardshadows
        #pragma target 3.0
        #pragma shader_feature _USEADDITIVEAMBIENT_ON

        #include "UnityPBSLighting.cginc"
        #include "Lighting.cginc"
        #include "AutoLight.cginc"

        sampler2D _MainTex;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _AdditiveAmbient;

        struct Input {
            float2 uv_MainTex;
        };

        // GI: run once per pixel; when custom ambient is on, replace scene ambient with _AdditiveAmbient.
        inline void LightingCustomAmbient_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi) {
            LightingStandard_GI(s, data, gi);
            #ifdef _USEADDITIVEAMBIENT_ON
                gi.indirect.diffuse = _AdditiveAmbient.rgb;
            #endif
        }

        // Single lighting entry point: direct + indirect. Ambient is in gi.indirect (set once in _GI).
        inline half4 LightingCustomAmbient(SurfaceOutputStandard s, half3 viewDir, UnityGI gi) {
            return LightingStandard(s, viewDir, gi);
        }

        void surf(Input IN, inout SurfaceOutputStandard o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Standard"
}
