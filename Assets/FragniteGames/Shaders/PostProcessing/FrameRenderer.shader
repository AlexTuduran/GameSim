Shader "Hidden/Fragnite Games/Frame Renderer" {
    SubShader {
        Cull Off
        ZWrite Off
        ZTest Always

        Pass {
CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #define ___HASH_TIME_STEP___                      (0.79531)

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            uniform sampler2D _ColorTexture;
            uniform float4 _ColorTexture_TexelSize;
            uniform sampler2D _DepthTexture;
            uniform float2 _DepthJitter;
            uniform float _Zoom;

            inline float3 Hash31(float p) {
               float3 p3 = frac(p * float3(0.1031, 0.1030, 0.0973));
               p3 += dot(p3, p3.yzx + 33.33);
               return frac((p3.xxy + p3.yzz) * p3.zyx); 
            }

            inline float3 Hash32(float2 p) {
                float3 p3 = frac(float3(p.xyx) * float3(0.1031, 0.1030, 0.0973));
                p3 += dot(p3, p3.yxz + 33.33);
                return frac((p3.xxy + p3.yzz) * p3.zyx);
            }

            inline float3 Hash33(float3 p3) {
                p3 = frac(p3 * float3(0.1031, 0.1030, 0.0973));
                p3 += dot(p3, p3.yxz + 33.33);
                return frac((p3.xxy + p3.yxx) * p3.zyx);
            }

            inline float3 Hash32UV(float2 uv, float step) {
                return Hash33(float3(uv * 14353.45646, (_Time.y % 100.0) * step));
            }

            v2f vert(appdata i) {
                v2f o;

                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv;

                return o;
            }

            float4 frag(v2f i) : SV_Target {
                float4 o;

                float2 uv = (i.uv - 0.5) * _Zoom + 0.5;

                [branch]
                if ((uv.x < 0.0) || (uv.x > 1.0) || (uv.y < 0.0) || (uv.y > 1.0)) {
                    return 0;
                }

                o.rgb = tex2D(_ColorTexture, uv);

                uv += _DepthJitter * _ColorTexture_TexelSize;
                o.a = tex2D(_DepthTexture, uv).r;

                return o;
            }
ENDCG
        }
    }
}
