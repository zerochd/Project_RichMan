using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Actor {

	Animator animator;

	[SerializeField] Actor owner;
	[SerializeField] Grid locateGrid;

	public void Init(Actor owner,Grid grid){
		this.owner = owner;
		this.locateGrid = grid;
		animator = GetComponent<Animator> ();
		actorTransform = animator.transform;
		grid.Arrived (this);

		actorTransform.position = grid.transform.position;
		animator.enabled = true;
	}

	public override void UnderAttack (AttackData attackData)
	{
		base.UnderAttack (attackData);
	}
}
