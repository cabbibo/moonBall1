  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyBasePos : MonoBehaviour {

  private Vector3 bp;

  public Transform basePos;
	// Use this for initialization
	void Start () {
    bp = transform.position;

		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		  transform.position = bp+basePos.position;
	}
}
