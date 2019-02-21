using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeHitColor : MonoBehaviour
{


    public HitColorShift shift;
    public Color startColor;
    public Color endColor;

    public float minHue;
    public float maxHue;
    // Start is called before the first frame update
    void Start()
    {
        if( shift == null ){ shift = gameObject.GetComponent<HitColorShift>();}
      shift.shiftTime = Random.Range( 0.1f , 1.5f );
      shift.endColor = Random.ColorHSV(minHue,maxHue, 1f, 1f,1, 1f);
      shift.startColor = Random.ColorHSV(minHue,maxHue, .5f, 1f, 1  , 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
