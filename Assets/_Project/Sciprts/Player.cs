using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackData
{
	public Actor attacker;
	public Actor attackTarget;
	public int damage;

	public AttackData ()
	{
	}

	public AttackData (Actor attacker, Actor attackTarget, int damage)
	{
		this.attacker = attacker;
		this.attackTarget = attackTarget;
		this.damage = damage;
	}
}

[System.Serializable]
public class PlayerData
{
	public string palyerName;
	public int hp = 5;
	public int maxHp = 5;
	public int damage = 1;
	public int defense = 0;
	public int speed = 6;
	public int money = 0;


	public int level = 1;
	public int exp = 0;
	public int nextLevelExp = 5;

}

public class Player : Actor
{
	public PlayerData playerData;

	[SerializeField] Grid standGrid;
	//	[SerializeField] Grid nextMoveGrid;

	//move one gird speed;
	[SerializeField] int moveSpeed = 4;
	[SerializeField] int moveStep;
	[SerializeField] bool isMoving = false;

	Animator animator;

	Stack<Grid> moveGridStack = new Stack<Grid> ();

	public Grid StandGrid {
		get {
			return standGrid;
		}
	}

	public int MoveStep {
		get {
			return moveStep;
		}
	}

	#region unity common function

	void Awake ()
	{
	}

	void Start ()
	{
		Init ();	
	}

	void Update ()
	{
		MoveUpdate ();
	}

	#endregion


	void MoveUpdate ()
	{
		//是否正在移动

		if (isMoving) {

			Grid _nextMoveGrid = null;

			if (moveGridStack.Count > 0) {
		
				_nextMoveGrid = moveGridStack.Peek ();
				
//				Debug.Log (Time.time + "nextMoveGrid:" + _nextMoveGrid.name);

				if (MoveDone (_nextMoveGrid)) {

					Anim_Idle ();

					standGrid = _nextMoveGrid;
					moveGridStack.Pop ();

					if (moveGridStack.Count <= 0)
						standGrid.Arrived (this);
				} else {


//					Anim_Turn (_nextMoveGrid);
					Anim_Move ();

					if (_nextMoveGrid.owner == null)
						MoveToGrid (_nextMoveGrid);
				}

			} else {

				if (!MoveDone (_nextMoveGrid))
					return;

				if (PlayerController.Instance != null) {
					PlayerController.Instance.PlayerMoveDone ();
				}
				
				Anim_Idle ();

				isMoving = false;
				_nextMoveGrid = null;
			}
		}




//		if (moveStep > 0) {
//			
//			if (standGrid != null)
//				nextMoveGrid = standGrid.NextGird;
//
//
//			if (MoveDone ()) {
//				
//				if (isMoving == false) {
//					Anim_Idle ();
//					GetExp (nextMoveGrid.exp);
//				}
//
//				isMoving = true;
//
//				standGrid = nextMoveGrid;
//				moveStep--;
//
//				//excute 
//				if (moveStep == 0 && standGrid != null) {
//					standGrid.Arrived (this);
//
//					if (standGrid is MoveGrid) {
//						//轮转至下一个玩家
//						if (GameManager.Instance != null) {
//							GameManager.Instance.RoundNextController ();
//						}
//					}
//
//				}
//					
//
//			} else {
//
//				if (isMoving == true) {
//
//					Anim_Turn (nextMoveGrid);
//
//					Anim_Move ();
//				}
//
//				isMoving = false;
//
//				if (nextMoveGrid.owner != null) {
//					Fight (nextMoveGrid.owner);
//				} else {
//					MoveToGrid (nextMoveGrid);
//				}
//			}
//				
//		} else {
//			
//			Anim_Idle ();
//		}
	}

	[ContextMenu ("Init")]
	void Init ()
	{
		animator = GetComponentInChildren<Animator> ();

		if (animator != null) {
			actorTransform = animator.transform;
		}

		isMoving = false;

		SetupBornGrid ();

		if (playerData.hp > playerData.maxHp) {
			playerData.hp = playerData.maxHp;
		}

		if (GUIController.Instance != null) {
			GUIController.Instance.UpdateMainUI (playerData);
		}
	}


	void SetupBornGrid ()
	{
		if (actorTransform == null)
			return;

		RaycastHit _hit;
		if (Physics.Raycast (actorTransform.position + actorTransform.up * 5f, actorTransform.up * (-1f), out _hit, 100f, 1 << LayerMask.NameToLayer ("Grid"))) {
			Grid _mg = _hit.collider.GetComponentInParent<Grid> ();
			standGrid = _mg;
			standGrid.Arrived (this);
		}
	}

