using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jugador_UI : MonoBehaviour 
{
	
	float hp;
	
	public Image imagen;

	// Use this for initialization
	void Start () 
	{
		hp = 1.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnCollisionEnter(Collision _col)
	{
		if(_col.collider.CompareTag("Bala_Enemigo"))
		{
			hp -= 0.1f;
			imagen.fillAmount = hp;
		}
	}
}
