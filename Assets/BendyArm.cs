using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BendyArm : MonoBehaviour {


  public Transform Center;
  public Rigidbody Parent;

  public Vector3 ogPos;

  private LineRenderer lr;
	// Use this for initialization
	void Start () {
		lr = GetComponent<LineRenderer>();
    ogPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

    lr.SetPosition(0, transform.position);
    lr.SetPosition(1, Center.transform.position);


		
	}
}
