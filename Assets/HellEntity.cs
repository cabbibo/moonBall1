using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellEntity : MonoBehaviour {

  public GameObject HellPlane;
  //public GameObject 
  public AudioClip clip;
  public PitchSquisher squisher;
  public AudioClip followClip;

  private Rigidbody rb;

  public Transform player;
  public bool following = false;
	// Use this for initialization
	void Start () {
    rb = GetComponent<Rigidbody>();
    rb.velocity = Random.insideUnitSphere * 100;
    AudioPlayer.Instance.Play( clip );
		
	}
	
	// Update is called once per frame
	void Update () {

    if( following == true ){
//      print( "force" );

      Vector3 d = (player.position - transform.position);
      rb.AddForce( d * .3f );
      squisher.Squish( d.magnitude );
    }else{

      squisher.Squish( 0 );
    }
		


	}

  void OnCollisionEnter( Collision c ){

//    print( c.collider.gameObject.tag );


    if( c.collider.gameObject.tag == "Ship" && following == false){
      following = true;
      player = c.transform;
     GetComponent<connectWithLine>().connected = player;
     GetComponent<TrailRenderer>().startWidth *= .04f;//connected = player.gameObject.transform;
     //GetComponent<TrailRenderer>().endWidth *= .04f;//connected = player.gameObject.transform;
      AudioPlayer.Instance.Play( followClip , 3 );
      rb.drag =  Random.Range( .2f , 1.9f );;
      rb.mass =  Random.Range( .05f , .1f );
      transform.localScale *= .04f;
    }
  }

}
