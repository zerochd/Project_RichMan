using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[InitializeOnLoad]
public class MultiComponentTool : Editor {

	static Dictionary<string,Component[]> componentsDictionary = new Dictionary<string, Component[]>();

	[MenuItem("Custom/MultiComponentTool/RagdollCopy")]
	public static void RagdollCopy(){

		componentsDictionary.Clear ();

		GameObject _gameObject = Selection.activeGameObject;

		foreach (var _rig in _gameObject.GetComponentsInChildren<Rigidbody>()) {
			componentsDictionary.Add (_rig.name, _rig.GetComponents<Component> ());
		}
		Debug.Log ("copy ok");
	}

	[MenuItem("Custom/MultiComponentTool/RagdollPaste")]
	public static void RagdollPaste(){

		Transform _transform = Selection.activeTransform;
		Component[] _components;
		if(componentsDictionary.TryGetValue(_transform.name,out _components)){

			foreach (Component _cpt in _components) {
				UnityEditorInternal.ComponentUtility.CopyComponent(_cpt);
				//如果有这个组件，则只是复制，不然就是复制新的
				Component _targetComponent = _transform.GetComponent (_cpt.GetType ());
				if (_targetComponent) {
					UnityEditorInternal.ComponentUtility.PasteComponentValues (_targetComponent);
				}
				UnityEditorInternal.ComponentUtility.PasteComponentAsNew (_transform.gameObject);

			}
			componentsDictionary.Remove (_transform.name);
				
		}

		PasteTransform(_transform);

		Debug.Log ("paste ok");

	}

	static void PasteTransform(Transform parentTransform)
	{
		Component[] _components;
		if(componentsDictionary.TryGetValue(parentTransform.name,out _components)){

			foreach (Component _cpt in _components) {
				UnityEditorInternal.ComponentUtility.CopyComponent(_cpt);
				//如果有这个组件，则只是复制，不然就是复制新的
				Component _targetComponent = parentTransform.GetComponent (_cpt.GetType ());
				if (_targetComponent) {
					UnityEditorInternal.ComponentUtility.PasteComponentValues (_targetComponent);
				}
				UnityEditorInternal.ComponentUtility.PasteComponentAsNew (parentTransform.gameObject);

			}
			componentsDictionary.Remove (parentTransform.name);

		}

		foreach (Transform child in parentTransform) {


			PasteTransform (child);
		}
	}

}
