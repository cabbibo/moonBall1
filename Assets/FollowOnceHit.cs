﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowOnceHit : MonoBehaviour {

public GO player;
public Transform target;
public AudioClip clip;
public AudioPlayer a;
public AudioSource aud;
public float lerpSpeed;
public Material onceFollowMaterial;

public float id; 
public float[] steps;//[0,2,3,5]
public bool upPitch;


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

    if( upPitch == false ){
      aud.volume = 0;
    }
  // sj = GetComponent<SpringJoint>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

    if( target != null){

      aud.volume = Mathf.Lerp(aud.volume, 1f , .1f );
      Vector3 f = transform.position - target.position;
      //rb.AddForce( -f );


      //rb.AddForce( -(transform.position - target.position)*20 );

      Vector3 fPos = player.transform.position + player.transform.TransformDirection( Vector3.up * (1+.6f * (float)ID));// +  target.forward * 1 * (2+(float)ID);
      ///transform.position = Vector3.Lerp(transform.position , fPos,.5f);//
      float fLerp = lerpSpeed / (2.3f*(float)ID+1);
      transform.position = Vector3.Lerp(transform.position, fPos ,fLerp);//


     // Vector3 dif = (transform.position - player.transform.position).normalized * 4;
      Vector3 dif = (transform.position -target.position ) * .4f;
      lr.SetPosition( 0 ,target.position);
      lr.SetPosition( 1 ,target.position + dif / .4f);

      lr.startWidth = .3f;//(ID+1);
      lr.endWidth = 0f;//(ID+2);



tr.startColor =  Color.HSVToRGB(((float)ID/10) %1,.8f,1);
tr.endColor =  Color.HSVToRGB(((float)ID/10) %1,.3f,1);
tr.time =  .5f + .1f * ID;;// Color.HSVToRGB(((float)ID/10) %1,.3f,1);
tr.startWidth = .01f+ .01f * (float)ID;;// Color.HSVToRGB(((float)ID/10) %1,.3f,1);

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

  public void End(){
    target = null;
    aud.volume = 0;
        a.Play(clip,.6f,10);
  }

  public void StartHit(Transform c){


    if( target == null  ){
      ID = player.tailsConnected;
      target =player.currentTailTip;
      GetComponent<MeshRenderer>().material = onceFollowMaterial;
      GetComponent<MeshRenderer>().material.SetColor("_EmissionColor" , Color.HSVToRGB(((float)ID/10) % 1,.8f,1));
      GetComponent<MeshRenderer>().material.SetColor("_Color" , Color.HSVToRGB(((float)ID/10) %1,.8f,1));


    //target = c.gameObject.GetComponent<Rigidbody>();//c.gameObject.GetComponent<GO>().currentTailTip;
   player.boostAmount +=1;

    a.Play(clip,.6f,10);

    if( upPitch == true ){
      aud.pitch =  Mathf.Pow( 1.05946f,steps[Random.Range(0,steps.Length)]);//Elements[Random.Range(0,Elements.Length)];
    
    aud.Play();
    }else{
      aud.pitch = 1;
      aud.volume = 1;
    }
  /*
  rb.drag = Random.Range(1,3.99f);
  rb.mass = Random.Range(1,3.99f);
  */


   // sj.connectedBody = target;
  //  rb.isKinematic = false;

    transform.localScale *= .003f;

     GetComponent<SphereCollider>().enabled = false;
    player.currentTailTip = transform;
     player.tailsConnected += 1;
     player.tails.Add( this.gameObject );

   }

  }
  void OnTriggerEnter(Collider c){

  }
}
