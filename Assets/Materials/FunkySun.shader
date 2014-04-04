Shader "Custom/FunkySun"
{
	Properties
	{
		[PerRendererData] _MainTex ("Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1, 1, 1, 1)
		_Back ("Background", Color) = (0, 0, 0, 1)
		Value ("Value", Range(0, 1)) = 1
		Radius("Radius", Float) = 0.1
		PivotX("Pivot X", Float) = 1
		PivotY("Pivot Y", Float) = 1
		AxisX ("Direction X", Float) = 1
		AxisY ("Direction Y", Float) = 0
		RepeatX ("Repeat X", Float) = 1
		RepeatY ("Repeat Y", Float) = 1
	}

	SubShader
	{
		Tags
		{ 
			"IgnoreProjector"="True" 
			"PreviewType"="Plane"
		}
			
		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile DUMMY PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color1    : COLOR;
				fixed4 color2	: COLOR1;
				half2 texcoord  : TEXCOORD0;
				fixed pivot : TEXCOORD1;
			};
			
			fixed4 _Color;
			fixed4 _Back;
			fixed RepeatX;
			fixed RepeatY;
			//inout float PivotX;
			fixed PivotY;

			v2f vert(appdata_t IN) {
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord =	float2(
					IN.texcoord.x,// * (RepeatX / RepeatY),
					IN.texcoord.y * (RepeatY / RepeatX)
					);
				OUT.color1 = IN.color * _Color;
				OUT.color2 = IN.color * _Back;
				OUT.pivot = PivotY * (RepeatY / RepeatX);
				return OUT;
			}

			float Radius;
			float PivotX;
			//float PivotY;
			float AxisX;
			float AxisY;
			float Value;

			fixed4 frag(v2f IN) : COLOR
			{
				//float4 OUT = tex2D(_MainTex, IN.texcoord) * IN.color;
				//Direction of the pie edge
				float2 axis = normalize(float2(AxisX, AxisY));
				//Direction from origin to pixel
				float2 dir = normalize(IN.texcoord - float2(PivotX, IN.pivot));
				//The sign of z determines which half of the circle the dot product is for
				float z = normalize(cross(float3(axis, 0), float3(dir, 0)).z);
				//The dot product is the cosine of the angle between two vectors:
				float prod = dot(axis, dir);
				//Do some weird math magic to deal with angles > 180
				// prod = 0.25 * z * (1 - prod) + 0.5;
				
			//	if (z > 0) {
			//		prod = (1-prod) * 0.5 ;
			//	}
			//	else {
			//		prod = prod * 0.5 + 0.5;
			//	}
			if (distance(IN.texcoord, float2(PivotX, IN.pivot)) < Radius) {
				return IN.color1;
			} else {
			float mod = fmod(abs(prod), Value) / Value;
				//return mod * IN.color1 + (1 - mod) * IN.color2;
			//}
			
			if (fmod(abs(prod), Value) >= Value * 0.5) {
				if (z > 0) {
					return IN.color1;
				}
				else {
					return  IN.color2;
				}
			}
			else {
				if (z > 0) {
					return IN.color2;
				}
				else {
					return  IN.color1;
				}
			}}
				//What follows is equivalent to this if statement:
				//if ( Value > 0.5 && prod  > Value || Value <= 0.5 && prod >= Value) {
				//	OUT.a = 0;
				//}
				//float mod = normalize(clamp(Value-0.5, 0, 1));
				//float diff = normalize(prod - Value);
				//OUT.a = OUT.a * ((1 - clamp(diff, 0, 1) * mod
				//	+ (clamp(-diff, 0, 1) - 1) * (1-mod)));
				
				//return OUT;
			}
		ENDCG
		}
	}
}
	