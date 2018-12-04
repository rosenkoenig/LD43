// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Animated_Screen_Shader"
{
	Properties
	{
		_AnimatedTexture("Animated Texture", 2D) = "white" {}
		_BaseTexture("Base Texture", 2D) = "white" {}
		_Vector0("Vector 0", Vector) = (0,0,0,0)
		_Color0("Color 0", Color) = (0.965031,1,0.5613208,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color0;
		uniform sampler2D _BaseTexture;
		uniform float2 _Vector0;
		uniform sampler2D _AnimatedTexture;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 panner18 = ( _Time.y * _Vector0 + i.uv_texcoord);
			float4 blendOpSrc26 = _Color0;
			float4 blendOpDest26 = tex2D( _BaseTexture, ( panner18 * 0.34 ) );
			float4 blendOpSrc7 = ( saturate( ( blendOpSrc26 * blendOpDest26 ) ));
			float4 blendOpDest7 = tex2D( _AnimatedTexture, panner18 );
			float4 temp_output_7_0 = ( saturate( ( blendOpSrc7 * blendOpDest7 ) ));
			o.Albedo = temp_output_7_0.rgb;
			o.Emission = temp_output_7_0.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16100
174;354;1433;829;406.5849;10.32692;1.433797;True;True
Node;AmplifyShaderEditor.SimpleTimeNode;23;207.7841,688.8693;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;24;126.7841,397.8693;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;19;124.4668,527.1222;Float;False;Property;_Vector0;Vector 0;2;0;Create;True;0;0;False;0;0,0;60,100;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;22;365.0591,249.3884;Float;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;False;0;0.34;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;18;411.067,531.722;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;573.6733,381.3162;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;6;757.5339,254.4793;Float;True;Property;_BaseTexture;Base Texture;1;0;Create;True;0;0;False;0;None;c251316d0ebb2f44ebd0dd7a78a31496;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;25;796.2164,74.26726;Float;False;Property;_Color0;Color 0;3;0;Create;True;0;0;False;0;0.965031,1,0.5613208,0;0.3600448,1,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;844.7904,526.1013;Float;True;Property;_AnimatedTexture;Animated Texture;0;0;Create;True;0;0;False;0;None;d8bd75fd30fd41240bc01f01a22ebf64;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;26;1194.811,289.3369;Float;False;Multiply;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;7;1445.652,418.6;Float;False;Multiply;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1776.305,260.5471;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Animated_Screen_Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;18;0;24;0
WireConnection;18;2;19;0
WireConnection;18;1;23;0
WireConnection;21;0;18;0
WireConnection;21;1;22;0
WireConnection;6;1;21;0
WireConnection;1;1;18;0
WireConnection;26;0;25;0
WireConnection;26;1;6;0
WireConnection;7;0;26;0
WireConnection;7;1;1;0
WireConnection;0;0;7;0
WireConnection;0;2;7;0
ASEEND*/
//CHKSM=47A161A7B8BDC22941DF58295EA1994E825B9C7D