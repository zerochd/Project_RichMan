using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandUI {

	int UseTime{ get; set; }

	void InitUi(Player controllerPlayer);

	bool CanExecute ();

	void Execute();

	void EnableCommand ();

	void CommandDone(Player controllerPlayer);

}
