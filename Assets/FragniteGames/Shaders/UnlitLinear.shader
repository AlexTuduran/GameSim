Shader "Fragnite Games/Unlit Linear" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader {
        Tags {
            "RenderType" = "Opaque"
        }

        LOD 100

        Pass {
            Cull Off
            Blend Off
            ZWrite On
            ZTest Always

CGPROGRAM
            //#pragma target 4.5
            //#pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
            //#pragma exclude_renderers flash gles ps3 xbox360
            //#pragma fragmentoption ARB_precision_hint_fastest

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

            struct fragmentOutput {
                float4 color : SV_Target;
                float depth : SV_Depth;
            };

            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;

            v2f vert(appdata i) {
                v2f o;

                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = TRANSFORM_TEX(i.uv, _MainTex);

                return o;
            }

            /*
            float4 frag(v2f i) : SV_Target {
                float4 color = tex2D(_MainTex, i.uv);
                return color;
            }
            */

            void frag(v2f i, out half4 color:SV_Target, out float depth : SV_Depth) {
                color = tex2D(_MainTex, i.uv);
                depth = 0.5;
            }

            /*
            fragmentOutput frag(v2f i) {
                fragmentOutput o;

                o.color = tex2D(_MainTex, i.uv);

                o.depth = 0.05;

                return o;
            }
            */
ENDCG
        }
    }
}
