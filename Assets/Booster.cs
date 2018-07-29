using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour {
  public float amount;
  public float multiplier;

  private ParticleSystem ps;
  private TrailRenderer tr;
	// Use this for initialization
	void Awake () {
    ps = GetComponent<ParticleSystem>();
		tr = GetComponent<TrailRenderer>();
	}
	
	// Update is called once per frame
	void Update () {   

    ps.Emit((int)(multiplier * amount));
    tr.time = Mathf.Lerp( tr.time, multiplier * amount* 4 , .05f);
		
	}
}
