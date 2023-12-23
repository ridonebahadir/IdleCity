using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace LeonBrave{

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private List<ObjectTypePool> objectTypePools;

    private void Awake()
    {
        SingletonHandler.Instance.RegisterToSingleton(this);
    }

    private void Start()
    {
        foreach (var item in objectTypePools)
        {
            item.StartSpawn();
        }
    }

    public void AddObject(GameObject droppableItemObject,ObjectType objectType)
    {
        FindAvailablePool(objectType).AddItem(droppableItemObject);
    }

    public GameObject TakeObject(ObjectType plantType)
    {
        return FindAvailablePool(plantType).TakeItem();
    }

    private ObjectTypePool FindAvailablePool(ObjectType objectType)
    {
        return objectTypePools[(int)objectType];
    }
    
}

[System.Serializable]
public class ObjectTypePool
{
    [SerializeField] private ObjectType objectType; 
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private Transform takeTransform;
    [SerializeField] private int count;
    [SerializeField] private List<GameObject> objects;
    
    public GameObject TakeItem()
    {
        GameObject itemGo;
        if (objects.Count > 0)
        {
            itemGo = objects[0];
            objects.Remove(itemGo);
        }
        else
        {
            itemGo = Object.Instantiate(objectPrefab);
        }

        itemGo.transform.parent = takeTransform;
        itemGo.gameObject.SetActive(false);
        return itemGo;
    }

    public void StartSpawn()
    {
        for (var i = 0; i < count; i++)
        {
            var obj=Object.Instantiate(objectPrefab, takeTransform);
            objects.Add(obj);
            obj.gameObject.SetActive(false);
        }
       
    }
    public void AddItem(GameObject itemGo)
    {
        if (objects.Contains(itemGo)) return;

        objects.Add(itemGo);
        itemGo.gameObject.SetActive(false);
        itemGo.transform.parent = takeTransform;
    }
}

public enum ObjectType
{
    Enemy=0,
    EnemyArcher=1,
    EnemyDigger=2,
    Soldier=3,
    SoldierArcher=4,
    SoliderDigger=5,
    EnemyArrow=6,
    SoldierArrow=7,
    Coin=8,
    Bomb=9,
}

}
