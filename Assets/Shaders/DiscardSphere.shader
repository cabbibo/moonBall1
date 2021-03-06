﻿Shader "Custom/DiscardSphere" {
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
    _BaseHue( "_BaseHue", Float ) = .3
    _CutoffVal( "Cutoff", Float ) = -1




  }

  SubShader {
 //Tags {"Queue"="Transparent" "RenderType"="Transparent" }

    Pass {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        //LOD 100

        //ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
      Cull Off
      CGPROGRAM

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"


      uniform int _NumberSteps;
      uniform float _TotalDepth;
      uniform float _PatternSize;
      uniform float _HueSize;
      uniform float _BaseHue;
      uniform float _CutoffVal;
      uniform sampler2D _MainTex;
      uniform sampler2D _AudioMap;

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
			    p += ( dg ) +time*.01;

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

        float dist = .03*length(pos - _ShipPosition);
        //pos *= .02;

      	float patternVal = sin( length( pos )  * _PatternSize );
      	float noiseVal =triNoise3D( pos * _PatternSize *.02, 10 , _Time.y );
    
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
      fixed4 frag(VertexOut v) : COLOR {

				// Ray origin 
        float3 ro 			= v.ro;

        // Ray direction
        float3 rd 			= v.rd;      
        float3 nor = v.normal; 
        float2 uv = v.uv;

        float3 d = ro * .01;
        nor += .6*float3(-triNoise3D(d *3,1,1),0,-triNoise3D(d*3+1000 ,1,1));
        nor = normalize(nor);

        // Our color starts off at zero,   
        float3 col = float3( 0.0 , 0.0 , 0.0 );

        float3 refl = reflect(ro - _WorldSpaceCameraPos, nor );



        float3 p;
        float hit = 0;
        float depthVal=0;
        int finalStep = 0;


        	float3 traceCol = float3(0,0,0);
        for( int i = 0; i < _NumberSteps; i++ ){

          finalStep = i;

          float v=   float( i ) / float(_NumberSteps);

        	// We get out position by adding the ray direction to the ray origin
        	// Keep in mind thtat because the ray direction is normalized, the depth
        	// into the step will be defined by our number of steps and total depth
          p = ro + rd * v * _TotalDepth;
  	
	
					// We get our value of how much of the volumetric material we have gone through
					// using the position
					float val = getFogVal( p );	


          if( val > _CutoffVal ){


           
            float offset = .6;
            float3 d = p - _ShipPosition;
            float val2 = getFogVal( p + normalize(d) * offset);
    
            float delta = clamp((val2 - val) / (.5*offset),0,1);
            hit = 1;
            depthVal = delta * .2;//  v*10 - val*2 +100* float( i %2);// val;
 						traceCol  += .4*hsv(val* _HueSize+_BaseHue,1,1);

            //col +=hsv( clamp((val+.2) * 1,0,1) * _HueSize + _Time.y * .2 + v * .2, val, 1-val) * ( 1 - float(i)/_NumberSteps);
          //break;
        }


        }
      
      
        if( hit == 0){
          discard;
        }

        col = traceCol;
      

       // float3 aCol = tex2D(_AudioMap,float2(dot(normalize(bNor),normalize(rd))*.1,0) ).xyz;
        //col = hsv( length(col)*.3 + _Time.y*.1 ,.6,.8);//col * .5 + aCol*aCol*aCol*1;// lerp( col , col * aCol*4 , length(aCol)).xyz;
        //col = saturate(traceCol);

		    fixed4 color;
        color = fixed4( col ,1);
        return color;
      }

      ENDCG
    }
  }
  FallBack "Diffuse"
}