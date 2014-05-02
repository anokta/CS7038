Shader "Custom/Linear" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		//[MaterialToggle] PixelSnap ("Pixel snap", Float) = 1
		_Color ("Tint", Color) = (1,1,1,1)
		DirectionX("Direction X", Float) = 1
		DirectionY("Direction Y", Float) = 0
		[MaterialToggle] Inverted("Invert", Float) = 1
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
			float4 color    : COLOR;
			half2 texcoord : TEXCOORD0;
		};

		struct v2f
		{
			half4 vertex   : SV_POSITION;
			half4 color    : COLOR;
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
		half DirectionX;
		half DirectionY;
		half Inverted;

		fixed4 frag(v2f IN) : COLOR
		{
			half4 OUT = tex2D(_MainTex, IN.texcoord) * IN.color;
			half2 dir = normalize(half2(DirectionX, DirectionY));
			half d = dot(dir, IN.texcoord.xy);
			//if (d < 0 && dir.x > 0 || d <= 0 && dir.x <= 0) { d = 1 + d; }
			d = clamp(d, 0, 1);
			if (Inverted) { d = 1 - d; }
			
			OUT.a *= d;
			OUT.a *= OUT.a;
			
			//OUT.a = sqrt(OUT.a);
			return OUT;
		}
		ENDCG
		} 
	}
}
