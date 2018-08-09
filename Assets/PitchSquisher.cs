using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchSquisher : MonoBehaviour {

  public AudioSource audio;
  public float volMultiplier;
  public float pitchMultiplier;
  public float pitchBase;
  public float basePitch;
  public float baseVol;
  public float powPitch = 1;

  public void Squish(float val){
      audio.volume = volMultiplier *val;
      audio.pitch = Mathf.Pow( pitchMultiplier  , powPitch ) * val + pitchBase;
  }

    public void UnSquish(){
      audio.volume = baseVol; //volMultiplier *val;
      audio.pitch = basePitch;//pitchMultiplier *val + pitchBase;
  }
}
