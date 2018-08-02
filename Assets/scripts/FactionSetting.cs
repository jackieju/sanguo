using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FactionSetting : MonoBehaviour {
	public int userid;
	public int max_operation_number = 1;

	//public List<string> character_names;
	public Dictionary<string, CharacterSetting> char_list= new Dictionary<string, CharacterSetting>();

	public void add_character(CharacterSetting cs){
		if (char_list.ContainsKey (cs.name)) {
			if (cs.clonable)
				char_list [cs.name + "_" + get_character_list ().Length] = cs;
		}
		else
			char_list [cs.name] = cs;
	}
	public CharacterSetting[] get_character_list(){
		return new List<CharacterSetting>(char_list.Values).ToArray();
	}
	public string[] get_character_name_list(){
		return new List<string>(char_list.Keys).ToArray();
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
