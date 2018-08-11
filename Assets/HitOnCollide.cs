using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOnCollide : MonoBehaviour {

  public AudioClip clip;
  public int[] clipSteps;
  public float volume;
  public float falloff;
//  public float clipMultiplier;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void OnCollisionEnter(){

    int step = clipSteps[Random.Range(0,clipSteps.Length)];
    AudioPlayer.Instance.Play( clip , step , volume , transform.position, falloff );

  }
}
