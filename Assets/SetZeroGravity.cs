using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetZeroGravity : MonoBehaviour {

private float baseGravity;
public GO player;
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
      Physics.gravity = Vector3.zero;
      player.dragMultiplier = 0;
    }
  }
}
