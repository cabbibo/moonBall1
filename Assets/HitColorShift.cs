using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitColorShift : MonoBehaviour {

  public string colorName;
  public float shiftTime;

  public Color startColor;
  public Color endColor;
  public float hitTime;

  public Color currentColor;


  private Renderer renderer;
	// Use this for initialization
	void Start () {
		renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {



    float fTime = 1-Mathf.Clamp((Time.time-hitTime) / shiftTime , 0 , 1);
    currentColor = Color.Lerp( startColor , endColor , fTime );

    renderer.material.SetColor(colorName,currentColor);

		
	}

  void OnCollisionEnter(Collision c){
    print("hmmm");

    hitTime = Time.time;

  }
}
