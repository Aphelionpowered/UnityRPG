using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	[HideInInspector]
	public TileMap tileMap;
	[HideInInspector]
	public PlayerManager playerManager;
	[HideInInspector]
	public GUIManager gui;
	[HideInInspector]
	public MapManager mapManager;
	[HideInInspector]
	public Player player;
	[HideInInspector]
	public TileMapData mapData;


	void Awake(){
		tileMap = GameObject.Find("Map").GetComponent<TileMap>();
		player = GameObject.Find("Player").GetComponent<Player>();
		playerManager = GetComponent<PlayerManager>();
		gui = GetComponent<GUIManager>();
		mapManager = GetComponent<MapManager>();
		mapData = GameObject.Find("Map").GetComponent<TileMapData>();
	}

	void Start()
	{
		CreateNewLevel(50,50);
		playerManager.SpawnPlayer();
	}



	void CreateNewLevel(int sizeX, int sizeY){
		gui.EnableDebugGUI();
		mapManager.CreateNewMap(sizeX, sizeY);


	}
}


	/*
NewLevel
  SetupGUI - Turn Game GUI
  CreateMap - Create MapData
            - Store MapData
            - Draw MapData
 SetupPlayer - Put player in map
             - Setup Camera
*/	