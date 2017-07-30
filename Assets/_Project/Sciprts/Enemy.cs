using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData : ActorData{
	public int exp;
}

public sealed class Enemy : Actor {
	
	public EnemyData enemyData;

	[SerializeField] bool isMoving = false;

	Stack<Grid> moveGridStack = new Stack<Grid> ();
	// Use this for initialization
	void Start () {
		Init ();
	}

	void Update(){
		if (!underControl)
			return;

		MoveUpdate ();
	}

	void MoveUpdate(){
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

				if (EnemyController.Instance != null) {
					EnemyController.Instance.EnemyMoveDone ();
				}

				Anim_Idle ();

				isMoving = false;
				_nextMoveGrid = null;
			}
		}
	}

	[ContextMenu("Init")]
	protected override void Init(){
		
		base.Init ();

		isMoving = false;

		SetupBornGrid ();

		if (EnemyController.Instance != null) {
			EnemyController.Instance.Register (this);
		}

	}
		
	public override void UnderAttack(AttackData attackData){

		int _finalDamage = attackData.damage - enemyData.defense;
		if (_finalDamage < 0)
			_finalDamage = 0;
		enemyData.hp -= _finalDamage;
		if (enemyData.hp <= 0) {

			if (attackData.attacker != null) {
				attackData.attacker.SendMessage ("GetExp", enemyData.exp, SendMessageOptions.DontRequireReceiver);
			}

			Dead ();
		}
	}

	public override void Dead ()
	{
		base.Dead ();
		if (EnemyController.Instance != null) {
			EnemyController.Instance.DeadActor (this);
		}
		standGrid.ResetValue ();
		standGrid = null;
		actorTransform.gameObject.SetActive (false);
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

		this.actorTransform.position = Vector3.MoveTowards (this.actorTransform.position, nextGrid.transform.position, 20f * Time.deltaTime);
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

	public void GetMoveStack (Stack<Grid> moveGridStack)
	{
		this.moveGridStack = moveGridStack;

		isMoving = true;
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

	#endregion
}
