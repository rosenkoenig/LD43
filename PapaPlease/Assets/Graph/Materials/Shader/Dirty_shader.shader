// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Dirty Shader"
{
	Properties
	{
		_CleanTexture("Clean Texture", 2D) = "white" {}
		_DirtTexture("Dirt Texture", 2D) = "white" {}
		_AjustColor("Ajust Color", Color) = (1,1,1,0)
		_DirtBlending("Dirt Blending", Range( 0 , 1)) = 0
		_DetailTexture("Detail Texture", 2D) = "white" {}
		_DetailColorBlending("Detail Color Blending", Range( 0 , 1)) = 0.5
		_DetailColorAjust("Detail Color Ajust", Color) = (0.5019608,0.5019608,0.5019608,0)
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_NormalTexture("Normal Texture", 2D) = "bump" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _NormalTexture;
		uniform float4 _NormalTexture_ST;
		uniform sampler2D _CleanTexture;
		uniform float4 _AjustColor;
		uniform sampler2D _DirtTexture;
		uniform float _DirtBlending;
		uniform sampler2D _DetailTexture;
		uniform float4 _DetailColorAjust;
		uniform float _DetailColorBlending;
		uniform float _Metallic;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NormalTexture = i.uv_texcoord * _NormalTexture_ST.xy + _NormalTexture_ST.zw;
			o.Normal = UnpackNormal( tex2D( _NormalTexture, uv_NormalTexture ) );
			float4 temp_output_18_0 = ( tex2D( _CleanTexture, i.uv_texcoord ) * _AjustColor );
			float4 blendOpSrc4 = temp_output_18_0;
			float4 blendOpDest4 = tex2D( _DirtTexture, i.uv_texcoord );
			float4 lerpResult5 = lerp( temp_output_18_0 , ( saturate( ( blendOpSrc4 * blendOpDest4 ) )) , _DirtBlending);
			float4 temp_cast_0 = (1.0).xxxx;
			float2 uv_TexCoord22 = i.uv_texcoord * float2( 0.5,0.5 );
			float4 tex2DNode11 = tex2D( _DetailTexture, uv_TexCoord22 );
			float4 blendOpSrc19 = _DetailColorAjust;
			float4 blendOpDest19 = tex2DNode11;
			float4 lerpResult15 = lerp( tex2DNode11 , ( saturate( (( blendOpDest19 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest19 - 0.5 ) ) * ( 1.0 - blendOpSrc19 ) ) : ( 2.0 * blendOpDest19 * blendOpSrc19 ) ) )) , _DetailColorBlending);
			float4 lerpResult21 = lerp( temp_cast_0 , lerpResult15 , _DetailColorBlending);
			float4 blendOpSrc12 = lerpResult5;
			float4 blendOpDest12 = lerpResult21;
			o.Albedo = ( saturate( ( blendOpSrc12 * blendOpDest12 ) )).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16100
137;370;1438;762;1541.886;-420.093;1.025095;True;True
Node;AmplifyShaderEditor.Vector2Node;24;-1091.798,780.2037;Float;False;Constant;_Vector1;Vector 1;10;0;Create;True;0;0;False;0;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;22;-871.0408,767.8748;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1494.97,66.84001;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;17;-906.9927,-41.66234;Float;False;Property;_AjustColor;Ajust Color;2;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;14;-509.0836,585.5709;Float;False;Property;_DetailColorAjust;Detail Color Ajust;6;0;Create;True;0;0;False;0;0.5019608,0.5019608,0.5019608,0;0.5,0.5,0.5,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;-581.2758,755.0609;Float;True;Property;_DetailTexture;Detail Texture;4;0;Create;True;0;0;False;0;None;4f3c685fadc6ce548b343d34875d8f1b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-911.2986,-238.81;Float;True;Property;_CleanTexture;Clean Texture;0;0;Create;True;0;0;False;0;None;bab7490592845464d984f5f7ff55badd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-550,122;Float;True;Property;_DirtTexture;Dirt Texture;1;0;Create;True;0;0;False;0;None;d25ad789f9a66db468431befd820ed5c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;19;-187.6962,642.5065;Float;False;Overlay;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-562.4938,954.3646;Float;False;Property;_DetailColorBlending;Detail Color Blending;5;0;Create;True;0;0;False;0;0.5;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-476.6436,-99.19234;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-531.9502,328.6516;Float;False;Property;_DirtBlending;Dirt Blending;3;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;15;127.423,760.4583;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;20;133.9814,683.1093;Float;False;Constant;_Float0;Float 0;10;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;4;-202.9502,34.65155;Float;False;Multiply;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;21;352.7467,803.6998;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;5;71.0498,-40.34845;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;7;-68.80497,-416.7267;Float;True;Property;_NormalTexture;Normal Texture;9;0;Create;True;0;0;False;0;None;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;13;-21.16072,-207.1988;Float;False;Constant;_Vector0;Vector 0;7;0;Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BlendOpsNode;12;825.4484,4.475281;Float;False;Multiply;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;9;882.2748,266.8745;Float;False;Property;_Smoothness;Smoothness;7;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;881.2064,187.125;Float;False;Property;_Metallic;Metallic;8;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1269.357,-60.68696;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Dirty Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;22;0;24;0
WireConnection;11;1;22;0
WireConnection;2;1;1;0
WireConnection;3;1;1;0
WireConnection;19;0;14;0
WireConnection;19;1;11;0
WireConnection;18;0;2;0
WireConnection;18;1;17;0
WireConnection;15;0;11;0
WireConnection;15;1;19;0
WireConnection;15;2;16;0
WireConnection;4;0;18;0
WireConnection;4;1;3;0
WireConnection;21;0;20;0
WireConnection;21;1;15;0
WireConnection;21;2;16;0
WireConnection;5;0;18;0
WireConnection;5;1;4;0
WireConnection;5;2;6;0
WireConnection;12;0;5;0
WireConnection;12;1;21;0
WireConnection;0;0;12;0
WireConnection;0;1;7;0
WireConnection;0;3;10;0
WireConnection;0;4;9;0
ASEEND*/
//CHKSM=0F9726FE4CF074845D95BA294C81DFF0A6D064A5