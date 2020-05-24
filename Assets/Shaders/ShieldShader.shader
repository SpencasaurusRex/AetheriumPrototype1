Shader "Unlit/ShieldShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Tint ("Tint", Color) = (1, 1, 1, 1)
        _Falloff ("Falloff", Float) = 3
        _BorderWidth ("BorderWidth", Float) = .01
        _BorderStrength ("BorderStrength", Float) = .01
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Tint;
            float _Falloff;
            float _BorderWidth;
            float _BorderStrength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, i.uv);
                float dist = length(float2(i.uv) - float2(0.5, 0.5));
                if (dist > 0.5)
                {
                    return float4(0, 0, 0, 0);
                }
                else if (dist > 0.5-_BorderWidth)
                {
                    return float4(_Tint.r, _Tint.g, _Tint.b, _BorderStrength);
                }
                return float4(_Tint.r, _Tint.g, _Tint.b, pow(2,_Falloff-1) * pow(dist,_Falloff));
            }
            ENDCG
        }
    }
}
