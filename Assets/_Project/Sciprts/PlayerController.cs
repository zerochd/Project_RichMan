using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

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
		DONE
		//结束
	}

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

	Grid hoverGrid;
	Stack<Grid> getGrids = new Stack<Grid> ();

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

	void Awake ()
	{
		_instance = this;
	}

	void Update ()
	{
		if (player == null)
			return;

		CommandUpdate ();
	}

	void CommandUpdate(){


		switch (command) {

		case COMMAND.NONE:
			break;
		case COMMAND.MOVE:
			{
				//ray cast
				Ray _ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit _hit = new RaycastHit ();

				if (Physics.Raycast (_ray, out _hit, 1000f)) {
					Grid _grid = _hit.collider.GetComponentInParent<Grid> ();

					if (_grid == null)
						return;

					if (_grid.owner == null) {

						if (hoverGrid && hoverGrid != _grid) {
							hoverGrid.ResetGridColor ();
							if (map != null)
								map.ResetColor ();

							getGrids = AStar.CalcPath (player.StandGrid, _grid, map);

							foreach (Grid _gd in getGrids.ToArray()) {
								_gd.SetGridColor (Color.blue);
							}

						}

						hoverGrid = _grid;	
						hoverGrid.SetGridColor (Color.yellow);

						//press mouse button 0
						if (Input.GetMouseButtonDown (0)) {

							if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject ())
								return;

							command = COMMAND.DOING;

							hoverGrid.SetGridColor (Color.red);

							//move player
							MovePlayer (player, getGrids);

							return;
						}

					} else {
						if (hoverGrid) {
							hoverGrid.ResetGridColor ();
						}
					}
				} else {
					if (hoverGrid) {
						hoverGrid.ResetGridColor ();
					}
				}
			}
			break;
		case COMMAND.ATTACK:
			{
				if (map != null && player.StandGrid != null) {

					#region show AttackRange

					//获得玩家所在grid的坐标
					VectorInt2 _playerVi = player.StandGrid.Vi;

					//获得攻击范围数据(fake temp)
					VectorInt2[] _attackViArray = new VectorInt2[4];
					int _index = 0;
					for (int i = -1; i <= 1; i++) {
						for (int j = -1; j <= 1; j++) {

							if (i == j || i == -j)
								continue;

							_attackViArray [_index] = new VectorInt2 (i, j);
							_index++;

							if (_index >= _attackViArray.Length)
								break;
						}
					}

					foreach (var _attackVi in _attackViArray) { 
						VectorInt2 _resultAttackVi = _playerVi + _attackVi;
						if (_resultAttackVi < VectorInt2.Zero || _resultAttackVi > (map.Edge - VectorInt2.One)) {
							continue;
						}
						if (map.gridMat [_resultAttackVi.x, _resultAttackVi.y] is BuildGrid) {
							continue;
						}

						map.gridMat [_resultAttackVi.x, _resultAttackVi.y].SetGridColor (Color.magenta);

					}

					#endregion


					Ray _ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit _hit = new RaycastHit ();

					if (Physics.Raycast (_ray, out _hit, 1000f)) {
						Grid _grid = _hit.collider.GetComponentInParent<Grid> ();

						if (_grid == null)
							return;

						if (_grid.owner != null) {
							if (Input.GetMouseButtonDown (0)) {
//								Debug.Log ("grid.owner:" + _grid.owner.name);

								if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject ())
									return;

								command = COMMAND.DOING;
								PlayerFight (player, _grid.owner);
								return;
							}

						}

					}

				}
			}
			break;
		case COMMAND.BUILD:
			{
				if (map != null && player.StandGrid != null) {

					#region show build range

					//获得玩家所在grid的坐标
					VectorInt2 _playerVi = player.StandGrid.Vi;

					//获得建造范围数据(fake temp)
					VectorInt2[] _buildViArray = new VectorInt2[4];
					int _index = 0;
					for (int i = -1; i <= 1; i++) {
						for (int j = -1; j <= 1; j++) {

							if (i == j || i == -j)
								continue;

							_buildViArray [_index] = new VectorInt2 (i, j);
							_index++;

							if (_index >= _buildViArray.Length)
								break;
						}
					}

					foreach (var _buildVi in _buildViArray) { 
						VectorInt2 _resultBuildVi = _playerVi + _buildVi;
						if (_resultBuildVi < VectorInt2.Zero || _resultBuildVi > (map.Edge - VectorInt2.One)) {
							continue;
						}
						if (map.gridMat [_resultBuildVi.x, _resultBuildVi.y] is BuildGrid) {
							continue;
						}

						map.gridMat [_resultBuildVi.x, _resultBuildVi.y].SetGridColor (Color.green);



					}

					#endregion
			
					Ray _ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit _hit = new RaycastHit ();

					if (Physics.Raycast (_ray, out _hit, 1000f)) {
						Grid _grid = _hit.collider.GetComponentInParent<Grid> ();

						if (_grid == null)
							return;

						if (_grid.owner == null) {
							if (Input.GetMouseButtonDown (0)) {

								if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject ())
									return;

//								command = COMMAND.DOING;
								PlayerBuild (player, _grid);
								return;
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
		case COMMAND.DONE:
			{
				command = COMMAND.NONE;
			}
			break;
		default:
			throw new System.ArgumentOutOfRangeException ();
		}
	}

	#region public FUNC

	public void ApplyCommand (COMMAND applyCommand)
	{
		if (map != null) {
			map.ResetColor ();
		}

		this.command = applyCommand;
	}


	public void PlayerMoveDone ()
	{
		map.ResetColor ();
		ApplyCommand (COMMAND.DONE);
	}

	//	public void MovePlayer(Stack<Grid> moveGridStack){
	//		MovePlayer (player, moveGridStack);
	//	}

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
	}

	void PlayerBuild(Player player,Grid grid){
		if (player == null) {
			Debug.LogError ("no player");
			return;
		}

//		GUIController.Instance.ShowGameEvent
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
