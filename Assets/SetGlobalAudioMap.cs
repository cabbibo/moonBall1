using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGlobalAudioMap : MonoBehaviour {

  AudioListenerTexture audio;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioListenerTexture>();
	}
	
	// Update is called once per frame
	void Update () {

    Shader.SetGlobalTexture("_AudioMap",audio.AudioTexture);
		
	}
}
