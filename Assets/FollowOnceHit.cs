using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowOnceHit : MonoBehaviour {


public Transform target;
public AudioClip clip;
public ShipAudio a;

public float id; 


private Rigidbody rb;
private LineRenderer lr;
private SpringJoint sj;

private int ID;


	// Use this for initialization
	void Start () {

    rb = GetComponent<Rigidbody>();
		lr = GetComponent<LineRenderer>();
  // sj = GetComponent<SpringJoint>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

    if( target != null){

      Vector3 f = transform.position - target.position;
      //rb.AddForce( -f );

      lr.SetPosition( 0 , transform.position);
      lr.SetPosition( 1 , target.position);

      //rb.AddForce( -(transform.position - target.position)*20 );

      Vector3 fPos = target.position +  target.forward * 1 * (2+(float)ID);
      ///transform.position = Vector3.Lerp(transform.position , fPos,.5f);//
      transform.position = Vector3.Lerp(transform.position, target.position,.1f);//


    }
		
	}

  void OnTriggerEnter(Collider c){

  if( target == null && c.gameObject.tag == "Ship"){

  ID =  c.gameObject.GetComponent<GO>().tailsConnected;
    target = c.gameObject.GetComponent<GO>().currentTailTip;

    GetComponent<MeshRenderer>().material.SetColor("_EmissionColor" , Color.HSVToRGB(((float)ID/10) % 1,.8f,1));
    GetComponent<MeshRenderer>().material.SetColor("_Color" , Color.HSVToRGB(((float)ID/10) %1,.8f,1));


GetComponent<TrailRenderer>().startColor =  Color.HSVToRGB(((float)ID/10) %1,.8f,1);
GetComponent<TrailRenderer>().endColor =  Color.HSVToRGB(((float)ID/10) %1,.8f,0);
  //target = c.gameObject.GetComponent<Rigidbody>();//c.gameObject.GetComponent<GO>().currentTailTip;
  c.gameObject.GetComponent<GO>().boostAmount +=1;

  a.Play(clip,10);
/*
rb.drag = Random.Range(1,3.99f);
rb.mass = Random.Range(1,3.99f);
*/


 // sj.connectedBody = target;
//  rb.isKinematic = false;

  transform.localScale *= .02f;


//GetComponent<SphereCollider>().isTrigger = false;
  c.gameObject.GetComponent<GO>().currentTailTip = transform;
   c.gameObject.GetComponent<GO>().tailsConnected += 1;
  //Destroy(gameObject);
}

  /*if( target == null && c.gameObject.tag == "Ship"){
    rb.isKinematic = false;
    target = c.rigidbody;
    }*/
  }
}
