using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Map))]
[InitializeOnLoad]
public class MapEditor : Editor {

	public static Vector3 CurrentHandlePosition = Vector3.zero;
	public static Vector3 CurrentHandleNormal = Vector3.up;

	static readonly EditorApplication.HierarchyWindowItemCallback hiearchyItemCallBack;

	static Vector3 m_OldHandlePosition = Vector3.zero;

	static Map map;

	static Grid hoverGrid;

	[SerializeField]
	static bool button_edit;

	[SerializeField]
	static bool button_apply;

	[SerializeField]
	static bool button_clear;

	static MapEditor(){
		SceneView.onSceneGUIDelegate -= OnSceneGUI;
		SceneView.onSceneGUIDelegate += OnSceneGUI;
	}

	void OnEnable(){
		map = target as Map;
	}

	void Destroy(){
		SceneView.onSceneGUIDelegate -= OnSceneGUI;
	}

	public override void OnInspectorGUI ()
	{
		base.DrawDefaultInspector();

		serializedObject.Update();

		#region draw select

		EditorGUILayout.BeginHorizontal();

		if(!button_edit){

			#region [Edit] Button

			button_edit = GUILayout.Button("Edit",GUILayout.ExpandWidth(true));

			#endregion

		}
		else{

			var oldColor = GUI.color;

			#region [Apply] Button

			GUI.color = Color.green;

			button_apply = GUILayout.Button("Apply",GUILayout.ExpandWidth(true));


			if(button_apply){
				button_edit = false;
				Apply();
			}

			#endregion

			#region [Clear] Button

			GUI.color = Color.red;

			button_clear = GUILayout.Button("Clear",GUILayout.ExpandWidth(true));

			if(button_clear){

				bool isYes = EditorUtility.DisplayDialog("Clear WayPoint","Are you sure Delete All WayPoint?","Yes","No");

				if(isYes){

					button_edit = false;
					Clear();
				}
			}


			#endregion

			GUI.color = oldColor;

		}

		EditorGUILayout.EndHorizontal();

		if(GUI.changed)
			EditorUtility.SetDirty(map);

		serializedObject.ApplyModifiedProperties();

		#endregion
	}

	#region button Event

	void Apply(){
		map.Init();
	}

	void Clear(){
		map.Clear();
	}

	#endregion

	static void OnSceneGUI(SceneView sceneView){

		if(!button_edit) return;


		UpdateHandlePosition ();

		CheckHoverGrid ();
		UpdateRepaint ();
		DrawHandlesPreview ();
		EventUpdate ();
	}

