Shader "Renderers/DepthWrite0"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _ColorMap("ColorMap", 2D) = "white" {}
    }

    HLSLINCLUDE

    #pragma target 4.5
    #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch

    // #pragma enable_d3d11_debug_symbols

    //enable GPU instancing support
    //#pragma multi_compile_instancing

    ENDHLSL

    SubShader
    {
        Pass
        {
            //Name "FirstPass"
            //Tags { "LightMode" = "ForwardOnly" }

            Blend Off
            ZWrite On
            ZTest Greater

            Cull Off

            HLSLPROGRAM

            // Toggle the alpha test
            //#define _ALPHATEST_ON

            // Toggle transparency
            //#define _SURFACE_TYPE_TRANSPARENT

            // Toggle fog on transparent
            //#define _ENABLE_FOG_ON_TRANSPARENT
            
            // List all the attributes needed in your shader (will be passed to the vertex shader)
            // you can see the complete list of these attributes in VaryingMesh.hlsl
            //#define ATTRIBUTES_NEED_TEXCOORD0
            //#define ATTRIBUTES_NEED_NORMAL
            //#define ATTRIBUTES_NEED_TANGENT

            // List all the varyings needed in your fragment shader
            //#define VARYINGS_NEED_TEXCOORD0
            //#define VARYINGS_NEED_TANGENT_TO_WORLD

            //#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassRenderers.hlsl"
            //#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/VertMesh.hlsl"
            #include "UnityCG.cginc"

            /*
            PackedVaryingsType Vert(AttributesMesh inputMesh)
            {
                VaryingsType varyingsType;
                varyingsType.vmesh = VertMesh(inputMesh);
                return PackVaryingsType(varyingsType);
            }
            */

            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
                        
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f Vert(appdata i) {
                v2f o;

                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = TRANSFORM_TEX(i.uv, _MainTex);

                return o;
            }

            float4 Frag(v2f i, out float outputDepth : SV_Depth) : SV_Target
            {
                //UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(packedInput);
                //FragInputs input = UnpackVaryingsMeshToFragInputs(packedInput.vmesh);

                // input.positionSS is SV_Position
                //PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

                outputDepth = tex2D(_MainTex, i.uv).r * 0.01;
                
                return tex2D(_MainTex, i.uv);
            }

            #pragma vertex Vert
            #pragma fragment Frag

            ENDHLSL
        }
    }
}
