using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {
	private GameManager gm;
	public GameObject debugGUI;


	void Awake () {
		gm = GetComponent<GameManager>();
	}

	public void EnableDebugGUI(){
		debugGUI.SetActive(true);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
