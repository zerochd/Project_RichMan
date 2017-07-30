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

	[SerializeField] Map map;
	[SerializeField] Enemy enemy;

	COMMAND command = COMMAND.NONE;
	COMMAND prevCommand = COMMAND.NONE;

	Stack<Grid> getMoveGrids = new Stack<Grid>();

	int enemyDoneNum;

	void Awake(){
		_instance = this;
		base.Init ();
		enemyDoneNum = 0;
	}

	void Update(){
		if (enemy == null)
			return;

		if (!enableControl)
			return;

		CommandUpdate ();
	}


	void CommandUpdate(){
		switch (command) {
		case COMMAND.NONE:
			command = COMMAND.MOVE;
			break;
		case COMMAND.MOVE:


			VectorInt2 _enemyVi = enemy.StandGrid.Vi;
			VectorInt2[] _moveViArray = Util.GetViCricleByRadius (enemy.enemyData.speed);

			Grid _betterMoveGrid = null;
			VectorInt2 _closeVi = VectorInt2.Zero;
			//temp get player
			Player _player = PlayerController.Instance.PlayerEntity;

			foreach (var _moveVi in _moveViArray) {
				
				VectorInt2 _resultMoveVi = _enemyVi + _moveVi;

				if (_resultMoveVi < VectorInt2.Zero || _resultMoveVi > (map.Edge - VectorInt2.One)) {
					continue;
				}
				if (map.gridMat [_resultMoveVi.x, _resultMoveVi.y] is BuildGrid) {
					continue;
				}
				if (map.gridMat [_resultMoveVi.x, _resultMoveVi.y].Owner != null) {
					continue;
				}

				VectorInt2 _tempVi = _resultMoveVi - _player.StandGrid.Vi;
				if (_closeVi == VectorInt2.Zero) {
					_closeVi = _tempVi;
					_betterMoveGrid = map.gridMat [_resultMoveVi.x, _resultMoveVi.y];
				} else if (_closeVi > _tempVi) {
					_closeVi = _tempVi;
					_betterMoveGrid = map.gridMat [_resultMoveVi.x, _resultMoveVi.y];
				}

			}

			getMoveGrids = AStar.CalcPath (enemy.StandGrid, _betterMoveGrid, map);

			command = COMMAND.DOING;

			MoveEnemy (enemy, getMoveGrids);

			//--select next command by enemy
//			command = COMMAND.ATTACK;
//			command = COMMAND.DEFENSE;
			break;
		case COMMAND.ATTACK:
			break;
		case COMMAND.BUILD:
			break;
		case COMMAND.DEFENSE:
			break;
		case COMMAND.CANCEL:
			break;
		case COMMAND.DOING:
			break;
		case COMMAND.END:
			break;
		case COMMAND.DONE:
			{
				command = COMMAND.NONE;
				RoundEndEnemy (enemy);
			}
			break;
		default:
			throw new System.ArgumentOutOfRangeException ();
		}

		prevCommand = command;
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
		enemyDoneNum = 0;	
		Debug.Log ("enter Enemy Round");

	}

	public void ApplyCommand (COMMAND applyCommand)
	{
		this.command = applyCommand;
	}

	public void EnemyMoveDone(){
		ApplyCommand (COMMAND.DONE);
	}
		

	void MoveEnemy(Enemy enemy,Stack<Grid> moveGridStack){
		if (enemy == null) {
			Debug.LogError ("no enemy");
			return;
		}
		enemy.GetMoveStack (moveGridStack);
	}

	public void RoundEndEnemy(Enemy nowEnemy){
		base.RoundEndActor (nowEnemy);
		enemyDoneNum++;

		if (enemyDoneNum >= actorList.Count) {
			enemyDoneNum = 0;
			if(GameManager.Instance != null){
				GameManager.Instance.RoundEndActorController(this);
			}
		} else {
			RoundNextEnemy (nowEnemy);
		}

	}

	public void RoundNextEnemy(Enemy nowEnemy){
		enemy = base.RoundNextActor (nowEnemy) as Enemy;
		enemy.ActiveActor (true);

		if (MyCamera.Instance != null) {
			MyCamera.Instance.LookAtTarget (nowEnemy.ActorTransform.position, 15f);
		}
	}

	public override Actor RoundNextActor (Actor nowActor)
	{
		return base.RoundNextActor (nowActor);
	}
		

}
