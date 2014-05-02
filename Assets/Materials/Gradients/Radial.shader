Shader "Custom/Radial" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		CenterX("Center X", Float) = 1
		CenterY("Center Y", Float) = 0
	}
	SubShader {
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

		struct appdata_t
		{
			half4 vertex   : POSITION;
			half4 color    : COLOR;
			half2 texcoord : TEXCOORD0;
		};

		struct v2f
		{
			half4 vertex   : SV_POSITION;
			fixed4 color    : COLOR;
			half2 texcoord  : TEXCOORD0;
		};
		
		fixed4 _Color;

		v2f vert(appdata_t IN)
		{
			v2f OUT;
			OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
			OUT.texcoord = IN.texcoord;
			OUT.color = IN.color * _Color;
			OUT.vertex = UnityPixelSnap (OUT.vertex);

			return OUT;
		}

		sampler2D _MainTex;
		float CenterX;
		float CenterY;

		fixed4 frag(v2f IN) : COLOR
		{
			half4 OUT = tex2D(_MainTex, IN.texcoord) * IN.color;
			half d = clamp(1 - distance(IN.texcoord.xy, half2(CenterX, 1-CenterY)), 0, 1);
			OUT.a *= d;
			//OUT.a = sqrt(OUT.a);
			OUT.a *= OUT.a;
			return OUT;
		}
		ENDCG
		} 
	}
}
