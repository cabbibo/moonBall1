﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


public class GO : MonoBehaviour {


  public bool newSus;
  public Horn horn;
  public float newSusDist;
  public float newSusMultiplier;
  public float forwardCollisionDist;
  public float forwardCollisionPower;
  public float distPow;

  public GameObject hookPoint;
  public GameObject LockPuller;
  public Camera cam;

  public PitchSquisher boostSquish;
  public PitchSquisher alwaysSquish;
  public PitchSquisher suspensionSquish;
  public PitchSquisher frontSuspensionSquish;
  public PitchSquisher torqueSquish;

  public float StickAdjust;

  public float boostAmount;
  public float oBoostAmount;


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
  public float upThrowMult;

  public float uprightTorque;


  public float _LockHitDist;
  public AudioClip lockHitClip;
  public bool canThrow = true;
  public int numMoons;

  public float dragBaseAmount;

  public Terrain terrain;
  public Rigidbody rb;

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
  public Renderer megaRepresentL;
  public Renderer megaRepresentR;
  public float MaxMegaBoost;

  public float dragMultiplier;
  public float baseGravity;


  public Transform currentTailTip;

  public Transform lockedObject;
  public Vector3 lockPos;

  public GameObject cameraHolder;

  private float startDrag;
  private connectWithLine cwl;

  public int tailsConnected = 0;
  public float lockStartTime;

  public List<GameObject> tails;

  private Player player;
	// Use this for initialization

  public void DropAll(){
    for( int i = 0; i < tails.Count; i++ ){
      tails[i].GetComponent<FollowOnceHit>().End();
    }
  }
	void Start () {

    Physics.gravity = new Vector3(0,baseGravity,0);
//      player.dragMultiplier = 1;


    tails = new List<GameObject>();

    player = ReInput.players.GetPlayer(0);
    rb = GetComponent<Rigidbody>();

    currentTailTip = body.transform;//back.transform;
    onGround = false;

    startDrag = rb.drag;
   // cwl = hookPoint.GetComponent<connectWithLine>();
		
	}

  void ProcessInput(){
//    print( player.GetAxis("JoystickLeftX") );
  }


