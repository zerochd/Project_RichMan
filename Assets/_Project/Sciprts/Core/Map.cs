using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Map : MonoBehaviour {
	
	private const string GridPath = "Assets/_Project/Prefab/GridPrefab";

	[SerializeField] private VectorInt2 edge;
	
	[SerializeField] private GameObject gridPrefab;
	[SerializeField] private Grid[] grids;
	
	public VectorInt2 Edge => edge;

	public Grid[,] gridMat;

	private void Start(){
		Init ();

		gridMat = new Grid[edge.x,edge.y];
		foreach (var grid in grids) {
			gridMat [grid.Vi.x, grid.Vi.y] = grid;
		}
	}

	[ContextMenu("Init")]
	public void Init(){

		edge = new VectorInt2 ();

		grids = GetComponentsInChildren<Grid> ();

		foreach (var grid in grids) {
			grid.Init ();
			if (edge < grid.Vi) {
				edge = grid.Vi;
			}
		}
		edge += VectorInt2.One;

	}

	public void ResetColor(){
		foreach (var grid in grids) {
			grid.ResetGridColor ();
		}
	}

	public void Clear(){

		Init ();

		foreach (var gd in grids)
		{
			Undo.DestroyObjectImmediate(gd.gameObject);
		}

		grids = new Grid[0];

	}

	public GameObject GetGridPrefab(){
		return gridPrefab;
	}


}
