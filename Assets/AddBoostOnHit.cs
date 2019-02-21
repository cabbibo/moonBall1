using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBoostOnHit : MonoBehaviour
{

  public GO player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision c){
      player.boostAmount += .3f;
    }
}
