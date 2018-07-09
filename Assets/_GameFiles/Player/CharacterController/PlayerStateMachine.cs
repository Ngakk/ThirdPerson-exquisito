using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos
{
    public class PlayerStateMachine : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.IsName("Pickup"))
            {
                StaticManager.playerController.setCanMove(false);
            }

            if (stateInfo.IsName("throw"))
            {
                //TODO: que agarre bien el throw
                Debug.Log("throw");
                StaticManager.playerController.setHoldingItem(false);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.IsName("Pickup"))
            {
                StaticManager.playerController.setCanMove(true);
                StaticManager.playerController.setHoldingItem(true);
            }
        }

        override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            animator.SetInteger("IdleId", Random.Range(0, 4));
        }
    }
}
