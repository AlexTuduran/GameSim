//
// Transparent Color Texture shader
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

// -------------------------------------------------------------------------- //

Shader "OmniSAR Technologies/Transparent Color Texture" {

// -------------------------------------------------------------------------- //

    Properties {
        [MainTexture]
        _MainTex ("Texture", 2D) = "white" {}
        [MainColor]
        [HDR]
        _Color ("Color", Color) = (1, 1, 1, 1)
    }

// -------------------------------------------------------------------------- //

CGINCLUDE

// -------------------------------------------------------------------------- //

    #include "UnityCG.cginc"

// -------------------------------------------------------------------------- //

    uniform sampler2D _MainTex;
    uniform float4 _MainTex_TexelSize;
    uniform float4 _MainTex_ST;
    uniform float4 _Color;

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
        return tex2D(_MainTex, i.uv) * _Color;
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
        Blend SrcAlpha OneMinusSrcAlpha
        
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
