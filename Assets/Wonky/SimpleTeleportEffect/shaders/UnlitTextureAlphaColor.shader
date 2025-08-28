Shader "Wonky/SimpleTeleportEffect/UnlitTextureAlphaColor" {
	Properties {
        [Header(Draw Setting)]
        [Enum(UnityEngine.Rendering.CullMode)] _CullMode("Cull Mode", float) = 2
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTestMode("ZTest", float) = 4
        //https://docs.unity3d.com/ScriptReference/Rendering.BlendMode.html
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Blend Source", float) = 5 // SrcAlpha
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Blend Destination", float) = 1 // One
        [Space(10)]
        [Header(Blink Mode)]
        [Toggle] _BlinkMode("Blink Mode", int) = 0
        _BlinkSpeed ("Blink Speed", float) = 1
        [Space(10)]
        [Header(Properties)]
        _Opacity("Opacity", Range(0,1)) = 1
        _ColorMultiplier ("Color Multiplier(HDR)", float) = 1
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		Cull [_CullMode]
        ZTest [_ZTestMode]
        Blend [_SrcBlend] [_DstBlend]
        

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
            #pragma shader_feature _BLINKMODE_ON

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};
            
            float _BlinkSpeed;
			float _Opacity;
            float _ColorMultiplier;
			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                col.rgb *= _ColorMultiplier;
                #ifdef _BLINKMODE_ON
                col.a *= _Opacity * abs(sin(_Time.z*_BlinkSpeed));
                #else
                col.a *= _Opacity;
                #endif
				return col;
			}
			ENDCG
		}
	}
}
