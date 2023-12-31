Shader "Custom/ForwardShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" { }
        _Speed ("Speed", Range(0.1, 10)) = 1.0
        _Alpha ("Alpha", Range(0.0, 1.0)) = 1.0
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        fixed4 _Color;
        float _Speed;
        float _Alpha;

        struct Input {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o) {
            // Texture'dan renk alın
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

            // Zamanı kullanarak sadece ileri doğru hareket ettirme (z ekseni boyunca)
            float time = _Time.y * _Speed;

            // Yeni UV koordinatlarını oluşturun (z ekseni boyunca hareket)
            float2 uv = IN.uv_MainTex;
            uv.y += time;

            // Hareket ettirilmiş UV'den renk alın
            fixed4 finalColor = tex2D(_MainTex, uv);

            // Son renk
            o.Albedo = finalColor.rgb;
            
            // Alpha değerini kullanıcı tanımlı uniform'dan alın
            o.Alpha = _Alpha;
        }
        ENDCG
    }
}
