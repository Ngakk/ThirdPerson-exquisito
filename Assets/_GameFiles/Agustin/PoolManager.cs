using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Uniat
{
	public class PoolManager : MangosBehaviour
	{
		//Default Settings
		private static int defaultPoolLimit = 10;
		
		//Diccionarios
		static Dictionary<int, Queue<GameObject>> pool = new Dictionary<int, Queue<GameObject>>();
		static Dictionary<int, int> poolLimit = new Dictionary<int, int>();
		static Dictionary<int, GameObject> poolHolder = new Dictionary<int, GameObject>();
		static Dictionary<int, bool> poolRecycle = new Dictionary<int, bool>();
		
		//Delete All Pool Objects
		public static void ClearPools()
		{
			pool.Clear();
			poolLimit.Clear();
			poolHolder.Clear();
			//TODO Delete objects
		}
		
		//Crear el Pool (Dictionary)
		public static void MakePool(GameObject prefab, int limit, int preSpawnObjects, bool recycle)
		{
			//Crear una cola, para guardar TODOS los objetos que sean del mismo tipo (segun su ID)
			if(!pool.ContainsKey(prefab.GetInstanceID()))
			{
				pool.Add(prefab.GetInstanceID(), new Queue<GameObject>());	
				poolHolder.Add(prefab.GetInstanceID(), new GameObject("_Pool["+prefab.name+"]"));
				SetPoolRecycle(prefab, recycle);
				SetPoolLimit(prefab, limit);
				
				PreSpawn(prefab,preSpawnObjects,false);
			}
		}
		
		public static void MakePool(GameObject prefab, int limit, int preSpawnObjects)
		{
			MakePool(prefab,limit,preSpawnObjects,false);
		}
		
		public static void MakePool(GameObject prefab, int limit)
		{
			MakePool(prefab, limit, 0);
		}
		
		public static void MakePool(GameObject prefab)
		{
			MakePool(prefab, defaultPoolLimit);
		}
		
		public static void SetPoolLimit(GameObject prefab, int newLimit)
		{
			//Si el pool ya existe, ponemos un limite
			if(poolLimit.ContainsKey(prefab.GetInstanceID()))
			{
				poolLimit[prefab.GetInstanceID()] = newLimit;
			}
			else //Si no existe, creamos un Pool
			{
				poolLimit.Add(prefab.GetInstanceID(), newLimit);
			}
		}
		
		public static void SetPoolRecycle(GameObject prefab, bool recycle)
		{
			if(poolRecycle.ContainsKey(prefab.GetInstanceID()))
			{
				poolRecycle[prefab.GetInstanceID()] = recycle;
			}
			else //Si no existe, creamos un Pool
			{
				poolRecycle.Add(prefab.GetInstanceID(), recycle);
			}
		}
		
		//Instantiate Controlado
		private static void AddNewItemToQueue(GameObject prefab, int id)
		{
			//Crear el objeto en la escena y ponerle un nombre
			GameObject go = GameObject.Instantiate(prefab);
			go.name = go.name + "_PoolInstance("+id+")";
			
			//Meter el objeto al Diccionario
			pool[prefab.GetInstanceID()].Enqueue(go);
			go.SetActive(false);
			go.transform.parent = poolHolder[prefab.GetInstanceID()].transform;
		}
		
		public static void PreSpawn(GameObject prefab, int amount, bool increaseLimit)
		{
			//Crear un pool con el tamaño del prespawn
			MakePool(prefab);
			
			if(increaseLimit)
			{
				//if(poolLimit.ContainsKey(prefab.GetInstanceID()))
				if(amount>poolLimit[prefab.GetInstanceID()])
				{
					SetPoolLimit(prefab,amount);
				}
			}
			
			//Crear For
			for(int i=0; (i<amount) && (i<poolLimit[prefab.GetInstanceID()]) ; i++)
			{
				AddNewItemToQueue(prefab, i);
			}
		}
		
		//Equivalente a Instantiate
		public static Transform Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
		{
			//Ver si ya existe algun pool con este tipo de objeto
			if(pool.ContainsKey(prefab.GetInstanceID()))
			{
				//Si ya existe el pool, se tomar el siguiente en la cola, para ser utilizado
				bool areAnyAvailable = false;
				//Ver si hay objetos disponibles
				//Recorre los objetos en la cola, y ve si alguno esta apagado, y envia los que esten prendidos al final.
				for(int i=0;i<pool[prefab.GetInstanceID()].Count; i++)
				{
					//Si el objeto esta activo (osea que esta siendo utilizado)
					if(pool[prefab.GetInstanceID()].Peek().activeSelf)
					{
						//Tomar el siguiente en la cola, y pasarlo al final
						GameObject tmp_go = pool[prefab.GetInstanceID()].Dequeue();
						pool[prefab.GetInstanceID()].Enqueue(tmp_go);
					}
					else //el siguiente no esta activo (esta disponible para ser usado)
					{
						areAnyAvailable = true;
						break;
					}
				}
				
				//Si no hay objetos disponibles
				if(areAnyAvailable==false)
				{
					//-- Ver si el pool esta lleno
					if(pool[prefab.GetInstanceID()].Count < poolLimit[prefab.GetInstanceID()])
					{
						//---- Si el pool aun tiene espacio, entonces crear un nuevo objeto y meterlo al pool		
						//Clonamos el Pool en un Array temporal
						var items = pool[prefab.GetInstanceID()].ToArray();
						//Limpiamos
						pool[prefab.GetInstanceID()].Clear();
						//Agregamos el nuevo item al inicio de la Cola
						AddNewItemToQueue(prefab,items.Length);
						//Agregamos todos los que ya habian despues del nuevo
						foreach(var item in items)
						{
							pool[prefab.GetInstanceID()].Enqueue(item);
						}
					}
					else
					{
						//---- Si el pool ya esta lleno, SALIR
						//Opcional, Despawnear al mas viejo y regresarlo si queremos que sea ciclico
						if(poolRecycle[prefab.GetInstanceID()]==true)
						{
							areAnyAvailable = true;
							/*
							GameObject tmp_go = pool[prefab.GetInstanceID()].Dequeue();
							pool[prefab.GetInstanceID()].Enqueue(tmp_go);
							return tmp_go.transform;
							*/
						}
						else
						{
							return null;	
						}
					}
				}
				//Si es que hay objetos disponibles
				GameObject go = pool[prefab.GetInstanceID()].Dequeue();
				
				//Antes de activarlo le ponemos en la posicion y rotacion deseada
				go.transform.position = position;
				go.transform.rotation = rotation;
				
				//Activamos el siguiente objeto en la cola
				go.SetActive(true);
				
				//Avisar a la instancia del objeto que ya fue spawneado
				go.SendMessage("OnSpawn",SendMessageOptions.DontRequireReceiver);
				
				//Lo regresamos a la cola
				pool[prefab.GetInstanceID()].Enqueue(go);
				//Simplemente regresamos la referencia al siguiente objeto en la cola
				return go.transform;
			}
			else//En el caso de que es el primer objeto que nos piden, y no existe el pool, Creamos un Pool con valores default
			{
				MakePool(prefab);
				//Recursivamente volver a pedir un objeto
				return Spawn(prefab,position,rotation);
			}
			
			//Regresar la referencia al transform del proximo objeto en el Pool
			//return new GameObject("Nothing").transform;
		}
		
		public static void Despawn(GameObject prefab)
		{
			//Ocultar el objeto que se quita
			//Y avisar a la instancia que fue removido
			prefab.SendMessage("OnDespawn",SendMessageOptions.DontRequireReceiver);
			prefab.SetActive(false);
		}
	}
}














