using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos {
	public class InputManager : MonoBehaviour {

        KeyCode attack = KeyCode.J;
        KeyCode attack2 = KeyCode.K;
        KeyCode dash = KeyCode.I;
        KeyCode interact = KeyCode.U;

		void Awake(){
			StaticManager.inputManager = this;
		}

		void Update(){
			switch (StaticManager.gameManager.gameState) {
                case GameState.mainMenu:

                    break;
                case GameState.mainGame:
                    StaticManager.playerController.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                    if (Input.GetKeyDown(attack))
                        StaticManager.playerController.onActionDown();
                    if (Input.GetKeyDown(attack2))
                        StaticManager.playerController.onAction2Down();
                    if (Input.GetKeyDown(dash))
                        StaticManager.playerController.onDashDown();
                    if (Input.GetKeyDown(interact))
                        StaticManager.playerController.onInteractDown();
				    break;

			}
		}
	}
}
