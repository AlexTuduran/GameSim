Shader "Hidden/Fragnite Games/Render Image" {
    SubShader {
        Cull Off
        ZWrite Off
        ZTest Always

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

            uniform sampler2D _InputTexture;
            uniform float _Zoom;

            v2f vert(appdata i) {
                v2f o;

                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv;

                return o;
            }

            float4 frag(v2f i) : SV_Target {
                return tex2D(_InputTexture, (i.uv - 0.5) * _Zoom + 0.5);
            }
ENDCG
        }
    }
}
