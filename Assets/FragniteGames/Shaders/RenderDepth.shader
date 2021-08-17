Shader "Hidden/Fragnite Games/Render Depth" {
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

            uniform sampler2D _CameraDepthTexture;
            uniform bool _LinearEye;
            uniform float _MinDistance;
            uniform float _MaxDistance;
            uniform float _Gamma;
            uniform float _Gain;

            v2f vert(appdata i) {
                v2f o;

                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv;

                return o;
            }

            inline float inverseLerp(float a, float b, float value) {
                return (b - value) / (b - a);
            }

            float4 frag(v2f i) : SV_Target {
                float depth = tex2D(_CameraDepthTexture, i.uv).r;

                [branch]
                if (_LinearEye) {
                    depth = LinearEyeDepth(depth);
                    depth = inverseLerp(_MinDistance, _MaxDistance, depth);
                }

                depth = pow(depth, _Gamma);
                depth *= _Gain;

                return depth;
            }
ENDCG
        }
    }
}
