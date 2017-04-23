Shader "Custom/VertexColor" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

			_Curvature("Curvature", Float) = 0.1
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float _Curvature;

		struct Input {
			float2 uv_MainTex;
			float4 vertexColor : COLOR;
		};

		void vert(inout appdata_full v) {
			// Transform the vertex coordinates from model space into world space
			float4 vv = mul(unity_ObjectToWorld, v.vertex);

			// Now adjust the coordinates to be relative to the camera position
			vv.xyz -= _WorldSpaceCameraPos.xyz;

			// Reduce the y coordinate (i.e. lower the "height") of each vertex based
			// on the square of the distance from the camera in the z axis, multiplied
			// by the chosen curvature factor
			vv = float4(0.0f, ((vv.x * vv.x) + (vv.z * vv.z)) * -_Curvature, 0.0f, 0.0f);

			//v.vertex.xyz += v.normal * .1;
			// Now apply the offset back to the vertices in model space
			v.vertex += mul(unity_WorldToObject, vv);
		}

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color * IN.vertexColor;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
