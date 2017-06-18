using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	[SerializeField] MoveGrid[] moveGrids;

	[ContextMenu("Init")]
	void Init(){

		moveGrids = GetComponentsInChildren<MoveGrid> ();
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
