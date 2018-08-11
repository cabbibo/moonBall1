using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Hook : MonoBehaviour {

  public float angle;
  public float angleUp;
  public float angleMultiplier;
  public float angleUpMultiplier;
  public float anglePow;

  private GO ship;

  public float distanceMultiplier;
  public float lerpPosition;

  public GameObject pointer;
  public GameObject LockObj;
  public GameObject aimer;
  public connectWithLine cwl;


  public float pointerRadius;
  private Player player;

  public bool lockDown = false;

private bool canThrow = true;
  public Vector3 lockPos;
  public Vector3 aimPos;
  //public Vector3 aimPos;
  public float aimAngle;
  public float aimAngleUp;

public Vector3 baseLockPos;
	// Use this for initialization
	void Start () {
		player = ReInput.players.GetPlayer(0);
    ship = GetComponent<GO>();
	}
	
	// Update is called once per frame
	void Update () {
		UpdatePointer();
	}

  void FixedUpdate(){

    UpdatePointer();
    if( player.GetAxis("[]") == 0 ){

  
      if( lockDown == true ){
        PickupLock();
      }
      canThrow = true;
    }else{
      if( lockDown == false && canThrow == true ){
        ThrowLock();
      }
    }

    if( lockDown == true ){
      LockObj.transform.position = lockPos;
      cwl.connected = LockObj.transform;

        Vector3 delta =  ship.transform.position - lockPos;
        ship.rb.AddForceAtPosition(  -delta.normalized*1000* ship._LockMULT  , pointer.transform.position );

          if( delta.magnitude < ship._LockHitDist ){

            canThrow = false;
            cwl.connected = null;

            lockDown = false;

          }


    }else{
       cwl.connected = null;
    }








  }

  Vector3 posFromAngles( float a1 , float a2 ){

    a1 = Mathf.Pow(a1,anglePow);
    Vector3 up = transform.up;
    Vector3 right = transform.right;
    Vector3 forward = transform.forward;
    return transform.position -(forward * Mathf.Cos( a1)  + right * -Mathf.Sin( a1)  + up *Mathf.Sin( a2 ) ).normalized * pointerRadius ;
 
  }


  void UpdatePointer(){

    print(player.GetAxis("JoystickLeftX") );
   
    aimAngle =  player.GetAxis("JoystickLeftX") * Mathf.PI * angleMultiplier;
    aimAngleUp =  player.GetAxis("JoystickLeftY") * Mathf.PI * angleUpMultiplier;
   // angle = Mathf.Lerp( angle, aimAngle , lerpPosition);

    aimPos =  posFromAngles( aimAngle , aimAngleUp ); //transform.position - dir;

    aimer.transform.position = aimPos;
    pointer.transform.position =  posFromAngles( angle , angleUp );  
   }

    void ThrowLock(){

      lockDown = true;
      angle = aimAngle;
      angleUp = aimAngleUp;
      pointer.transform.position  =  posFromAngles( angle , angleUp ); 
      baseLockPos = (pointer.transform.position - transform.position );
      lockPos = (pointer.transform.position - transform.position ) * distanceMultiplier + transform.position;

    }

    void PickupLock(){

      lockDown = false;

      lockPos = new Vector3(100000000,0,0);//(pointer.transform.position - transform.position ) * 10 + transform.position;

    }
}
