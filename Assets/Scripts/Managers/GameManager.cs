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
	public MapManager map;
	[HideInInspector]
	public Player player;


	void Awake(){
		tileMap = GameObject.Find("TileMap").GetComponent<TileMap>();
		player = GameObject.Find("Player").GetComponent<Player>();
		playerm = GetComponent<PlayerManager>();
		gui = GetComponent<GUIManager>();
		map = GetComponent<MapManager>();
	}



	void CreateNewLevel(){
		gui.EnableDebugGUI();


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