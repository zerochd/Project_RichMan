using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSMB : StateMachineBehaviour {

	[SerializeField]
	enum STATE{
		Enter,
		Stay,
		Exit
	}

	[SerializeField]
	STATE timing;

	public string functionName;

	public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if(timing == STATE.Enter){
			animator.SendMessage(functionName,null,SendMessageOptions.DontRequireReceiver);
		}
	}

	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if(timing == STATE.Stay){
			animator.SendMessage(functionName,null,SendMessageOptions.DontRequireReceiver);
		}
	}

	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if(timing == STATE.Exit){
			animator.SendMessage(functionName,null,SendMessageOptions.DontRequireReceiver);
		}
	}

}
