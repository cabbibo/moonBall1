using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchSquisher : MonoBehaviour {

  public AudioSource audio;
  public float volMultiplier;
  public float pitchMultiplier;
  public float pitchBase;
  [SerializeField] private float pitchLerp = .1f;
  [SerializeField] private float volumeLerp = .1f;
  public float targetPitch;
  public float targetVolume;
  public float basePitch;
  public float baseVol;
  public float powPitch = 1;

  public void Squish(float val){
      targetVolume = Mathf.Clamp( Mathf.Abs( volMultiplier * val ) +  .00001f  , .01f , 10 );
      targetPitch = Mathf.Pow( pitchMultiplier  , powPitch ) * val + pitchBase;
  }

    public void UnSquish(){
      targetVolume = baseVol; //volMultiplier *val;
      targetPitch = basePitch;//pitchMultiplier *val + pitchBase;
  }

 public void Update(){
  audio.volume = Mathf.Lerp( audio.volume , targetVolume , volumeLerp);
  audio.pitch = Mathf.Lerp( audio.pitch , targetPitch , pitchLerp);
 }
}
