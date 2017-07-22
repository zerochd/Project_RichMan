using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : ActorController {

	static EnemyController _instance;

	public static EnemyController Instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<EnemyController> ();
			}
			return _instance;
		}
	}

	[SerializeField] Enemy enemy;

		
	void Awake(){
		_instance = this;
		base.Init ();
	}

	public override void Register (Actor newActor)
	{
		base.Register (newActor);
	}

	public override void RoundFirstActor ()
	{
		base.RoundFirstActor ();

		if (actorList.Count >= 1) {
			enemy = actorList [0] as Enemy;
			MyCamera.Instance.LookAtTarget (enemy.ActorTransform.position, 15f);
			enemy.ActiveActor (true);
		}
			
		Debug.Log ("enter Enemy Round");

		Invoke ("Test", 2f);
//		if(GameManager.Instance != null){
//			GameManager.Instance.RoundEndActorController(this);
//		}
	}
		

	void Test(){
		if(GameManager.Instance != null){
			GameManager.Instance.RoundEndActorController(this);
		}
	}

	public override void RoundEndActor (Actor nowActor)
	{
		base.RoundEndActor (nowActor);
	}

	public override Actor RoundNextActor (Actor nowActor)
	{
		return base.RoundNextActor (nowActor);
	}

	void Update(){
		if (!enableControl)
			return;
	}
		

}
