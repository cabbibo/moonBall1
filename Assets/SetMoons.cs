using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMoons : MonoBehaviour {

  public Transform moon1;
  public Transform moon2;
  public Transform moon3;
  public Transform moon4;
  public Transform moon5;
  public Transform moon6;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    Shader.SetGlobalVector("_Light1",moon1.position);
    Shader.SetGlobalVector("_Light2",moon2.position);
    Shader.SetGlobalVector("_Light3",moon3.position);
    Shader.SetGlobalVector("_Light4",moon4.position);
    Shader.SetGlobalVector("_Light5",moon5.position);
    Shader.SetGlobalVector("_Light6",moon6.position);
		
	}
}
