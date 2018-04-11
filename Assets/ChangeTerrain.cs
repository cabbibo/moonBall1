using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTerrain : MonoBehaviour {


  public Terrain terrain;

  private float[,] originalHeights;

  public int size;
  public float desiredHeight;

  private int hmWidth;
  private int hmHeight;

  private int posXInTerrain;
  private int posYInTerrain;



  private Rigidbody rb;
  private GO go;

	// Use this for initialization
	void OnEnable() {

    rb = GetComponent<Rigidbody>();
    go = GetComponent<GO>();

    hmWidth = terrain.terrainData.heightmapWidth;
    hmHeight = terrain.terrainData.heightmapHeight;

    originalHeights = terrain.terrainData.GetHeights( 0, 0, terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);
		
	}

  void OnDisable(){
    terrain.terrainData.SetHeights(0, 0, this.originalHeights);
  }
	
	// Update is called once per frame
	void Update () {

    if( go.onGround == true ){

       float h = terrain.SampleHeight(transform.position);

    // get the normalized position of this game object relative to the terrain
 Vector3 tempCoord = (transform.position - terrain.gameObject.transform.position+ 10 * transform.forward);
 Vector3 coord;
 coord.x = tempCoord.x / terrain.terrainData.size.x;
 coord.y = tempCoord.y / terrain.terrainData.size.y;
 coord.z = tempCoord.z / terrain.terrainData.size.z;
 

 // get the position of the terrain heightmap where this game object is
 posXInTerrain = (int) (coord.x * hmWidth); 
 posYInTerrain = (int) (coord.z * hmHeight);

 // we set an offset so that all the raising terrain is under this game object
 int offset = size / 2;
 // get the heights of the terrain under this game object
 float[,] heights = terrain.terrainData.GetHeights(posXInTerrain-offset,posYInTerrain-offset,size,size);


 // we set each sample of the terrain in the size to the desired height
 for (int i=0; i < size; i++)
     for (int j=0; j < size; j++)
         heights[i,j] += rb.velocity.magnitude * desiredHeight;

 // go raising the terrain slowly
 //desiredHeight += Time.deltaTime;
 // set the new height
 terrain.terrainData.SetHeights(posXInTerrain-offset,posYInTerrain-offset,heights);


float[,] allHeights = terrain.terrainData.GetHeights( 0, 0, terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);

 for (int i=0; i < terrain.terrainData.heightmapWidth; i++){
     for (int j=0; j <  terrain.terrainData.heightmapHeight; j++){
         allHeights[i,j] = Mathf.Lerp( allHeights[i,j] , originalHeights[i,j] , .1f);
  }
}


 terrain.terrainData.SetHeights(0,0,allHeights);
		
	}
}
}
