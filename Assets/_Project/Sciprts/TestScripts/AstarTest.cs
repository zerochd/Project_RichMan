using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarTest : MonoBehaviour {

	public enum STATE{
		SELECT_START,
		SELECT_END,
		SELECT_RESULT,
		SELECT_RESET
	}

	[SerializeField] Grid startGrid;
	[SerializeField] Grid endGrid;
	[SerializeField] Map map;

	[SerializeField] STATE state = STATE.SELECT_START;

	// Update is called once per frame
	void Update () {
		switch (state) {
		case STATE.SELECT_START:
			{
				if (!Input.GetMouseButtonDown (0))
					return;

				var _ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit _hit = new RaycastHit ();
				if (Physics.Raycast (_ray, out _hit, 1000.0f, 1 << LayerMask.NameToLayer ("Grid"))) {
					Grid _grid = _hit.collider.GetComponentInParent<Grid> ();
					if (_grid) {
						startGrid = _grid;
						state = STATE.SELECT_END;
						startGrid.GetComponentInChildren<MeshRenderer> ().material.color = Color.green;

					}
				}
			}
			break;
		case STATE.SELECT_END:
			{
				if (!Input.GetMouseButtonDown (0))
					return;
				
				Ray _ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit _hit = new RaycastHit ();
				if (Physics.Raycast (_ray, out _hit, 1000.0f, 1 << LayerMask.NameToLayer ("Grid"))) {
					Grid _grid = _hit.collider.GetComponentInParent<Grid> ();
					if (_grid) {
						endGrid = _grid;
						state = STATE.SELECT_RESULT;
						endGrid.GetComponentInChildren<MeshRenderer> ().material.color = Color.red;

					}
				}
			}
			break;
		case STATE.SELECT_RESULT:
			AStarFun ();
			state = STATE.SELECT_RESET;
			break;
		case STATE.SELECT_RESET:
			if (Input.GetKeyDown (KeyCode.Space)) {
				map.ResetColor ();
				startGrid = null;
				endGrid = null;
				state = STATE.SELECT_START;
			}
			break;
		default:
			throw new System.ArgumentOutOfRangeException ();
		}
	}


	void AStarFun(){
//		//A* function
//		Debug.Log("map is null?:"+(map == null));
//		Debug.Log ("mapMat is null?:" + (map.gridMat == null));
		Stack<Grid> outGrids = AStar.CalcPath(startGrid,endGrid,map);

		while (outGrids != null && outGrids.Count > 0) {
			Grid  _oGrid = outGrids.Pop ();
			_oGrid.GetComponentInChildren<MeshRenderer> ().material.color = Color.yellow;
		}
	}

	void OnDrawGizmos(){


	}
}