	// Update is called once per frame
	void FixedUpdate () {

    ProcessInput();

    cam.fieldOfView = Mathf.Lerp( cam.fieldOfView , 60 + Mathf.Clamp( rb.velocity.magnitude , 0 , 60), .1f );




    cameraHolder.GetComponent<Rigidbody>().AddTorque(   Vector3.up *StickAdjust* player.GetAxis("JoystickRightX") * 80);
 


    float lilSquish =  (.4f+.01f*Vector3.Dot( -rb.velocity, transform.forward.normalized ))/2;

    alwaysSquish.Squish(lilSquish);

    float deltaBoost= boostAmount - oBoostAmount;



    //print( deltaBoost );

    boosterR.GetComponent<MeshRenderer>().material.SetFloat("_BoostValue" , deltaBoost * 3000 );
    boosterL.GetComponent<MeshRenderer>().material.SetFloat("_BoostValue" , deltaBoost * 3000 );

    if( deltaBoost != 0 ){ megaRepresentL.material.SetColor("_Color" , Color.red);}else{megaRepresentL.material.SetColor("_Color" , Color.blue);}
    if( deltaBoost != 0 ){ megaRepresentR.material.SetColor("_Color" , Color.red);}else{megaRepresentR.material.SetColor("_Color" , Color.blue);}

/*

  BOOST STUFF

*/


      if( boostAmount > MaxMegaBoost ){ boostAmount = MaxMegaBoost; }
      float boooooooooost = player.GetAxis("O");

//      print( boooooooooost);

      if( boostAmount <= 0 ){
        boooooooooost = 0;
         megaBoostL.GetComponent<Booster>().amount =0;
         megaBoostR.GetComponent<Booster>().amount =0;
         boostSquish.UnSquish();
      }else{

        if( boooooooooost > 0.001f){
          boostAmount -= .03f;

          megaBoostL.GetComponent<Booster>().amount = boooooooooost;
          megaBoostR.GetComponent<Booster>().amount = boooooooooost;  
          boostSquish.Squish( lilSquish);



        }else{
          megaBoostL.GetComponent<Booster>().amount =0;
          megaBoostR.GetComponent<Booster>().amount =0;
            boostSquish.UnSquish();


        }
      }


    megaBoostAmountL.transform.localScale = new Vector3( 1.1f , boostAmount / MaxMegaBoost , 1.1f );
    megaBoostAmountL.transform.localPosition = new Vector3( 0, 1-.5f*boostAmount / MaxMegaBoost , 0);

    megaBoostAmountR.transform.localScale = new Vector3( 1.1f , boostAmount / MaxMegaBoost , 1.1f );
    megaBoostAmountR.transform.localPosition = new Vector3( 0, 1-.5f*boostAmount / MaxMegaBoost , 0);

    if( newSus ){
      DoNewSus();
    }
    
    if( onGround == true ){

     // print("on Ground");
      OnGround(boooooooooost);





      // IN AIR




    }else{

//      print( "in air");

          InAir(boooooooooost);
    }

    deltaBoost = boostAmount - oBoostAmount;
    if( deltaBoost > 0 ){ megaRepresentL.material.SetColor("_Color" , Color.red);
    }else if ( deltaBoost < 0 ){megaRepresentL.material.SetColor("_Color" , Color.green); 
    }else{megaRepresentL.material.SetColor("_Color" , Color.blue);}
    if( deltaBoost > 0 ){ megaRepresentR.material.SetColor("_Color" , Color.red);
    }else if ( deltaBoost < 0 ){megaRepresentR.material.SetColor("_Color" , Color.green); 
    }else{megaRepresentR.material.SetColor("_Color" , Color.blue);}


    oBoostAmount = boostAmount;

    horn.value = player.GetAxis("T");


	}

void InAir(float boost){

    rb.angularDrag = _AirRotDrag;


    float extraRoll = 1 + player.GetAxis("X") * _AirRollBoost;




    rb.AddTorque( StickAdjust * transform.up * player.GetAxis("JoystickLeftX")*(1-player.GetAxis("L1"))  * 4* 180* _AirRotMULT* extraRoll) ;
    rb.AddTorque( StickAdjust * transform.right * player.GetAxis("JoystickLeftY") * 4*180* _AirRotMULT* extraRoll);

    rb.AddTorque(  StickAdjust * transform.forward *  player.GetAxis("JoystickLeftX")  *(player.GetAxis("L1")) * 4*180* _AirRotMULT* extraRoll);
    Vector3 final =100 * _AirBoostMULT * ( 1 + 3 * boost) * (-player.GetAxis("R2")+player.GetAxis("L2")) * transform.forward;
    rb.AddForce(final);



      
      boosterL.GetComponent<Booster>().amount = final.magnitude;
      boosterR.GetComponent<Booster>().amount = final.magnitude;


    


}
  void OnGround(float boost){
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
        rb.AddForce( 800 * Vector3.up * player.GetAxis("X") * _JumpMULT);

        rb.AddTorque( StickAdjust * transform.up * player.GetAxis("JoystickLeftX")*(1-player.GetAxis("L1"))  * (300)* _RotMULT);



        if( player.GetAxis("[]") == 0 ){
          rb.AddTorque(  StickAdjust *transform.right * player.GetAxis("JoystickLeftY") * 400* _RotMULT);
        }
        
        rb.AddTorque(  StickAdjust *transform.forward *  player.GetAxis("JoystickLeftX")  *(player.GetAxis("L1")) * 400* _RotMULT);



        }
     
 
      Vector3 final = 100  * (-player.GetAxis("R2")+player.GetAxis("L2")) * transform.forward * _BoostMULT * ( 1 + 3 * boost);
      rb.AddForce(final);

