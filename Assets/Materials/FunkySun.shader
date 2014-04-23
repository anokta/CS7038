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
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				half4 vertex   : POSITION;
				half2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				half4 vertex   : SV_POSITION;
				half2 texcoord  : TEXCOORD0;
				half2 pivot : TEXCOORD1;
				half2 axis : TEXCOORD2;
			};
			
			
			fixed RepeatX;
			fixed RepeatY;
			//inout float PivotX;
			half AxisX;
			half AxisY;
			half PivotY;
			half PivotX;
			v2f vert(appdata_t IN) {
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord =	float2(
					IN.texcoord.x,// * (RepeatX / RepeatY),
					IN.texcoord.y * (RepeatY / RepeatX)
					);
				OUT.pivot.y = PivotY * (RepeatY / RepeatX);
				OUT.pivot.x = PivotX;
				OUT.axis = normalize(half2(AxisX, AxisY));
				return OUT;
			}

			half Radius;
			//float PivotY;
			
			half Value;
			fixed4 _Color;
			fixed4 _Back;

			fixed4 frag(v2f IN) : COLOR
			{
				//float4 OUT = tex2D(_MainTex, IN.texcoord) * IN.color;
				//Direction of the pie edge
				//half2 axis = 
				//Direction from origin to pixel
				half2 dir = normalize(IN.texcoord - IN.pivot);
				//The sign of z determines which half of the circle the dot product is for
				half z = normalize(cross(half3(IN.axis, 0), half3(dir, 0)).z);
				//The dot product is the cosine of the angle between two vectors:
				half prod = dot(IN.axis, dir);
				//Do some weird math magic to deal with angles > 180
				// prod = 0.25 * z * (1 - prod) + 0.5;
				
			//	if (z > 0) {
			//		prod = (1-prod) * 0.5 ;
			//	}
			//	else {
			//		prod = prod * 0.5 + 0.5;
			//	}
				if (distance(IN.texcoord, IN.pivot) < Radius) {
					return _Color;
				} else {
				//float mod = fmod(abs(prod), Value) / Value;
					//return mod * IN.color1 + (1 - mod) * IN.color2;
				//}
				
					if (fmod(abs(prod), Value) >= Value * 0.5) {
						if (z > 0) {
							return _Color;
						}
						else {
							return  _Back;
						}
					}
					else {
						if (z > 0) {
							return _Back;
						}
						else {
							return  _Color;
						}
					}
				}
			}
		ENDCG
		}
	}
}
	