using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBoard : MonoBehaviour {
	public GameObject GridPrefab; 

	public int yOffset = 1;
	public int cellSize = 2;

	Vector3 MeshVertex(int x, int z, float h) {
		return new Vector3(x * cellSize, h + yOffset, z * cellSize);
	}
	// Use this for initialization
	void Start () {
		yOffset = 1;
		cellSize = 2;
		// create grid
		for (int z = 0; z < 100; z++) {
			for (int x = 0; x < 100; x++) {
				GameObject go = Instantiate (GridPrefab, new Vector3 (x * cellSize, 0, z*cellSize), new Quaternion (0, 0, 0, 0));
				go.transform.parent = transform;
				go.transform.localPosition = new Vector3(0,0,0);

				RaycastHit hitInfo;
				Vector3 origin;

				origin = new Vector3(x*cellSize, 200, z*cellSize);
				print ("haha:" + gameObject);
				Physics.Raycast(gameObject.transform.TransformPoint(transform.TransformPoint(origin)), Vector3.down, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Terrain"));
				float h = hitInfo.point.y;
				print ("h=:" + h);
		
				Mesh mesh = go.GetComponent<MeshFilter> ().mesh;
				mesh.vertices =new Vector3[] {
					MeshVertex(x, z, h),
					MeshVertex(x, z + 1, h),
					MeshVertex(x + 1, z, h),
					MeshVertex(x + 1, z + 1, h),
				};
				mesh.RecalculateBounds (); // otherwise mesh will not be visible without in break mode


			}
		}
		//go.transform.localPosition = new Vector3(0,0,0);
		//Mesh mesh = pf.GetComponent<MeshFilter> ().mesh;
		//mesh.RecalculateBounds ();
		print ("====>create boardgrid");

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
