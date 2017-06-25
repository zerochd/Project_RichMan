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


	[SerializeField] VectorInt2 vi;

	public int f;
	public int g;
	public int h;

	public Grid gridParent;
	public Actor owner;
	public Color lastColor = Color.white;

	public VectorInt2 Vi {
		get {
			return vi;
		}
	}

	public void Init()
	{
		vi.x = Mathf.RoundToInt(this.transform.position.x / 4);
		vi.y = Mathf.RoundToInt(this.transform.position.z / 4);
		ResetValue ();

	}

	public void ResetGridColor(){
		if (owner == null) 
			SetGridColor (Color.white);
	}

	public void SetGridColor(Color color){
		Material _mat = GetComponentInChildren<MeshRenderer> ().material;
		lastColor = _mat.color;
		_mat.color = color;
	}

	public void ResetValue(){
		gridParent = null;
		f = 0;
		g = 0;
		h = 0;
	}

	public virtual bool Arrived(Actor actor){
		if (owner == null) {
			owner = actor;

			GetComponentInChildren<MeshRenderer> ().material.color = Color.red;

			return true;
		}
		else
			return false;
	}

	public virtual void Free(){

		if(owner != null)
			GetComponentInChildren<MeshRenderer> ().material.color = Color.white;
		owner = null;
	}

	#region IComparable implementation

	public int CompareTo (Grid other)
	{

		if (this.f < other.f) {
			//升序
			return -1;
		}
		if (this.f > other.f) {
			//降序
			return 1;
		}
		return 0;
	}

	#endregion

	#if UNITY_EDITOR

	void OnDrawGizmos(){

		UnityEditor.Handles.Label (this.transform.position, "(" + vi.x + "," + vi.y + ")");
//		UnityEditor.Handles.color = Color.green;
//
//		if (h != 0)
//			UnityEditor.Handles.color = Color.red;
//
//		UnityEditor.Handles.Label (this.transform.position - this.transform.forward * 0.5f, ("f:" + f + " g:" + g + " h:" + h));
	}

	#endif
}
