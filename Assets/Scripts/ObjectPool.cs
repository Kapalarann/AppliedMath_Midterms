using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField] private List<PoolItem> poolItems;

    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private Dictionary<string, PoolItem> poolLookup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InitializePools();
    }

    private void InitializePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        poolLookup = new Dictionary<string, PoolItem>();

        foreach (var item in poolItems)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();

            for (int i = 0; i < item.initialSize; i++)
            {
                GameObject obj = Instantiate(item.prefab, transform);
                obj.SetActive(false);
                objectQueue.Enqueue(obj);
            }

            poolDictionary[item.itemTag] = objectQueue;
            poolLookup[item.itemTag] = item;
        }
    }

    public GameObject Dequeue(string itemTag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(itemTag))
        {
            Debug.LogError($"[ObjectPool] No pool with tag '{itemTag}'");
            return null;
        }

        Queue<GameObject> queue = poolDictionary[itemTag];
        GameObject obj;

        if (queue.Count > 0)
        {
            obj = queue.Dequeue();
        }
        else
        {
            PoolItem item = poolLookup[itemTag];
            if (!item.expandable)
            {
                Debug.LogWarning($"[ObjectPool] Pool '{itemTag}' is empty and not expandable.");
                return null;
            }

            obj = Instantiate(item.prefab, transform);
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

    public void Enqueue(string itemTag, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(itemTag))
        {
            Debug.LogWarning($"[ObjectPool] Trying to return object to unknown pool '{itemTag}'. Destroying.");
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        obj.transform.SetParent(transform);
        poolDictionary[itemTag].Enqueue(obj);
    }
}

[Serializable]
public class PoolItem
{
    public string itemTag;
    public GameObject prefab;
    public int initialSize = 10;
    public bool expandable = true;
}