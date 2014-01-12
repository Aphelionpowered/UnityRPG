using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
	private GameManager gm;
	private Player player;
	

	void Awake ()
	{
		gm = GetComponent<GameManager>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

	public int TileID
	{
		get {
			Vector3 currentCoords = gm.player.getRCoords();
			return ConvertTransformCoordsToTileID(currentCoords);
		}
	}

	public void SpawnPlayer()
	{
		Vector3 location = gm.mapManager.getSpawnPoint();
		player.setPlayerLocation(location);
	}

	public int ConvertTransformCoordsToTileID(Vector3 MapTransformPosition)
	{
		Tile t = gm.mapManager.getTile((int)MapTransformPosition.x,(int)MapTransformPosition.z, gm.mapManager.currentMap);
		return t.Id;
	}

	public Vector3 PGetCoords()
	{
		return gm.player.getRCoords();
	}
	#region Movement
	public void pMoveUp()
	{
		gm.player.moveUp();
	}
	
	public void pMoveDown()
	{
		gm.player.moveDown();
	}
	
	public void pMoveRight()
	{
		gm.player.moveRight();
	}
	
	public void pMoveLeft()
	{
		gm.player.moveLeft();
	}
	#endregion
}
