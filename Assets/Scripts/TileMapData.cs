using UnityEngine;
using System.Collections.Generic;

public class TileMapData : MonoBehaviour {
	int size_x;
	int size_y;

	public Tile[,] map_data;
	public List<Room> rooms;
	public int maxFails = 8;
	public int maxRooms = 30;

	public Dictionary<int,Tile> tiles;

	Tile floor = new Tile(1,Tile.Kind.Floor,true,"Floor",0);
	Tile wall = new Tile(2,Tile.Kind.Wall,false,"Wall",1);
	Tile stone = new Tile(3,Tile.Kind.Stone,false,"Stone",2);	

	/*
	public List<Vector2> getWalkableTiles(){
		foreach(Tile t in map_data) {
			if(t.Walkable){

			}
		}

	}
	*/

	public TileMapData(int size_x, int size_y) {
		
		Room r;
		this.size_x = size_x;
		this.size_y = size_y;
		
		map_data = new Tile[size_x,size_y];
		
		for(int x=0;x<size_x;x++) {
			for(int y=0;y<size_y;y++) {
				map_data[x,y] = stone;
			}
		}

		rooms = new List<Room>();

		while(rooms.Count < maxRooms) {
			int rsx = Random.Range(4,14);
			int rsy = Random.Range(4,10);
			
			r = new Room();
			r.left = Random.Range(0, size_x - rsx);
			r.top = Random.Range(0, size_y-rsy);
			r.width = rsx;
			r.height = rsy;

			if(!RoomCollides(r)) {
					rooms.Add (r);
			}else {
				maxFails--;
				if(maxFails <=0)
					break;
			}		
		}
		
		foreach(Room rr in rooms) {
			MakeRoom(rr);
		}	
		
		for(int i=0; i < rooms.Count; i++) {
			if(!rooms[i].isConnected) {
				int j = Random.Range(1, rooms.Count);
				MakeCorridor(rooms[i], rooms[(i + j) % rooms.Count ]);
			}
		}
		
		MakeWalls();	
	}

	bool RoomCollides(Room r) {
		foreach(Room r2 in rooms) {
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
					map_data[r.left+x,r.top+y] = wall;
				}
				else {
					map_data[r.left+x,r.top+y] = floor;
				}
			}
		}	
	}
	
	void MakeCorridor(Room r1, Room r2) {
		int x = r1.center_x;
		int y = r1.center_y;
		
		while( x != r2.center_x) {
			map_data[x,y] = floor;
			
			x += x < r2.center_x ? 1 : -1;
		}
		
		while( y != r2.center_y ) {
			map_data[x,y] = floor;
			
			y += y < r2.center_y ? 1 : -1;
		}
		
		r1.isConnected = true;
		r2.isConnected = true;
		
	}
	
	void MakeWalls() {
		for(int x=0; x< size_x;x++) {
			for(int y=0; y< size_y;y++) {
				if(map_data[x,y]==stone && HasAdjacentFloor(x,y)) {
					map_data[x,y]= wall;
				}
			}
		}
	}
	
	bool HasAdjacentFloor(int x, int y) {
		if( x > 0 && map_data[x-1,y] == floor )
			return true;
		if( x < size_x-1 && map_data[x+1,y] == floor )
			return true;
		if( y > 0 && map_data[x,y-1] == floor )
			return true;
		if( y < size_y-1 && map_data[x,y+1] == floor )
			return true;
		
		if( x > 0 && y > 0 && map_data[x-1,y-1] == floor )
			return true;
		if( x < size_x-1 && y > 0 && map_data[x+1,y-1] == floor )
			return true;
		
		if( x > 0 && y < size_y-1 && map_data[x-1,y+1] == floor )
			return true;
		if( x < size_x-1 && y < size_y-1 && map_data[x+1,y+1] == floor )
			return true;

		return false;
	}
}