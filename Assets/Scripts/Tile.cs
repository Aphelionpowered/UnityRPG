using UnityEngine;
using System.Collections;

public class Tile {
	//Tile Graphic Storage
	Texture2D terrainTiles = (Texture2D)Resources.Load("testapple");
	//END
	
	public int Id;
	public Kind _Kind;
	public int Myresolution;
	public float Size;
	public bool Walkable;
	public string Name;
	public Color[] Graphic;
	private int _Height;
	
	public enum Kind {
		Unknown,
		Floor,
		Wall,
		Stone,
	}
	
	public int Height {
		get { return _Height; }
		set {
			Walkable = value == 0;
			_Height = value;
		}
	}

	public Tile(int id, Kind kind, bool walkable, string name, int height) {
		Id = id;
		_Kind = kind;
		Myresolution = 16;
		Size = 1.0f;
		Walkable = walkable;
		Name = name;
		Graphic = tilegraphic(id);
		Height = height;
	}

	Color[] tilegraphic(int tileid){
		//int tileid =(int) k;
		int numTilesPerRow = terrainTiles.width / 16;
		int numRows = terrainTiles.height / 16;
		Color[][] tiles = new Color[numTilesPerRow * numRows][];
		for(int b=0; b < numRows; b++) {
			for(int a=0; a < numTilesPerRow; a++){
				tiles[b*numTilesPerRow + a] = terrainTiles.GetPixels( a*Myresolution, b*Myresolution, Myresolution, Myresolution);
			}
		}
		Color[] c = tiles[tileid];
		return c;
	}
}