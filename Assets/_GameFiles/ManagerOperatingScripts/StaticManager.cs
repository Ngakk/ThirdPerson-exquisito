using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos {
    //Meta
	public enum GameState {
		mainMenu,
		mainGame,
		pause,
	}

	public enum AppState {
		running,
		loading
	}
	
    //Game
	public enum HitType{
		edgeSlice,
        lightStrike,
        heavyStrike
	}

    public enum ObjectHoldId : int
    {
        nothing,
        gloves,
        oneHandRight,
        oneHandHeavyRight,
        oneHandLeft,
        oneHandHeavyLeft,
        twoHand,
        twoHandHeavy,
        twoHandOverhead
    }

    public enum ActionId : int
    {
        pocket,
        pickup,
        use
    }

    //Data carriers
	public struct HitData{
		public HitType hitType;
		public Vector3 hitterPos;
		public Vector3 hitPos;
        public Vector3 hitForceDir;
		public float power;
	}

    public class StaticManager
    {
        public static AppManager appManager;
        public static InputManager inputManager;
        public static GameManager gameManager;
        public static AudioManager audioManager;
        public static ThirdPersonCharacterController playerController;
    }

}
