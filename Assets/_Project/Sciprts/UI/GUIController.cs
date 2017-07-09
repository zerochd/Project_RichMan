using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {

	private static GUIController _instance;

	public static GUIController Instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<GUIController> ();
			}
			return _instance;
		}
	}

	[SerializeField] EventUI eventUI;
	[SerializeField] BuyUI buyUI;
	[SerializeField] GoUI goUI;

	[SerializeField] IMainUI[] iMainUIArray;

	void Awake(){
		_instance = this;
		Init ();
	}

	public void ShowGameEvent(GameEvent gameEvent){
		if (eventUI == null)
			return;
		eventUI.Show (gameEvent);
	}
		

	public void ShowBuyUI(BuyEvent buyEvent){
		if(buyUI == null)
			return;
		buyUI.Show (buyEvent);
	}

	[ContextMenu("Init")]
	void Init(){
		if(eventUI == null)
			eventUI = GetComponentInChildren<EventUI> ();
		if(goUI == null)
			goUI = GetComponentInChildren<GoUI> ();
		if (buyUI == null)
			buyUI = GetComponentInChildren<BuyUI> ();
		
		iMainUIArray = GetComponentsInChildren<IMainUI> ();

//		Debug.Log ("ima:" + iMainUIArray.Length);
	}

	public void UpdateMainUI(PlayerData playerData){
		foreach (IMainUI mainUI in iMainUIArray) {
			mainUI.UpdateUI (playerData);
		}
	}

	public void UpdateGo(PlayerController playerController){
		if (goUI != null) {
			goUI.PlayerController = playerController;
			goUI.CanGo ();
		}
	}
}
