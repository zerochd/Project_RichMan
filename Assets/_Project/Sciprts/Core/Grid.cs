using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public struct VectorInt2{
	public int x;
	public int y;

	public VectorInt2(int x,int y){
		this.x = x;
		this.y = y;
	}

	static VectorInt2 zero = new VectorInt2 (0, 0);

	static VectorInt2 one = new VectorInt2(1,1);

	public static VectorInt2 Zero{
		get{
			return zero;
		}
	}

	public static VectorInt2 One {
		get {
			return one;
		}
	}
		
	public static VectorInt2 operator +(VectorInt2 lhs,VectorInt2 rhs){
		return new VectorInt2 (lhs.x + rhs.x, lhs.y + rhs.y);
	}

	public static VectorInt2 operator -(VectorInt2 lhs,VectorInt2 rhs){
		return new VectorInt2 (lhs.x - rhs.x, lhs.y - rhs.y);
	}

	public static bool operator <(VectorInt2 lhs,VectorInt2 rhs){
		if (lhs.x < rhs.x || lhs.y < rhs.y)
			return true;
		return false;
	}

	public static bool operator >(VectorInt2 lhs,VectorInt2 rhs){
		if (lhs.x > rhs.x || lhs.y > rhs.y)
			return true;
		return false;
	}

	public static bool operator ==(VectorInt2 lhs,VectorInt2 rhs){
		if (lhs.x == rhs.x && lhs.y == rhs.y)
			return true;
		return false;
	}

	public static bool operator !=(VectorInt2 lhs,VectorInt2 rhs){
		if (lhs.x != rhs.x || lhs.y != rhs.y)
			return true;
		return false;
	}

}

public class Grid : MonoBehaviour,IComparable<Grid> {


	[SerializeField] private VectorInt2 vi;
	[SerializeField] private GirdEvent gridEvent;

	public Color lastColor = Color.white;

	public int F { get; set; }

	public int G { get; set; }

	public int H { get; set; }

	public Grid GridParent { get; set; }

	public Actor Owner { get; private set; }


	public VectorInt2 Vi => vi;

	public void Init()
	{
		var position = this.transform.position;
		vi.x = Mathf.RoundToInt(position.x / 4);
		vi.y = Mathf.RoundToInt(position.z / 4);
		ResetValue ();

	}

	public void ResetGridColor(){
		if (Owner == null) 
			SetGridColor (Color.white);
	}

	public void SetGridColor(Color color){
		var mat = GetComponentInChildren<MeshRenderer> ().material;
		lastColor = mat.color;
		mat.color = color;
	}

	public void ResetValue(){
		GridParent = null;
		F = 0;
		G = 0;
		H = 0;
	}

	public virtual bool Arrived(Actor actor){
		if (Owner == null) {
			Owner = actor;

			GetComponentInChildren<MeshRenderer> ().material.color = Color.red;

			if (gridEvent != null) {
				gridEvent.ExcuteEvent (actor);
			}

			return true;
		}
		else
			return false;
	}

	public virtual void Free(){

		if(Owner != null)
			GetComponentInChildren<MeshRenderer> ().material.color = Color.white;
		Owner = null;
	}

	#region IComparable implementation

	public int CompareTo (Grid other)
	{

		if (this.F < other.F) {
			//升序
			return -1;
		}
		return this.F > other.F ? 1 : 0;
	}

	#endregion

	#if UNITY_EDITOR

	void OnDrawGizmos(){

		UnityEditor.Handles.Label (this.transform.position, "(" + vi.x + "," + vi.y + ")");

		if (GridParent != null)
		{
			UnityEditor.Handles.DrawDottedLine(
				this.transform.position + Vector3.up *0.1f,
					GridParent.transform.position + Vector3.up* 0.5f,
				1f
				);
		}
		
//		UnityEditor.Handles.color = Color.green;
//
//		if (h != 0)
//			UnityEditor.Handles.color = Color.red;
//
//		UnityEditor.Handles.Label (this.transform.position - this.transform.forward * 0.5f, ("f:" + f + " g:" + g + " h:" + h));
	}

	#endif
}
