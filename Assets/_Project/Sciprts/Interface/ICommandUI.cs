using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandUI {

	bool CanExcute ();

	void Excute();

	void EnableCommand ();
}
