﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
#if UNITY_EDITOR 
using UnityEditor;
#endif

public class TerrainGrid : MonoBehaviour {
	public bool globalGrid = false;
	public bool objectInGridCenter = true;
	public Terrain terrain; //terrain grid is attached to
	private Vector3 terrainOrigin;
	private float cellSize = 1; // controlled by CentralController.gridCellSize

	// gridWidth and gridHight not used in global mode
	public int gridWidth = 10;
	public int gridHeight = 10;

	public float yOffset = 0.3f;
	public Material cellMaterialValid;
	public Material cellMaterialInvalid;
	public Material cellMaterialEnemy;

	private GameObject[] _cells;
	private float[] _heights;
	private Vector3 gridOrigin;

	public void inactiveAllCells(){
		for (int z = 0; z < gridHeight; z++) {
			for (int x = 0; x < gridWidth; x++) {
				_cells [z * gridWidth + x].SetActive (false);
			}
		}
	}
	public void updateCell(int x, int y){
		GameObject cell = _cells [y * gridWidth + x];
		MeshRenderer meshRenderer = cell.GetComponent<MeshRenderer>();
		MeshFilter meshFilter = cell.GetComponent<MeshFilter>();

		int layerMask =  (1 << LayerMask.NameToLayer("Char"));
		GameObject o = getObjectOnCell (x, y, layerMask);
		Character enemy = null;
		if (o) 
			enemy = o.GetComponent<Character> ();
		
		if (enemy != null && enemy.getFaction ().factionID != 0) {
			meshRenderer.material = cellMaterialEnemy;
		} else {

			meshRenderer.material = IsCellValid (x, y) ? cellMaterialValid : cellMaterialInvalid;
		}
			//UpdateMesh(meshFilter.mesh, x, z);

	}
	public void activeCell(int x, int y){
		//print ("active cell:" + x + "," + y);
		if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
			return;
		_cells [y * gridWidth + x].SetActive(true);
		updateCell (x, y);

	}

	public GameObject[] getCells(){
		return this._cells;
	}
	public GameObject getCell(int x, int y){
		return this._cells[y * gridWidth + x];
	}
	public GameObject getCell(Vector2 pos){
		return this._cells[(int)pos.y * gridWidth + (int)pos.x];
	}
	void Awake(){
		cellSize = CentralController.gridCellSize;
		if (terrain == null)
			terrain = CentralController.inst.terrain;
		if (cellMaterialValid == null)
			cellMaterialValid = (Material)Resources.Load<Material> ("Materials/Fire Smoke");
		if (cellMaterialInvalid == null)
			cellMaterialInvalid = (Material)Resources.Load<Material> ("Materials/Bark");
		if (cellMaterialEnemy == null)
			cellMaterialInvalid = (Material)Resources.Load<Material> ("Materials/Bark");
	}

	public void DestroyGrids(){
		for (int z = 0; z < gridHeight; z++) {
			for (int x = 0; x < gridWidth; x++) {
				_cells [z * gridWidth + x].SetActive (false);
				Destroy(_cells[z * gridWidth + x]);
			}
		}
	}

	void Start() {

		CentralController.gridCellSize = this.cellSize;
		print ("start grid");
		terrainOrigin = terrain.transform.position;
		print("terrainOrigin:"+terrainOrigin);

		// align current game object position to grid
		if (!globalGrid)
			alignPosition ();

		print("transforM:"+transform.position);


		if (objectInGridCenter)
			gridOrigin = new Vector3 (0 - gridWidth * cellSize / 2 - cellSize / 2, 0, 0 - gridHeight * cellSize / 2 - cellSize / 2);
		else
			gridOrigin = new Vector3 (0- cellSize / 2, 0,0 - cellSize / 2);
		

		
		if (globalGrid) {
			Vector3 size = terrain.terrainData.size;
			print ("terrain size:" + size);
			gridHeight = (int)(size.z / cellSize);
			gridWidth = (int)(size.x / cellSize );
			gridOrigin = new Vector3 (0 - cellSize / 2, 0, 0 - cellSize / 2);
		}
		print ("gridWidth:" + gridWidth);
		print ("gridHeight:" + gridHeight);

		// create mesh for each grid
		_cells = new GameObject[gridHeight * gridWidth];
		_heights = new float[(gridHeight + 1) * (gridWidth + 1)];

		for (int z = 0; z < gridHeight; z++) {
			for (int x = 0; x < gridWidth; x++) {
				_cells[z * gridWidth + x] = CreateChild();
			}
		}
		print ("test update");
		// adjust size and position according setting and parent(terrain)
		if (!globalGrid)
			UpdateSize();
		//UpdatePosition();
		UpdateHeights();
		UpdateCells ();

		for (int z = 0; z < gridHeight; z++) {
			for (int x = 0; x < gridWidth; x++) {
				GameObject cell = _cells[z * gridWidth + x];
			
				cell.AddComponent<MeshCollider>();
				if (x == 0 && z == 0) {
					//savegrid(cell, cell.GetComponent<MeshFilter>(), "./", "./");
				}
			}
		}


		print ("start complete");

		inactiveAllCells ();

		CentralController.inst.gameworld_ready = true;
	}

