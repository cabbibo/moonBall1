using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class connectWithLine : MonoBehaviour {

  public Transform connected;

  private LineRenderer lr;

	// Use this for initialization
	void Start () {
		lr = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
    lr.SetPosition( 0 , transform.position );
    if( connected != null ){
       lr.SetPosition( 1 ,connected.position );
    }else{
      lr.SetPosition( 1 , transform.position );
    }
	}
}
