using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSetting : MonoBehaviour{
	public string name;
	public string prefab_name;
	public Dictionary<string, string> prop;
	public bool clonable = false;


	public int movability=1;
	public int attack = 1;
	public int knowledge = 1;
	public int maxhp = 100;
	public int maxmp = 100;

	public CharacterSetting()
	{
		
	}

	public CharacterSetting(string name, string prefab=""){
		this.name = name;
		if (prefab == "")
			this.prefab_name = name;
		else
			this.prefab_name = prefab;
		prop = new Dictionary<string, string> ();
	}
	public void set(string name, string value){
		prop [name] = value;
	}
}