using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGrid : MoveGrid {

	[SerializeField]
	GameEvent gameEvent;

	void Start(){
	
	}

	void Init(){

	}

	public override void Arrived (Player player)
	{
		Debug.Log ("This is GameEvent:" + player.name + " Arrived");

		if (GUIController.Instance != null) {
			GUIController.Instance.ShowGameEvent (gameEvent);
		}
	}
}
