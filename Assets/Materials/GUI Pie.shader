Shader "GUI/Pie"
{
	Properties
	{
		[PerRendererData] _MainTex ("Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1, 1, 1, 1)
		[MaterialToggle] Clockwise ("Clockwise", Float) = 1
		Value ("Value", Range(0, 1)) = 0
		AxisX ("Direction X", Float) = 0
		AxisY ("Direction Y", Float) = -1
		PivotX ("Pivot X", Float) = 0.5
		PivotY ("Pivot Y", Float) = 0.5
	}

	SubShader
	{
		Tags
		{ 
			"ForceSupported" = "True"
			"RenderType" = "Overlay" 
			"PreviewType"="Plane"
		}
			
		Lighting Off 
		Blend SrcAlpha OneMinusSrcAlpha 
		Cull Off 
		ZWrite Off 
		Fog { Mode Off } 
		ZTest Always

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				half2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				half2 axis : TEXCOORD1;
				half2 pivot : TEXCOORD2;
			};
			
			fixed4 _Color;
			half AxisX;
			half AxisY;
			half PivotX;
			half PivotY;

			v2f vert(appdata_t IN) {
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				//OUT.vertex = UnityPixelSnap (OUT.vertex);
				OUT.axis = normalize(half2(AxisX, AxisY));
				OUT.pivot = half2(PivotX, PivotY);
				return OUT;
			}

			sampler2D _MainTex;

			half Value;
			half Clockwise;

			fixed4 frag(v2f IN) : COLOR
			{
				fixed4 OUT = tex2D(_MainTex, IN.texcoord) * IN.color;
				//Direction of the pie edge
				//float2 axis = normalize(float2(AxisX, AxisY));
				//Direction from origin to pixel
				half2 dir = normalize(IN.texcoord - IN.pivot);
				//The sign of z determines which half of the circle the dot product is for
				half z = normalize(cross(half3(IN.axis, 0), half3(dir, 0)).z);
				//The dot product is the cosine of the angle between two vectors:
				half prod = dot(IN.axis, dir);
				//Do some weird math magic to deal with angles > 180
				prod = 0.25 * z * (1 - prod) + 0.5;
				//Apply clockwise modifier
				prod = Clockwise * (1 - 2 * prod) + prod;
				
				if ( Value > 0.5 && prod  > Value || Value <= 0.5 && prod >= Value) {
					OUT.a = 0;
				}
				//float mod = normalize(clamp(Value-0.5, 0, 1));
				//float diff = normalize(prod - Value);
				//OUT.a = OUT.a * ((1 - clamp(diff, 0, 1) * mod
				//	+ (clamp(-diff, 0, 1) - 1) * (1-mod)));

				return 2 * OUT;
			}
		ENDCG
		}
	}
}
	