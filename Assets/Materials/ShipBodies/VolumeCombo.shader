﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/VolumeCombo" {

  Properties {
    _MainTex ("Texture", 2D) = "red" {}
  
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

      CGPROGRAM

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"


      uniform int _NumberSteps;
      uniform float _TotalDepth;
      uniform float _PatternSize;
      uniform float _HueSize;
      uniform sampler2D _MainTex;

      uniform float3 _ShipPosition;
      uniform float3 _Light1;
      uniform float3 _Light2;
      uniform float3 _Light3;
      uniform float3 _Light4;
      uniform float3 _Light5;
      uniform float3 _Light6;

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
        noiseVal -= .04/ (4*r*r);
      }
      	return noiseVal;
      }
      
      VertexOut vert(VertexIn v) {
        
        VertexOut o;

        o.normal = mul( unity_ObjectToWorld , float4(v.normal,0)).xyz;

   

        o.uv = v.texcoord;
       
  
        // Getting the position for actual position
        o.pos = UnityObjectToClipPos(  v.position );
     
        float3 mPos = mul( unity_ObjectToWorld , v.position );

        // The ray origin will be right where the position is of the surface
        o.ro = mPos;


        float3 camPos = _WorldSpaceCameraPos;//mul( unity_WorldToObject , float4( _WorldSpaceCameraPos , 1. )).xyz;

        // the ray direction will use the position of the camera in local space, and 
        // draw a ray from the camera to the position shooting a ray through that point
        //o.rd = normalize( v.position.xyz - camPos );
        o.rd = normalize( mPos.xyz - camPos );

        return o;

      }

      // Fragment Shader
      fixed4 frag(VertexOut i) : COLOR {

				// Ray origin 
        float3 ro 			= i.ro;

        // Ray direction
        float3 rd 			= i.rd;      
        float3 nor = i.normal; 
        float2 uv = i.uv;

        float3 d = ro * .01;
        nor += .3*float3(-triNoise3D(d ,1,1),0,-triNoise3D(d+1000 ,1,1));
        nor = normalize(nor);

        // Our color starts off at zero,   
        float3 col = float3( 0.0 , 0.0 , 0.0 );



        float3 p;
        float hit = 0;
        for( int i = 0; i < _NumberSteps; i++ ){

          float v=   float( i ) / float(_NumberSteps);

        	// We get out position by adding the ray direction to the ray origin
        	// Keep in mind thtat because the ray direction is normalized, the depth
        	// into the step will be defined by our number of steps and total depth
          p = ro + rd * v * _TotalDepth;
  	
	
					// We get our value of how much of the volumetric material we have gone through
					// using the position
					float val = getFogVal( p );	


          if( val > .2){



            float offset = .6;
            float3 d = p - _ShipPosition;
            float val2 = getFogVal( p + normalize(d) * offset);
    
              float delta = clamp((val2 - val) / (.5*offset),0,1);
            hit = 1;
            col +=hsv( delta * _HueSize + .5 + length( d) * .01, 1, .5)* ( 1 - float(i)/_NumberSteps);
          break;
        }


        }

        if( hit < .1 ){
         // discard;
        }
        //col /=  _NumberSteps;

        //col = float3(0,0,0);

        float d1 = (100/length(ro - _Light1)*1);//max(-dot( nor , reflect( rd , normalize(ro - _Light1))),0);
        float d2 = (100/length(ro - _Light2)*1);//max(-dot( nor , reflect( rd , normalize(ro - _Light2))),0);
        float d3 = (100/length(ro - _Light3)*1);//max(-dot( nor , reflect( rd , normalize(ro - _Light3))),0);
        float d4 = (100/length(ro - _Light4)*1);//max(-dot( nor , reflect( rd , normalize(ro - _Light4))),0);
        float d5 = (100/length(ro - _Light5)*1);//max(-dot( nor , reflect( rd , normalize(ro - _Light5))),0);
        float d6 = (100/length(ro - _Light6)*1);//max(-dot( nor , reflect( rd , normalize(ro - _Light6))),0);

        col += hsv(pow(d1,1)* .3+.2,10,.001*pow(d1,10)); 
        col += hsv(pow(d2,1)* .3+.5,10,.001*pow(d2,10)); 
        col += hsv(pow(d3,1)* .3+.7,10,.001*pow(d3,10)); 
        col += hsv(pow(d4,1)* .3+.9,10,.001*pow(d4,10)); 
        col += hsv(pow(d5,1)* .3+.4,10,.001*pow(d5,10)); 
        col += hsv(pow(d6,1)* .3+.0,10,.001*pow(d6,10)); 
        //col +=hsv(d2*d2 * .2+.2,1,d2*d2); 
        //col +=hsv(d3*d3 * .2+.7,1,d3*d3); 

        //col = tex2D(_MainTex,uv).xyz;


		    fixed4 color;
        color = fixed4( col , 1. );
        return color;
      }

      ENDCG
    }
  }
  FallBack "Diffuse"
}