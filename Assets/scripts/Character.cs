using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
	public string name;
	public MBS.tbbPlayerInfo go;

	public static Character create(string name){
		Character ret;
		ret = new Character();
		ret.name = name;
		ret.loadPrefab(name);
		return ret;
	
	}
	public MBS.tbbPlayerInfo loadPrefab(string name){

		go = Instantiate (Resources.Load<MBS.tbbPlayerInfo>("Prefabs/Characters/"+name));
		print ("g " + go.GetType().ToString());
		print ("go " + go.character_name);
	//	go.transform.position = new Vector3 (10, 10, 10);
		return go;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
