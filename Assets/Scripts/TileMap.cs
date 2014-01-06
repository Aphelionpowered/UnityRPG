using UnityEngine;
using System.Collections;

public class TileMap : MonoBehaviour {
	private int _size_X = 50;
	private int _size_Z = 50;
	private GameManager gm;

	public int size_X {
		get {
			return _size_X;
		}
		set {
			_size_X = value;
		}
	}
	
	public int size_Z {
		get {
			return _size_Z;
		}
		set {
			_size_Z = value;
		}
	}

	void Awake() {
		gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}
	// Use this for initialization
	void Start () {

	}
	// Update is called once per frame
	void Update () {
	
	}
}
