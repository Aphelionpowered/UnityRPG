using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
	private GameManager gm;

	void Awake () {
		gm = GetComponent<GameManager>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int TileID {
		get {
			Vector3 currentCoords = gm.player.getRCoords();
			return ConvertTransformCoordsToTileID(currentCoords);
		}
	}

	
	public int ConvertTransformCoordsToTileID(Vector3 MapTransformPosition){
		Tile t = gm.map.GetCurrentMap.getTile((int)MapTransformPosition.x,(int)MapTransformPosition.z, TileMapData.Location.Current);
		return t.Id;
	}

	public Vector3 PGetCoords(){
		return gm.player.getRCoords();
	}
	#region Movement
	public void pMoveUp(){
		gm.player.moveUp();
	}
	
	public void pMoveDown(){
		gm.player.moveDown();
	}
	
	public void pMoveRight(){
		gm.player.moveRight();
	}
	
	public void pMoveLeft(){
		gm.player.moveLeft();
	}
	#endregion
}
