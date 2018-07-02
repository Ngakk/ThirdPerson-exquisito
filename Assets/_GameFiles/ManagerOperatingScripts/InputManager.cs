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
			case GameState.mainGame:

			default:
				break;
			}
		}
	}
}