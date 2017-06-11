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

	void Awake(){
		_instance = this;
	}

	public void ShowGameEvent(GameEvent gameEvent){
		if (eventUI == null)
			return;
		eventUI.Show (gameEvent.eventDescription);
	}
}
