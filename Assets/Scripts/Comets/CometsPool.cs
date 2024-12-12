using System.Collections.Generic;
using CometRush.Enums;
using UnityEngine;

public class CometsPool : MonoBehaviour
{
    [System.Serializable]
    public class PoolSettings
    {
        public CometType poolName;
        public int poolSize;
        public GameObject pooledObject;
    }

    [Header("Comet Pools")]
    [SerializeField] private List<PoolSettings> _poolSettings;

    private Dictionary<CometType, Stack<GameObject>> _pools;

    private void Awake()
    {
        _pools = new Dictionary<CometType, Stack<GameObject>>();

        foreach (var settings in _poolSettings)
        {
            InitializePool(settings.poolName, settings.pooledObject, settings.poolSize);
        }
    }

    private void InitializePool(CometType poolName, GameObject pooledObject, int poolSize)
    {
        var poolStack = new Stack<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject instance = Instantiate(pooledObject, transform.position, Quaternion.identity, transform);
            instance.SetActive(false);
            poolStack.Push(instance);
        }
        
        _pools[poolName] = poolStack;
    }

    public GameObject GetObject(CometType poolName)
    {
        if (!_pools.TryGetValue(poolName, out var poolStack))
        {
            return null;
        }

        if (poolStack.Count == 0)
        {
            GameObject instance = Instantiate(_poolSettings.Find(p => p.poolName == poolName).pooledObject, transform.position, Quaternion.identity, transform);
            return instance;
        }

        GameObject nextInstance = poolStack.Pop();
        nextInstance.SetActive(true);
        return nextInstance;
    }

    public void ReturnToPool(CometType poolName, GameObject instance)
    {
        if (!_pools.ContainsKey(poolName))
        {
            Destroy(instance);
            return;
        }

        instance.SetActive(false);
        _pools[poolName].Push(instance);
    }
}
