using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorData
{
	public string actorName;
	public int hp = 5;
	public int maxHp = 5;
	public int damage = 1;
	public int defense = 0;
	public int speed = 3;
}


public class Actor : MonoBehaviour {

	[SerializeField] protected Transform actorTransform;
	[SerializeField] protected Animator animator;
	[SerializeField] protected Grid standGrid;
	[SerializeField] protected bool underControl;

	public Transform ActorTransform {
		get {
			return actorTransform;
		}
	}

	public Grid StandGrid {
		get {
			return standGrid;
		}
	}

	protected virtual void Init(){
		
		animator = GetComponentInChildren<Animator> ();

		if (animator != null) {
			actorTransform = animator.transform;
		}
	}

	protected virtual void SetupBornGrid ()
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

	public virtual void UnderAttack(AttackData attackData){
		
	}

	public virtual void ActiveActor(bool val){
		if (animator != null)
			animator.enabled = val;
		underControl = val;
	}

	public virtual void Dead(){
		
	}
}
