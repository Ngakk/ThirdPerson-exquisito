using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos{
	public class PoolManager : MonoBehaviour 
	{
		//Dictionary 
		//Listas dinamicas Cola Queue
		static Dictionary<int, Queue<GameObject>> pool = new Dictionary<int, Queue<GameObject>>();
		//Crear un contenedor
		static Dictionary<int, GameObject> poolHolder = new Dictionary<int, GameObject>();
		//limites de cada pool
		static Dictionary<int,int> poolLimit = new Dictionary<int, int>();

	    //Clear Pools
	    public static void ClearPools()
	    {
	        pool.Clear();
	        poolHolder.Clear();
	        poolLimit.Clear();
	    }

	    //Pre Spawn 
	    public static void PreSpawn(GameObject prefab, int amount)//Cuantos
		{
			//Crear la cola de objetos o el pool
			MakePool(prefab);
			//For 0 a tantos y creamos objetos
			//Agregar a la cola la cantidad de elementos iniciales del tipo prefab
			//Se crean N instancias iniciales
			for(int i = 0; i < amount; i++)
			{
				//Crear un objeto en el indice i
				AddNewItemToQueue(prefab, i);	
			}
		}
		//AgregarItems
		private static void AddNewItemToQueue(GameObject prefab, int i)
		{
			//Crear un GO Instantiate
			GameObject go = (GameObject)Instantiate(prefab);
			//Datos
			go.name = go.name + "_PoolInstance("+i+")";
			//Meterlo al pool o dictionary que le corresponde
			pool[prefab.GetInstanceID()].Enqueue(go);
			//Apagar la instancia
			go.SetActive(false);
			//Crear un Manager o contenedor de objetos
			go.transform.parent = poolHolder[prefab.GetInstanceID()].transform;
		}
		
		//TODO crear funcion para quitar un item del Queue / pool (No es muy necesario hacerlo)
		
		//Metodos publicos para llamar al Pool
		public static Transform Spawn(GameObject prefab, Vector3 position, Quaternion rotation)// va ser como un Instantiate
		{
			//Logica para crear un objeto y quitar el que no se necesite
			//ver si ya existe algun pool con este tipo de objeto
			if(pool.ContainsKey(prefab.GetInstanceID()))
			{
				//Si ya existe el pool, se toma el siguiente en la cola, para ser utilizado
				//ver si hay objetos disponibles	
				bool areAnyAvailable = false;
				//TODO cambiar este for por otra logica para que siempre revise el siguiente
				for(int i = 0; i < pool[prefab.GetInstanceID()].Count; i++)
				{
					if(pool[prefab.GetInstanceID()].Peek().activeSelf )
					{
						//Esta en uso este objeto, hay que pasarlo al final de la cola
						GameObject tmp_go = pool[prefab.GetInstanceID()].Dequeue();
						pool[prefab.GetInstanceID()].Enqueue(tmp_go);
					}
					else
					{
						//Si hay un objeto disponible
						areAnyAvailable = true;
						break;
					}
				}
				
				//si no hay objetos, hay que ver si el pool esta lleno
				if(areAnyAvailable==false)
				{
					//Ver si el pool esta lleno
					if(pool[prefab.GetInstanceID()].Count < poolLimit[prefab.GetInstanceID()])
					{
						//Esta lleno pero aun hay espacio en el limite
						//Todos los objetos estan en uso, hay que crear uno nuevo 
						//crear un objeto de ese tipo y meterlo al pool
						//Crear una copia temporal de los objetos en un array
						var items = pool[prefab.GetInstanceID()].ToArray();
						//limpiar la cola actual
						pool[prefab.GetInstanceID()].Clear();
						//Meter al nuevo objeto
						AddNewItemToQueue(prefab,items.Length);
						//Regresar a los que estaban originalmente pero un lugar atras, para que el nuevo sea el siguiente
						foreach(var item in items)
						{
							pool[prefab.GetInstanceID()].Enqueue(item);
						}
					}
					else
					{
						//Todos estan en uso y a demas ya no puede crecer mas el pool
						return null;
					}
				}
				
				//Simular un Instantiate
				//Regresar al siguiente disponible
				GameObject go = pool[prefab.GetInstanceID()].Dequeue();

				//Asignarle los valores que pide el usuario a posicion y rotacion
				go.transform.position = position;
				go.transform.rotation = rotation;
				//Activar el objeto 
				go.SetActive(true);
				//Avisar al objeto que se acaba de crear que ha sido recien instanciado, es como un Start Fake
				go.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
				//Volverlo a meter al final de la cola
				pool[prefab.GetInstanceID()].Enqueue(go);
				//Opcional
				//Podria sacarlo del objeto del queue, del contenedor
				//go.transform.parent = null;
				//regreso la referencia al objeto instanciado
				return go.transform;
			}
			else// si no existe hay que crearlo
			{
				//Crear un pool
				PreSpawn(prefab,2);
				return Spawn(prefab, position, rotation);
			}
		}
		//Esto va a ser similar a un Destroy
		public static void Despawn(GameObject prefab)
		{
			//Ocultar al objeto
			prefab.SendMessage("OnDespawn", SendMessageOptions.DontRequireReceiver);
			prefab.SetActive(false);
		}
		
		//Metodo para crear un pool de uso interno para que todo sea mas facil
		private static void MakePool(GameObject prefab)
		{
	        //Crear una nueva cola, para guardar TODOS los objetos que sean de este mismo tipo
	        //Crear los diccionarios para referenciar despues
	        if (!poolLimit.ContainsKey(prefab.GetInstanceID()))
	        {
	            pool.Add(prefab.GetInstanceID(), new Queue<GameObject>());
	            poolHolder.Add(prefab.GetInstanceID(), new GameObject("_Pool[" + prefab.name + "]"));
	            poolLimit.Add(prefab.GetInstanceID(), 5);
	        }
		}
		
		//Configurar el limite del pool, una especie de constructor
		public static void SetPoolLimit(GameObject prefab, int newLimit)
		{
			//Si el pool ya existe ponemos el limite
			if(poolLimit.ContainsKey(prefab.GetInstanceID()))
			{
				poolLimit[prefab.GetInstanceID()] = newLimit;
			}
			else
			{
				//Si no existe, creamos un pool con este limite
				poolLimit.Add(prefab.GetInstanceID(), newLimit);
			}
		}
	}
}
