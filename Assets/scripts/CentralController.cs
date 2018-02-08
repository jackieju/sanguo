using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class CentralController : MonoBehaviour {

	public static CentralController inst;
	public int FACTION_NUMBER = 1;
	public Faction[] factions;
	public FactionSetting[] fs;

	public GameObject currentSelectedChar;
	public Terrain terrain;
	public int state;// 100: char selected

	void Awake(){
		inst = this;

		fs = new FactionSetting[FACTION_NUMBER];
		fs [0] = new FactionSetting ();
		factions = new Faction[FACTION_NUMBER];

		print ("fs" + fs.GetType().ToString());


	}

	// Use this for initialization
	void Start () {
		// prepare testing data


		CharacterSetting _cs = new CharacterSetting("Ethan");
		fs[0].add_character(_cs);
		_cs = new CharacterSetting("RedSamurai");
		fs[0].add_character(_cs);

		// create factions object from factionsettings

		for (int i = 0; i < fs.Length; i++) {
			factions [i] = new Faction ();

			CharacterSetting[] char_list = fs [i].get_character_list ();

			// create characters
			Character[] characters;
			characters = new Character[char_list.Length]; 
			/*characters[0] = Character.create("Ethan");
			characters[0] = Character.create("RedSamurai");*/
			for (int j = 0; j < char_list.Length; j++) {
				CharacterSetting cs = char_list [j];
				characters [j] = Character.create (cs);

			}

			factions [i].loadCharacters (characters);
		}
	}
	
	// Update is called once per frame
	void Update () {


	}
}
