using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyData{
	public int hp;
	public int damage;
	public int defense;
	public int exp;
}

public class Enemy : Actor {

	public string enemyName;

	public EnemyData enemyData;

	[SerializeField] MoveGrid standMoveGrid;
	[SerializeField] bool moveDone = true;

	Animator animator;

	// Use this for initialization
	void Start () {
		Init ();
	}

	[ContextMenu("Init")]
	void Init(){
		animator = GetComponentInChildren<Animator> ();

		if (animator != null) {
			actorTransform = animator.transform;
		}

		moveDone = true;

		SetupBornGrid ();
	}
		
	public virtual void UnderAttack(AttackData attackData){

		int _finalDamage = attackData.damage - enemyData.defense;
		if (_finalDamage < 0)
			_finalDamage = 0;
		enemyData.hp -= _finalDamage;
		if (enemyData.hp < 0) {

			if (attackData.attacker != null) {
				attackData.attacker.GetExp (enemyData.exp);
			}

			Dead ();
		}
	}

	public virtual void Dead(){
		standMoveGrid.Reset ();
		standMoveGrid = null;
		actorTransform.gameObject.SetActive (false);
	}

	void SetupBornGrid(){
		if (actorTransform == null)
			return;
		RaycastHit _hit;
		if (Physics.Raycast (actorTransform.position + actorTransform.up * 0.2f, actorTransform.up * (-1f), out _hit, 3f, 1 << LayerMask.NameToLayer ("Grid"))) {
			
			MoveGrid _mg = _hit.collider.GetComponent<MoveGrid> ();
			standMoveGrid = _mg;
			standMoveGrid.Arrived (this);
		} 
	}
}
