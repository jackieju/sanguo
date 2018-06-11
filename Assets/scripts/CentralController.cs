using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI;

public class CentralController : MonoBehaviour {


	// for update property panel 
	public Text npc_name;
	public Image npc_attack;
	public Image npc_int;
	public RawImage npc_image;
	public Text npc_desc;

	public GameObject pref1;
	public bool gameworld_ready = false;
	public static CentralController inst;
	public int FACTION_NUMBER = 1;
	public Faction[] factions;
	public FactionSetting[] fs;

	public GameObject currentSelectedChar;
	public Canvas canvas;
	public Terrain terrain;
	public Camera uiCamera;
	public int state;// 100: char selected
	private Vector3 terrainCenterPoint;
	public static float gridCellSize = 4;
	private TerrainGrid _tg; // do not access this variable directly, use getGlobalTerainGrid instead please

	public GameObject char_panel; // character attributes panel

	private Color[] colors = {Color.white, Color.red, Color.yellow, Color.blue};

	public TerrainGrid getGlobalTerainGrid(){
		if (_tg != null)
			return _tg;
		else
			_tg = GameObject.Find("GlobalTerainGrid").GetComponent<TerrainGrid>();
		return _tg;
	}
	public GameObject[] getGrids(){
		TerrainGrid gtg = getGlobalTerainGrid ();
		return gtg.getCells();
	}
	public Vector3 getCordFromPos(Vector2 pos){
		//Vector2 size = terrain.terrainData.size;
		//print("gridCellSize:"+gridCellSize);
		return new Vector3 (pos.x*gridCellSize + gridCellSize/2, 0, pos.y*gridCellSize+gridCellSize/2);
	}
	public Vector3 getCordFromPos(int x, int y){
		//Vector2 size = terrain.terrainData.size;
		return new Vector3 (x*gridCellSize + gridCellSize/2, 0, y*gridCellSize+gridCellSize/2);
	}
	public Vector2 getPosFromCord(Vector3 cord){
		return new Vector2 ((int)(cord.x /gridCellSize), (int)(cord.z /gridCellSize));

	}

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
		//colors = new Color[FACTION_NUMBER];

