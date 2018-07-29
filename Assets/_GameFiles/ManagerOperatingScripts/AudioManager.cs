using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos {	
	
	enum soundIndx : int{
		gunShot = 0,
		granadeLauncher,
		explosion,
		swoosh,
		metalStrike,
		emptyShoot,
		woodCrash,
		woodHit1,
		woodHit2,
		woodHit3
	}
	
	
	
	public class AudioManager : MonoBehaviour {

		public float volumeSFX = 1;
		public float volumeMusic = 1;
		public float MasterVolume = 1;

		public AudioClip[] clips;
		public int[] maxSimultaneousClip;
		public int maxSimultaneousSounds;
		List<GameObject> sounds = new List<GameObject>();
		
		
		void Awake(){
			StaticManager.audioManager = this;
		}
		
		void Start()
		{
			for(int i = 0; i < clips.Length; i++)
			{
				GameObject temp = new GameObject();
				
				temp.AddComponent<AudioSource>();
				temp.GetComponent<AudioSource>().clip = clips[i];
				temp.AddComponent<DefaultSound>();
				temp.GetComponent<DefaultSound>().dj = temp.GetComponent<AudioSource>();
				temp.name = "soundMaker" + i.ToString();
				sounds.Add(temp);
				PoolManager.PreSpawn(sounds[i], (int)Mathf.Round(maxSimultaneousClip[i]/2));
				PoolManager.SetPoolLimit(sounds[i], maxSimultaneousClip[i]);
			}
		}
		
		
		public void PlayIndexedSound(int i, Vector3 pos){
			PoolManager.Spawn(sounds[i], pos, Quaternion.identity);
		}
		public void PlayExplosion(Vector3 pos){
			PoolManager.Spawn(sounds[(int)soundIndx.explosion], pos, Quaternion.identity);
		}
		public void PlayGranadaLauncher(Vector3 pos){
			PoolManager.Spawn(sounds[(int)soundIndx.granadeLauncher], pos, Quaternion.identity);
		}
		public void PlayGunShot(Vector3 pos){
			PoolManager.Spawn(sounds[(int)soundIndx.gunShot], pos, Quaternion.identity);
		}
		public void PlaySwoosh(Vector3 pos){
			PoolManager.Spawn(sounds[(int)soundIndx.swoosh], pos, Quaternion.identity);
		}
		public void PlayMetalStrike(Vector3 pos){
			PoolManager.Spawn(sounds[(int)soundIndx.metalStrike], pos, Quaternion.identity);
		}
		public void PlayEmptyShoot(Vector3 pos){
			PoolManager.Spawn(sounds[(int)soundIndx.emptyShoot], pos, Quaternion.identity);
		}
		public void PlayWoodCrash(Vector3 pos){
			PoolManager.Spawn(sounds[(int)soundIndx.woodCrash], pos, Quaternion.identity);
		}
		public void PlayWoodHit(Vector3 pos){
			int selector = Random.Range(0, 2);
			PoolManager.Spawn(sounds[(int)soundIndx.woodHit1 + selector], pos, Quaternion.identity);
		}
	}
}

