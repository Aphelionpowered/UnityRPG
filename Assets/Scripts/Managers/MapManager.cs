using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapManager : MonoBehaviour {
	private GameManager gm;
	public float tileSize = 1.0f;
	public Tile[,] currentMap;
	public List<Room> currentRooms;
	public GameObject map;


	void Awake () {
		gm = GetComponent<GameManager>();
	}

	public void CreateNewMap(int sizeX, int sizeZ){
		gm.mapData.CreateTileMapData(sizeX, sizeZ);
		currentMap = gm.mapData.MapData;
		currentRooms = gm.mapData.Rooms;
		BuildMesh(map, sizeX, sizeZ);
		BuildTexture(map, sizeX, sizeZ);
	}

	public Tile getTile(int x, int z, Tile[,] map) {
		return map[x,z];
	}

	public Vector3 getSpawnPoint() {
		Room room = currentRooms.FirstOrDefault();
		Vector3 location = new Vector3(room.centerX, 0, room.centerZ);
		return location;

	}

	public bool isTileMoveable(Vector3 currentPlayerPosition, Player.Direction direction) {
		int x = (int)currentPlayerPosition.x;
		int z = (int)currentPlayerPosition.z;

		switch (direction) {
		case Player.Direction.up:
			return getTile(x,z + 1, currentMap).Walkable;
		case Player.Direction.right:
			return getTile(x + 1,z, currentMap).Walkable;
		case Player.Direction.down:
			return getTile(x, z - 1, currentMap).Walkable;
		case Player.Direction.left:
			return getTile(x - 1, z, currentMap).Walkable;
		default:
			return false;
		}
	}
	
	public void BuildTexture (GameObject map, int size_X, int size_Z) { 
		int texWidth = size_X * 16;
		int texHeight = size_Z * 16;
		Texture2D mytex = new Texture2D(texWidth,texHeight);
		
		for(int z=0; z < size_Z; z++) {
			for(int x=0; x < size_X; x++){
				Tile t = getTile(x,z,currentMap);
				mytex.SetPixels(x*t.Myresolution, z*t.Myresolution, t.Myresolution, t.Myresolution, t.Graphic);
			}
		}
		mytex.filterMode = FilterMode.Point;
		mytex.wrapMode = TextureWrapMode.Clamp;
		mytex.Apply();
		MeshRenderer mesh_renderer = map.GetComponent<MeshRenderer>();
		mesh_renderer.sharedMaterials[0].mainTexture = mytex;
		Debug.Log("Done Texture!");
	}

	public void BuildMesh(GameObject map, int size_X, int size_Z) {
		//Get the total number of tiles for the given map
		int numTiles = (size_X * size_Z);
		
		//Get the total number of triangle for the given map
		int numTris = numTiles * 2;
		
		//Get Number of vertices on the x axis (+1 is for the remaining one at the end)
		int vsize_x = size_X + 1;
		
		//Get Number of vertices on the z axis (+1 is for the remaining one at the end)
		int vsize_z = size_Z + 1;
		
		//Get Number of vertices in total by multiplying the axis together
		int numVerts = vsize_x * vsize_z;
		
		// Generate the mesh data
		Vector3[] vertices = new Vector3[ numVerts ];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];
		
		//Get an array of triangleCoords from the number of triangles
		int[] triangleCoords = new int[ numTris * 3 ];

		int x, z;
		
		//Create one tile row at a time working horizontally from 0 total axis vertices
		for(z=0; z < vsize_z; z++) {
			for(x=0; x < vsize_x; x++) {
				vertices[ z * vsize_x + x ] = new Vector3( x*tileSize, 0, z*tileSize ); 
				//First row = [[0,0,0],[1,0,0],[2,0,0],[3,0,0],...]  
				//Second row = [[0,0,0],[1,0,1],[2,0,1],[3,0,1],...] 
				
				normals[ z * vsize_x + x ] = Vector3.up;
				uv[ z * vsize_x + x ] = new Vector2( (float)x / (size_X), (float)z / (size_Z) );
			}
		}
		Debug.Log ("Done Verts!");
	
		//Create one row of tiles at a time horizontally (4 unique vertices)(6 for each square to draw triangles)
		for(z=0; z < size_Z; z++) {
			for(x=0; x < size_X; x++) {
				
				
				int squareIndex = z * size_X + x;
				//0
				//1
				//2
				//3
				//Second Row
				//50
				//51
				//52
				//53
				
				int verticesIndex = squareIndex * 6;
				//0
				//6
				//12
				//18
				//Second Row
				//300
				//306
				//312
				//318
				
				triangleCoords[verticesIndex + 0] = z * vsize_x + x + 		   0;
				//0
				//1
				//2
				//Second Row
				//51
				//52
				//53
				
				triangleCoords[verticesIndex + 1] = z * vsize_x + x + vsize_x + 0;
				triangleCoords[verticesIndex + 2] = z * vsize_x + x + vsize_x + 1;			
				triangleCoords[verticesIndex + 3] = z * vsize_x + x + 		    0;
				triangleCoords[verticesIndex + 4] = z * vsize_x + x + vsize_x + 1;
				triangleCoords[verticesIndex + 5] = z * vsize_x + x + 		    1;
				//First Row
				//[[0,51,52,0,52,1],[1,52,53,1,53,2]
				//Second Row
				//[[51,102,103,51,103,52],[52,103,104,52,104,53]
				
			}
		}
		
		Debug.Log ("Done triangleCoords!");
		
		// Create a new Mesh and populate with the data
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangleCoords;
		mesh.normals = normals;
		mesh.uv = uv;
		
		// Assign our mesh to our filter/renderer/collider
		MeshFilter mf = map.GetComponent<MeshFilter>();
		MeshCollider mc = map.GetComponent<MeshCollider>();
		DestroyImmediate (mf.sharedMesh);
		DestroyImmediate (mc.sharedMesh);
		mf.sharedMesh = mesh;
		mc.sharedMesh = mesh;
		Debug.Log ("Done Mesh!");
	}	
}