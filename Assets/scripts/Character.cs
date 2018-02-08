using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NJG;

public class Character : MonoBehaviour {
	public string name;
	public MBS.tbbPlayerInfo go; // gameobject
	public CharacterSetting setting;

	public static Character create(CharacterSetting cs){

		Character ret;
		ret = new Character();
		ret.setting = cs;
		ret.name = cs.name;
		ret.loadPrefab(cs.name);
		return ret;
	
	}
	public MBS.tbbPlayerInfo loadPrefab(string name){

		go = Instantiate (Resources.Load<MBS.tbbPlayerInfo>("Prefabs/Characters/"+name));
		print ("g " + go.GetType().ToString());
		print ("go " + go.character_name);
	//	go.transform.position = new Vector3 (10, 10, 10);

		// attach script for mapitem
		//We need to fetch the Type
		//System.Type MyScriptType = System.Type.GetType ("MapItem" + ",Assembly-CSharp");
		//print ("myscripttype:" + MyScriptType.GetType().ToString());
		//Now that we have the Type we can use it to Add Component
		MapItem mi = go.gameObject.AddComponent<MapItem>();
		//MapItem mi = go.gameObject.GetComponent<MapItem>;
		mi.type = 1;
		go.gameObject.AddComponent<BoxCollider> ();
		go.gameObject.AddComponent<CharacterController> ();
		return go;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
