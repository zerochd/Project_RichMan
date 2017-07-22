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

public sealed class Enemy : Actor {

	public string enemyName;
	public EnemyData enemyData;

	[SerializeField] bool moveDone = true;

	// Use this for initialization
	void Start () {
		Init ();
	}

	[ContextMenu("Init")]
	protected override void Init(){
		
		base.Init ();

		moveDone = true;

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


}
