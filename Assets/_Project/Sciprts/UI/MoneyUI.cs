using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour,IMainUI {

	static MoneyUI _instance;

	public static MoneyUI Instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<MoneyUI> ();
			}
			return _instance;
		}
	}

	[SerializeField] Image moneyImage;
	[SerializeField] Text moneyText;

	void Awake(){
		_instance = this;
	}

	#region IMainUI implementation

	public void ShowUI ()
	{
		if (moneyImage != null)
			moneyImage.gameObject.SetActive (true);
		if (moneyText != null)
			moneyText.gameObject.SetActive (true);
	}

	public void HideUI ()
	{
		if (moneyText != null)
			moneyText.gameObject.SetActive (false);
		if(moneyImage != null)
			moneyImage.gameObject.SetActive(false);
	}

	public void UpdateUI (PlayerData playerData)
	{
		if (moneyText != null) {
			moneyText.text = ""+playerData.money;
		}
	}

	#endregion

}
