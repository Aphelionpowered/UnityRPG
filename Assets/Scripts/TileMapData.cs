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
	private int _SizeX;
	private int _SizeZ;
	private Tile[,] _MapData;
	private List<Room> _Rooms;
	private int _MyMaxFails = 10;
	private int _MyMaxRooms = 30;
	private Tile floor, wall, stone, unknown;
	private bool isExitSpawned;
	private int numSpawnedRooms = 0;
	private List<Vector2> _PossibleExits;
	private Vector2 _ExitCoords;
	private List<Vector2> _PossibleDoors;
	private Vector2 _DoorCoords;
	private Vector2 RealExit;
	GameObject cube;

	public int MaxFails
	{
		get {
			return _MyMaxFails;
		}
		set {
			_MyMaxFails = value;
		}
	}
	public int MaxRooms
	{
		get {
			return _MyMaxRooms;
		}
		set {
			_MyMaxRooms = value;
		}
	}

	public Tile[,] MapData
	{
		get {
			return _MapData;
		}
		set {
			_MapData = value;
		}
	}
	public List<Room> Rooms
	{
		get {
			return _Rooms;
		}
		set {
			_Rooms = value;
		}
	}

	bool randomBoolean()
	{
		return (Random.value > 0.5f); 
	}

	void Awake ()
	{
		gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		unknown = new Tile(0,Tile.Kind.Unknown,false,"Unknown",99);
		floor = new Tile(1,Tile.Kind.Floor,true,"Floor",0);
		wall = new Tile(2,Tile.Kind.Wall,false,"Wall",1);
		stone = new Tile(3,Tile.Kind.Stone,false,"Stone",2);
	}

	public void CreateTileMapData(int sizeX, int sizeZ)
	{
		this._SizeX = sizeX;
		this._SizeZ = sizeZ;

		_MapData = new Tile[sizeX,sizeZ];
		_Rooms = new List<Room>();
		_PossibleExits = new List<Vector2>();
		_PossibleDoors = new List<Vector2>();

		FillMap();
		CreateRooms();
		foreach(Room rr in _Rooms)
		{
			MakeRoom(rr);
		}
		MakeExit();
		for(int i=0; i < _Rooms.Count; i++)
		{
			if(!_Rooms[i].isConnected)
			{
				int j = Random.Range(1, _Rooms.Count);
				MakeCorridor(_Rooms[i], _Rooms[(i + j) % _Rooms.Count ]);
			}
		}
		MakeWalls();
		MakeDoor();
	}

	bool RoomCollides(Room r)
	{
		foreach(Room r2 in _Rooms)
		{
			if(r.CollidesWith(r2))
			{
				return true;
			}
		}	
	return false;
	}

	void FillMap()
	{
		for(int x=0;x < _SizeX;x++) 
		{
			for(int z=0;z < _SizeZ;z++) 
			{
				_MapData[x,z] = stone;
			}
		}
	}

	void CreateRooms()
	{
		Room r;
		
		while(_Rooms.Count < MaxRooms)
		{
			int roomSizeX = Random.Range(4,14);
			int roomSizeZ = Random.Range(4,10);
			
			r = new Room();
			r.left = Random.Range(0, _SizeX - roomSizeX);
			r.bottom = Random.Range(0, _SizeZ - roomSizeZ);
			r.width = roomSizeX;
			r.height = roomSizeZ;
			r.hasDoor = true;
			
			if(!RoomCollides(r))
			{
				_Rooms.Add (r);
			} 
			else 
			{
				MaxFails--;
				if(MaxFails <=0)
					break;
			}		
		}
	}

	void MakeRoom(Room r)
	{	
		for(int x=0; x < r.width; x++)
		{
			for(int z=0; z < r.height; z++)
			{
				if(x==0 || x == r.width-1 || z==0 || z == r.height-1 || x<0 || z< 0)
				{
					_MapData[r.left+x,r.bottom+z] = wall;

				} else {
					_MapData[r.left+x,r.bottom+z] = floor;
				}
				
			}
		}	
	}
	
	void MakeCorridor(Room r1, Room r2)
	{
			int x = r1.centerX;
			int z = r1.centerZ;

			while( x != r2.centerX )
			{
				_MapData[x,z] = floor;
				x += x < r2.centerX ? 1 : -1;
			if(_MapData[x,z] == wall && (x== r1.left || x == r2.left || x== r1.left + r1.width - 1 || x == r2.left + r2.width - 1)){
					_DoorCoords = new Vector2(x,z);
					_PossibleDoors.Add(_DoorCoords);
				}
			}
			while( z != r2.centerZ ) 
			{
				_MapData[x,z] = floor;
				z += z < r2.centerZ ? 1 : -1;
			if(_MapData[x,z] == wall && (z == r1.bottom || z == r2.bottom || z == r1.bottom + r1.height - 1 || z == r2.bottom + r2.height - 1)){
					_DoorCoords = new Vector2(x,z);
					_PossibleDoors.Add(_DoorCoords);
				}
			}
		r1.isConnected = true;
		r2.isConnected = true;	
	}
	
	void MakeWalls() {
		for(int x=0; x< _SizeX;x++) {
			for(int z=0; z< _SizeZ;z++) {
				if(_MapData[x,z]==stone && HasAdjacentFloor(x,z)) {
					_MapData[x,z]= wall;
				}
			}
		}
	}

	void MakeDoor()
	{
		foreach( Vector2 doorCoords in _PossibleDoors)
		{
			cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.position = new Vector3(doorCoords.x + 0.5f , 0.5f, doorCoords.y + 0.5f);
		}
	}


	void MakeExit() {
		for(int x=0; x< _SizeX;x++) {
			for(int z=0; z< _SizeZ;z++) {
				if(_MapData[x,z]==wall && ValidDoor(x,z)) {
					_ExitCoords = new Vector2(x,z);
					_PossibleExits.Add(_ExitCoords);
				}
			}
		}
		Vector2 RealExit = _PossibleExits[Random.Range(0,_PossibleExits.Count)];
		_MapData[(int)RealExit.x,(int)RealExit.y] = unknown;
	}
	
	bool HasAdjacentFloor(int x, int z) {
		if( x > 0 && _MapData[x-1,z] == floor )
			return true;
		if( x < _SizeX-1 && _MapData[x+1,z] == floor )
			return true;
		if( z > 0 && _MapData[x,z-1] == floor )
			return true;
		if( z < _SizeZ-1 && _MapData[x,z+1] == floor )
			return true;
		if( x > 0 && z > 0 && _MapData[x-1,z-1] == floor )
			return true;
		if( x < _SizeX - 1 && z > 0 && _MapData[x+1,z-1] == floor )
			return true;
		if( x > 0 && z < _SizeZ-1 && _MapData[x-1,z+1] == floor )
			return true;
		if( x < _SizeX-1 && z < _SizeZ-1 && _MapData[x+1,z+1] == floor )
			return true;
		return false;
	}
	/*
	bool ValidDoor(int x, int z) {
		if( x > 0 && _MapData[x-1,z] == floor )
			return true;
		if( x < _SizeX-1 && _MapData[x+1,z] == floor )
			return true;
		if( z > 0 && _MapData[x,z-1] == floor )
			return true;
		if( z < _SizeZ-1 && _MapData[x,z+1] == floor )
			return true;
		return false;
	}
	*/

	bool ValidDoor(int x, int z) {
		if( x > 0 && _MapData[x-1,z] == floor )
			return true;
		if( x < _SizeX-1 && _MapData[x+1,z] == floor )
			return true;
		if( z > 0 && _MapData[x,z-1] == floor )
			return true;
		if( z < _SizeZ-1 && _MapData[x,z+1] == floor )
			return true;
		return false;
	}
}