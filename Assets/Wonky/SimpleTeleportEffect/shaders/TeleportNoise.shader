Shader "Wonky/SimpleTeleportEffect/TeleportNoise" {
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
		_Speed ("Speed", float) = 1
        _ColorMultiplier ("Color Multiplier(HDR)", float) = 1
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		Cull [_CullMode]
        ZTest [_ZTestMode]
        Blend [_SrcBlend] [_DstBlend]

		CGPROGRAM
		#pragma surface surf Lambert noambient alpha
        #pragma shader_feature _BLINKMODE_ON
        
        float _BlinkSpeed;
        
		float _Speed;
        float _ColorMultiplier;
		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) {
			float2 uvTime = float2(_Time.x, -_Time.y) * _Speed;
			fixed4 c1 = tex2D (_MainTex, IN.uv_MainTex * float2(1,0.1)+uvTime);
			fixed4 c2 = tex2D (_MainTex, IN.uv_MainTex);
			o.Emission = _Color.rgb * _ColorMultiplier;
            float alpha = _Color.a * c1.r * c2.a + c2.a*0.5;
            #ifdef _BLINKMODE_ON
            alpha *= abs(sin(_Time.z*_BlinkSpeed));
            #endif
			o.Alpha = alpha;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
