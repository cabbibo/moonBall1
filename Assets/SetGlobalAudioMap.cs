using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGlobalAudioMap : MonoBehaviour {

  AudioListenerTexture audioT;
	// Use this for initialization
	void Start () {
		audioT = GetComponent<AudioListenerTexture>();
	}
	
	// Update is called once per frame
	void Update () {

    Shader.SetGlobalTexture("_AudioMap",audioT.AudioTexture);
		
	}
}