      boosterL.GetComponent<Booster>().amount = final.magnitude;
      boosterR.GetComponent<Booster>().amount = final.magnitude;




      rb.drag = (1 - Mathf.Abs(Vector3.Dot( rb.velocity.normalized , -transform.forward.normalized)) + dragBaseAmount) * dragMultiplier; 


      if( newSus == false ){
        DoSuspension();
      }
      




  }

  void DoRayCast( Transform  t , Vector3 down, out Vector3 normal , out float dist ){
    
    RaycastHit hit;
    // Does the ray intersect any objects excluding the player layer
    if (Physics.Raycast(t.position, t.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
    {

      normal = hit.normal;
      dist = hit.distance;

    }else{
      normal = new Vector3(0,1,0);
      dist = 1000000;
    }

  }

  void DoNewSus(){
   
    Vector3 normal;
    float dist;
    float lilSquish;
    DoRayCast( body.transform , transform.TransformDirection(Vector3.down) , out normal , out dist );
   // dist = body.transform.position.y - Terrain.activeTerrain.SampleHeight(transform.position);
//    print(dist);
    if( dist < newSusDist && rb.velocity.magnitude > 0){

      dist = Mathf.Abs( dist );
      Vector3 projection = Vector3.Project( rb.velocity , normal.normalized );
      float d = Vector3.Dot( rb.velocity.normalized , normal.normalized ); 
      rb.velocity += newSusMultiplier * (1/Mathf.Pow(dist,distPow)) * projection.magnitude * normal.normalized;// newSusMultiplier;
    
    lilSquish = newSusMultiplier * (1/Mathf.Pow(dist,distPow)) * projection.magnitude ;// newSusMultiplier;
    
//    print("Sus Squish : " + lilSquish);
      suspensionSquish.Squish(lilSquish);

      var rot = Quaternion.FromToRotation(transform.up, normal);
      rb.AddTorque(new Vector3(rot.x, rot.y, rot.z)*uprightTorque * (1/dist) * rb.velocity.magnitude);
      float e = rot.eulerAngles.magnitude;
      lilSquish = e * uprightTorque * (1/dist) * rb.velocity.magnitude;
      torqueSquish.Squish(lilSquish * .001f);

    }else{

      suspensionSquish.Squish(0);
      torqueSquish.Squish(0);
    }

    Vector3 fwd = transform.TransformDirection(Vector3.forward);

    DoRayCast( body.transform , fwd , out normal , out dist );
    
    if( dist < forwardCollisionDist && rb.velocity.magnitude > 0){

      Vector3 projection = Vector3.Project( rb.velocity , fwd);
      float d = Vector3.Dot( rb.velocity.normalized , normal.normalized ); 
      rb.velocity += forwardCollisionPower * (1-Mathf.Abs(d)) * projection.magnitude *  normal.normalized  / dist;
     lilSquish = forwardCollisionPower * (1-Mathf.Abs(d)) * projection.magnitude   / dist;
     

      frontSuspensionSquish.Squish(lilSquish);
     }else{
       frontSuspensionSquish.Squish(0);
      //torqueSquish.Squish(0);
     }







 





  }


  void DoSuspension(){

    AddDifForce df1 = front.GetComponent<AddDifForce>();
    AddDifForce df2 = back.GetComponent<AddDifForce>();

    AddDifForce bodyForce = body.GetComponent<AddDifForce>();


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
 

 var rot = Quaternion.FromToRotation(transform.up, bodyForce.normal);
 //rb.AddTorque(new Vector3(rot.x, rot.y, rot.z)*uprightTorque);


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


  void OnTriggerEnter(Collider c){

    if( c.tag == "Ground" ){ 
      //print("ya"); 

      onGround = true;

    }//else{ print("na");}

  }

  void OnTriggerExit(Collider c){

    if( c.tag == "Ground" ){ 
//  s    print("ya"); 

      onGround = false;

    }//else{ print("na");}

  }
}
