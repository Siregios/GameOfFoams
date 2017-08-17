Shader "Custom/ObjectSkybox" {
	Properties {
		_Color ("Tint Color", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white" {}
        _Cube("Cubemap", CUBE) = "" {}
	}
	SubShader {
        Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf NoLighting alpha:blend//fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
        samplerCUBE _Cube;
        fixed4 _Color;

        fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten) {
            fixed4 c;
            c.rgb = s.Albedo;
            c.a = s.Alpha;
            return c;
        }

        struct Input {
            float2 uv_MainTex;
            float3 worldRefl;
            float3 viewDir;
		};

		void surf (Input IN, inout SurfaceOutput o) {
            o.Albedo = texCUBE(_Cube, IN.viewDir).rgb * _Color;
            o.Alpha = tex2D(_MainTex, IN.uv_MainTex);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
