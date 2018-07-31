using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos {
    public class InputManager : MonoBehaviour {
	    KeyCode keyAttack = KeyCode.J;
	    KeyCode keyAttack2 = KeyCode.K;
	    KeyCode keyInteract = KeyCode.U;
        

		void Awake(){
			StaticManager.inputManager = this;
		}

		void Update(){
            bool attack = Input.GetButtonDown("attack");
            bool attack2 = Input.GetButtonDown("attack2");
            bool interact = Input.GetButtonDown("interact");
            switch (StaticManager.gameManager.gameState)
            {
                case GameState.mainMenu:

                    break;
                case GameState.mainGame:
                    StaticManager.playerController.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	                if (attack || Input.GetKeyDown(keyAttack))
                    {
                        StaticManager.playerController.onActionDown();
                    }
	                if (attack2 || Input.GetKeyDown(keyAttack2))
                    {
                        StaticManager.playerController.onAction2Down();
                    }
	                if (interact || Input.GetKeyDown(keyInteract))
                    {
                        StaticManager.playerController.onInteractDown();
                    }
				    break;

			}
		}
	}
}
