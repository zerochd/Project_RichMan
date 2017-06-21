using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class MoveGrid : Grid {

	public int exp = 1;

	[SerializeField] 
	private MoveGrid nextGird;

	public MoveGrid NextGird {
		get {
			return nextGird;
		}
	}

	public void ConnectGrid(MoveGrid connectGrid){
		nextGird = connectGrid;

		//save serializeField
		EditorUtility.SetDirty (this);

	}

	public virtual bool Arrived(Actor actor){
		return base.Arrived (actor);
	}
		
}
