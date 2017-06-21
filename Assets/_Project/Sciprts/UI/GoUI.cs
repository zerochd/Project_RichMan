using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoUI : MonoBehaviour {

	const int randomShowNum = 50;

	static GoUI _instance;

	[SerializeField] Button eventButton;
	[SerializeField] Text eventText;
	[SerializeField] PlayerController playerController;

	bool startGO = false;

	public static GoUI Instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<GoUI> ();
			}
			return _instance;
		}
	}

	public PlayerController PlayerController {
		set {
			playerController = value;
		}
	}

	[Header("[DEBUG]")]
	[SerializeField] int _showFinalResult;
	[SerializeField] float _showTimeNow;

	void Awake(){
		_instance = this;
		Init ();
	}

	// Use this for initialization
	void Start () {
		eventButton.onClick.AddListener (delegate {
			GO();
		});

	}

	/// <summary>
	/// GO!!!
	/// </summary>
	void GO(){
		if (playerController == null) {
			Debug.LogError ("no playerController");
			return;
		} else {
			if (startGO)
				return;

			startGO = true;
			int _speed = 0;
			_speed = playerController.PlayerEntry.playerData.speed + 1;
			StartCoroutine(GO_Cor(_speed,3f));
		}
	}

	IEnumerator GO_Cor(int speed,float delay){

		//get final result
		int _finalResult = Random.Range (1, speed);

		_showFinalResult = _finalResult;


		//random show

//		float _timeNow = 0;
//
//		while (_timeNow < delay - 0.1f) {
//			_showTimeNow = _timeNow;
//			float _timeNext = Mathf.Lerp (_timeNow, delay, Time.deltaTime);
//			float _perDelay = _timeNext - _timeNow;
//			int _fakeTemp = Random.Range (0, speed);
//			eventText.text = "" + _fakeTemp;
//			_timeNow = _timeNext;
//			yield return new WaitForSeconds (_perDelay);
//		}

		//show final result
		eventText.text = ""+_finalResult;

		yield return new WaitForSeconds (0.5f);
		playerController.MovePlayer (_finalResult);
	}

	public void CanGo(){
		startGO = false;
		eventText.text = "Go!";
	}

	[ContextMenu("Init")]
	void Init(){
		if (eventButton == null) {
			eventButton = GetComponentInChildren<Button> ();
		}
		if (eventText == null) {
			eventText = GetComponentInChildren<Text> ();
		}
	}
}
