using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private int _poolSize;
    [SerializeField] private GameObject _pooledObject;

    private Stack<GameObject> _poolStack;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        _poolStack = new Stack<GameObject>();

        for (int i = 0; i < _poolSize; i++) { 
            GameObject instance = Instantiate(_pooledObject, transform.position, Quaternion.identity, transform);
            instance.SetActive(false);
            _poolStack.Push(instance);
        }
        
    }

    public GameObject GetPooledObject() {

        if (_poolStack.Count == 0) { 
            GameObject instance = Instantiate(_pooledObject, transform.position, Quaternion.identity, transform);
            return instance;
        }
        
        GameObject nextInstance = _poolStack.Pop();
        nextInstance.SetActive(true);
        return nextInstance;
    }

    public void ReturnToPool(GameObject instance) { 
        _poolStack.Push(instance);
        instance.SetActive(false);
    }
}
