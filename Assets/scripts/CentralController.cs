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
	private Vector3 terrainCenterPoint;

	public Vector3 getTerrainCenterPoint(){
		print ("tc3:" + terrain.terrainData.size);
		print ("tc4:" + terrainCenterPoint);

		if (terrainCenterPoint == null || terrainCenterPoint == Vector3.zero){
			terrainCenterPoint = new Vector3(terrain.terrainData.size.x/2, 0, terrain.terrainData.size.z/2);
		}
		print ("tc2:" + terrainCenterPoint);
		return terrainCenterPoint;
	}

	void Awake(){
		inst = this;

		fs = new FactionSetting[FACTION_NUMBER];
		for (int i = 0; i < FACTION_NUMBER; i++) {
			FactionSetting _fs  = new FactionSetting ();
			fs [i] = _fs;

		}
		factions = new Faction[FACTION_NUMBER];

		print ("fs" + fs.GetType().ToString());


	}

	// Use this for initialization
	void Start () {
		// prepare testing data

		// create faction settings for testing
		print ("fs size :" + FACTION_NUMBER);
		// faction settings should be passed in runtime
		// character setting is pre-designed in game and also contain data for user specific hero settings, e.g. position, experience, ming-wen addings
		CharacterSetting _cs = new CharacterSetting("Ethan"); // create hero named Ethan
		fs[0].add_character(_cs);
		_cs = new CharacterSetting("RedSamurai"); // create hero "redsamurai"
		fs[0].add_character(_cs);


		if (FACTION_NUMBER > 1) {
			_cs = new CharacterSetting ("RedSamurai");
			_cs.clonable = true;
			fs [1].add_character (_cs);
			fs [1].add_character (_cs);

			if (FACTION_NUMBER > 2) {
				_cs = new CharacterSetting ("Ethan");
				_cs.clonable = true;
				fs [2].add_character (_cs);
				fs [2].add_character (_cs);

				if (FACTION_NUMBER > 3) {
					_cs = new CharacterSetting ("RedSamurai");
					_cs.clonable = true;
					fs [3].add_character (_cs);
					fs [3].add_character (_cs);
				}
			}
		}
		// create factions object according factionsettings

		for (int i = 0; i < fs.Length; i++) {
			Faction f  = new Faction (fs [i]);
			f.factionID = i;
			factions [i] = f;
			loadFactionCharacters (factions [i], fs [i]);
		
		}
	}

	protected void loadFactionCharacters( Faction f, FactionSetting fs) {
		CharacterSetting[] char_list = fs.get_character_list ();
		print ("char_list size:" + char_list.Length);
		// create characters
		Character[] characters;
		characters = new Character[char_list.Length]; 
		/*characters[0] = Character.create("Ethan");
			characters[0] = Character.create("RedSamurai");*/
		for (int j = 0; j < char_list.Length; j++) {
			CharacterSetting cs = char_list [j];
			print ("create ch " + j);
			//characters [j] = Character.create (cs);
			characters [j] = Character.load (cs);

		}
		f.deployCharacters (characters);
		//f.loadCharacters (characters);
	}
	
	// Update is called once per frame
	void Update () {


	}
}
