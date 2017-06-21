using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
	public Actor owner;

	public virtual bool Arrived(Actor actor){
		if (owner == null) {
			owner = actor;
			return true;
		}
		else
			return false;
	}

	public virtual void Reset(){
		owner = null;
	}
}
