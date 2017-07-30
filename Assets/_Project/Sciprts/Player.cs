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
public class PlayerData : ActorData
{
	public int money = 0;
	public int level = 1;
	public int exp = 0;
	public int nextLevelExp = 5;

}

public sealed class Player : Actor
{
	public PlayerData playerData;

	//move one gird speed;
	[SerializeField] int moveSpeed = 4;
	[SerializeField] int moveStep;
	[SerializeField] bool isMoving = false;

	MeshRenderer meshRenderer;
	Stack<Grid> moveGridStack = new Stack<Grid> ();

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
		if (!underControl)
			return;

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

				if (MoveDone (_nextMoveGrid)) {

					Anim_Idle ();

					standGrid = _nextMoveGrid;
					moveGridStack.Pop ();

					if (moveGridStack.Count <= 0)
						standGrid.Arrived (this);
				} else {

					Anim_Move ();

					if (_nextMoveGrid.Owner == null) {
						
						MoveToGrid (_nextMoveGrid);
					}
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

	}

	[ContextMenu ("Init")]
	protected override void Init ()
	{
		base.Init ();

		if (meshRenderer == null) {
			meshRenderer = GetComponentInChildren<MeshRenderer> ();
		}

		isMoving = false;

		SetupBornGrid ();

		if (playerData.hp > playerData.maxHp) {
			playerData.hp = playerData.maxHp;
		}

		if (GUIController.Instance != null) {
			GUIController.Instance.UpdateMainUI (playerData);
		}

		if (PlayerController.Instance != null) {
			PlayerController.Instance.Register (this);
		}

//		ActiveActor (true);
	}

	public void TurnToGrid(Grid nextGrid){
		if (nextGrid == null || nextGrid.transform == null)
			return;

		this.actorTransform.LookAt (nextGrid.transform.position, Vector2.up);
		
	}

	public void MoveToGrid (Grid nextGrid)
	{
		if (nextGrid == null)
			return;

		if (standGrid != null) {
			standGrid.Free ();
			standGrid = null;
			TurnToGrid (nextGrid);
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
		this.actorTransform.LookAt (other.ActorTransform.position, Vector3.up);
		other.ActorTransform.LookAt (this.actorTransform.position,Vector3.up);
		attackData = new AttackData (this, other, playerData.damage);
		animator.SetInteger("AttackID",1);
		animator.SetTrigger("once");

	}

	public override void UnderAttack (AttackData attackData)
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

	public override void ActiveActor(bool val){
		if (val) {
			if (GUIController.Instance != null) {
				GUIController.Instance.UpdateMainUI (playerData);
			}
		} else {

		}
			
		base.ActiveActor (val);
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

		if(attackData == null)
			return;

		if (attackData.attackTarget is Enemy) {
						
			Debug.Log("is enemy");
			Enemy _enemy = attackData.attackTarget as Enemy;
			AttackData _attackData = new AttackData (this, _enemy, playerData.damage);
			_enemy.UnderAttack (_attackData);
			if (GUIController.Instance != null) {
				GUIController.Instance.UpdateMainUI (playerData);	
			}

		} else if (attackData.attackTarget is Player) {
		
		}else if(attackData.attackTarget is Building){
			Debug.Log ("is building");
			Building _building = attackData.attackTarget as Building;
			AttackData _attackData = new AttackData (this, _building, playerData.damage);
		}

		if (PlayerController.Instance != null) {
			PlayerController.Instance.PlayerFightDone ();
		}



	}

	#endregion


}
