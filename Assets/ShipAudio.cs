using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAudio : MonoBehaviour {


  public int bufferSize;
  private AudioSource[] buffer;
  private int currentAudio = 0;

	// Use this for initialization
	void Start () {
		
    buffer = new AudioSource[bufferSize];
    for( int i = 0; i< bufferSize; i++ ){
      buffer[i] = gameObject.AddComponent<AudioSource>();
    }
	}

  public void Play(AudioClip clip){

    buffer[currentAudio].clip = clip;
    buffer[currentAudio].Play();

    currentAudio += 1;
    currentAudio %= bufferSize;

  }

  public void Play(AudioClip clip,float volume){

    buffer[currentAudio].clip = clip;
    buffer[currentAudio].volume = volume;
    buffer[currentAudio].Play();

    currentAudio += 1;
    currentAudio %= bufferSize;

  }

   public void Play(AudioClip clip,float volume,float pitch){

    buffer[currentAudio].clip = clip;
    buffer[currentAudio].volume = volume;
    buffer[currentAudio].pitch = pitch;
    buffer[currentAudio].Play();

    currentAudio += 1;
    currentAudio %= bufferSize;

  }
	
	// Update is called once per frame
	void Update () {
		
	}
}
