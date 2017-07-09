using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BuyUI : MonoBehaviour {

	[SerializeField] ToggleGroup[] buyGroup;
	[SerializeField] ToggleGroup selectBuyGroup;

	[SerializeField] Button yesButton;
	[SerializeField] Button cancelButton;

	BuyEvent getBuyEvent;

	void Awake(){
//		Init ();
	}
		
	void Start(){
		if (cancelButton != null) {
			cancelButton.onClick.AddListener (delegate {
				Hide();	
			});
		}
		if (yesButton != null) {
			yesButton.onClick.AddListener (delegate {
				Apply();
			});
		}
	}

	public void Show(BuyEvent buyEvent){
		if (buyEvent == null)
			return;
		
		getBuyEvent = buyEvent;

		this.gameObject.SetActive (true);
		if (yesButton != null)
			yesButton.gameObject.SetActive (true);
		if (cancelButton != null)
			cancelButton.gameObject.SetActive (true);

		string _compareType = buyEvent.GetType().ToString();
//		Debug.Log (_compareType);
		foreach (var buyUI in buyGroup) {
//			Debug.Log ("buyUI:" + buyUI.tag);
			if(buyUI.CompareTag("BuyType/"+_compareType)){
				buyUI.gameObject.SetActive (true);
				selectBuyGroup = buyUI;
				break;
			}
		}
	}

	public void Apply(){

		if (getBuyEvent == null)
			return;

		if (selectBuyGroup != null) {
			foreach (var toggle in selectBuyGroup.GetComponentsInChildren<Toggle>()) {
				if (toggle.isOn) {
//					Debug.Log ("select toggle:" + toggle.name);

					if (getBuyEvent is BuyBuildingEvent) {
						BuildingItemUI _buildItemUI = toggle.GetComponent<BuildingItemUI> ();
						BuyBuildingEvent _buildEvent = getBuyEvent as BuyBuildingEvent;
						_buildItemUI.Build (_buildEvent);
					}
					JobDone ();
				}
			}
		}
	}

	public void Hide(){
		gameObject.SetActive (false);
	}

	public void JobDone(){
		Hide ();

		if (PlayerController.Instance != null) {
			PlayerController.Instance.PlayerBuildDone ();
		}
	}

	[ContextMenu("Init")]
	void Init(){
		buyGroup = GetComponentsInChildren<ToggleGroup> ();
		foreach (var buyUI in buyGroup) {
			buyUI.gameObject.SetActive (false);
		}
		selectBuyGroup = null;
		if (yesButton != null)
			yesButton.gameObject.SetActive (false);
		if (cancelButton != null)
			cancelButton.gameObject.SetActive (false);
		#if UNITY_EDITOR
		EditorUtility.SetDirty(this.gameObject);
		#endif
	}
}
