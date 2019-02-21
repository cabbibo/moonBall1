// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "TAIL"
{
    Properties
    {
        // we have removed support for texture tiling/offset,
        // so make them not be displayed in material inspector
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            // use "vert" function as the vertex shader
            #pragma vertex vert
            // use "frag" function as the pixel (fragment) shader
            #pragma fragment frag


            // vertex shader inputs
            struct appdata
            {
                float4 vertex : POSITION; // vertex position
                float3 normal : NORMAL; // vertex position
                float4 tangent : TANGENT; // vertex position
                float2 uv : TEXCOORD0; // texture coordinate
            };

            // vertex shader outputs ("vertex to fragment")
            struct v2f
            {
                float2 uv : TEXCOORD0; // texture coordinate
                float4 vertex : SV_POSITION; // clip space position
            };

            // texture we will sample
            sampler2D _AudioMap;
            // vertex shader
            v2f vert (appdata v)
            {
                v2f o;
                // transform position to clip space
                // (multiply with model*view*projection matrix)
                float4 aVal = tex2Dlod(_AudioMap,float4( v.uv.x, 0,0,0));
                float3 fVert = v.vertex.xyz;// - ( v.tangent * (v.uv.y -.5) * 10 *(length(aVal)-.1)) ;
                o.vertex = UnityObjectToClipPos(float4(fVert,1));
                // just pass the texture coordinate
                o.uv = v.uv;
                return o;
            }
            

            // pixel shader; returns low precision ("fixed4" type)
            // color ("SV_Target" semantic)
            fixed4 frag (v2f i) : SV_Target
            {
                // sample texture and return it
                fixed4 col = tex2D(_AudioMap, i.uv);
                if( abs(i.uv.y-.5) > .46 ){ col = float4(1,1,1,1);}
                return col;
            }
            ENDCG
        }
    }
}