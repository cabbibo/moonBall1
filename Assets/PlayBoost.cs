using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBoost : MonoBehaviour {

 
  public float pitchSpeedMultiplier;
  public float pitchSpeedBase;
  public Booster b;
  public Rigidbody rb;
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
     AudioPlayer.Instance.Play(c, rb.velocity.magnitude * pitchSpeedMultiplier + pitchSpeedBase,  .8f);
      }

	}
}
