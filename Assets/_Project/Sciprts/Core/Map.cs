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
			Debug.Log ("moveGird:" + moveGrids [i].name + " connect +" + moveGrids [i + 1].name);
			moveGrids [i].ConnectGrid (moveGrids [i + 1]);
//			moveGrids [i].NextGird = moveGrids [i + 1];
		}
	}

}
