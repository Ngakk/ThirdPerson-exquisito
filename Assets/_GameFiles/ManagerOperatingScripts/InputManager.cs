using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos {
    public class InputManager : MonoBehaviour {

        

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
                    if (attack)
                    {
                        StaticManager.playerController.onActionDown();
                    }
                    if (attack2)
                    {
                        StaticManager.playerController.onAction2Down();
                    }
                    if (interact)
                    {
                        StaticManager.playerController.onInteractDown();
                    }
				    break;

			}
		}
	}
}
