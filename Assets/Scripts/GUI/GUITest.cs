using UnityEngine;
using System.Collections;

public class GUITest : MonoBehaviour {
	private GameManager gm;

	void Awake(){
		gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager> ();
	}

	void OnGUI(){
		GUILayout.BeginArea( new Rect (0, 0, Screen.width, Screen.height));
		GUILayout.BeginHorizontal();

		//Column 1
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		GUILayout.Label("X");
		if (GUILayout.Button ("Set Tile to 5")){
			gm.tileMap.size_X = 5;
		}
		if (GUILayout.Button ("Set Tile to 10")){
			gm.tileMap.size_X = 10;
		}
		if (GUILayout.Button ("Set Tile to 50")){
			gm.tileMap.size_X = 50;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();

		//Column 2
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Z");
		if (GUILayout.Button ("Set Tile to 5")){
			gm.tileMap.size_Z = 5;
		}
		if (GUILayout.Button ("Set Tile to 10")){
			gm.tileMap.size_Z = 10;
		}
		if (GUILayout.Button ("Set Tile to 50")){
			gm.tileMap.size_Z = 50;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();

		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		//Player Pos Info
		GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
				GUILayout.Label("Current Player Position");
			GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
						GUILayout.BeginVertical();
							GUILayout.Label("X");
							GUILayout.Label(gm.playerManager.PGetCoords().x.ToString());
						GUILayout.EndVertical();
						GUILayout.BeginVertical();
							GUILayout.Label("Y");
							GUILayout.Label(gm.playerManager.PGetCoords().y.ToString());
						GUILayout.EndVertical();
						GUILayout.BeginVertical();
							GUILayout.Label("Z");
							GUILayout.Label(gm.playerManager.PGetCoords().z.ToString());
						GUILayout.EndVertical();
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
					//GUILayout.Label("Player Current TileID: " + gm.playerm.TileID.ToString());
				GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
}