		print ("fs" + fs.GetType().ToString());


	}
	// !!! wrong, need rotate according to faction id
	public int _move_char(Character c, Vector2 pos){

		c.transform.position = getCordFromPos((int)pos.x, (int)pos.y);


		//print ("pos:" + pos);
		//print ("cell:" + getGlobalTerainGrid ().getCell (pos));
		Cell cell = getGlobalTerainGrid().getCell(pos).GetComponent<Cell>();
		cell.character = c;

		return 0;
	}

	FactionSetting loadFactionSettingFromUser(int userid){
		//int char_number = 36;
		//CharacterSetting[] cs = new CharacterSetting[4];
		/*cs[0] = new CharacterSetting("黄忠", "Ethan");
		string name = "Huangzhong";
		//UnityEditor.MonoScript go = Instantiate (Resources.Load<UnityEditor.MonoScript>("NPC/" + name ));
		//CharacterSetting go = Instantiate (Resources.Load<CharacterSetting>("NPC/" + name ));

		//string someString = "huangzhong";
		//System.Type myType = someString.GetType();
		//dynamic go = myType.GetConstructor (System.Type.EmptyTypes).Invoke (null);
		print("===>333"+ go.GetType());
	*/

		/*

		cs [0].clonable = true;
		cs [0].movability = 20;
		cs [0].attack = 10;
		cs [0].knowledge = 1;
		cs [0].maxhp = 100;
		cs [0].maxmp = 100;

		cs[1] = new CharacterSetting("RedSamurai"); 
		cs [1].clonable = true;
		cs [1].movability = 10;
		cs [1].attack = 10;
		cs [1].knowledge = 10;
		cs [1].maxhp = 60;
		cs [1].maxmp = 60;

		cs[2] = new CharacterSetting("Kyle"); 
		cs [2].clonable = true;
		cs [2].movability = 10;
		cs [2].attack = 10;
		cs [2].knowledge = 10;
		cs [2].maxhp = 60;
		cs [2].maxmp = 60;

		cs[3] = new CharacterSetting("Troll"); 
		cs [3].clonable = true;
		cs [3].movability = 10;
		cs [3].attack = 10;
		cs [3].knowledge = 10;
		cs [3].maxhp = 60;
		cs [3].maxmp = 60;

		FactionSetting fs = new FactionSetting ();
		fs.userid = userid;
		for (int i = 0; i < char_number; i++) {
			fs.add_character (cs[Random.Range(0, cs.Length)]);
		}*/

		string[] chars = { "Huangzhong", "Zhaoyun", "Lvbu", "Caocao", "Sunquan", "Liubei", "Zhugeliang"};
		FactionSetting fs = new FactionSetting ();
		fs.userid = userid;
		for (int i = 0; i < chars.Length; i++) {
			CharacterSetting cs = CentralController.load_charsetting (chars [i]);
			cs.setup ();
			cs.after_setup ();
			print("==cs.name:"+cs.GetType());
			fs.add_character (cs);
		}





		return fs;
	}

	public static CharacterSetting load_charsetting(string name)
	{
		System.Type type = System.Type.GetType(name);
		CharacterSetting cs = (CharacterSetting)System.Activator.CreateInstance (type);

		return cs;
	}

	void loadTestData(){
		CharacterSetting _cs = new CharacterSetting("Ethan"); // create hero named Ethan
		fs[0].add_character(_cs);
		_cs = new CharacterSetting("RedSamurai"); // create hero "redsamurai"
		_cs.clonable = true;
		for (int i = 0; i < 35; i++) {
			fs [0].add_character (_cs);
		}

		int char_number = 36;
		if (FACTION_NUMBER > 1) {
			_cs = new CharacterSetting ("RedSamurai");
			_cs.clonable = true;
			for (int i = 0; i < char_number; i++) {
				fs [1].add_character (_cs);
			}


			if (FACTION_NUMBER > 2) {
				_cs = new CharacterSetting ("Ethan");
				_cs.clonable = true;
				for (int i = 0; i < char_number; i++) {
					fs [2].add_character (_cs);
				}

				if (FACTION_NUMBER > 3) {
					_cs = new CharacterSetting ("RedSamurai");
					_cs.clonable = true;
					for (int i = 0; i < char_number; i++) {
						fs [3].add_character (_cs);
					}
				}
			}
		}
		// create factions object according factionsettings

		for (int i = 0; i < fs.Length; i++) {
			Faction f  = new Faction (fs [i]);
			f.factionID = i;
			factions [i] = f;
			factions [i].color = colors [i];
			loadFactionCharacters (factions [i], fs [i]);

		}
	}

	IEnumerator prepareTestGame(){
		
		while (gameworld_ready == false) {
			print ("wait1");

			yield return new WaitForSeconds (1);
			print ("wait2");

		}

		for (int  i=0;i<FACTION_NUMBER;i++){
			fs[i] = loadFactionSettingFromUser (i);
		}


		// create factions object according factionsettings

		for (int i = 0; i < fs.Length; i++) {
			Faction f  = new Faction (fs [i]);
			f.factionID = i;
			factions [i] = f;
			factions [i].color = colors [i];
			loadFactionCharacters (factions [i], fs [i]);

		}

		print ("prepareTestData DONE!");
	}
	// Use this for initialization
	void Start () {




		// prepare testing data1

		// create faction settings for testing
		print ("fs size :" + FACTION_NUMBER);
		// faction settings should be passed in runtime
		// character setting is pre-designed in game and also contain data for user specific hero settings, e.g. position, experience, ming-wen addings
		// Character setting should be store in database for each user

		StartCoroutine (prepareTestGame());
		//loadTestData();


		// test Instantiate
		/*Character c = Resources.Load<Character>("Prefabs/Characters/CharacterPrefabs/Ethan");
		print ("====>ethan:" + c);
		//c.gameObject.name = "bbbbbbb";
		Character o1 = Instantiate (c, new Vector3(14, 0, 14), Quaternion.identity);
		o1.name = "aaaaa2";
		o1.transform.localScale = new Vector3 (4, 3, 3);

		GameObject o = Instantiate (pref1, new Vector3(10, 0, 10), Quaternion.identity);
		o.name = "aaaaa";
		o.transform.localScale = new Vector3 (2, 2, 2);*/
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
			characters [j].setFaction (f);
		}
		f.deployCharacters (characters);
		//f.loadCharacters (characters);
	}


	public void updatePropertyPanel(Character ch){
		CharacterSetting cs = ch.setting;
		npc_int.fillAmount = cs.knowledge/ 100.0f;
		npc_attack.fillAmount = cs.attack / 100.0f;
		npc_name.text = ch.name;
		npc_image.texture = ch.head_image;

		npc_desc.text = cs.desc;
	}

	// Update is called once per frame
	void Update () {


	}
}
