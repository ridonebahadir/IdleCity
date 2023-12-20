using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeonBrave{

public class SingletonHandler : MonoBehaviour
{
	public static SingletonHandler Instance;
	
	void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
		else if(Instance != this)
		{
			Destroy(gameObject);

		}
		_singletons = new Dictionary<Type, MonoBehaviour>();
		//	RegisterSingletons();
		
	}

	private List<MonoBehaviour> singletonList;

	private Dictionary<Type, MonoBehaviour> _singletons;
	private void RegisterSingletons()
	{
		foreach (var singleton in singletonList)
		{
			var type = singleton.GetType();
			if (!_singletons.ContainsKey(type))
			{
				_singletons.Add(type, singleton);
			}
			else
			{
				Debug.LogWarning($"SingletonManager already contains a singleton of type {type}. Duplicate will be ignored.");
			}
		}
	}

	public void RegisterToSingleton(MonoBehaviour singleton)
	{
		var type = singleton.GetType();
		if (!_singletons.ContainsKey(type))
		{
			_singletons.Add(type, singleton);
		}
		else
		{
			Debug.LogWarning($"SingletonManager already contains a singleton of type {type}. Duplicate will be ignored.");
		}
	}

	public void UnRegisterToSingleton(MonoBehaviour singleton)
	{
		var type = singleton.GetType();
		if (_singletons.ContainsKey(type))
		{
			_singletons.Remove(type);
		}
		else
		{
			Debug.LogWarning($"No singleton of type {type} is registered.");
		}
	}
        
	public T GetSingleton<T>() where T : MonoBehaviour
	{
		Type type = typeof(T);
		if (_singletons.TryGetValue(type, out MonoBehaviour singleton))
		{
			return singleton as T;
		}

		Debug.LogWarning($"No singleton of type {type} is registered.");
		return null;
	}
}

}
