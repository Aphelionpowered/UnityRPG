using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Player : MonoBehaviour {
	private GameManager gm;
	private bool canMove = false;
	private bool waitActive = false; //so wait function wouldn't be called many times per frame
	


	private Vector3 offset;



	void Awake (){
		gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

	// Use this for initialization
	void Start () {
		setPOffset();
	}

	private void setPOffset(){
		transform.localPosition = gm.tileMap.renderer.bounds.extents;
		Vector3 ex = renderer.bounds.extents;
		offset = ex;
		transform.localPosition = new Vector3 (transform.localPosition.x + ex.x,transform.localPosition.y + ex.y, transform.localPosition.z + ex.z);
	}

	// Update is called once per frame
	void Update () {

	}

	public Vector3 getRCoords(){
		Vector3 coords = new Vector3(transform.localPosition.x - offset.x,transform.localPosition.y - offset.y,transform.localPosition.z - offset.z);
		return coords;
	}

	#region Movement
		#region Wait
		IEnumerator Wait(){
			waitActive = true;
			yield return new WaitForSeconds (0.2f);
			canMove = true;
			waitActive = false;
		}	
		#endregion

	public void moveUp(){
		if(!waitActive){
			StartCoroutine(Wait());
		}
		if(canMove && gm.map.GetCurrentMap.getTile((int)getRCoords().x,(int)getRCoords().z,TileMapData.Location.Above).Walkable){
			transform.localPosition = new Vector3( transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 1);
			canMove = false;
		}
	}
	public void moveDown(){
		if(!waitActive){
			StartCoroutine(Wait());
		}
		if(canMove && gm.map.GetCurrentMap.getTile((int)getRCoords().x,(int)getRCoords().z,TileMapData.Location.Below).Walkable){
			transform.localPosition = new Vector3( transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 1);
			canMove = false;
		}
	}
	public void moveRight(){
		if(!waitActive){
			StartCoroutine(Wait());
		}
		if(canMove && gm.map.GetCurrentMap.getTile((int)getRCoords().x,(int)getRCoords().z,TileMapData.Location.Right).Walkable){
		transform.localPosition = new Vector3( transform.localPosition.x + 1, transform.localPosition.y, transform.localPosition.z);
			canMove = false;
		}
	}
	public void moveLeft(){
		if(!waitActive){
			StartCoroutine(Wait());
		}
		if(canMove && gm.map.GetCurrentMap.getTile((int)getRCoords().x,(int)getRCoords().z,TileMapData.Location.Left).Walkable){
		transform.localPosition = new Vector3( transform.localPosition.x - 1, transform.localPosition.y, transform.localPosition.z);
			canMove = false;
		}
	}
	#endregion

}
