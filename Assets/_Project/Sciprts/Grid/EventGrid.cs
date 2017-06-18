using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGrid : MoveGrid {

	[SerializeField]
	GameEvent gameEvent;


	void Init(){

	}

	public override bool Arrived (Actor actor)
	{
		if (GUIController.Instance != null) {
			GUIController.Instance.ShowGameEvent (gameEvent);
		}
		return base.Arrived (actor);

	}
}
