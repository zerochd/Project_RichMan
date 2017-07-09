using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager _instance;

	public static GameManager Instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<GameManager> ();
			}
			return _instance;
		}
	}

	[SerializeField] bool GameStart;


	[SerializeField] PlayerController m_nowControler;

	public PlayerController NowControler {
		get {
			return m_nowControler;
		}
	}

	void Awake(){
		_instance = this;
	}
	// Use this for initialization
	void Start () {
		GameStart = true;
	}
		
	
//	// Update is called once per frame
//	void Update () {
//		
//	}
		
}
