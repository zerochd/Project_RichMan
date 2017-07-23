using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {

	[SerializeField] protected bool enableControl = false;
	[SerializeField] protected List<Actor> actorList = new List<Actor>();


	public bool EnableControl {
		get {
			return enableControl;
		}
		set {
			enableControl = value;
		}
	}

	protected virtual void Init(){
		actorList.Clear ();
	}

	public virtual void Register(Actor newActor){
		if (actorList != null && !actorList.Contains (newActor)) {
			actorList.Add (newActor);
			newActor.ActiveActor (false);
		}
	}

	public virtual void RoundFirstActor(){
		
	}

	public virtual void RoundEndActor(Actor nowActor)
	{
		nowActor.ActiveActor (false);
	}

	public virtual Actor RoundNextActor(Actor nowActor){
		int _tempIndex = actorList.IndexOf (nowActor) + 1;

		if (_tempIndex >= actorList.Count) {
			_tempIndex = 0;
		}

		return actorList [_tempIndex];
	}

	public virtual void DeadActor(Actor actor){
		if (actorList.Contains (actor)) {
			actorList.Remove (actor);
		}
		if (actorList.Count <= 0) {
			GameManager.Instance.CheckGameOver (this);
		}
	}

}
