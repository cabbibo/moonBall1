using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpUp : MonoBehaviour {


  private bool onGround = false;

  public Rigidbody rb;
  public float xForce;
  public float baseForce;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {


      if( onGround == true ){
      rb.AddForce( baseForce * Vector3.up * (1 +xForce* Input.GetAxis("X")));
    }
    
		
	}

    void OnTriggerEnter(Collider c){

    if( c.tag == "Ground" ){ 
//      print("ya"); 

      onGround = true;

    };//else{ print("na");}

  }

  void OnTriggerExit(Collider c){

    if( c.tag == "Ground" ){ 
//      print("ya"); 

      onGround = false;

    }//else{ print("na");}

  }
}
