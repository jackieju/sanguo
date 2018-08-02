using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSetting : ScriptableObject{
	public string name;
	public string prefab_name;
	public Dictionary<string, string> prop;
	public bool clonable = false;


	public int movability=1;
	public int attack = 1;
	public int knowledge = 1;
	public int maxhp = 100;
	public int maxmp = 100;

	public string[] kill;
	public string[] bekilled;
	public string desc;
	public Dictionary<string, int> nations;// nation and his officer level

	public string head_image="AvatarEthan";

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
	public virtual void  setup(){
		Debug.Log ("===>call setup");
	}
	public void after_setup(){
		string s_kill = "";
		// prepare its kill and bekilled
		if (kill != null) {
			string[] ar_kill = new string[kill.Length];
			//skill = string.Join (", ", kill);
			for (int i = 0; i < kill.Length; i++) {
				CharacterSetting cs = CentralController.load_charsetting (kill [i]);
				cs.setup ();
				Debug.Log ("==>kill:" + cs.name);
				ar_kill [i] = cs.name;
			}
			s_kill = string.Join (" ", ar_kill);
		}
		desc = desc + "\n" + "克制:   " + s_kill;
	}
}