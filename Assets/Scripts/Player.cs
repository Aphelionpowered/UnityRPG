using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Player : MonoBehaviour {
	private GameManager gm;
	public bool canMove = true;
	
	public enum Direction {
		up,
		right,
		down,
		left
	}

	private Vector3 offset;


	void Awake (){
		gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

	public void setPlayerLocation(Vector3 location){
		Vector3 offsetLocation = new Vector3(location.x + 0.5f, location.y + 0.5f ,location.z+0.5f);
		transform.localPosition = offsetLocation;
	}


	public Vector3 getRCoords(){
		Vector3 coords = new Vector3(transform.localPosition.x - offset.x,transform.localPosition.y - offset.y,transform.localPosition.z - offset.z);
		return coords;
	}

	#region Movement
		#region Wait
		IEnumerator Wait(){
			canMove = false;
			yield return new WaitForSeconds (0.2f);
			canMove = true;
		}	
		#endregion

	public void moveUp()
	{
		if(canMove)
		{
			if(gm.mapManager.isTileMoveable(transform.localPosition, Direction.up))
			{
				transform.localPosition = new Vector3( transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 1);
				StartCoroutine(Wait());
			}
		}
	}
	public void moveDown()
	{
		if(canMove)
		{
			if(gm.mapManager.isTileMoveable(transform.localPosition, Direction.down))
			{
				transform.localPosition = new Vector3( transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 1);
				StartCoroutine(Wait());
			}
		}

	}
	public void moveRight()
	{
		if(canMove)
		{
			if(gm.mapManager.isTileMoveable(transform.localPosition, Direction.right))
			{
				transform.localPosition = new Vector3( transform.localPosition.x + 1, transform.localPosition.y, transform.localPosition.z);
				StartCoroutine(Wait());
			}
		}
	}
	public void moveLeft()
	{
		if(canMove)
		{
			if(gm.mapManager.isTileMoveable(transform.localPosition, Direction.left))
			{
				transform.localPosition = new Vector3( transform.localPosition.x - 1, transform.localPosition.y, transform.localPosition.z);
				StartCoroutine(Wait());
			}
		}
	}
	#endregion

}
