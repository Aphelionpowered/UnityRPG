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

	//MAP
	private int _SizeX;
	private int _SizeZ;
	private Tile[,] _MapData;
	private int _MyMaxRooms = 50;
	private int _MyMaxFails = 15;
	private bool isExitSpawned;
	private Vector2 RealExit;

	//ROOM
	private List<Room> _Rooms;
	private Room _CurrentRoom;

	//CORRIDOR
	private List<Vector2> _Corridors;

	private Tile floor, wall, stone, unknown;

	private int numSpawnedRooms = 0;
	private Vector2 _ExitCoords;
	private List<Vector2> _GoodDoors;
	private List<Vector2> _PossibleDoors;

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

	public Tile[,] CurrentMapData
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
		_PossibleDoors = new List<Vector2>();

		FillMap();
		CreateRooms();

		foreach(Room rr in _Rooms)
		{
			MakeRoom(rr);
		}

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

			if( r.hasDoor){
				r.DoorX = Random.Range(r.left, r.left + r.width - 1);
			}
			
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
					_PossibleDoors.Add(new Vector2(r.left+x,r.bottom+z));
				}
				else
				{
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

			}
			while( z != r2.centerZ ) 
			{
				_MapData[x,z] = floor;
				z += z < r2.centerZ ? 1 : -1;
			}
		r1.isConnected = true;
		r2.isConnected = true;	
	}
	
	void MakeWalls()
	{
		for(int x=0; x< _SizeX;x++)
		{
			for(int z=0; z< _SizeZ;z++)
			{
				if(_MapData[x,z]==stone && HasAdjacentTile(x,z,floor))
				{
					_MapData[x,z]= wall;
				}
			}
		}
	}

	void MakeDoor()
	{
		foreach(Vector2 possibleDoor in _PossibleDoors)
		{
			int x = (int)possibleDoor.x;
			int z = (int)possibleDoor.y;
			if(HasParallelTile(x,z,wall) && _MapData[x,z] == floor)
			{
				cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.position = new Vector3(x + 0.5f , 0.5f, z + 0.5f);
			}
		}
	}

		
	/*
	void MakeExit() {
		for(int x=0; x< _SizeX;x++)
		{
			for(int z=0; z< _SizeZ;z++)
			{
				if(_MapData[x,z]==wall && ValidExit(x,z))
				{
					_ExitCoords = new Vector2(x,z);
					_PossibleExits.Add(_ExitCoords);
				}
			}
		}
		Vector2 RealExit = _PossibleExits[Random.Range(0,_PossibleExits.Count)];
		_MapData[(int)RealExit.x,(int)RealExit.y] = unknown;
	}
	*/
	
	bool HasAdjacentTile(int x, int z, Tile t)
	{
		if( x > 0 && _MapData[x-1,z] == t )
			return true;
		if( x < _SizeX-1 && _MapData[x+1,z] == t )
			return true;
		if( z > 0 && _MapData[x,z-1] == t )
			return true;
		if( z < _SizeZ-1 && _MapData[x,z+1] == t )
			return true;
		if( x > 0 && z > 0 && _MapData[x-1,z-1] == t )
			return true;
		if( x < _SizeX - 1 && z > 0 && _MapData[x+1,z-1] == t )
			return true;
		if( x > 0 && z < _SizeZ-1 && _MapData[x-1,z+1] == t )
			return true;
		if( x < _SizeX-1 && z < _SizeZ-1 && _MapData[x+1,z+1] == t )
			return true;
		return false;
	}

	bool HasAdjacentRoom(int x, int z)
	{
		return true;

	}



	bool HasParallelTile(int x, int z, Tile t)
	{
		if( _MapData[x-1,z] == t && _MapData[x+1,z] == t )
			return true;
		if( _MapData[x,z-1] == t && _MapData[x,z+1] == t)
			return true;
		return false;
	}

	bool DoubleDoor(int x, int z){
		if( _MapData[x-2,z] == wall && _MapData[x+2,z] == wall )
		{
			if(HasParallelTile(x,z,floor))
			{
				return true;
			}
		}
		if( _MapData[x,z-2] == wall && _MapData[x,z+2] == wall)
			if(HasParallelTile(x,z,floor))
			{
				return true;
			}
		return false;
	}


	bool NotCornerTile(int x, int z, Tile t)
	{
		if( x > 0 && _MapData[x-1,z] == t )
			return true;
		if( x < _SizeX-1 && _MapData[x+1,z] == t )
			return true;
		if( z > 0 && _MapData[x,z-1] == t )
			return true;
		if( z < _SizeZ-1 && _MapData[x,z+1] == t )
			return true;
		return false;
	}

}