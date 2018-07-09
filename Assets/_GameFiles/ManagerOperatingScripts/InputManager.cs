using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos {
	public class InputManager : MonoBehaviour {
		
		void Awake(){
			StaticManager.inputManager = this;
		}
			
		void Update(){
			switch (StaticManager.gameManager.gameState) {
                case GameState.mainMenu:

                    break;
                case GameState.mainGame:
                    StaticManager.playerController.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
				    break;
                
			}
		}
	}
}