using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum COMMAND
{
	NONE = -1,
	//无指令
	MOVE = 0,
	//移动
	ATTACK,
	//攻击
	BUILD,
	//建造
	DEFENSE,
	//防御
	CANCEL,
	//取消
	DOING,
	//执行中
	END,
	//直接结束
	DONE
	//结束
}

public class PlayerController : ActorController
{

	#region var

	static PlayerController _instance;

	public static PlayerController Instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<PlayerController> ();
			}
			return _instance;
		}
	}
		
	[SerializeField] Player player;
	[SerializeField] Map map;
	[SerializeField] COMMAND command = COMMAND.NONE;
	[SerializeField] Transform gridSelect;

	COMMAND prevCommand = COMMAND.NONE;

//	Grid hoverGrid;
	List<Grid> getActiveGirds = new List<Grid> ();
	Stack<Grid> getMoveGrids = new Stack<Grid> ();
	int playerDoneNum;

	public Player PlayerEntity {
		get {
			return player;
		}
	}
	public COMMAND Command {
		get {
			return command;
		}
	}
		
	#endregion

	void Awake ()
	{
		_instance = this;
		base.Init ();
		playerDoneNum = 0;
		if (gridSelect != null) {
			gridSelect.gameObject.SetActive (false);
		}
	}

	void Update ()
	{
		if (player == null)
			return;

		if (!enableControl)
			return;

		CommandUpdate ();
	}

	void CommandUpdate(){


		switch (command) {

		case COMMAND.NONE:
			break;
		case COMMAND.MOVE:
			{
				if (prevCommand != COMMAND.MOVE) {
					getActiveGirds.Clear ();
					if (gridSelect != null) {
						gridSelect.gameObject.SetActive (false);
					}
				}

				#region showMoveRange
				VectorInt2 _playerVi = player.StandGrid.Vi;

				VectorInt2[] _moveViArray = Util.GetViCricleByRadius(player.playerData.speed);

				getActiveGirds.Clear();

				foreach (var _moveVi in _moveViArray) { 
					VectorInt2 _resultMoveVi = _playerVi + _moveVi;
					if (_resultMoveVi < VectorInt2.Zero || _resultMoveVi > (map.Edge - VectorInt2.One)) {
						continue;
					}
					if (map.gridMat [_resultMoveVi.x, _resultMoveVi.y] is BuildGrid) {
						continue;
					}
					getActiveGirds.Add(map.gridMat[_resultMoveVi.x, _resultMoveVi.y]);
					map.gridMat [_resultMoveVi.x, _resultMoveVi.y].SetGridColor (Color.blue);

				}
				#endregion

				//ray cast
				Ray _ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit _hit = new RaycastHit ();

				if (Physics.Raycast (_ray, out _hit, 1000f)) {
					Grid _grid = _hit.collider.GetComponentInParent<Grid> ();

					if (_grid == null)
						break;

					if (!getActiveGirds.Contains (_grid))
						break;

					if (gridSelect != null) {
						gridSelect.gameObject.SetActive (true);
						gridSelect.transform.position = _grid.transform.position;
					}

					if (_grid.Owner == null) {

						getMoveGrids = AStar.CalcPath (player.StandGrid, _grid, map);

						//press mouse button 0
						if (Input.GetMouseButtonDown (0)) {

							if (EventSystem.current.IsPointerOverGameObject ())
								break;

							if (gridSelect != null) {
								gridSelect.gameObject.SetActive (false);
							}

							command = COMMAND.DOING;
							//move player
							MovePlayer (player, getMoveGrids);


							break;
						}

					}
				}
			}
			break;
		case COMMAND.ATTACK:
			{

				if (prevCommand != COMMAND.ATTACK) {
					getActiveGirds.Clear ();
					if (gridSelect != null) {
						gridSelect.gameObject.SetActive (false);
					}
				}

				if (map != null && player.StandGrid != null) {

					#region show AttackRange

					//获得玩家所在grid的坐标
					VectorInt2 _playerVi = player.StandGrid.Vi;

					VectorInt2[] _attackViArray = Util.GetViCricleByRadius(1);

					getActiveGirds.Clear();

					foreach (var _attackVi in _attackViArray) { 
						VectorInt2 _resultAttackVi = _playerVi + _attackVi;
						if (_resultAttackVi < VectorInt2.Zero || _resultAttackVi > (map.Edge - VectorInt2.One)) {
							continue;
						}
						if (map.gridMat [_resultAttackVi.x, _resultAttackVi.y] is BuildGrid) {
							continue;
						}
						getActiveGirds.Add(map.gridMat[_resultAttackVi.x, _resultAttackVi.y]);
						map.gridMat [_resultAttackVi.x, _resultAttackVi.y].SetGridColor (Color.magenta);

					}

					#endregion


					Ray _ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit _hit = new RaycastHit ();

					if (Physics.Raycast (_ray, out _hit, 1000f)) {
						Grid _grid = _hit.collider.GetComponentInParent<Grid> ();

						if (_grid == null)
							break;

						if (!getActiveGirds.Contains (_grid))
							break;

						if (gridSelect != null) {
							gridSelect.gameObject.SetActive (true);
							gridSelect.transform.position = _grid.transform.position;
						}

						if (_grid.Owner != null) {

							if (Input.GetMouseButtonDown (0)) {

								if (EventSystem.current.IsPointerOverGameObject ())
									break;
								if (gridSelect != null) {
									gridSelect.gameObject.SetActive (false);
								}
								command = COMMAND.DOING;
								PlayerFight (player, _grid.Owner);
								break;
							}

						}

					}

				}
			}
			break;
		case COMMAND.BUILD:
			{
				if (prevCommand != COMMAND.BUILD) {
					getActiveGirds.Clear ();
					if (gridSelect != null) {
						gridSelect.gameObject.SetActive (false);
					}
				}

				if (map != null && player.StandGrid != null) {

					#region show build range

					//获得玩家所在grid的坐标
					VectorInt2 _playerVi = player.StandGrid.Vi;

					VectorInt2[] _buildViArray = Util.GetViCricleByRadius(1);

					getActiveGirds.Clear ();

					foreach (var _buildVi in _buildViArray) { 
						VectorInt2 _resultBuildVi = _playerVi + _buildVi;
						if (_resultBuildVi < VectorInt2.Zero || _resultBuildVi > (map.Edge - VectorInt2.One)) {
							continue;
						}
						if (map.gridMat [_resultBuildVi.x, _resultBuildVi.y] is BuildGrid) {
							continue;
						}

						getActiveGirds.Add(map.gridMat[_resultBuildVi.x, _resultBuildVi.y]);

						map.gridMat [_resultBuildVi.x, _resultBuildVi.y].SetGridColor (Color.green);

					}

					#endregion
			
					Ray _ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit _hit = new RaycastHit ();

					if (Physics.Raycast (_ray, out _hit, 1000f)) {
						Grid _grid = _hit.collider.GetComponentInParent<Grid> ();

						if (_grid == null)
							break;;

						if (!getActiveGirds.Contains (_grid))
							break;

						if (gridSelect != null) {
							gridSelect.gameObject.SetActive (true);
							gridSelect.transform.position = _grid.transform.position;
						}

						if (_grid.Owner == null) {

							if (Input.GetMouseButtonDown (0)) {

								if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject ())
									break;
								
								if (gridSelect != null) {
									gridSelect.gameObject.SetActive (false);
								}

								command = COMMAND.DOING;
								PlayerBuild (player, _grid);

								break;
							}

						}

					}

				}
			}
			break;
		case COMMAND.DEFENSE:
			{
			}
			break;
		case COMMAND.CANCEL:
			{
			}
			break;
		case COMMAND.DOING:
			{
			}
			break;
		case COMMAND.END:
			{
				PlayerEnd ();
			}
			break;
		case COMMAND.DONE:
			{
				command = COMMAND.NONE;
			}
			break;
		default:
			throw new System.ArgumentOutOfRangeException ();
		}

		prevCommand = command;
	}

	#region Command FUNC


	#endregion


	#region public FUNC

	public void ApplyCommand (COMMAND applyCommand)
	{
		if (map != null) {
			map.ResetColor ();
		}
		this.command = applyCommand;

	}

	public void PlayerEnd(){
		map.ResetColor ();
		ApplyCommand (COMMAND.DONE);

		if (CommandController.Instance != null && CommandController.Instance.LastCommandUI != null) {
			CommandController.Instance.LastCommandUI.CommandDone (player);
		} else {
			Debug.LogError ("CommandController Error");
		}
	}

	public void PlayerMoveDone ()
	{
		map.ResetColor ();
		ApplyCommand (COMMAND.DONE);

		if (CommandController.Instance != null && CommandController.Instance.LastCommandUI != null) {
			CommandController.Instance.LastCommandUI.CommandDone (player);
		} else {
			Debug.LogError ("CommandController Error");
		}
	}

	public void PlayerBuildDone(){
		map.ResetColor ();
		ApplyCommand (COMMAND.DONE);

		if (CommandController.Instance != null && CommandController.Instance.LastCommandUI != null) {
			CommandController.Instance.LastCommandUI.CommandDone (player);
		} else {
			Debug.LogError ("CommandController Error");
		}
			
	}

	public void PlayerFightDone(){
		
		map.ResetColor ();
		ApplyCommand (COMMAND.DONE);

		if (CommandController.Instance != null) {
			CommandController.Instance.ShowUI ();
		}

		if (CommandController.Instance != null && CommandController.Instance.LastCommandUI != null) {
			CommandController.Instance.LastCommandUI.CommandDone (player);
		} else {
			Debug.LogError ("CommandController Error");
		}

	}
		

	public override void Register (Actor newActor)
	{
		base.Register (newActor);
	}

	public override void RoundFirstActor ()
	{
		base.RoundFirstActor ();
		if (actorList.Count >= 1) {
			player = actorList [0] as Player;
			MyCamera.Instance.LookAtTarget (player.ActorTransform.position, 15f);
			player.ActiveActor (true);
		}
		playerDoneNum = 0;
		Debug.Log ("enter Player Round");
		if (CommandController.Instance != null) {
			CommandController.Instance.UpdateUI (player);
		}
	}


	public void RoundEndPlayer(Player nowPlayer){

		base.RoundEndActor (nowPlayer);

		playerDoneNum++;

		if (gridSelect != null) {
			gridSelect.gameObject.SetActive (false);
		}

		//如果所有人都结束了换下一波势力
		if (playerDoneNum >= actorList.Count) {
			playerDoneNum = 0;
			if(MainGameManager.Instance != null){
				MainGameManager.Instance.RoundEndActorController(this);
			}
			if (CommandController.Instance != null) {
				CommandController.Instance.HideUI ();
			}
		} else {
			
			RoundNextPlayer (nowPlayer);
		}
	}
		
	public void RoundNextPlayer(Player nowPlayer){

		player = base.RoundNextActor(nowPlayer) as Player;

		player.ActiveActor (true);

		if (MyCamera.Instance != null) {
			MyCamera.Instance.LookAtTarget (player.ActorTransform.position, 15f);
		}

		if (GUIController.Instance != null) {
			GUIController.Instance.UpdateMainUI (player.playerData);
		}
		if (CommandController.Instance != null) {
			CommandController.Instance.UpdateUI (player);
		}
	}

	#endregion


	#region private FUNC

	void MovePlayer (Player player, Stack<Grid> moveGridStack)
	{
		if (player == null) {
			Debug.LogError ("no player");
			return;
		}
		player.GetMoveStack (moveGridStack);
	}

	void PlayerFight (Player player, Actor other)
	{
		if (player == null) {
			Debug.LogError ("no player");
			return;
		}
		player.Fight (other);

		if (CommandController.Instance != null) {
			CommandController.Instance.HideUI ();
		}
	}

	void PlayerBuild(Player player,Grid grid){
		if (player == null) {
			Debug.LogError ("no player");
			return;
		}
		BuyBuildingEvent _buyBuildEvent = new BuyBuildingEvent ();
		_buyBuildEvent.buyers = player;
		_buyBuildEvent.setGrid = grid;

		GUIController.Instance.ShowBuyUI (_buyBuildEvent);
	}

	#endregion



	[System.Obsolete ("this function is odd,use void MovePlayer(Stack<Grid> moveGridStack) instand of ")]
	public void MovePlayer (int step)
	{
		MovePlayer (player, step);
	}

	[System.Obsolete ("this function is odd,use void MovePlayer(Stack<Grid> moveGridStack) instand of ")]
	void MovePlayer (Player player, int step)
	{
		if (player == null) {
			Debug.LogError ("no player");
			return;
		}
		player.CalcMoveStep (step);
	}
		
}
