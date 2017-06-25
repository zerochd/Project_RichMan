using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUI : MonoBehaviour,IMainUI {

	static HpUI _instance;

	public static HpUI Instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<HpUI> ();
			}
			return _instance;
		}
	}

	[SerializeField] Image hpImage;
	[SerializeField] Slider hpSlider;
	[SerializeField] Text hpText;

//	float playerHpPercent;

	void Awake(){
		_instance = this;
	}

//	void Update(){
//		if (hpSlider != null && Mathf.Abs(hpSlider.value - playerHpPercent) > 0.01f) {
//			hpSlider.value = Mathf.Lerp (hpSlider.value, playerHpPercent, Time.deltaTime);
//		}
//	}

	#region IMainUI implementation
	public void ShowUI ()
	{
		if (hpImage != null) 
			hpImage.gameObject.SetActive (true);
		if (hpSlider != null) 
			hpSlider.gameObject.SetActive (true);
		if (hpText != null) 
			hpText.gameObject.SetActive (true);
	}
	public void HideUI ()
	{
		if (hpImage != null) 
			hpImage.gameObject.SetActive (false);
		if (hpSlider != null) 
			hpSlider.gameObject.SetActive (false);
		if (hpText != null) 
			hpText.gameObject.SetActive (false);
	}
	public void UpdateUI (PlayerData playerData)
	{
		if (hpText != null) {
			hpText.text = playerData.hp + "/" + playerData.maxHp;
		}
		if (hpSlider != null) {
			hpSlider.value = (float)playerData.hp / playerData.maxHp;
		}
//		playerHpPercent = (float)playerData.hp / playerData.maxHp;
	}
	#endregion
}
