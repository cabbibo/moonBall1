using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellEntity : MonoBehaviour {

  public GameObject HellPlane;

  private Rigidbody rb;
	// Use this for initialization
	void Start () {
    rb = GetComponent<Rigidbody>();
    rb.velocity = Random.insideUnitSphere * 100;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
