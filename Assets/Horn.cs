using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horn : MonoBehaviour
{

  public float value;
  public float oldValue;
  public AudioPlayer player;

  public AudioClip sample;
  public PitchSquisher squisher;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
          squisher.Squish(value);
         

        oldValue = value; 
    
    }
}
