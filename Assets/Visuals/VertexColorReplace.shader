// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/VertexColorReplace" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
			_Curvate("Curvature", Float) = 0.1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
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

		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			// Transform the vertex coordinates from model space into world space
			float4 vv = mul(unity_ObjectToWorld, v.vertex);

			// Now adjust the coordinates to be relative to the camera position
			vv.xyz -= _WorldSpaceCameraPos.xyz;

			// Reduce the y coordinate (i.e. lower the "height") of each vertex based
			// on the square of the distance from the camera in the z axis, multiplied
			// by the chosen curvature factor
			vv = float4(0.0f, ((vv.x * vv.x) + (vv.z * vv.z)) * -_Curvature, 0.0f, 0.0f);

			// Now apply the offset back to the vertices in model space
			v.vertex += mul(unity_WorldToObject, vv);
		}

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color

			float grey = (IN.vertexColor.r + IN.vertexColor.g + IN.vertexColor.b) * .333333;
			float diff = (IN.vertexColor.r - IN.vertexColor.g - IN.vertexColor.b);
			grey *= 1.2;

			fixed4 c = IN.vertexColor;

			if(diff > .01)
				c = tex2D (_MainTex, IN.uv_MainTex) * _Color * (grey + (3 * grey));

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
