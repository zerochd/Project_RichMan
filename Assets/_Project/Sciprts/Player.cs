using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	[SerializeField] MoveGrid standMoveGrid;
	[SerializeField] MoveGrid nextMoveGrid;

	[SerializeField] int moveSpeed = 4;

	[SerializeField] int moveStep;

	public int MoveStep {
		get {
			return moveStep;
		}
	}

	[SerializeField] bool moveDone;

	Transform actortTransform;
	Animator animator;

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

				standMoveGrid = nextMoveGrid;
				moveStep -= 1;

				//excute 
				if (moveStep == 0 && standMoveGrid != null) {
					standMoveGrid.Arrived (this);
				}

				moveDone = true;

			} else {
				
				moveDone = false;
				MoveToGrid (nextMoveGrid);

			}
				
		} 
	}

	void Init ()
	{
		animator = GetComponentInChildren<Animator> ();

		if (animator != null) {
			actortTransform = animator.transform;
		}

	}

	public void MoveToGrid (MoveGrid nextMoveGrid)
	{
		if (nextMoveGrid == null)
			return;

		this.actortTransform.position = Vector3.MoveTowards (this.actortTransform.position, nextMoveGrid.transform.position, moveSpeed * Time.deltaTime);

	}


	public void CalcMoveStep (int step)
	{
		moveStep = step;

	}

	bool MoveDone ()
	{
		if (actortTransform == null || nextMoveGrid == null)
			return true;

		float _distance = Vector3.Distance (nextMoveGrid.transform.position, actortTransform.position);

		if (_distance <= float.Epsilon + 0.01f) {
			return true;
		}

		return false;
	}

	bool isAnimationIdle ()
	{
		if (animator == null)
			return true;
		AnimatorStateInfo _stateInfo = animator.GetCurrentAnimatorStateInfo (animator.GetLayerIndex ("Base"));
		return _stateInfo.IsName ("Idle");
	}


	//	IEnumerator MoveCor(int step){
	//
	//
	//	}

}
