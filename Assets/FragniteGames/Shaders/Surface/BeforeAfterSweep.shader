Shader "Fragnite Games/Before-After Sweep" {
    Properties {
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
        _CoverageAmount ("Coverage Amount", Range(0, 1)) = 0.5
        _SeparationLineColor ("Separation Line Color", Color) = (1, 1, 1, 1)
        _SeparationLineMarginColor ("Separation Line Margin Color", Color) = (0, 0, 0, 1)
        _SeparationLineWidth ("Separation Line Width", Float) = 2.0
        _SeparationLineMargin ("Separation Line Margin", Float) = 1.0
        [ToggleOff] _Inverted ("Inverted", Float) = 0
    }

    SubShader {
        LOD 100

        Tags {
            "QUEUE"="Transparent"
            "IGNOREPROJECTOR"="true"
            "RenderType"="Transparent"
        }

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {

 CGPROGRAM
 
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform float4 _MainTex_TexelSize;

            uniform float _CoverageAmount;
            uniform float4 _SeparationLineColor;
            uniform float4 _SeparationLineMarginColor;
            uniform float _SeparationLineWidth;
            uniform float _SeparationLineMargin;
            uniform bool _Inverted;

            v2f vert(appdata i) {
                v2f o;

                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = TRANSFORM_TEX(i.uv, _MainTex);

                return o;
            }

            float4 frag(v2f i) : SV_Target {
                float4 color = tex2D(_MainTex, i.uv);

                float separationLineWidth = _SeparationLineWidth * _MainTex_TexelSize.x;
                float separationLineMargin = _SeparationLineMargin * _MainTex_TexelSize.x;

                [branch]
                if (_Inverted) {
                    float s = step(i.uv.x, _CoverageAmount + separationLineWidth);
                    float separationLineBlend = s * (1.0 - step(i.uv.x, _CoverageAmount - separationLineWidth));

                    float marginS = step(i.uv.x, _CoverageAmount + separationLineWidth + separationLineMargin);
                    float separationLineMarginBlend = marginS * (1.0 - step(i.uv.x, _CoverageAmount - separationLineWidth - separationLineMargin));
    
#if 0
                    color.rgb = separationLineBlend;
                    color.rgb = separationLineMarginBlend;
#endif

                    color.rgb = lerp(color.rgb, _SeparationLineMarginColor.rgb, separationLineMarginBlend);
                    color.rgb = lerp(color.rgb, _SeparationLineColor.rgb, separationLineBlend);

                    color.a = marginS;
                } else {
                    float s = 1.0 - step(i.uv.x, _CoverageAmount - separationLineWidth);
                    float separationLineBlend = s * step(i.uv.x, _CoverageAmount + separationLineWidth);
    
                    float marginS = 1.0 - step(i.uv.x, _CoverageAmount - separationLineWidth - separationLineMargin);
                    float separationLineMarginBlend = marginS * step(i.uv.x, _CoverageAmount + separationLineWidth + separationLineMargin);

#if 0
                    color.rgb = separationLineBlend;
                    color.rgb = separationLineMarginBlend;
#endif

                    color.rgb = lerp(color.rgb, _SeparationLineMarginColor.rgb, separationLineMarginBlend);
                    color.rgb = lerp(color.rgb, _SeparationLineColor.rgb, separationLineBlend);

                    color.a = marginS;
                }

                return color;
            }

ENDCG
        }
    }
}
