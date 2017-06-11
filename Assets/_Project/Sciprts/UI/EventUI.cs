using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventUI : MonoBehaviour {

	public Text eventText;
	public Button eventButton;

	void Awake(){
		Init ();
	}

	void Start(){
		eventButton.onClick.AddListener (delegate {
			EventExcute();
		});

	}

	public void Show(string text){
		if (eventText == null)
			return;
		
		eventText.text = text;
		gameObject.SetActive (true);
	}

	[ContextMenu("Init")]
	void Init(){
		eventText = GetComponentInChildren<Text> ();
		eventButton = GetComponentInChildren<Button> ();
	}

	public void EventExcute(){
		Debug.Log ("Click");
		
	}

}
