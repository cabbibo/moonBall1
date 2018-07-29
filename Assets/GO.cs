using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GO : MonoBehaviour {


  public GameObject hookPoint;
  public Camera cam;


  public float boostAmount;

  public float _BoostMULT;
  public float _LerpSpeed;
  public float _RotMULT;
  public float _JumpMULT;
  public float _AirRotMULT;
  public float _AirBoostMULT;
  public float _LockMULT;

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

   }else{

    if( lockedObject == null ){

   
      ThrowLock();

     /* transform.left *  ( Input.GetAxis("LeftStickX")
        lockPos = c.contacts[0].point;
      lockedObject = wingLeft.transform;


      rb.AddTorque( transform.forward *  Input.GetAxis("LeftStickX") *  4000 * Input.GetAxis("[]"));*/

    }

   }


    /*
      Do our lock stuff;
    */

    if( lockedObject != null){

      cwl.connected = lockedObject;
      //rb.drag = 10;
      Vector3 delta = lockedObject.position - lockPos;

      float timeLocked = Time.time - lockStartTime;
      timeLocked *= 3;
      timeLocked = Mathf.Clamp( timeLocked , 0, 1);
     // rb.AddForceAtPosition( -delta *30* _MULT * timeLocked ,lockedObject.position);
    //  rb.
      rb.AddForceAtPosition(  -delta.normalized*1000* _LockMULT * timeLocked ,lockedObject.position + delta * .5f );


      hookPoint.transform.position = lockPos;

    }else{


      hookPoint.transform.position = Vector3.left * 10000;
      //rb.drag = startDrag;
    }

    cameraHolder.GetComponent<Rigidbody>().AddTorque(   Vector3.up * Input.GetAxis("RightStickX") * 80);
 

    if( onGround == true ){


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


    }else{



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




    rb.AddTorque( transform.up * Input.GetAxis("LeftStickX")*(1-Input.GetAxis("L1"))  * 4* 180* _AirRotMULT);
    rb.AddTorque( transform.right * Input.GetAxis("LeftStickY") * 4*180* _AirRotMULT);

    rb.AddTorque( transform.forward *  Input.GetAxis("LeftStickX")  *(Input.GetAxis("L1")) * 4*180* _AirRotMULT);
    Vector3 final =100 * _AirBoostMULT * ( 1 + 3 * boooooooooost) * (-Input.GetAxis("R2")+Input.GetAxis("L2")) * transform.forward;
    rb.AddForce(final);



      
      boosterL.GetComponent<Booster>().amount = final.magnitude;
      boosterR.GetComponent<Booster>().amount = final.magnitude;


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

      lockStartTime = Time.time;

       Vector3 p = transform.forward *  Input.GetAxis("LeftStickY") * 40;



      if(Input.GetAxis("LeftStickX") < -.05f ){


        lockedObject = wingLeft.transform;
        lockPos = wingLeft.transform.position + p  - transform.right * Input.GetAxis("LeftStickX") * 100;

        //lockedObject = front.transform;
        //lockPos =   front.transform.position + p - transform.right * Input.GetAxis("LeftStickX") * 100;

        float h = terrain.SampleHeight( lockPos );
       // lockPos = new Vector3( lockPos.x , h , lockPos.z );

      }else if(Input.GetAxis("LeftStickX")  > .05f ){

        lockedObject = wingRight.transform;
        lockPos = wingRight.transform.position + p  - transform.right * Input.GetAxis("LeftStickX") * 100;


       // lockedObject = front.transform;
       // lockPos = front.transform.position+ p - transform.right * Input.GetAxis("LeftStickX") * 100;

        float h = terrain.SampleHeight( lockPos );
        //lockPos = new Vector3( lockPos.x , h , lockPos.z );


      }else{


          print(Input.GetAxis("LeftStickY") ); 

        if(Input.GetAxis("LeftStickY")  <= 0 ){
          print(Input.GetAxis("LeftStickY") );

        lockedObject = front.transform;
        lockPos = front.transform.position + p * 4 - transform.right * Input.GetAxis("LeftStickX") * 100;;
        float h = terrain.SampleHeight( lockPos );
        //lockPos = new Vector3( lockPos.x , h , lockPos.z );
        }else{
           lockedObject = back.transform;
        lockPos = back.transform.position + p * 4 - transform.right * Input.GetAxis("LeftStickX") * 100;;
        float h = terrain.SampleHeight( lockPos );
        //lockPos = new Vector3( lockPos.x , h , lockPos.z );
        }
      }
      lockedObject = body.transform;

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
