using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandUI {

	int UseTime{ get; set; }

	void InitUI(Player controllerPlayer);

	bool CanExcute ();

	void Excute();

	void EnableCommand ();

	void CommandDone(Player controllerPlayer);

}
