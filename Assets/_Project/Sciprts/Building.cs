using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Actor {


	[SerializeField] Actor owner;
	[SerializeField] Grid locateGrid;

	public void InitBuilding(Actor owner,Grid grid){
		base.Init ();
		this.owner = owner;
		this.locateGrid = grid;
		grid.Arrived (this);

		actorTransform.position = grid.transform.position;
		animator.enabled = true;
	}

	public override void UnderAttack (AttackData attackData)
	{
		base.UnderAttack (attackData);
	}
}
