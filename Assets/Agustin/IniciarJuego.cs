using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IniciarJuego : MonoBehaviour {
	
	public void Juego()
	{
		SceneManager.LoadScene("NavMesh");
	}
}
