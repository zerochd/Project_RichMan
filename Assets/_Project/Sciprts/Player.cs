using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackData{
	public Player attacker;
	public Actor attackTarget;
	public int damage;

	public AttackData(){
	}

	public AttackData(Player attacker,Actor attackTarget,int damage){
		this.attacker = attacker;
		this.attackTarget = attackTarget;
		this.damage = damage;
	}
}

[System.Serializable]
public struct PlayerData{
	public int hp;
	public int damage;
	public int defense;
	public int speed;
	public int level;
	public int exp;
	public int nextLevelExp;

}

public class Player : Actor
{
	public string playerName;

	public PlayerData playerData;

	[SerializeField] MoveGrid standMoveGrid;
	[SerializeField] MoveGrid nextMoveGrid;

	//move one gird speed;
	[SerializeField] int moveSpeed = 4;
	[SerializeField] int moveStep;
	[SerializeField] bool moveDone = true;


	Animator animator;

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
		
		if (moveStep > 0) {
			
			if (standMoveGrid != null)
				nextMoveGrid = standMoveGrid.NextGird;


			if (MoveDone ()) {
				
				if (moveDone == false) {
					Anim_Idle ();
					GetExp (nextMoveGrid.exp);
				}

				moveDone = true;

				standMoveGrid = nextMoveGrid;
				moveStep--;

				//excute 
				if (moveStep == 0 && standMoveGrid != null) {
					standMoveGrid.Arrived (this);

					if (standMoveGrid is MoveGrid) {
						//轮转至下一个玩家
						if (GameManager.Instance != null) {
							GameManager.Instance.RoundNextController ();
						}
					}

				}
					

			} else {

				if (moveDone == true) {

					Anim_Turn (nextMoveGrid);

					Anim_Move ();
				}

				moveDone = false;

				if (nextMoveGrid.owner != null) {
					Fight (nextMoveGrid.owner);
				} else {
					MoveToGrid (nextMoveGrid);
				}
			}
				
		} else {
			
			Anim_Idle ();
		}
	}

	[ContextMenu("Init")]
	void Init ()
	{
		animator = GetComponentInChildren<Animator> ();

		if (animator != null) {
			actorTransform = animator.transform;
		}

		moveDone = true;

		SetupBornGrid ();
	}

	void SetupBornGrid(){
		if (actorTransform == null)
			return;

		RaycastHit _hit;

		if(Physics.Raycast(actorTransform.position + actorTransform.up * 0.2f,actorTransform.up * (-1f),out _hit,2f,1 << LayerMask.NameToLayer("Grid"))){
			MoveGrid _mg = _hit.collider.GetComponent<MoveGrid> ();
			standMoveGrid = _mg;
			standMoveGrid.Arrived (this);
		}
	}

	public void MoveToGrid (MoveGrid nextMoveGrid)
	{
		if (standMoveGrid != null) {
			standMoveGrid.Reset ();
		}

		if (nextMoveGrid == null)
			return;

		this.actorTransform.position = Vector3.MoveTowards (this.actorTransform.position, nextMoveGrid.transform.position, moveSpeed * Time.deltaTime);

	}


	public void CalcMoveStep (int step)
	{
		moveStep = step;

	}

	bool MoveDone ()
	{
		if (actorTransform == null || nextMoveGrid == null)
			return true;

		Vector3 _actorTransform_noY = new Vector3 (actorTransform.position.x, nextMoveGrid.transform.position.y, actorTransform.position.z);

		float _distance = Vector3.Distance (nextMoveGrid.transform.position, _actorTransform_noY);

		if (_distance <= float.Epsilon + 0.01f) {
			return true;
		}

		return false;
	}

	public bool Fight(Actor owner){
		if (owner is Enemy) {
			Enemy _enemy = owner as Enemy;
			AttackData _attackdata = new AttackData (this, _enemy, playerData.damage);
			_enemy.UnderAttack (_attackdata);

			return true;
		} else if (owner is Player) {
		
		}

		return false;
	}

	public void GetExp(int exp){
		playerData.exp += exp;
		LevelUP ();
	}

	public void LevelUP(){

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

	void Anim_Turn(MoveGrid nextGird){

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

	void Anim_Move(){

//		if (!isAnimationIdle ())
//			return;
		
		animator.SetBool ("move",true);
	}

	void Anim_Idle()
	{
		animator.SetBool ("move", false);
	}

	#endregion
	//	IEnumerator MoveCor(int step){
	//
	//
	//	}

}
