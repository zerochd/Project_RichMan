using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public enum BUY_EVENT_TYPE{
//	PROP,
//	BUILDING
//}

public class BuyEvent{
	public Player buyers;
	public Actor item;
//	public BUY_EVENT_TYPE buyType;
}

public class BuyBuildingEvent : BuyEvent{
	public Grid setGrid;
}

public enum GAME_EVENT_TYPE{
	GIVE_MONEY,
	GIVE_LIFE,
	GIVE_EXP
}

[Serializable]
public class GameEvent{

	public string eventDescription;
	public GAME_EVENT_TYPE eventType;

	public int money = 10;

}




