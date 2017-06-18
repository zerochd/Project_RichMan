using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GAME_EVENT_TYPE{
	GIVE_MONEY,
	GIVE_LIFE,
	GIVE_EXP
}

[Serializable]
public class GameEvent {

	public string eventDescription;
	public GAME_EVENT_TYPE eventType;

	public int money = 10;

}




