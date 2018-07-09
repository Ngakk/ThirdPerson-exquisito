using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
	    animator.SetInteger("IdleId", Random.Range(0, 4));
    }
}
