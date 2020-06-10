Shader "Unlit/WaypointIndicatorShader"
{
    Properties
    {
		_HighBase("High Base", Color) = (1,1,1,1)
		_LowBase("Low Base", Color) = (1,1,1,1)
		_HighSpec("High Spec", Color) = (1,1,1,1)
		_LowSpec("Low Spec", Color) = (1,1,1,1)
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
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 worldNormal : NORMAL;
				float3 viewDir : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert (appdata v)
            {
                v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				o.worldNormal = mul(unity_ObjectToWorld, v.normal);
				o.viewDir = WorldSpaceViewDir(v.vertex);
                return o;
            }

			float3 _HighBase;
			float3 _LowBase;
			float3 _HighSpec;
			float3 _LowSpec;

            fixed4 frag (v2f i) : SV_Target
            { 
				i.worldNormal = normalize(i.worldNormal);
				float shade = dot(normalize(i.viewDir), i.worldNormal);
				shade = pow(shade, 2);

				float dome = i.worldNormal.y * .5 + .5;
				float3 col = lerp(_LowBase, _HighBase, dome);
				float3 spec = lerp(_LowSpec, _HighSpec, dome);

				col += spec * shade;
				return float4(col, 1);
            }
            ENDCG
        }
    }
}
