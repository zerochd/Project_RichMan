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

	public bool GameStart;

	[SerializeField] PlayerController[] playerControllerArray;

	Queue<PlayerController> m_playerControllerQueue;

	public Queue<PlayerController> PlayerControllerQueue {
		get {
			return m_playerControllerQueue;
		}
	}

	[SerializeField] PlayerController m_nowControler;

	public PlayerController NowControler {
		get {
			return m_nowControler;
		}
	}

	void Awake(){
		_instance = this;
		SetupQueue ();
	}

	void SetupQueue(){

		if (m_playerControllerQueue == null) {
			m_playerControllerQueue = new Queue<PlayerController> ();
		} else {
			m_playerControllerQueue.Clear ();
		}
			
		foreach (var pc in playerControllerArray) {
			m_playerControllerQueue.Enqueue (pc);
		}

		m_nowControler = m_playerControllerQueue.Dequeue ();

	}


	// Use this for initialization
	void Start () {
		GameStart = true;
	}

	public void RoundNextController(){
		m_playerControllerQueue.Enqueue (m_nowControler);
		m_nowControler = m_playerControllerQueue.Dequeue ();
		if (GUIController.Instance != null) {
			GUIController.Instance.UpdatePlayerController (m_nowControler);
		}

	}
	
//	// Update is called once per frame
//	void Update () {
//		
//	}
		
}
