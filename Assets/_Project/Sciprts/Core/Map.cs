using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Map : MonoBehaviour {

	const string GRID_PATH = "Assets/_Project/Prefab/GridPrefab";

	public int row = 9;
	public int column = 9; 

	[SerializeField] GameObject gridPrefab;
	[SerializeField] Grid[] grids;

	[SerializeField] VectorInt2 edge;

	public VectorInt2 Edge {
		get {
			return edge;
		}
	}

	public Grid[,] gridMat;

	void Start(){
		Init ();

		gridMat = new Grid[edge.x,edge.y];
		foreach (Grid grid in grids) {
			gridMat [grid.Vi.x, grid.Vi.y] = grid;
		}
	}

	[ContextMenu("Init")]
	public void Init(){

		edge = new VectorInt2 ();

		grids = GetComponentsInChildren<Grid> ();

		foreach (Grid grid in grids) {
			grid.Init ();
			if (edge < grid.Vi) {
				edge = grid.Vi;
			}
		}
		edge += VectorInt2.One;

	}

	public void ResetColor(){
		foreach (Grid grid in grids) {
			grid.ResetGridColor ();
		}
	}

	public void Clear(){

		Init ();

		foreach (Grid gd in grids) {
			DestroyImmediate (gd.gameObject);
		}

		grids = new Grid[0];

	}

	public GameObject GetGridPrefab(){
		return gridPrefab;
	}

//	[ContextMenu("SetNextGridBatch")]
//	void SetNextGridBatch(){
//		for (int i = 0; i < girds.Length -1; i++) {
//			girds [i].ConnectGrid (girds [i + 1]);
//		}
//	}

//	public Grid GetNextGrid(int nextIndex){
//		
//		if (nextIndex >= 0 && nextIndex < girds.Length) {
//			return girds [nextIndex];
//		}
//
//		return null;
//	}

}