	public void MoveToGrid (Grid nextGrid)
	{
		if (nextGrid == null)
			return;

		if (standGrid != null) {
			standGrid.Free ();
			standGrid = null;
		}

		this.actorTransform.position = Vector3.MoveTowards (this.actorTransform.position, nextGrid.transform.position, moveSpeed * Time.deltaTime);

	}


	public void CalcMoveStep (int step)
	{
		moveStep = step;
	}

	public void GetMoveStack (Stack<Grid> moveGridStack)
	{
		this.moveGridStack = moveGridStack;

//		Debug.Log ("called: " + this.moveGridStack.Count);
		isMoving = true;
	}

	bool MoveDone (Grid nextGrid)
	{
		if (actorTransform == null) {
			Debug.LogError ("no actorTransform");
			return true;
		}

		if (nextGrid == null) {
			return true;
		}

		Vector3 _actorTransform_noY = new Vector3 (actorTransform.position.x, nextGrid.transform.position.y, actorTransform.position.z);

		float _distance = Vector3.Distance (nextGrid.transform.position, _actorTransform_noY);

		if (_distance <= float.Epsilon + 0.01f) {
			return true;
		}

		return false;
	}

	AttackData attackData;
	public void Fight (Actor other)
	{
		attackData = new AttackData (this, other, playerData.damage);
		animator.SetInteger("AttackID",1);
		animator.SetTrigger("once");

//		if (other is Enemy) {
//			
//			Enemy _enemy = other as Enemy;
//			AttackData _attackData = new AttackData (this, _enemy, playerData.damage);
//			_enemy.UnderAttack (_attackData);
//			if (GUIController.Instance != null) {
//				GUIController.Instance.UpdateMainUI (playerData);	
//			}
//
//			return true;
//		} else if (other is Player) {
//		
//		}
//
//		return false;


	}



	public void UnderAttack (AttackData attackData)
	{
		int _finalDamage = attackData.damage - playerData.defense;
		if (_finalDamage < 0)
			_finalDamage = 0;
		playerData.hp -= _finalDamage;

		if (playerData.hp < 0) {
			Dead ();
		}
			
	}

	public void Dead ()
	{
		
	}

	public void GetExp (int exp)
	{
		playerData.exp += exp;
		LevelUP ();
	}

	public void LevelUP ()
	{

		if (playerData.exp >= playerData.nextLevelExp) {
			playerData.level++;
			playerData.nextLevelExp = 500;
		}
	}

	#region anim_FUNCTION

	bool isAnimationIdle ()
	{
		if (animator == null)
			return true;
		AnimatorStateInfo _stateInfo = animator.GetCurrentAnimatorStateInfo (animator.GetLayerIndex ("Base"));
		return _stateInfo.IsName ("Idle");
	}

	void Anim_Turn (Grid nextGird)
	{

		Vector3 _direction_player_2_targetPosition = nextGird.transform.position - actorTransform.position;

		float _angle = Vector3.Angle (actorTransform.forward, _direction_player_2_targetPosition);
		//y > 0 is right
		//else is left

		if (Mathf.Abs (_angle) > 45f) {

			float _y = Vector3.Cross (actorTransform.forward.normalized, _direction_player_2_targetPosition.normalized).y;

			if (_y > 0f) {
				animator.SetTrigger ("right");
			} else {
				animator.SetTrigger ("left");
			}
		}

	}

	void Anim_Move ()
	{
		animator.SetBool ("move", true);
	}

	void Anim_Idle ()
	{
		animator.SetBool ("move", false);
	}


	//message-function
	void AttackEnd(){

//		Debug.Log("called");

		if(attackData == null)
			return;

//		Debug.Log("attackTarget:"+attackData.attackTarget.name);
//		Debug.Log("attackTarget is:"+attackData.attackTarget is Enemy);


		if (attackData.attackTarget is Enemy) {
						
			Debug.Log("is enemy");
			Enemy _enemy = attackData.attackTarget  as Enemy;
			AttackData _attackData = new AttackData (this, _enemy, playerData.damage);
			_enemy.UnderAttack (_attackData);
			if (GUIController.Instance != null) {
				GUIController.Instance.UpdateMainUI (playerData);	
			}

		} else if (attackData.attackTarget  is Player) {
		
		}
	}

	#endregion


}
