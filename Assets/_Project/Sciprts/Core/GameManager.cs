using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GAME_STATE{
	START,
	RUNNING,
	OVER
}

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
	[SerializeField] List<ActorController> actorControllerList = new List<ActorController> ();

	[SerializeField] ActorController nowActorController;

	[SerializeField] PlayerController playerControler;
	[SerializeField] EnemyController enemyController;

	public PlayerController NowControler {
		get {
			return playerControler;
		}
	}

	void Awake(){
		_instance = this;
//		actorControllerList.Clear ();
	}
	// Use this for initialization
//	void Start () {
//		Init ();
//	}

	IEnumerator Start(){

		yield return new WaitForSeconds (2f);

		Init ();
	}

	void Init(){
		GameStart = true;
		foreach (var actorController in actorControllerList) {
			actorController.EnableControl = false;
		}

		if (actorControllerList.Count >= 1) {
			nowActorController = actorControllerList [0];
			nowActorController.EnableControl = true;
			nowActorController.RoundFirstActor ();

		}

	}
		
	public void RoundEndActorController(ActorController actorController){

		actorController.EnableControl = false;

		if (!GameStart)
			return;

		RoundNextActorController (actorController);
	}

	public void RoundNextActorController(ActorController actorController){
		int _tempIndex = actorControllerList.IndexOf (actorController) + 1;

		if (_tempIndex >= actorControllerList.Count) {
			_tempIndex = 0;
		}

		ActorController _actorController = actorControllerList [_tempIndex];
		_actorController.EnableControl = true;
		_actorController.RoundFirstActor ();

	}

	public void CheckGameOver(ActorController actorController){
		if (actorController is EnemyController) {
			Debug.Log ("Player win");
		} else {
			Debug.Log ("Enemy win");
		}
		GameStart = false;
		Debug.Log ("GameOver");
	}
		
}
