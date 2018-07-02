using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos {
	public class AppManager : MonoBehaviour {
		void Awake(){
			StaticManager.appManager = this;
		}
	}
}
