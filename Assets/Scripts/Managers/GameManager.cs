using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	[HideInInspector]
	public TileMap tileMap;
	[HideInInspector]
	public PlayerManager playerm;
	[HideInInspector]
	public GUIManager gui;
	[HideInInspector]
	public MapManager mapManager;
	[HideInInspector]
	public Player player;


	void Awake(){
		tileMap = GameObject.Find("Map").GetComponent<TileMap>();
		player = GameObject.Find("Player").GetComponent<Player>();
		playerm = GetComponent<PlayerManager>();
		gui = GetComponent<GUIManager>();
		mapManager = GetComponent<MapManager>();
	}



	void CreateNewLevel(){
		gui.EnableDebugGUI();
		mapManager.CreateNewMap(50,50); 


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