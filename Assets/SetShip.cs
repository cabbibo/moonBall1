using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetShip : MonoBehaviour {
  public Transform ship;
  private Material m;

	// Use this for initialization
	void Start () {
    if( GetComponent<MeshRenderer>() != null){
    m = GetComponent<MeshRenderer>().sharedMaterial;
    }else{
		m = GetComponent<Terrain>().materialTemplate;
  }
	}
	
	// Update is called once per frame
	void Update () {

    Shader.SetGlobalVector("_ShipPosition", ship.position );
	//	m.SetVector("_ShipPosition", ship.position );
	}
}