	void Update () {
		return;
		print (this._cells);

		UpdateSize();
		//UpdatePosition();
		UpdateHeights();

		UpdateCells();
	}

	void OnMouseDown(){
		print ("Terrain grid clicked111");
		if (Input.GetMouseButtonDown (0)) {
			print ("Terrain grid clicked:" + this.gameObject.name);
			CentralController cc = CentralController.inst;
			/*if (cc.state == 100) { // move character onto this cell
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (this.gameObject.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity)) {
					Vector2 pos = cc.getPosFromCord(hit.point);
					print ("click cell " + pos);
				}

			}*/
		}

	}

	void alignPosition(){
		if (objectInGridCenter == false) {
			transform.position = new Vector3 (cellSize / 2, 0, cellSize / 2);
			return;
		}
		Vector3 p= new Vector3(transform.position.x, transform.position.y, transform.position.z);
		float x1 = transform.position.x - terrainOrigin.x;
		print ("x1=" + x1);
		float m = x1 % cellSize;
		print("m="+m);

		if (m != 0){
			p.x = x1 - m + cellSize/2;
		}

		float z1 = transform.position.z - terrainOrigin.z;
		print ("z1=" + z1);
		float m2 = z1 % cellSize;
		print("m2="+m2);

		if (m2 != 0){
			p.z = z1 - m2 + cellSize/2;
		}
		transform.position = p;
	}
	GameObject CreateChild() {
		GameObject go = new GameObject();

		go.name = "GridCell";
		go.transform.parent = transform;

		if (!globalGrid)
			go.transform.localPosition = gridOrigin;

		//go.transform.localPosition = Vector3.zero;
		go.AddComponent<MeshRenderer>();
		go.AddComponent<MeshFilter>().mesh = CreateMesh();
		//go.AddComponent<MeshCollider>();
		//MeshCollider meshc = go.AddComponent(typeof(MeshCollider)) as MeshCollider;
		//meshc.sharedMesh = go.GetComponent<Mesh>(); // Give it your mesh here.
	
		go.AddComponent<Cell> ();
		return go;
	}

	// not used in global mode
	void UpdateSize() {
		int newSize = gridHeight * gridWidth;
		print (_cells);
		int oldSize = _cells.Length;

		if (newSize == oldSize)
			return;

		GameObject[] oldCells = _cells;
		_cells = new GameObject[newSize];

		if (newSize < oldSize) {
			for (int i = 0; i < newSize; i++) {
				_cells[i] = oldCells[i];
			}

			for (int i = newSize; i < oldSize; i++) {
				Destroy(oldCells[i]);
			}
		}
		else if (newSize > oldSize) {
			for (int i = 0; i < oldSize; i++) {
				_cells[i] = oldCells[i];
			}

			for (int i = oldSize; i < newSize; i++) {
				_cells[i] = CreateChild();
				//_cells [i].AddComponent<Cell> ();
			}
		}

		_heights = new float[(gridHeight + 1) * (gridWidth + 1)];
	}

