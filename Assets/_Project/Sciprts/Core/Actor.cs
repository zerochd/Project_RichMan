using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

	[SerializeField] protected Transform actorTransform;

	public Transform ActorTransform {
		get {
			return actorTransform;
		}
	}

	public virtual void UnderAttack(AttackData attackData){
		
	}
}
