using UnityEngine;
using System.Collections;
/*
[RequireComponent(typeof(TileMap))]
public class TileMapMouse : MonoBehaviour {
	
	private TileMap _tileMap;
	
	public Transform selectionCube;
	
	Vector3 currentTileCoord;
	
	void Start(){
			_tileMap = GetComponent<TileMap>();
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.mainCamera.ScreenPointToRay( Input.mousePosition );
		RaycastHit hitInfo;
		
		if(collider.Raycast(ray, out hitInfo, Mathf.Infinity)){
			Vector3 p = hitInfo.point; 
			Vector3 lp = transform.InverseTransformPoint(p);
			int x = Mathf.FloorToInt(lp.x / _tileMap.tileSize);
			int z = Mathf.FloorToInt(lp.z / _tileMap.tileSize);
			//Debug.Log("tile: " + x + ", " + z);
			
			currentTileCoord.x = x;
			currentTileCoord.z = z;
			selectionCube.transform.position = currentTileCoord * _tileMap.tileSize;
			
		} else {
			//other stuff
		}	
	}
}
*/