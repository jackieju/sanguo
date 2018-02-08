using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSetting{
	public string name;
	public Dictionary<string, string> prop;
	public CharacterSetting(string name){
		this.name = name;
	}
	public void set(string name, string value){
		prop [name] = value;
	}
}
public class FactionSetting : MonoBehaviour {
	//public List<string> character_names;
	public Dictionary<string, CharacterSetting> char_list= new Dictionary<string, CharacterSetting>();

	public void add_character(CharacterSetting cs){
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
