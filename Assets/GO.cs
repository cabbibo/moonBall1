using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GO : MonoBehaviour {


  public GameObject hookPoint;
  public Camera cam;


  public float boostAmount;

  public float suspension;
  public float _MaxSuspensionHeight;

  public float _BoostMULT;
  public float _LerpSpeed;
  public float _RotMULT;
  public float _JumpMULT;
  public float _AirRotMULT;
  public float _AirBoostMULT;
  public float _LockMULT;
  public float _AirRotDrag;
  public float _AirRollBoost;
  public float _GroundRotDrag;
  public float _LockThrowDistance;
  public float _LockThrowDistanceFront;
  public float _LockThrowHeight;

  public GameObject LockPuller;

  public float _LockHitDist;
  public AudioClip lockHitClip;
  public bool canThrow = true;

  public float dragBaseAmount;

  public Terrain terrain;
  private Rigidbody rb;

  private Vector3 x;
  private Vector3 y;

  public bool onGround;
  public GameObject rudder;

  public GameObject wingLeft;
  public GameObject wingRight;
  public GameObject front;
  public GameObject back;
  public GameObject body;
  public GameObject boosterR;
  public GameObject boosterL;
  public GameObject megaBoostL;
  public GameObject megaBoostR;
  public GameObject megaBoostAmountL;
  public GameObject megaBoostAmountR;
  public float MaxMegaBoost;



  public Transform currentTailTip;

  public Transform lockedObject;
  public Vector3 lockPos;

  public GameObject cameraHolder;

  private float startDrag;
  private connectWithLine cwl;

  public int tailsConnected = 0;
  public float lockStartTime;
	// Use this for initialization
	void Start () {
    rb = GetComponent<Rigidbody>();

    currentTailTip = body.transform;//back.transform;
    onGround = false;

    startDrag = rb.drag;
    cwl = hookPoint.GetComponent<connectWithLine>();
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

    cam.fieldOfView = Mathf.Lerp( cam.fieldOfView , 60 + Mathf.Clamp( rb.velocity.magnitude , 0 , 60), .1f );

   if( Input.GetAxis("[]") == 0 ){
     lockedObject = null;
     cwl.connected = null;
     canThrow = true;

   }else{

    if( lockedObject == null ){

    if( canThrow == true ){
      ThrowLock();
    
    }

     /* transform.left *  ( Input.GetAxis("LeftStickX")
        lockPos = c.contacts[0].point;
      lockedObject = wingLeft.transform;


      rb.AddTorque( transform.forward *  Input.GetAxis("LeftStickX") *  4000 * Input.GetAxis("[]"));*/

    }

   }


    /*
      Do our lock stuff;

      LOCK

    */

    if( lockedObject != null){

      cwl.connected = lockedObject;
      cwl.connected = LockPuller.transform;
     // lockedObject 
      //rb.drag = 10;


      Vector3 position = Vector3.Lerp( body.transform.position ,lockPos , .5f);


      LockPuller.transform.position = position;

      Vector3 delta = position - lockPos;

      float timeLocked = Time.time - lockStartTime;
      timeLocked *= 3;
      timeLocked = Mathf.Clamp( timeLocked , 0, 1);
     // rb.AddForceAtPosition( -delta *30* _MULT * timeLocked ,lockedObject.position);
    //  rb.

      rb.AddForceAtPosition(  -delta.normalized*1000* _LockMULT * timeLocked ,position );
      hookPoint.transform.localScale = new Vector3( _LockHitDist,_LockHitDist,_LockHitDist);
      hookPoint.transform.position = lockPos;

      if( delta.magnitude < _LockHitDist ){

        GetComponent<ShipAudio>().Play( lockHitClip );
        lockedObject = null;
        canThrow = false;
        cwl.connected = null;
        hookPoint.transform.position = lockPos;

      }

    }else{


      LockPuller.transform.position = Vector3.Lerp( LockPuller.transform.position , body.transform.position , .1f);
      hookPoint.transform.position = Vector3.left * 10000;
      //rb.drag = startDrag;
    }

    cameraHolder.GetComponent<Rigidbody>().AddTorque(   Vector3.up * Input.GetAxis("RightStickX") * 80);
 

    if( onGround == true ){


      rb.angularDrag = _GroundRotDrag;

     RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
          


        float dif =  hit.distance;

        Vector3 tangent = Vector3.Cross( transform.forward , rb.velocity ).normalized;
        Vector3 groundForward = Vector3.Cross( tangent , hit.normal ).normalized;


      //Vector3 velForward = -groundForward.normalized * rb.velocity.magnitude;
      Vector3 velForward = -transform.forward * rb.velocity.magnitude;
      float z = rb.velocity.z;
      float x = rb.velocity.x;
      float y = rb.velocity.y;
     
      x = Mathf.Lerp( x , velForward.x , _LerpSpeed );
      z = Mathf.Lerp( z , velForward.z , _LerpSpeed );
      rb.velocity = new Vector3( x , y,z);// = Vector3.Lerp( z , velForward  , .1f);


        // jump
        rb.AddForce( 800 * Vector3.up * Input.GetAxis("X") * _JumpMULT);

        rb.AddTorque( transform.up * Input.GetAxis("LeftStickX")*(1-Input.GetAxis("L1"))  * (300)* _RotMULT);

        rb.AddTorque( transform.right * Input.GetAxis("LeftStickY") * 40* _RotMULT);
        rb.AddTorque( transform.forward *  Input.GetAxis("LeftStickX")  *(Input.GetAxis("L1")) * 400* _RotMULT);



        }
     
 



      float boooooooooost = Input.GetAxis("O");

//      print( boooooooooost);


      if( boostAmount > MaxMegaBoost ){ boostAmount = MaxMegaBoost; }
      if( boostAmount <= 0 ){
        boooooooooost = 0;
         megaBoostL.GetComponent<Booster>().amount =0;
         megaBoostR.GetComponent<Booster>().amount =0;
      }else{

        if( boooooooooost > 0.001f){
          boostAmount -= .01f;

          megaBoostL.GetComponent<Booster>().amount = boooooooooost;
          megaBoostR.GetComponent<Booster>().amount = boooooooooost;

        }else{
          megaBoostL.GetComponent<Booster>().amount =0;
          megaBoostR.GetComponent<Booster>().amount =0;
        }
      }

      Vector3 final = 100  * (-Input.GetAxis("R2")+Input.GetAxis("L2")) * transform.forward * _BoostMULT * ( 1 + 3 * boooooooooost);
		  rb.AddForce(final);

      boosterL.GetComponent<Booster>().amount = final.magnitude;
      boosterR.GetComponent<Booster>().amount = final.magnitude;




      rb.drag = 1 - Mathf.Abs(Vector3.Dot( rb.velocity.normalized , -transform.forward.normalized)) + dragBaseAmount; 


      DoSuspension();
      






      // IN AIR




    }else{

 rb.angularDrag = _AirRotDrag;

      if( boostAmount > MaxMegaBoost ){ boostAmount = MaxMegaBoost; }
      float boooooooooost = Input.GetAxis("O");

//      print( boooooooooost);

      if( boostAmount <= 0 ){
        boooooooooost = 0;
         megaBoostL.GetComponent<Booster>().amount =0;
         megaBoostR.GetComponent<Booster>().amount =0;
      }else{

        if( boooooooooost > 0.001f){
          boostAmount -= .01f;

          megaBoostL.GetComponent<Booster>().amount = boooooooooost;
          megaBoostR.GetComponent<Booster>().amount = boooooooooost;

        }else{
          megaBoostL.GetComponent<Booster>().amount =0;
          megaBoostR.GetComponent<Booster>().amount =0;
        }
      }

    
    float extraRoll = 1+Input.GetAxis("X") * _AirRollBoost;


    rb.AddTorque( transform.up * Input.GetAxis("LeftStickX")*(1-Input.GetAxis("L1"))  * 4* 180* _AirRotMULT* extraRoll) ;
    rb.AddTorque( transform.right * Input.GetAxis("LeftStickY") * 4*180* _AirRotMULT* extraRoll);

    rb.AddTorque( transform.forward *  Input.GetAxis("LeftStickX")  *(Input.GetAxis("L1")) * 4*180* _AirRotMULT* extraRoll);
    Vector3 final =100 * _AirBoostMULT * ( 1 + 3 * boooooooooost) * (-Input.GetAxis("R2")+Input.GetAxis("L2")) * transform.forward;
    rb.AddForce(final);



      
      boosterL.GetComponent<Booster>().amount = final.magnitude;
      boosterR.GetComponent<Booster>().amount = final.magnitude;


    }

    megaBoostAmountL.transform.localScale = new Vector3( 1.1f , boostAmount / MaxMegaBoost , 1.1f );
    megaBoostAmountL.transform.localPosition = new Vector3( 0, 1-.5f*boostAmount / MaxMegaBoost , 0);

    megaBoostAmountR.transform.localScale = new Vector3( 1.1f , boostAmount / MaxMegaBoost , 1.1f );
    megaBoostAmountR.transform.localPosition = new Vector3( 0, 1-.5f*boostAmount / MaxMegaBoost , 0);


	}

  void DoSuspension(){

    AddDifForce df1 = front.GetComponent<AddDifForce>();
    AddDifForce df2 = back.GetComponent<AddDifForce>();


    if(  df1.dist  < _MaxSuspensionHeight ){

      //rb.AddForceAtPosition( (2/df1.dist) * df1.normal * 1 * suspension, front.transform.position );
      rb.AddForceAtPosition( (1/df1.dist) * df1.normal * -1 * suspension, back.transform.position );
    }


    if(  df2.dist < _MaxSuspensionHeight ){
      //rb.AddForceAtPosition( (2/df2.dist) * df2.normal * 1 * suspension, back.transform.position );
      rb.AddForceAtPosition( (1/df2.dist)  * df2.normal *  -1 * suspension, front.transform.position );
    }


    df1 = wingLeft.GetComponent<AddDifForce>();
    df2 = wingRight.GetComponent<AddDifForce>();


    if(  df1.dist  < _MaxSuspensionHeight ){
      //rb.AddForceAtPosition( (2/df1.dist) * df1.normal * 1 * suspension, wingLeft.transform.position );
      rb.AddForceAtPosition( (1/df1.dist) * df1.normal * -1 * suspension, wingRight.transform.position );
    }


    if(  df2.dist < _MaxSuspensionHeight ){
      //rb.AddForceAtPosition( (2/df2.dist) * df2.normal * 1 * suspension, wingRight.transform.position );
      rb.AddForceAtPosition((1/df2.dist)  * df2.normal *  -1 * suspension, wingLeft.transform.position );
    }



  }


  void OnCollisionEnter( Collision c ){


    float strength = c.impulse.magnitude;
//    print( strength);

    strength = Mathf.Clamp( strength * .3f , 0 , 2);

    if( lockedObject == null ){

    if( c.contacts[0].thisCollider.gameObject == wingLeft ){
      wingLeft.GetComponent<AddDifForce>().EmitParticles(1);
    }

    if( c.contacts[0].thisCollider.gameObject == wingRight ){
      wingRight.GetComponent<AddDifForce>().EmitParticles(1);
    }

    if( c.contacts[0].thisCollider.gameObject == front ){
      front.GetComponent<AddDifForce>().EmitParticles(1);
    }

    if( c.contacts[0].thisCollider.gameObject == back ){
      back.GetComponent<AddDifForce>().EmitParticles(1);
    }

    if( c.contacts[0].thisCollider.gameObject == body ){
      body.GetComponent<AddDifForce>().EmitParticles(1);
    }
  }

  }

  void ThrowLock(){


if( onGround ==  false ){
      lockStartTime = Time.time;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(front.transform.position+transform.TransformDirection(-Vector3.forward)*10f, transform.TransformDirection(-Vector3.forward+Vector3.down*.01f), out hit, 100))
        {

lockedObject = front.transform;
 lockPos = hit.point+hit.normal*10;
          return;
          

        }
}else{



       Vector3 p = transform.forward *  Input.GetAxis("LeftStickY") * 100 *_LockThrowDistance;


      if(Input.GetAxis("LeftStickX") < -.05f ){


        lockedObject = wingLeft.transform;
        lockPos = wingLeft.transform.position + p  - transform.right * Input.GetAxis("LeftStickX") * 100 *_LockThrowDistance;

        //lockedObject = front.transform;
        //lockPos =   front.transform.position + p - transform.right * Input.GetAxis("LeftStickX") * 100 *_LockThrowDistance;


      }else if(Input.GetAxis("LeftStickX")  > .05f ){

        lockedObject = wingRight.transform;
        lockPos = wingRight.transform.position + p  - transform.right * Input.GetAxis("LeftStickX") * 100 *_LockThrowDistance;

      }else{


          print(Input.GetAxis("LeftStickY") ); 

        if(Input.GetAxis("LeftStickY")  <= 0 ){
          print(Input.GetAxis("LeftStickY") );

        lockedObject = front.transform;
        lockPos = front.transform.position + p * 4 - transform.forward * Input.GetAxis("LeftStickX") * 300 *_LockThrowDistance;;

        }else{
           lockedObject = back.transform;
        lockPos = back.transform.position + p * 4 - transform.forward * Input.GetAxis("LeftStickX") * 100 *_LockThrowDistance;;

        }



      }

          float h = terrain.SampleHeight( lockPos );
        lockPos = new Vector3( lockPos.x , h+ _LockThrowHeight, lockPos.z );
       

}
      //lockedObject = body.transform;

  }


  void OnTriggerEnter(Collider c){

    if( c.tag == "Ground" ){ 
      //print("ya"); 

      onGround = true;

    }else{ print("na");}

  }

  void OnTriggerExit(Collider c){

    if( c.tag == "Ground" ){ 
//      print("ya"); 

      onGround = false;

    }else{ print("na");}

  }
}
