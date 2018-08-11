using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellEntity : MonoBehaviour {

  public GameObject HellPlane;
  //public GameObject 
  public AudioClip clip;
  public AudioClip followClip;

  private Rigidbody rb;

  public GO player;
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
      rb.AddForce( (player.transform.position - transform.position) * .3f );
    }
		


	}

  void OnCollisionEnter( Collision c ){

//    print( c.collider.gameObject.tag );


    if( c.collider.gameObject.tag == "Ship" && following == false){
      following = true;
     GetComponent<connectWithLine>().connected = player.gameObject.transform;
     GetComponent<TrailRenderer>().startWidth *= .04f;//connected = player.gameObject.transform;
     //GetComponent<TrailRenderer>().endWidth *= .04f;//connected = player.gameObject.transform;
      AudioPlayer.Instance.Play( followClip , 3 );
      rb.drag = 1;
      rb.mass = .1f;
      transform.localScale *= .04f;
    }
  }

}
