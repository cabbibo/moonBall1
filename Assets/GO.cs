using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GO : MonoBehaviour {


  public GameObject hookPoint;
  public Camera cam;


  public float boostAmount;

  public float _MULT;

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
  public GameObject booster;
  public GameObject megaBoost;

  public Transform lockedObject;
  public Vector3 lockPos;

  public GameObject cameraHolder;

  private float startDrag;
  private connectWithLine cwl;

  public float lockStartTime;
	// Use this for initialization
	void Start () {
    rb= GetComponent<Rigidbody>();
    onGround = false;

    startDrag = rb.drag;
    cwl = hookPoint.GetComponent<connectWithLine>();
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

    cam.fov = Mathf.Lerp( cam.fov , 60 + Mathf.Clamp( rb.velocity.magnitude , 0 , 60), .1f );

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
      timeLocked *= 1;
      timeLocked = Mathf.Clamp( timeLocked , 0, 1);
      rb.AddForceAtPosition( -delta *30* _MULT * timeLocked ,lockedObject.position);


      hookPoint.transform.position = lockPos;

    }else{


      hookPoint.transform.position = Vector3.left * 10000;
      //rb.drag = startDrag;
    }

    cameraHolder.GetComponent<Rigidbody>().AddTorque(   Vector3.up * Input.GetAxis("RightStickX") * 80);
 

    if( onGround == true ){

      float h = terrain.SampleHeight(transform.position);

      float eps = .01f;


      Vector3 left = transform.position + Vector3.left * eps;
      Vector3 right = transform.position - Vector3.left * eps;

      Vector3 up = transform.position + Vector3.forward * eps;
      Vector3 down = transform.position - Vector3.forward * eps;

      float hL = terrain.SampleHeight(left);
      float hR = terrain.SampleHeight(right);

      float hU = terrain.SampleHeight(up);
      float hD = terrain.SampleHeight(down);

      left  = new Vector3( left.x   , hL , left.z   );
      right = new Vector3( right.x  , hR , right.z  );
      up    = new Vector3( up.x     , hU , up.z     );
      down  = new Vector3( down.x   , hD , down.z   );

      Vector3 nor = Vector3.Cross( (left-right).normalized,(up-down).normalized );


     // rb.velocity = Vector3.Lerp( rb.velocity , transform.forward , .1f);

      float dif =  transform.position.y-h;


      // jump
      rb.AddForce( 800 * Vector3.up * Input.GetAxis("X"));

      rb.AddTorque( transform.up * Input.GetAxis("LeftStickX")*(1-Input.GetAxis("L1"))  * (300)* _MULT);

      rb.AddTorque( transform.right * Input.GetAxis("LeftStickY") * 40* _MULT);
      rb.AddTorque( transform.forward *  Input.GetAxis("LeftStickX")  *(Input.GetAxis("L1")) * 400* _MULT);




      float boooooooooost = Input.GetAxis("O");

      print( boooooooooost);

      if( boostAmount <= 0 ){
        boooooooooost = 0;
      }else{

        if( boooooooooost > 0.001f){
          boostAmount -= .01f;

          megaBoost.GetComponent<Booster>().amount = boooooooooost;

        }
      }

      Vector3 final = 100  * (-Input.GetAxis("R2")+Input.GetAxis("L2")) * transform.forward * _MULT * ( 1 + 3 * boooooooooost);
		  rb.AddForce(final);

      booster.GetComponent<Booster>().amount = final.magnitude;



      Vector3 velForward = -transform.forward.normalized * rb.velocity.magnitude;
      rb.velocity = Vector3.Lerp( rb.velocity , velForward  , .1f);

      rb.drag = 1- Mathf.Abs(Vector3.Dot( rb.velocity.normalized , -transform.forward.normalized)) + .05f; 


    }else{



      float boooooooooost = Input.GetAxis("O");

      if( boostAmount <= 0 ){
        boooooooooost = 0;
      }else{

        if( boooooooooost > 0.001f){
        boostAmount -= .01f;
      }
      }


    rb.AddTorque( transform.up * Input.GetAxis("LeftStickX")*(1-Input.GetAxis("L1"))  * 4* 180* _MULT);
    rb.AddTorque( transform.right * Input.GetAxis("LeftStickY") * 4*180* _MULT);

    rb.AddTorque( transform.forward *  Input.GetAxis("LeftStickX")  *(Input.GetAxis("L1")) * 4*180* _MULT);
    Vector3 final =100 * _MULT  * (-Input.GetAxis("R2")+Input.GetAxis("L2")) * transform.forward;
    rb.AddForce(final);



      booster.GetComponent<Booster>().amount = final.magnitude;

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
        lockPos = new Vector3( lockPos.x , h , lockPos.z );

      }else if(Input.GetAxis("LeftStickX")  > .05f ){

        lockedObject = wingRight.transform;
        lockPos = wingRight.transform.position + p  - transform.right * Input.GetAxis("LeftStickX") * 100;


       // lockedObject = front.transform;
       // lockPos = front.transform.position+ p - transform.right * Input.GetAxis("LeftStickX") * 100;

        float h = terrain.SampleHeight( lockPos );
        lockPos = new Vector3( lockPos.x , h , lockPos.z );


      }else{

        lockedObject = front.transform;
        lockPos = front.transform.position + p * 4 - transform.right * Input.GetAxis("LeftStickX") * 100;;
        float h = terrain.SampleHeight( lockPos );
        lockPos = new Vector3( lockPos.x , h , lockPos.z );


      }

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
