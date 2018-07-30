using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowOnceHit : MonoBehaviour {

public GO player;
public Transform target;
public AudioClip clip;
public ShipAudio a;
public AudioSource aud;
public float lerpSpeed;

public float id; 


private Rigidbody rb;
private LineRenderer lr;
private TrailRenderer tr;
private SpringJoint sj;

private int ID;


	// Use this for initialization
	void Awake() {

    aud = GetComponent<AudioSource>();
    rb = GetComponent<Rigidbody>();
    lr = GetComponent<LineRenderer>();
		tr = GetComponent<TrailRenderer>();
  // sj = GetComponent<SpringJoint>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

    if( target != null){

      aud.volume = Mathf.Lerp(aud.volume, 1f , .1f );
      Vector3 f = transform.position - target.position;
      //rb.AddForce( -f );


      //rb.AddForce( -(transform.position - target.position)*20 );

      Vector3 fPos = target.position +  target.forward * 1 * (2+(float)ID);
      ///transform.position = Vector3.Lerp(transform.position , fPos,.5f);//
      transform.position = Vector3.Lerp(transform.position, target.position + target.TransformDirection( Vector3.up * .6f),lerpSpeed);//


     // Vector3 dif = (transform.position - player.transform.position).normalized * 4;
      Vector3 dif = (transform.position -target.position ) * .4f;
      lr.SetPosition( 0 ,target.position);
      lr.SetPosition( 1 ,target.position + dif / .4f);

      lr.startWidth = .3f;//(ID+1);
      lr.endWidth = 0f;//(ID+2);



tr.startColor =  Color.HSVToRGB(((float)ID/10) %1,.8f,1);
tr.endColor =  Color.HSVToRGB(((float)ID/10) %1,.3f,1);
tr.time =  .01f + .1f * ID;;// Color.HSVToRGB(((float)ID/10) %1,.3f,1);
tr.startWidth = .01f + .01f * (float)ID;;// Color.HSVToRGB(((float)ID/10) %1,.3f,1);

lr.endColor =  Color.HSVToRGB(((float)ID/10) %1,.8f,1);
lr.startColor =  Color.HSVToRGB((((float)ID+1)/10) %1,.8f,1);





    }else{


     // Vector3 dif = (transform.position - player.transform.position).normalized * 4;
      Vector3 dif = (transform.position - player.transform.position) * .8f;
      lr.SetPosition( 0 , player.transform.position + dif.normalized *10 );
      lr.SetPosition( 1 , player.transform.position + dif.normalized *10 + dif.normalized * (400/dif.magnitude));

      lr.startWidth = 1;
      lr.endWidth = 0;

    }
		
	}

  void OnTriggerEnter(Collider c){

  if( target == null && c.gameObject.tag == "Ship"){

  ID =  c.gameObject.GetComponent<GO>().tailsConnected;
    target = c.gameObject.GetComponent<GO>().currentTailTip;

    GetComponent<MeshRenderer>().material.SetColor("_EmissionColor" , Color.HSVToRGB(((float)ID/10) % 1,.8f,1));
    GetComponent<MeshRenderer>().material.SetColor("_Color" , Color.HSVToRGB(((float)ID/10) %1,.8f,1));


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

   GetComponent<SphereCollider>().enabled = false;
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
