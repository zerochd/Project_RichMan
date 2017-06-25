using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CancelUI : MonoBehaviour,ICommandUI {

	[SerializeField] Button eventButton;
	[SerializeField] Text eventText;

	void Awake(){
		Init ();
	}

	void Start(){
		if(eventButton != null)
		{
			eventButton.onClick.AddListener (delegate {
				
				Excute();
			});
		}
	}

	#region ICommandUI implementation

	public bool CanExcute ()
	{
		if (PlayerController.Instance.Command != PlayerController.COMMAND.NONE)
			return true;
		return false;
	}
	public void Excute ()
	{

		if (PlayerController.Instance == null)
			return;

//		if (!CanExcute())
//			return;

		if (CommandController.Instance != null) {
			CommandController.Instance.Use (this);
		}

		PlayerController.Instance.ApplyCommand (PlayerController.COMMAND.NONE);
	}

	public void EnableCommand(){
		if(this.eventButton)
			this.eventButton.interactable = true;
	}

	#endregion

	[ContextMenu("Init")]
	void Init(){
		if (eventButton == null) {
			eventButton = GetComponentInChildren<Button> ();

		}
		if (eventText == null) {
			eventText = GetComponentInChildren<Text> ();
		}

		#if UNITY_EDITOR
		EditorUtility.SetDirty(this.gameObject);
		#endif
	}
}
