//
// Gradient-Mapped Transparent Color Texture shader
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

// -------------------------------------------------------------------------- //

Shader "OmniSAR Technologies/Gradient-Mapped Transparent Color Texture" {

// -------------------------------------------------------------------------- //

    Properties {
        [NoScaleOffset] _GradientMapTex ("Gradient Map Texture", 2D) = "white" {}
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _Color ("Color", Color) = (1, 1, 1, 1)
        [Enum(UnityEngine.Rendering.BlendMode)] _BlendMode ("Blend Mode", Float) = 1
    }

// -------------------------------------------------------------------------- //

CGINCLUDE

// -------------------------------------------------------------------------- //

    #include "UnityCG.cginc"

// -------------------------------------------------------------------------- //

    uniform sampler2D _GradientMapTex;
    uniform sampler2D _MainTex;
CBUFFER_START(UnityPerMaterial)
    uniform float4 _MainTex_TexelSize;
    uniform float4 _MainTex_ST;
    uniform float4 _Color;
CBUFFER_END

// -------------------------------------------------------------------------- //

    struct appdata {
        float4 vertex : POSITION;
        float2 uv : TEXCOORD0;
    };

    struct v2f {
        float2 uv : TEXCOORD0;
        float4 vertex : SV_POSITION;
    };

// -------------------------------------------------------------------------- //

    v2f vert(appdata v) {
        v2f o;

        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = TRANSFORM_TEX(v.uv, _MainTex);

#if UNITY_UV_STARTS_AT_TOP
        if (_MainTex_TexelSize.y < 0) {
            o.uv.y = 1.0 - o.uv.y;
        }
#endif // UNITY_UV_STARTS_AT_TOP

        return o;
    }

// -------------------------------------------------------------------------- //

    float4 frag(v2f i) : SV_Target {
        float4 color = tex2D(_MainTex, i.uv);

        float intensity = color.a;
        intensity *= _Color.a;
        intensity = saturate(intensity);

        float2 gradientMapUV = float2(intensity, 0.5);

        float4 gradientColor = tex2D(_GradientMapTex, gradientMapUV);
        gradientColor.a = color.a;
        gradientColor.rgb *= _Color.rgb;
        return gradientColor;
    }

// -------------------------------------------------------------------------- //

ENDCG

// -------------------------------------------------------------------------- //

    SubShader {

// -------------------------------------------------------------------------- //

        Tags {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
        }

        LOD 100

        ZWrite Off
        Blend SrcAlpha [_BlendMode]
        
// -------------------------------------------------------------------------- //

        Pass {
CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
ENDCG
        } // pass

// -------------------------------------------------------------------------- //

    } // sub-shader

// -------------------------------------------------------------------------- //
    
} // shader

// -------------------------------------------------------------------------- //
