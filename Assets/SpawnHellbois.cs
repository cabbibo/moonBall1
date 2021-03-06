﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHellbois : MonoBehaviour {

  public GameObject HellboiPrefab;
  public int numberBois;
  public AudioClip clip;
  public float clipPitch;
  public float clipVolume;
  public List<Transform> bois;
  public GameObject HellPlane;
  public bool spawned = false;
  public GO player;





  


	// Use this for initialization
	void Start () {
		bois = new List<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void OnCollisionStay(){
      player.boostAmount += .3f;
  }

  void OnCollisionEnter( Collision c ){

    if( c.collider.gameObject.tag == "Ship" ){


      //player.boostAmount += .3f;
    if(  spawned == false){
      spawned = true; 
      print("HELL BOIIIZ");
     // bois = new Transform[numberBois];
    for( int i = 0; i < numberBois; i++ ){

      Transform boi = (Instantiate(HellboiPrefab)).transform;
      boi.position = transform.position + Random.Range(-.99f,.99f) * Vector3.left *1000+ Random.Range(-.99f,.99f)* Vector3.forward *1000  + Random.Range(0,.99f) * Vector3.up * 200;
      boi.gameObject.GetComponent<Rigidbody>().mass = Random.Range( .2f , 1.9f );
      boi.gameObject.GetComponent<Rigidbody>().velocity = Random.insideUnitSphere * 10;
      
      boi.gameObject.GetComponent<PitchSquisher>().pitchMultiplier = Random.Range( .05f , .15f );
      bois.Add(boi);  
      

    }

      HellPlane.GetComponent<Renderer>().material.SetFloat("_CutoffVal" , .3f);
      print(HellPlane.GetComponent<Renderer>().material);//.SetFloat("_Cutoff" , .3f);
      AudioPlayer.Instance.Play( clip ,clipPitch,clipVolume );
    }
  }
    
  }
}
