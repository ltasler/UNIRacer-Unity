using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasScript : MonoBehaviour {
	
	GameObject flat_terrain_button;
	GameObject rough_terrain_button;
	
	// Use this for initialization
	void Start () {
		flat_terrain_button = GameObject.Find("flat_terrain");
		rough_terrain_button = GameObject.Find("rough_terrain");
		flat_terrain_button.SetActive(false); 
		rough_terrain_button.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
