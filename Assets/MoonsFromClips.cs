using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonsFromClips : MonoBehaviour {

  public float _HeightMax;
  public AudioClip[] clips;
  public AudioPlayer audio;
  public Transform[] Moons;
  public GameObject MoonPrefab;

	// Use this for initialization
	void Awake () {

  /*  Moons = new Transform[clips.Length];

    for( int i = 0; i < clips.Length; i ++ ){

      GameObject moon = Instantiate(MoonPrefab);
      moon.name = "MOON + " + i;
      moon.transform.position = SetMoonPosition( i );

      moon.GetComponent<AudioSource>().clip = clips[i];
      moon.GetComponent<AudioSource>().Play();
      Moons[i] = moon.transform;

    }*/
		
	}

  Vector3 SetMoonPosition( int i ){
    return new Vector3(Mathf.Sin(i * 100) ,  (Mathf.Sin(i * 1000+100)+1) * _HeightMax, Mathf.Cos(i * 1000+100)) *800;
  }

	
	// Update is called once per frame
	void Update () {
		
	}


}
