using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddDifForce : MonoBehaviour {

  public float multiplier;
  public float baseVal;
  public float maxForce;
  public float overallMult;
  public float fallOff;
  public float upOrDown;

  private ParticleSystem ps;
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

	}


  public void EmitParticles(float collisionStrength){

   //ps.Emit((int)(100 * collisionStrength));

    AudioPlayer.Instance.Play( clip , 10 *collisionStrength, 1);//collisionStrength);
  }

  void OnCollisionEnter( Collision c ){

  }


}
