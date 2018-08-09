using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectSquisher : MonoBehaviour {
  private connectWithLine cwl;
  private PitchSquisher ps;
  private Transform connectedTransform;
  public float magMultiplier;
  private bool squishing = false;
	// Use this for initialization
	void Start () {
    cwl = GetComponent<connectWithLine>();
    ps = GetComponent<PitchSquisher>();

		
	}
	
	// Update is called once per frame
	void Update () {

    if( cwl.connected != null  && connectedTransform == null ){
      connectedTransform = cwl.connected;

      squishing = true;

    }else if( cwl.connected == null  && connectedTransform != null ){
      connectedTransform = null;
      squishing = false;
      ps.UnSquish();
    }

    if( squishing == true && connectedTransform != null ){
      ps.Squish(magMultiplier * ( transform.position - connectedTransform.position).magnitude);


    }
		
	}
}
