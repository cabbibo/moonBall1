using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBoost : MonoBehaviour {

 

  public ShipAudio a;
  public Booster b;
  public AudioClip c;
  public float soundChance;

	// Use this for initialization
	void Start () {

    b = GetComponent<Booster>();
		
	}
	
	// Update is called once per frame
	void Update () {
		float v = Random.Range(0,.99f);

    if( v < b.amount * .001f * soundChance ){
      //print("d");
     a.Play(c, 10.2f,  .8f);
      }

	}
}
