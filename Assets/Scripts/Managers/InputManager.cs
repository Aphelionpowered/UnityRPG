using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	private GameManager gm;

	void Awake () {
		gm = GetComponent<GameManager>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W)){
			gm.playerManager.pMoveUp();
		}
		if (Input.GetKey (KeyCode.S)){
			gm.playerManager.pMoveDown();
		}
		if (Input.GetKey (KeyCode.A)){
			gm.playerManager.pMoveLeft();
		}
		if (Input.GetKey (KeyCode.D)){
			gm.playerManager.pMoveRight();
			}
		}
	}
