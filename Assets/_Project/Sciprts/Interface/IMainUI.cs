using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMainUI{

	void ShowUI();

	void HideUI();

	void UpdateUI (PlayerData playerData);
}
