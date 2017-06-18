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
	[SerializeField] GoUI goUI;

	void Awake(){
		_instance = this;
		Init ();
	}

	public void ShowGameEvent(GameEvent gameEvent){
		if (eventUI == null)
			return;
//		eventUI.Show (gameEvent.eventDescription);
		eventUI.Show (gameEvent);
	}

	[ContextMenu("Init")]
	void Init(){
		if(eventUI == null)
			eventUI = GetComponentInChildren<EventUI> ();
		if(goUI == null)
			goUI = GetComponentInChildren<GoUI> ();
	}

	public void UpdatePlayerController(PlayerController playerController){
		if (goUI != null) {
			goUI.PlayerController = playerController;
			goUI.CanGo ();
		}
	}
}
