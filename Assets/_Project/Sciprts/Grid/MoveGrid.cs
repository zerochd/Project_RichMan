using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGrid : Gird {

	[SerializeField] private MoveGrid nextGird;

	public MoveGrid NextGird {
		get {
			return nextGird;
		}
	}

	public void ConnectGrid(MoveGrid connectGrid){
		this.nextGird = connectGrid;
	}

	public virtual void Arrived(Player player){
		Debug.Log (player.name + "  Arrived");
	}
		
}
