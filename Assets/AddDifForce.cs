using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddDifForce : MonoBehaviour {

  public float multiplier;
  public float baseVal;
  public float maxForce;
  public float overallMult;
  public float fallOff;

  private ParticleSystem ps;
  public ShipAudio audio;
  public AudioClip clip;

  public float maxHeight;

  public Vector3 normal;
  public float dist;

  public Rigidbody parent;
  public Terrain terrain;

	// Use this for initialization
	void Start () {
		
    ps = GetComponent<ParticleSystem>();


	}
	
	// Update is called once per frame
	void FixedUpdate () {

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {


        float dif =  hit.distance;
        dist = dif;
        normal = transform.TransformDirection(Vector3.down);

        float newDif = (dif-baseVal);

        if( newDif < maxHeight ){

          float forceVal= overallMult * multiplier/ Mathf.Pow( newDif , fallOff);
        //  ps.Emit( (int)(forceVal*.01f));

          Vector3 f = forceVal * hit.normal;

          if( f.magnitude > maxForce ){
            f = f.normalized * maxForce;
          }

          float velAdder =.7f;// Mathf.Clamp( .04f * parent.velocity.magnitude , 0, 1);
          parent.AddForceAtPosition( f , transform.position );
          //GetComponent<Rigidbody>().AddForce( f  );
        }
      



        }

/*       float h = terrain.SampleHeight(transform.position);

      float eps = .01f;


      Vector3 left = transform.position + Vector3.left * eps;
      Vector3 right = transform.position - Vector3.left * eps;

      Vector3 up = transform.position + Vector3.forward * eps;
      Vector3 down = transform.position - Vector3.forward * eps;

      float hL = terrain.SampleHeight(left);
      float hR = terrain.SampleHeight(right);
      float hU = terrain.SampleHeight(up);
      float hD = terrain.SampleHeight(down);



      left  = new Vector3( left.x   , hL , left.z   );
      right = new Vector3( right.x  , hR , right.z  );
      up    = new Vector3( up.x     , hU , up.z     );
      down  = new Vector3( down.x   , hD , down.z   );

      
      Vector3 nor = Vector3.Cross( (left-right).normalized,(up-down).normalized );


      float dif =  transform.position.y-h;

      float newDif = (dif-baseVal);

      if( newDif < maxHeight ){

        Vector3 f = overallMult * multiplier * nor / Mathf.Pow(newDif , .5f);

        if( f.magnitude > maxForce ){
          f = f.normalized * maxForce;
        }

        float velAdder =.7f;// Mathf.Clamp( .04f * parent.velocity.magnitude , 0, 1);
        parent.AddForceAtPosition( f * (.3f + velAdder) , transform.position );
        //GetComponent<Rigidbody>().AddForce( f  );
      }
		*/

	}


  public void EmitParticles(float collisionStrength){

//    ps.Emit((int)(10 * collisionStrength));

    audio.Play( clip ,collisionStrength* .1f, collisionStrength);
  }

  void OnCollisionEnter( Collision c ){

  }


}
