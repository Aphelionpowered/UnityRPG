using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {
	private GameManager gm;
	public float tileSize = 1.0f;
	public TileMapData currentMap;


	void Awake () {
		gm = GetComponent<GameManager>();
	}
	
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

	}
	public void CreateNewMap(int sizex, int sizez){
		TileMapData tmpmap = new TileMapData(sizex, sizez);
		currentMap = tmpmap;

	}

	public Tile getTile(int x, int y, TileMapData map) 
	{
		return map.map_data[x,y];
	}

	public bool isTileMoveable(Vector3 currentPlayerPosition, Player.Direction direction)
	{
		int x = (int)currentPlayerPosition.x;
		int z = (int)currentPlayerPosition.z;

		switch (direction)
		{
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
	
	public void BuildTexture (GameObject go, TileMapData mapinstance, int size_X, int size_Z) { 
		int texWidth = size_X * 16;
		int texHeight = size_Z * 16;
		Texture2D mytex = new Texture2D(texWidth,texHeight);
		
		for(int b=0; b < size_Z; b++) {
			for(int a=0; a < size_X; a++){
				Color[] c = getTile(a,b,currentMap).Graphic;
				//returns map_data[int x,int y]
				mytex.SetPixels(a*16, b*16, 16, 16, c);
			}
		}
		mytex.filterMode = FilterMode.Point;
		mytex.wrapMode = TextureWrapMode.Clamp;
		mytex.Apply();
		MeshRenderer mesh_renderer = go.GetComponent<MeshRenderer>();
		mesh_renderer.sharedMaterials[0].mainTexture = mytex;
		Debug.Log("Done Texture!");
	}

	public void BuildMesh(GameObject go, int size_X, int size_Z) {
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
		
		
		//a = x axis interator
		//b = z axis interator
		int a, b;
		
		//Create one tile row at a time working horizontally from 0 total axis vertices
		for(b=0; b < vsize_z; b++) {
			for(a=0; a < vsize_x; a++) {
				vertices[ b * vsize_x + a ] = new Vector3( a*tileSize, 0, b*tileSize ); 
				//First row = [[0,0,0],[1,0,0],[2,0,0],[3,0,0],...]  
				//Second row = [[0,0,0],[1,0,1],[2,0,1],[3,0,1],...] 
				
				normals[ b * vsize_x + a ] = Vector3.up;
				uv[ b * vsize_x + a ] = new Vector2( (float)a / (size_X), (float)b / (size_Z) );
			}
		}
		Debug.Log ("Done Verts!");
		
		
		//LOOP OF DEATH
		
		//Create one row of tiles at a time horizontally (4 unique vertices)(6 for each square to draw triangles)
		for(b=0; b < size_Z; b++) {
			for(a=0; a < size_X; a++) {
				
				
				int squareIndex = b * size_X + a;
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
				
				triangleCoords[verticesIndex + 0] = b * vsize_x + a + 		   0;
				//0
				//1
				//2
				//Second Row
				//51
				//52
				//53
				
				triangleCoords[verticesIndex + 1] = b * vsize_x + a + vsize_x + 0;
				triangleCoords[verticesIndex + 2] = b * vsize_x + a + vsize_x + 1;			
				triangleCoords[verticesIndex + 3] = b * vsize_x + a + 		   0;
				triangleCoords[verticesIndex + 4] = b * vsize_x + a + vsize_x + 1;
				triangleCoords[verticesIndex + 5] = b * vsize_x + a + 		   1;
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
		MeshFilter mf = go.GetComponent<MeshFilter>();
		MeshCollider mc = go.GetComponent<MeshCollider>();
		DestroyImmediate (mf.sharedMesh);
		DestroyImmediate (mc.sharedMesh);
		mf.sharedMesh = mesh;
		mc.sharedMesh = mesh;
		Debug.Log ("Done Mesh!");
	}	
}