using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandController : MonoBehaviour {
	
	private static CommandController _instance;

	public static CommandController Instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<CommandController> ();
			}
			return _instance;
		}
	}

	[SerializeField] PlayerController playerController;
	[SerializeField] ICommandUI[] iCommandUIArray;

	ICommandUI lastCommandUI;

	public ICommandUI LastCommandUI {
		get {
			return lastCommandUI;
		}
	}

	void Awake(){
		_instance = this;
		Init ();
	}

	void Init(){
		iCommandUIArray = GetComponentsInChildren<ICommandUI> ();
	}

	public void ShowUI(){
		this.gameObject.SetActive (true);
	}

	public void HideUI(){
		this.gameObject.SetActive (false);
	}

	public void Use(ICommandUI commadUI){
		
		if (lastCommandUI != null && lastCommandUI != commadUI) {
			lastCommandUI.EnableCommand ();
		}

		lastCommandUI = commadUI;
	}
}
