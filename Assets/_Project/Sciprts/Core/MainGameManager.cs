using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum GAME_STATE{
	START,
	RUNNING,
	OVER
}

public class MainGameManager : MonoBehaviour {

	public static MainGameManager _instance;

	public static MainGameManager Instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<MainGameManager> ();
			}
			return _instance;
		}
	}

	[FormerlySerializedAs("GameStart")] [SerializeField] bool gameStart;
	[SerializeField] private List<ActorController> actorControllerList = new List<ActorController> ();

	[SerializeField] private ActorController nowActorController;

	[SerializeField] private PlayerController playerControler;
	[SerializeField] private EnemyController enemyController;

	public PlayerController NowControler {
		get {
			return playerControler;
		}
	}

	void Awake(){
		_instance = this;
	}

	IEnumerator Start(){

		yield return new WaitForSeconds (2f);

		Init ();
	}

	void Init(){
		gameStart = true;
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

		if (!gameStart)
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
		gameStart = false;
		Debug.Log ("GameOver");
	}
		
}
