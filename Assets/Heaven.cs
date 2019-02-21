using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heaven : MonoBehaviour {

  public float baseGravity;
  private bool inHeaven = false;
  public GO player;
 //Physics.gravity = new Vector3(0, -1.0F, 0);

	// Use this for initialization
	void Start () {
		baseGravity = Physics.gravity.y;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void OnTriggerEnter(Collider c){

if( c.gameObject.tag == "Ship"){
    print("HEAVE");

    inHeaven = true;
   // gravity = 0;
    Physics.gravity = Vector3.zero;
   // player.

    player.dragMultiplier = 0;
    player.DropAll();
  }

  }

  void OnTriggerExit(Collider c){
    if( c.gameObject.tag == "Ship"){
      Physics.gravity = new Vector3(0,baseGravity,0);
      player.dragMultiplier = 1;
    }
  }
}
