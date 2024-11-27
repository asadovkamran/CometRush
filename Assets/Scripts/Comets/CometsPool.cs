using System.Collections.Generic;
using UnityEngine;

public class CometsPool : MonoBehaviour
{
    [System.Serializable]
    public class PoolSettings
    {
        public string poolName; // Name to identify the pool
        public int poolSize;
        public GameObject pooledObject;
    }

    [Header("Comet Pools")]
    [SerializeField] private List<PoolSettings> _poolSettings;

    private Dictionary<string, Stack<GameObject>> _pools;

    private void Awake()
    {
        _pools = new Dictionary<string, Stack<GameObject>>();

        foreach (var settings in _poolSettings)
        {
            InitializePool(settings.poolName, settings.pooledObject, settings.poolSize);
        }
    }

    private void InitializePool(string poolName, GameObject pooledObject, int poolSize)
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

    public GameObject GetObject(string poolName)
    {
        if (!_pools.ContainsKey(poolName))
        {
            Debug.LogError($"Pool with name {poolName} does not exist.");
            return null;
        }

        var poolStack = _pools[poolName];

        if (poolStack.Count == 0)
        {
            Debug.LogWarning($"Pool {poolName} is empty. Creating a new object.");
            GameObject instance = Instantiate(_poolSettings.Find(p => p.poolName == poolName).pooledObject, transform.position, Quaternion.identity, transform);
            return instance;
        }

        GameObject nextInstance = poolStack.Pop();
        nextInstance.SetActive(true);
        return nextInstance;
    }

    public void ReturnToPool(string poolName, GameObject instance)
    {
        if (!_pools.ContainsKey(poolName))
        {
            Debug.LogError($"Pool with name {poolName} does not exist.");
            Destroy(instance);
            return;
        }

        instance.SetActive(false);
        _pools[poolName].Push(instance);
    }
}
