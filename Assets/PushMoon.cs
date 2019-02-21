using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PushMoon : MonoBehaviour {

  public Transform Inner;

  public float pushCutoff;
  public GO player;

  private Transform insideObject;

  public AudioSource audio;

  public PitchSquisher[] pitchSquishers;

  private FollowOnceHit  follow;
  public UnityEvent onStart;


  private bool started;

	// Use this for initialization
	void Start () {
    started = false;

    if( audio == null ){ audio = GetComponent<AudioSource>(); }
    follow = GetComponent<FollowOnceHit>();
		
	}
	
	// Update is called once per frame
	void Update () {

    if( insideObject != null && started == false ){

      float distFromCenter = (transform.position - insideObject.position).magnitude / transform.localScale.magnitude;
      float val = 1- (distFromCenter*3);


      // Here is where we add the boost value!
      player.boostAmount += .4f * val;

//      print(distFromCenter);
      Inner.localScale = distFromCenter * Vector3.one * 3;

  if( val > pushCutoff ){

        player.numMoons += 1;
        if( follow != null ){
        follow.StartHit(insideObject);}
        onStart.Invoke();  
        started = true;
        for( int i = 0 ;  i< pitchSquishers.Length; i++ ){
          pitchSquishers[i].UnSquish();;
        }


      }
		
      if( started == false ){

    
      for( int i = 0 ;  i< pitchSquishers.Length; i++ ){
        pitchSquishers[i].Squish( val );
      }}
  
    }
	}

  void OnTriggerEnter( Collider c ){
    if(  c.gameObject.tag == "Ship" ){
    insideObject = c.gameObject.transform;
   } 

  }

  void OnTriggerExit( Collider c ){


    if( insideObject != null ){

      if( c.gameObject.transform == insideObject ){
      for( int i = 0 ;  i< pitchSquishers.Length; i++ ){
        pitchSquishers[i].UnSquish();
      }


      print( c.gameObject.transform );
      insideObject = null;
     // started = true;
     // Inner.gameObject.GetComponent<MeshRenderer>().enabled = false;
     // follow.StartHit( c );
    }
    }

  }
}
