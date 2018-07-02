using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mangos {
	public class GameManager : MonoBehaviour {
		public Mangos.GameState gameState;
		void Awake(){
			StaticManager.gameManager = this;
		}
		
		public void GoToMenu(){
			
		}
		
		public void GoToGame(){
			
		}
		
		public void GoToWin()
		{
			
		}
	}
}
