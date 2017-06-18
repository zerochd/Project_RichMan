using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField] Player player;

	public Player PlayerEntry {
		get {
			return player;
		}
	}

	[SerializeField] int moveStep;
	[SerializeField] bool debugMove;


	void Update(){
			
		if (debugMove) {
			debugMove = false;

			MovePlayer (player,moveStep);

		}

	}
		

	public void MovePlayer(int step){
		MovePlayer (player, step);
	}

	void MovePlayer(Player player,int step){
		if (player == null) {
			Debug.LogError ("no player");
			return;
		}
		player.CalcMoveStep (step);
	}
		
}
