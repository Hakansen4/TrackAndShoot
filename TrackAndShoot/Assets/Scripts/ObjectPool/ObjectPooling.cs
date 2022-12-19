using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    private Queue<GameObject> poolObjects;
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private int poolSize;

    private void Awake()
    {
        poolObjects = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.SetActive(false);
            poolObjects.Enqueue(obj);
        }
    }
    public GameObject GetPooledObject()
    {
        GameObject obj = poolObjects.Dequeue();
        obj.SetActive(true);
        poolObjects.Enqueue(obj);
        return obj;
    }
}
