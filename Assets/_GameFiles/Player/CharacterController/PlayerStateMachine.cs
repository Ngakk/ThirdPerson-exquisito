using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace Mangos
{
    public class PlayerStateMachine : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            /*if (stateInfo.IsName("PickUp"))
            {
	            StaticManager.playerController.setCanMove(false);
            }*/

            
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }

        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash, AnimatorControllerPlayable controller)
        {
            if(stateMachinePathHash == Animator.StringToHash("Base Layer.Interact"))
            {
                StaticManager.playerController.setCanMove(false);
                animator.SetTrigger("ExitHold");
            }

            if (stateMachinePathHash == Animator.StringToHash("Base Layer.Idles"))
            {
                animator.SetInteger("IdleId", Random.Range(0, 4));
                Debug.Log("idles");
            }
        }

        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            if (stateMachinePathHash == Animator.StringToHash("Base Layer.Interact"))
            {
                animator.SetTrigger("EnterHold");
                StaticManager.playerController.setCanMove(true);
            }
        }
    }
}