	//确认是否hover一个MovePoint
	static void CheckHoverGrid(){
//		Ray ray = HandleUtility.GUIPointToWorldRay( Event.current.mousePosition );
		Ray _ray = new Ray (CurrentHandlePosition + Vector3.up * (-20f), CurrentHandleNormal);  
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(_ray, out hit, 1000.0f,1 << LayerMask.NameToLayer("Grid"))) {
			Grid _hoverGrid = hit.collider.GetComponentInParent<Grid>();
			if(_hoverGrid){
				hoverGrid = _hoverGrid;
			}
			else{
				hoverGrid = null;
			}
		}
		else{
			hoverGrid = null;
		}
	}

	//更新画布
	static void UpdateRepaint(){
		if(CurrentHandlePosition != m_OldHandlePosition){
			SceneView.RepaintAll();
			m_OldHandlePosition = CurrentHandlePosition;
		}
	}

	//更新HandlePosition
	static void UpdateHandlePosition(){

		Ray ray = HandleUtility.GUIPointToWorldRay( Event.current.mousePosition );
		RaycastHit _hit = new RaycastHit();
		LayerMask _layerMask = 1 << LayerMask.NameToLayer("Ground");
		if (Physics.Raycast(ray, out _hit, 1000.0f, _layerMask)){
			Vector3 offset = Vector3.zero;

			CurrentHandlePosition = _hit.point;
			CurrentHandleNormal = _hit.normal;

			offset = _hit.normal;

//			CurrentHandlePosition.x = Mathf.Floor( _hit.point.x - _hit.normal.x * 0.001f + offset.x );
			CurrentHandlePosition.y = Mathf.Floor( _hit.point.y - _hit.normal.y * 0.001f + offset.y );
//			CurrentHandlePosition.z = Mathf.Floor( _hit.point.z - _hit.normal.z * 0.001f + offset.z );
			CurrentHandlePosition.x = Mathf.RoundToInt(CurrentHandlePosition.x / 4) * 4;
			CurrentHandlePosition.z = Mathf.RoundToInt(CurrentHandlePosition.z / 4) * 4;

			CurrentHandlePosition += Vector3.up * 0.5f;
		}
	}

	//事件更新
	static void EventUpdate(){

		// 通过创建一个新的ControlID我们可以把鼠标输入的Scene视图反应权从Unity默认的行为中抢过来
		// FocusType.Passive意味着这个控制权不会接受键盘输入而只关心鼠标输入
		int controlId = GUIUtility.GetControlID(FocusType.Passive);

		if(Event.current.type == EventType.MouseDown &&
			 Event.current.button == 0)
		{
			//删除
			if (hoverGrid != null) {

				if (Event.current.shift == true) {

					Undo.DestroyObjectImmediate (hoverGrid.gameObject);
					return;
				}
			} else {

				//添加
				if (map.GetGridPrefab () != null) {

//					GameObject _go = Instantiate (map.GetGridPrefab ());
					GameObject _go = PrefabUtility.InstantiatePrefab (map.GetGridPrefab ()) as GameObject;
					_go.transform.position = CurrentHandlePosition + Vector3.up * 0.5f;
//				_go.transform.localEulerAngles = Vector3.up * angle;

					_go.transform.SetParent (map.transform);
					_go.name = _go.name.Replace ("(Clone)", "") + _go.transform.GetSiblingIndex ();
					_go.transform.localScale = Vector3.one;

					Undo.RegisterCreatedObjectUndo (_go, "Create go");

					EditorUtility.SetDirty (_go);

				} else {
					
					EditorUtility.DisplayDialog ("Warning", "MovePointPrefab is missing", "ok");

				}
			}

		}

		// 把我们自己的controlId添加到默认的control里，这样Unity就会选择我们的控制权而非Unity默认的Scene视图行为
		HandleUtility.AddDefaultControl(controlId);

	}

	static void DrawHandlesPreview(){
		Color _handleColor = Color.green;

		if (Event.current.shift == true && hoverGrid != null) {
			_handleColor = Color.red;
		}

		Handles.color = _handleColor;
		DrawHandle(CurrentHandlePosition);
	}

	static void DrawHandle(Vector3 center){



		Vector3 p1 = center + Vector3.up * 0.5f + Vector3.right * 2f + Vector3.forward * 2f;
		Vector3 p2 = center + Vector3.up * 0.5f + Vector3.right * 2f - Vector3.forward * 2f;
		Vector3 p3 = center + Vector3.up * 0.5f - Vector3.right * 2f - Vector3.forward * 2f;
		Vector3 p4 = center + Vector3.up * 0.5f - Vector3.right * 2f + Vector3.forward * 2f;

		Vector3 p5 = center - Vector3.up * 0.5f + Vector3.right * 2f + Vector3.forward * 2f;
		Vector3 p6 = center - Vector3.up * 0.5f + Vector3.right * 2f - Vector3.forward * 2f;
		Vector3 p7 = center - Vector3.up * 0.5f - Vector3.right * 2f - Vector3.forward * 2f;
		Vector3 p8 = center - Vector3.up * 0.5f - Vector3.right * 2f + Vector3.forward * 2f;

		Handles.DrawLine( p1, p2 );
		Handles.DrawLine( p2, p3 );
		Handles.DrawLine( p3, p4 );
		Handles.DrawLine( p4, p1 );

		Handles.DrawLine( p5, p6 );
		Handles.DrawLine( p6, p7 );
		Handles.DrawLine( p7, p8 );
		Handles.DrawLine( p8, p5 );

		Handles.DrawLine( p1, p5 );
		Handles.DrawLine( p2, p6 );
		Handles.DrawLine( p3, p7 );   
		Handles.DrawLine( p4, p8 );
	}

}
#endif
