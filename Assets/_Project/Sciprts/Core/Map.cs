using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	const string GRID_PATH = "Assets/_Project/Prefab/GridPrefab";

	[SerializeField] MoveGrid[] moveGrids;
	[SerializeField] GameObject moveGridPrefab;

	[ContextMenu("Init")]
	public void Init(){

		moveGrids = GetComponentsInChildren<MoveGrid> ();
	}

	public void Clear(){

		foreach (MoveGrid mgd in moveGrids) {
			DestroyImmediate (mgd.gameObject);
		}

		moveGrids = new MoveGrid[0];

	}

	public GameObject GetGridPrefab(){
		return moveGridPrefab;
	}

	[ContextMenu("SetNextMoveGridBatch")]
	void SetNextMoveGridBatch(){
		for (int i = 0; i < moveGrids.Length -1; i++) {
			moveGrids [i].ConnectGrid (moveGrids [i + 1]);
		}
	}

	public MoveGrid GetNextGrid(int nextIndex){
		
		if (nextIndex >= 0 && nextIndex < moveGrids.Length) {
			return moveGrids [nextIndex];
		}

		return null;
	}

}
