using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/*
 * Global:
 * MyVariableBanana
 * 
 * Local:
 * myVariableBanana
 * 
 * Private:
 * _MyVariableBanana
 * 
 */

public class TileMapData : MonoBehaviour {
	private GameManager gm;
	private int _Size_X;
	private int _Size_Y;
	private Tile[,] _MapData;
	private List<Room> _Rooms;
	private int _MyMaxFails = 10;
	private int _MyMaxRooms = 25;
	private Tile floor, wall, stone;

	public int MaxFails {
		get {
			return _MyMaxFails;
		}
		set {
			_MyMaxFails = value;
		}
	}
	public int MaxRooms {
		get {
			return _MyMaxRooms;
		}
		set {
			_MyMaxRooms = value;
		}
	}

	public Tile[,] MapData {
		get {
			return _MapData;
		}
		set {
			_MapData = value;
		}
	}
	public List<Room> Rooms {
		get {
			return _Rooms;
		}
		set {
			_Rooms = value;
		}
	}

	void Awake (){
		gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

	void Start(){
		floor = new Tile(1,Tile.Kind.Floor,true,"Floor",0);
		wall = new Tile(2,Tile.Kind.Wall,false,"Wall",1);
		stone = new Tile(3,Tile.Kind.Stone,false,"Stone",2);
	}

	public void CreateTileMapData(int size_X, int size_Y) {
		this._Size_X = size_X;
		this._Size_Y = size_Y;

		_MapData = new Tile[size_X,size_Y];

		for(int x=0;x<size_X;x++) {
			for(int y=0;y<size_Y;y++) {
				_MapData[x,y] = stone;
			}
		}

		_Rooms = new List<Room>();
		Room r;
		
		while(_Rooms.Count < MaxRooms) {
			int rsx = Random.Range(4,14);
			int rsy = Random.Range(4,10);
			
			r = new Room();
			r.left = Random.Range(0, _Size_X - rsx);
			r.top = Random.Range(0, _Size_Y - rsy);
			r.width = rsx;
			r.height = rsy;
			
			if(!RoomCollides(r)) {
				_Rooms.Add (r);
			}else {
				MaxFails--;
				if(MaxFails <=0)
					break;
			}		
		}

		foreach(Room rr in _Rooms) {
			MakeRoom(rr);
		}

		for(int i=0; i < _Rooms.Count; i++) {
			if(!_Rooms[i].isConnected) {
				int j = Random.Range(1, _Rooms.Count);
				MakeCorridor(_Rooms[i], _Rooms[(i + j) % _Rooms.Count ]);
			}
		}

		MakeWalls();
	}

	bool RoomCollides(Room r) {
		foreach(Room r2 in _Rooms) {
			if(r.CollidesWith(r2)) {
				return true;
			}
		}	
	return false;
	}

	void MakeRoom(Room r) {	
		for(int x=0; x < r.width; x++) {
			for(int y=0; y < r.height; y++){
				if(x==0 || x == r.width-1 || y==0 || y == r.height-1 || x<0 || y< 0) {
					_MapData[r.left+x,r.top+y] = wall;
				}
				else {
					_MapData[r.left+x,r.top+y] = floor;
				}
			}
		}	
	}
	
	void MakeCorridor(Room r1, Room r2) {
		int x = r1.centerX;
		int y = r1.centerZ;
		
		while( x != r2.centerX ) {
			_MapData[x,y] = floor;
			x += x < r2.centerX ? 1 : -1;
		}
		while( y != r2.centerZ ) {
			_MapData[x,y] = floor;
			y += y < r2.centerZ ? 1 : -1;
		}
		r1.isConnected = true;
		r2.isConnected = true;	
	}
	
	void MakeWalls() {
		for(int x=0; x< _Size_X;x++) {
			for(int y=0; y< _Size_Y;y++) {
				if(_MapData[x,y]==stone && HasAdjacentFloor(x,y)) {
					_MapData[x,y]= wall;
				}
			}
		}
	}
	
	bool HasAdjacentFloor(int x, int y) {
		if( x > 0 && _MapData[x-1,y] == floor )
			return true;
		if( x < _Size_X-1 && _MapData[x+1,y] == floor )
			return true;
		if( y > 0 && _MapData[x,y-1] == floor )
			return true;
		if( y < _Size_Y-1 && _MapData[x,y+1] == floor )
			return true;
		if( x > 0 && y > 0 && _MapData[x-1,y-1] == floor )
			return true;
		if( x < _Size_X - 1 && y > 0 && _MapData[x+1,y-1] == floor )
			return true;
		if( x > 0 && y < _Size_Y-1 && _MapData[x-1,y+1] == floor )
			return true;
		if( x < _Size_X-1 && y < _Size_Y-1 && _MapData[x+1,y+1] == floor )
			return true;

		return false;
	}
}