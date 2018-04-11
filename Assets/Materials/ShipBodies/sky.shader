Shader "Custom/sky" {
	  Properties {
  
 		// This is how many steps the trace will take.
 		// Keep in mind that increasing this will increase
 		// Cost
    _NumberSteps( "Number Steps", Int ) = 3

    // Total Depth of the trace. Deeper means more parallax
    // but also less precision per step
    _TotalDepth( "Total Depth", Float ) = 0.16


    _PatternSize( "Pattern Size", Float ) = 10
    _HueSize( "Hue Size", Float ) = .3




  }

  SubShader {


    Pass {

    	Cull Front
      CGPROGRAM

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"


      uniform int _NumberSteps;
      uniform float _TotalDepth;
      uniform float _PatternSize;
      uniform float _HueSize;

      uniform float3 _ShipPosition;

      struct VertexIn{
         float4 position  : POSITION; 
         float3 normal    : NORMAL; 
         float4 texcoord  : TEXCOORD0; 
         float4 tangent   : TANGENT;
      };


      struct VertexOut {
          float4 pos    	: POSITION; 
          float3 normal 	: NORMAL; 
          float4 uv     	: TEXCOORD0; 
          float3 ro     	: TEXCOORD1;
          float3 rd     	: TEXCOORD2;
      };


			float3 hsv(float h, float s, float v){
        return lerp( float3( 1.0,1,1 ), clamp(( abs( frac(h + float3( 3.0, 2.0, 1.0 ) / 3.0 )
        					 * 6.0 - 3.0 ) - 1.0 ), 0.0, 1.0 ), s ) * v;
      }


				// Taken from https://www.shadertoy.com/view/4ts3z2
			// By NIMITZ  (twitter: @stormoid)
			// good god that dudes a genius...

			float tri( float x ){ 
			  return abs( frac(x) - .5 );
			}

			float3 tri3( float3 p ){
			 
			  return float3( 
			      tri( p.z + tri( p.y * 1. ) ), 
			      tri( p.z + tri( p.x * 1. ) ), 
			      tri( p.y + tri( p.x * 1. ) )
			  );

			}
			                                 
			float triNoise3D( float3 p, float spd , float time){
			  
			  float z  = 1.4;
				float rz =  0.;
			  float3  bp =   p;

				for( float i = 0.; i <= 3.; i++ ){
			   
			    float3 dg = tri3( bp * 2. );
			    p += ( dg );

			    bp *= 1.8;
					z  *= 1.5;
					p  *= 1.2; 
			      
			    float t = tri( p.z + tri( p.x + tri( p.y )));
			    rz += t / z;
			    bp += 0.14;

				}

				return rz;

			}

      float getFogVal( float3 pos ){

        float dist = .02*length(pos - _ShipPosition);
        pos *= .02;

      	float patternVal = sin( length( pos )  * _PatternSize );
      	float noiseVal = triNoise3D( pos , 1000 , _Time.y );

        float r = dist-.001;

        if( r < 0){
          noiseVal = 0;
          }else{
        noiseVal -= .04/ (60*r*r);
      }
      	return noiseVal;
      }
      
      VertexOut vert(VertexIn v) {
        
        VertexOut o;

        o.normal = v.normal;

        o.uv = v.texcoord;
       
  
        // Getting the position for actual position
        o.pos = UnityObjectToClipPos(  v.position );
     
        float3 mPos = mul( unity_ObjectToWorld , v.position );

        // The ray origin will be right where the position is of the surface
        o.ro = mPos;

        o.normal= mul( unity_ObjectToWorld , float4(v.normal,0) ).xyz;

        float3 camPos = _WorldSpaceCameraPos;//mul( unity_WorldToObject , float4( _WorldSpaceCameraPos , 1. )).xyz;

        // the ray direction will use the position of the camera in local space, and 
        // draw a ray from the camera to the position shooting a ray through that point
        //o.rd = normalize( v.position.xyz - camPos );
        o.rd = normalize( mPos.xyz - camPos );

        return o;

      }

      // Fragment Shader
      fixed4 frag(VertexOut v) : COLOR {

				// Ray origin 
        float3 ro 			= v.ro;

        // Ray direction
        float3 rd 			= v.rd;       


        float d = length( v.ro - _ShipPosition);

        // Our color starts off at zero,   
        float3 col = float3( 0.0 , 0.0 , 0.0 );


       // col = (v.normal * .5 + .5);// * (2+sin(d * 1.4)) /d;
        col += hsv(v.ro.y*_HueSize, 1,1);//sin(v.ro) * .5 + .5;

		    fixed4 color;
        color = fixed4( col , 1. );
        return color;
      }

      ENDCG
    }
  }
  FallBack "Diffuse"
}