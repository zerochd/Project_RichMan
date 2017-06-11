using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public Player controllerPlayer;

	[SerializeField] int moveStep;

	[SerializeField] bool debugMove;

	void Start(){
	}

	void Update(){
			
		if (debugMove) {
			debugMove = false;

			moveStep = Random.Range (1, 7);
			moveStep = 2;
			MovePlayer (controllerPlayer,moveStep);

		}

	}

	void MovePlayer(Player player,int step){
		player.CalcMoveStep (step);
	}
		
}
