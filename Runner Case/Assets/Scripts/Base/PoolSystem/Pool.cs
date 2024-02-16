using System.Collections.Generic;
using Base.PoolSystem.PoolTypes.Abstracts;
using Managers;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [Header("Pool Type")]
    [SerializeField] PoolVariants poolVariant;

    [Header("Pool Prefab")]
    [SerializeField] PoolObject poolGameObject;
    
    [Header("Pool Size")]
    [SerializeField] int poolStartSize = 10;
    
    
    private Queue<PoolObject> _poolObjects = new();
    
    private void Awake() 
    {
        FillPool();
    }

    private void OnEnable()
    {
        switch ((int)poolVariant)
        {
            case 0:
                EventManager.Instance.ReturnCoinPool.AddListener(ReturnPool);
                break;
            case 1:
                EventManager.Instance.ReturnWayPartPool.AddListener(ReturnPool);
                break;
        }
        
    }
    
    public PoolObject GetPooledObject()
    {
        while (_poolObjects.Count < 1)
        {
            CreatePoolObject();
        }
        
        var obj = _poolObjects.Dequeue();
        return obj;
    }

    private void CreatePoolObject()
    {
        PoolObject poolObject = Instantiate(poolGameObject,Vector3.zero, Quaternion.identity);
        poolObject.gameObject.SetActive(false);
        poolObject.transform.parent = gameObject.transform;
        _poolObjects.Enqueue(poolObject);
    }
    private void FillPool()
    {
        for (int i = 0; i < poolStartSize; i++)
        {
            CreatePoolObject();
        }
    }
    private void ReturnPool(PoolObject obj)
    {
        obj.gameObject.SetActive(false);
        _poolObjects.Enqueue(obj);
    }
    
}

public enum PoolVariants { CoinPool = 0, WayPartPool = 1}