using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalSphere : MonoBehaviour {

private float baseGravity;
public GO player;
public bool madeIt = false;
  void Start () {
    baseGravity = Physics.gravity.y;
  }
  

  void OnTriggerEnter(Collider c){

    if( c.gameObject.tag == "Ship"){
      Physics.gravity = new Vector3(0,baseGravity,0);
      player.dragMultiplier = 1;
    }

  }

  void OnTriggerExit(Collider c){

    if( c.gameObject.tag == "Ship"){
      print("MADDDEEITITITI");
      madeIt = true;
      Physics.gravity = Vector3.zero;
      player.dragMultiplier = 0;
      player._AirBoostMULT = 40;
      GetComponent<AudioSource>().Play();
    }
  }

  void Update(){
    if( madeIt == true ){
      player.boostAmount += 10;
    }
  }
}
