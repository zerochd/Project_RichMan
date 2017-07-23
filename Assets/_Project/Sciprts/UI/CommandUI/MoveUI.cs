using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class MoveUI : MonoBehaviour,ICommandUI {

	[SerializeField] Button eventButton;
	[SerializeField] Text eventText;

	int useTime = 1;

	void Awake(){
		Init ();
	}

	void Start(){
		if(eventButton != null)
		{
			eventButton.onClick.AddListener (delegate {
//				Debug.Log("click");
				Excute();
			});
		}
	}

	#region ICommandUI implementation

	public void InitUI (Player controllerPlayer)
	{
		if (controllerPlayer == null) {

		} else {
			useTime = 1;
		}
		EnableCommand ();
	}

	public int UseTime {
		get {
			return useTime;
		}
		set {
			useTime = value;
		}
	}

	public bool CanExcute ()
	{
		if (PlayerController.Instance.Command != COMMAND.DONE 
			&& PlayerController.Instance.Command != COMMAND.DOING)
			return true;
		return false;
	}
	public void Excute ()
	{

		if (PlayerController.Instance == null)
			return;

		if (!CanExcute())
			return;

		if (CommandController.Instance != null) {
			CommandController.Instance.Use (this);
		}

		PlayerController.Instance.ApplyCommand (COMMAND.MOVE);
		if(this.eventButton)
			this.eventButton.interactable = false;


	}

	public void EnableCommand(){

		if (useTime <= 0)
			return;

		if(this.eventButton)
			this.eventButton.interactable = true;
	}

	public void CommandDone(Player controllerPlayer){

		useTime--;

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
