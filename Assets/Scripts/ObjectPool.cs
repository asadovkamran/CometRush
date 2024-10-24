using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private int poolSize;
    [SerializeField] private GameObject pooledObject;

    private Stack<GameObject> poolStack;

    private void Awake()
    {
        InitializePool();
    }

    void InitializePool()
    {
        poolStack = new Stack<GameObject>();

        for (int i = 0; i < poolSize; i++) { 
            GameObject instance = Instantiate(pooledObject, transform.position, Quaternion.identity, transform);
            instance.SetActive(false);
            poolStack.Push(instance);
        }
        
    }

    public GameObject GetPooledObject() {
        // if stack is empty then instantiate new object
        if (poolStack.Count == 0) { 
            GameObject instance = Instantiate(pooledObject, transform.position, Quaternion.identity, transform);
            return instance;
        }
        // otherwise take the next object in stack and set it active
        GameObject nextInstance = poolStack.Pop();
        nextInstance.SetActive(true);
        return nextInstance;
    }

    public void ReturnToPool(GameObject instance) { 
        poolStack.Push(instance);
        instance.SetActive(false);
    }
}
