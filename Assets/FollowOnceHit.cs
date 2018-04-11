using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowOnceHit : MonoBehaviour {


public Rigidbody target;
public AudioClip clip;
public ShipAudio a;


private Rigidbody rb;
private LineRenderer lr;
private SpringJoint sj;

private int ID;


	// Use this for initialization
	void Start () {

    //rb = GetComponent<Rigidbody>();
		lr = GetComponent<LineRenderer>();
  // sj = GetComponent<SpringJoint>();
	}
	
	// Update is called once per frame
	void Update () {

    if( target != null){

      Vector3 f = transform.position - target.position;
      //rb.AddForce( -f );

      lr.SetPosition( 0 , transform.position);
      lr.SetPosition( 1 , target.position);

      transform.position = target.position + target.gameObject.transform.up * 1 +  target.gameObject.transform.right * 1 + target.gameObject.transform.forward * 1 * (1+(float)ID);

    }
		
	}

  void OnCollisionEnter(Collision c){

  if( target == null && c.gameObject.tag == "Ship"){



  target = c.gameObject.GetComponent<Rigidbody>();//c.gameObject.GetComponent<GO>().currentTailTip;
  c.gameObject.GetComponent<GO>().boostAmount +=1;

  a.Play(clip);

  ID =  c.gameObject.GetComponent<GO>().tailsConnected;


 // sj.connectedBody = target;
//  rb.isKinematic = false;

  transform.localScale *= .02f;


  c.gameObject.GetComponent<GO>().currentTailTip = c.gameObject.GetComponent<Rigidbody>();
   c.gameObject.GetComponent<GO>().tailsConnected += 1;
  //Destroy(gameObject);
}

  /*if( target == null && c.gameObject.tag == "Ship"){
    rb.isKinematic = false;
    target = c.rigidbody;
    }*/
  }
}
