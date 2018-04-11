using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowOnceHit : MonoBehaviour {


public Rigidbody target;
public AudioClip clip;
public ShipAudio a;
private Rigidbody rb;
private LineRenderer lr;
	// Use this for initialization
	void Start () {

    rb = GetComponent<Rigidbody>();
		lr = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

    if( target != null){

      Vector3 f = transform.position - target.position;
      rb.AddForce( -f );

      lr.SetPosition( 0 , transform.position);
      lr.SetPosition( 1 , target.position);

    }
		
	}

  void OnCollisionEnter(Collision c){

  if( c.gameObject.tag == "Ship"){
  c.gameObject.GetComponent<GO>().boostAmount +=1;
  a.Play(clip);
  Destroy(gameObject);
}

  /*if( target == null && c.gameObject.tag == "Ship"){
    rb.isKinematic = false;
    target = c.rigidbody;
    }*/
  }
}
