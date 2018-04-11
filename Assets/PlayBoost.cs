using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBoost : MonoBehaviour {

 

  public ShipAudio a;
  public Booster b;
  public AudioClip c;

	// Use this for initialization
	void Start () {

    b = GetComponent<Booster>();
		
	}
	
	// Update is called once per frame
	void Update () {
		float v = Random.Range(0,.99f);

    print(b.amount * .009f);
    if( v < b.amount * .005f ){
      print("d");
     a.Play(c, v*.3f,  .8f+ v * .3f);
      }

	}
}