	void UpdatePosition() {
		RaycastHit hitInfo;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Physics.Raycast(ray, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Terrain"));
		//Physics.Raycast(ray, out hitInfo, Mathf.Infinity);

		Vector3 position = hitInfo.point;

		position.x -= hitInfo.point.x % cellSize + gridWidth * cellSize / 2;
		position.z -= hitInfo.point.z % cellSize + gridHeight * cellSize / 2;
		position.y = 0;

		transform.position = position;
	}

	void UpdateHeights() {
		RaycastHit hitInfo;
		Vector3 origin;

		for (int z = 0; z < gridHeight + 1; z++) {
			for (int x = 0; x < gridWidth + 1; x++) {
				origin = new Vector3(x * cellSize + gridOrigin.x, 200, z * cellSize + gridOrigin.z);
				//origin = new Vector3(x * cellSize + cellSize/2 , 200, z * cellSize + cellSize/2);
				Physics.Raycast(terrain.transform.TransformPoint(transform.TransformPoint(origin)), Vector3.down, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Terrain"));
				//Physics.Raycast(transform.TransformPoint(origin), Vector3.down, out hitInfo, Mathf.Infinity);


				_heights[z * (gridWidth + 1) + x] = hitInfo.point.y;
				//print ("height:" + hitInfo.point.y);
			}
		}
	}

	void UpdateCells() {
		for (int z = 0; z < gridHeight; z++) {
			for (int x = 0; x < gridWidth; x++) {
				GameObject cell = _cells[z * gridWidth + x];
				MeshRenderer meshRenderer = cell.GetComponent<MeshRenderer>();
				MeshFilter meshFilter = cell.GetComponent<MeshFilter>();

				meshRenderer.material = IsCellValid(x, z) ? cellMaterialValid : cellMaterialInvalid;
				UpdateMesh(meshFilter.mesh, x, z);
			}
		}
	}

	Vector3 getGridCenterLocalPosition(int x, int z){
		// return 	new Vector3(x * cellSize + gridOrigin.x+ cellSize/2, 200, z * cellSize + gridOrigin.z+ cellSize/2);
		return new Vector3(x * cellSize + cellSize/2, 200, z * cellSize + cellSize/2);

	}

	public GameObject getObjectOnCell(int x, int z, int layerMask){
		RaycastHit hitInfo;
		Vector3 origin = getGridCenterLocalPosition(x,z);
		//print ("cell Size:" + cellSize + ",gridOrigin:" + gridOrigin);
		//print ("grid1 cord:" + origin);
		//Physics.Raycast(transform.TransformPoint(origin), Vector3.down, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Buildings"));
		//int layerMask =  (1 << LayerMask.NameToLayer("Tree")) | (1 << LayerMask.NameToLayer("Char"));
		//print ("ray1 from:" + terrain.transform.TransformPoint(transform.TransformPoint(origin)));

		Physics.Raycast(terrain.transform.TransformPoint(transform.TransformPoint(origin)), Vector3.down, out hitInfo, Mathf.Infinity, layerMask);
		bool a = hitInfo.collider == null;
		if (hitInfo.collider) {
			//print ("isvalid:(" + x + "," + z + "):" + hitInfo.collider.gameObject.name);
			return hitInfo.collider.gameObject;
		} else
			return null;
	}

	public bool IsCellValid(int x, int z) {
		int layerMask =  (1 << LayerMask.NameToLayer("Tree")) | (1 << LayerMask.NameToLayer("Char"));

		//if (hitInfo.collider == null || hitInfo.collider.gameObject.name == "Terrain")
			//return true;
		GameObject go = getObjectOnCell(x, z, layerMask);
		return go == null;
	}

	Mesh CreateMesh() {
		Mesh mesh = new Mesh();

		mesh.name = "Grid Cell";
		mesh.vertices = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
/*
		int[] triangles=new int[36];
		int currentCount=0;
		for(int i=0;i<24;i=i+4)
		{
		        triangles[currentCount++]=i;
		        triangles[currentCount++]=i+3;
		        triangles[currentCount++]=i+1;
		        
		        triangles[currentCount++]=i;
		        triangles[currentCount++]=i+2;
		        triangles[currentCount++]=i+3;
		        
		}
		mesh.triangles = triangles;
*/
		mesh.triangles = new int[] { 0, 1, 2, 2, 1, 3 };
		mesh.normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
		mesh.uv = new Vector2[] { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 0) };

		return mesh;
	}

	void UpdateMesh(Mesh mesh, int x, int z) {
		
		mesh.vertices = new Vector3[] {
			MeshVertex(x, z),
			MeshVertex(x, z + 1),
			MeshVertex(x + 1, z),
			MeshVertex(x + 1, z + 1),
		};
		mesh.RecalculateBounds (); // otherwise mesh will not be visible without in break mode
	}

	Vector3 MeshVertex(int x, int z) {
		return new Vector3(x * cellSize, _heights[z * (gridWidth + 1) + x] + yOffset, z * cellSize);
	}
/*
	public Vector3 GetWorldPosition(Vector3 gridPosition)
	{
		return new Vector3(terrainOrigin.z + (gridPosition.x * cellSize), terrainOrigin.y, terrainOrigin.x + (gridPosition.y * cellSize));
	}

	public Vector2 GetGridPosition(Vector3 worldPosition)
	{
		return new Vector2(worldPosition.z / cellSize, worldPosition.x / cellSize);
	}*/


	private string MeshToString(MeshFilter mf, Vector3 scale)  
	{  
	    Mesh          mesh            = mf.mesh;  
	    Material[]    sharedMaterials = mf.GetComponent<Renderer>().sharedMaterials;  
	    Vector2       textureOffset   = mf.GetComponent<Renderer>().material.GetTextureOffset("_MainTex");  
	    Vector2       textureScale    = mf.GetComponent<Renderer>().material.GetTextureScale ("_MainTex");  
	  
	    StringBuilder stringBuilder   = new StringBuilder().Append("mtllib design.mtl")  
	        .Append("\n")  
	        .Append("g ")  
	        .Append(mf.name)  
	        .Append("\n");  
	  
	    Vector3[] vertices = mesh.vertices;  
	    for (int i = 0; i < vertices.Length; i++)  
	    {  
	        Vector3 vector = vertices[i];  
	        stringBuilder.Append(string.Format("v {0} {1} {2}\n", vector.x * scale.x, vector.y * scale.y, vector.z * scale.z));  
	    }  
	  
	    stringBuilder.Append("\n");  
	  
	    Dictionary<int, int> dictionary = new Dictionary<int, int>();  
	  
	    if (mesh.subMeshCount > 1)  
	    {  
	        int[] triangles = mesh.GetTriangles(1);  
	  
	        for (int j = 0; j < triangles.Length; j += 3)  
	        {  
	            if (!dictionary.ContainsKey(triangles[j]))  
	            {  
	                dictionary.Add(triangles[j], 1);  
	            }  
	  
	            if (!dictionary.ContainsKey(triangles[j + 1]))  
	            {  
	                dictionary.Add(triangles[j + 1], 1);  
	            }  
	  
	            if (!dictionary.ContainsKey(triangles[j + 2]))  
	            {  
	                dictionary.Add(triangles[j + 2], 1);  
	            }  
	        }  
	    }  
	  
	    for (int num = 0; num != mesh.uv.Length; num++)  
	    {  
	        Vector2 vector2 = Vector2.Scale(mesh.uv[num], textureScale) + textureOffset;  
	  
	        if (dictionary.ContainsKey(num))  
	        {  
	            stringBuilder.Append(string.Format("vt {0} {1}\n", mesh.uv[num].x, mesh.uv[num].y));  
	        }  
	        else  
	        {  
	            stringBuilder.Append(string.Format("vt {0} {1}\n", vector2.x, vector2.y));  
	        }  
	    }  
	  
	    for (int k = 0; k < mesh.subMeshCount; k++)  
	    {  
	        stringBuilder.Append("\n");  
	  
	        if (k == 0)  
	        {  
	            stringBuilder.Append("usemtl ").Append("Material_design").Append("\n");  
	        }  
	  
	        if (k == 1)  
	        {  
	            stringBuilder.Append("usemtl ").Append("Material_logo").Append("\n");  
	        }  
	  
	        int[] triangles2 = mesh.GetTriangles(k);  
	  
	        for (int l = 0; l < triangles2.Length; l += 3)  
	        {  
	            stringBuilder.Append(string.Format("f {0}/{0} {1}/{1} {2}/{2}\n", triangles2[l] + 1, triangles2[l + 2] + 1, triangles2[l + 1] + 1));  
	        }  
	    }  
	  
	    return stringBuilder.ToString();  
	}  

#if UNITY_EDITOR 

    // for creating prefab from runtime mesh
	void savegrid(GameObject go, MeshFilter mf, string datPath, string projectPath){
        //using (StreamWriter streamWriter = new StreamWriter(string.Format("{0}{1}.obj", datPath, "mesh1")))  
		using (StreamWriter streamWriter = new StreamWriter("Assets/Resources/Prefabs/gridmesh.obj"))  
		{  
            streamWriter.Write(MeshToString(mf, new Vector3(-1f, 1f, 1f)));  
            streamWriter.Close();  
        }  
        AssetDatabase.Refresh();
        
        // create prefab  
		//Mesh mesh   = AssetDatabase.LoadAssetAtPath<Mesh>(string.Format("{0}{1}.obj", projectPath, "mesh1"));
		//Mesh mesh   = AssetDatabase.LoadAssetAtPath<Mesh>("mesh1.obj");
		Mesh mesh   = AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Resources/Prefabs/gridmesh.obj");

		print ("mesh:" + mesh);
        mf.mesh     = mesh;  
  
		//PrefabUtility.CreatePrefab(string.Format("{0}{1}.prefab", projectPath, "mesh2"), go);  
		PrefabUtility.CreatePrefab("Assets/Resources/Prefabs/bgrid2.prefab", go);  
        AssetDatabase.Refresh();

		print ("mesh saved");
    }
#endif
}