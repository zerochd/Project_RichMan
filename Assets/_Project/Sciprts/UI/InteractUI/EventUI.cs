using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventUI : MonoBehaviour {

	[SerializeField] Text eventText;
	[SerializeField] Button eventButton;

	GameEvent getEvent;

	void Awake(){
		Init ();
	}

	void Start(){
		if (eventButton != null) {
			eventButton.onClick.AddListener (delegate {
				EventExcute ();
			});
		}
	}

	public void Show(string text){
		if (eventText == null)
			return;
		
		eventText.text = text;
		gameObject.SetActive (true);
	}

	public void Show(GameEvent gameEvent){
		if (gameEvent == null)
			return;
		getEvent = gameEvent;
		Show (gameEvent.eventDescription);

		Time.timeScale = 0f;
	}
		

	public void Hide(){
		gameObject.SetActive (false);
	}

	[ContextMenu("Init")]
	void Init(){
		if(eventText == null)
			eventText = GetComponentInChildren<Text> ();
		if(eventButton == null)
			eventButton = GetComponentInChildren<Button> ();
	}

	public void EventExcute(){
		Debug.Log ("Click");
		if (getEvent != null) {
			if (getEvent.eventType == GAME_EVENT_TYPE.GIVE_MONEY) {
				Debug.Log ("GetEventMoney:" + getEvent.money);

			} else {
				Debug.Log ("GetEventOther");
			}
		}

		Hide ();

		Time.timeScale = 1f;
	}

}
