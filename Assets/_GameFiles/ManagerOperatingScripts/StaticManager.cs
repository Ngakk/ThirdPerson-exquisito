using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos {
	public enum GameState {
		mainMenu,
		mainGame,
		pause,
	}

	public enum AppState {
		running,
		loading
	}
	
	public enum Weapon{
		sniper,
		granade,
		axe,
		homeRun
	}

	public struct HitData{
		public Weapon weapon;
		public Vector3 shooterPos;
		public Vector3 hitPos;
		public float power;
		public RaycastHit rayHit;
	}

    public class StaticManager
    {
        public static AppManager appManager;
        public static InputManager inputManager;
        public static GameManager gameManager;
        public static AudioManager audioManager;
    }

}
