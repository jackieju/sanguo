using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainGrid : MonoBehaviour {
	public bool globalGrid = false;
	public bool objectInGridCenter = true;
	public Terrain terrain; //terrain grid is attached to
	private Vector3 terrainOrigin;
	public float cellSize = 2;
	public int gridWidth = 10;
	public int gridHeight = 10;
	public float yOffset = 0.3f;
	public Material cellMaterialValid;
	public Material cellMaterialInvalid;

	private GameObject[] _cells;
	private float[] _heights;
	private Vector3 gridOrigin;

	void Awake(){
		if (terrain == null)
			terrain = CentralController.inst.terrain;
		if (cellMaterialValid == null)
			cellMaterialValid = (Material)Resources.Load<Material> ("Materials/Fire Smoke");
		if (cellMaterialInvalid == null)
			cellMaterialInvalid = (Material)Resources.Load<Material> ("Materials/Bark");
	}
	void Start() {
		print ("start grid");
		terrainOrigin = terrain.transform.position;

		// align current game object position to grid
		alignPosition ();

		print("transforM:"+transform.position);
		if (objectInGridCenter)
			gridOrigin = new Vector3 (0 - gridWidth * cellSize / 2 - cellSize / 2, 0, 0 - gridHeight * cellSize / 2 - cellSize / 2);
		else
			gridOrigin = new Vector3 (0- cellSize / 2, 0,0 - cellSize / 2);

		if (globalGrid) {
			Vector3 size = terrain.terrainData.size;
			gridHeight = (int)(size.y / cellSize /10);
			gridWidth = (int)(size.x / cellSize /10);
		}

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
		UpdateSize();
		//UpdatePosition();
		UpdateHeights();
		UpdateCells ();

		for (int z = 0; z < gridHeight; z++) {
			for (int x = 0; x < gridWidth; x++) {
				GameObject cell = _cells[z * gridWidth + x];
			
				cell.AddComponent<MeshCollider>();
			}
		}


		print ("start complete");
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
			print ("Terrain grid clicked:"+this.gameObject.name);

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

		go.name = "Grid Cell";
		go.transform.parent = transform;

		go.transform.localPosition = gridOrigin;
		//go.transform.localPosition = Vector3.zero;
		go.AddComponent<MeshRenderer>();
		go.AddComponent<MeshFilter>().mesh = CreateMesh();
		//go.AddComponent<MeshCollider>();
		//MeshCollider meshc = go.AddComponent(typeof(MeshCollider)) as MeshCollider;
		//meshc.sharedMesh = go.GetComponent<Mesh>(); // Give it your mesh here.
	

		return go;
	}

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
		return 	new Vector3(x * cellSize + gridOrigin.x+ cellSize/2, 200, z * cellSize + gridOrigin.z+ cellSize/2);

	}
	bool IsCellValid(int x, int z) {
		RaycastHit hitInfo;
		Vector3 origin = getGridCenterLocalPosition(x,z);
		//Physics.Raycast(transform.TransformPoint(origin), Vector3.down, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Buildings"));
		Physics.Raycast(terrain.transform.TransformPoint(transform.TransformPoint(origin)), Vector3.down, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Tree"));
		bool a = hitInfo.collider == null;
		if (hitInfo.collider)
			print("isvalid:"+hitInfo.collider.gameObject.name);
		//if (hitInfo.collider == null || hitInfo.collider.gameObject.name == "Terrain")
			//return true;

		return hitInfo.collider == null;
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
		mesh.RecalculateBounds ();
	}

	Vector3 MeshVertex(int x, int z) {
		return new Vector3(x * cellSize, _heights[z * (gridWidth + 1) + x] + yOffset, z * cellSize);
	}

	public Vector3 GetWorldPosition(Vector3 gridPosition)
	{
		return new Vector3(terrainOrigin.z + (gridPosition.x * cellSize), terrainOrigin.y, terrainOrigin.x + (gridPosition.y * cellSize));
	}

	public Vector2 GetGridPosition(Vector3 worldPosition)
	{
		return new Vector2(worldPosition.z / cellSize, worldPosition.x / cellSize);
	}
}