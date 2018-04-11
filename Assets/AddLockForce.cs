using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLockForce : MonoBehaviour {


  public Rigidbody parent;
  public Vector3 lockPos;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

   if( Input.GetAxis("[]") == 0 ){
     lockPos = new Vector3(0,0,0);
   }  

   Vector3 delta = transform.position - lockPos;
    if( lockPos.magnitude != 0 ){
      parent.AddForceAtPosition( delta *1 ,transform.position);
    }
		
	}

  void OnCollisionEnter(Collision c){

    print( "HEEE");

     if( lockPos.magnitude == 0 ){

    if( Input.GetAxis("[]") > 0 ){

      lockPos = c.contacts[0].point;
    }

  }

  }


  void OnCollisionExit(Collision c){


  }

}
