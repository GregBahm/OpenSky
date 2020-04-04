Shader "Unlit/WaypointIndicatorShader"
{
    Properties
    {
		_Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 worldNormal : NORMAL;
				float3 viewDir : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				o.worldNormal = mul(unity_ObjectToWorld, v.normal);
				o.viewDir = WorldSpaceViewDir(v.vertex);
                return o;
            }

			float4 _Color;

            fixed4 frag (v2f i) : SV_Target
            { 
				float3 col = normalize(i.worldNormal) * .5 + .5;
				return float4(col, 1);
            }
            ENDCG
        }
    }
}
