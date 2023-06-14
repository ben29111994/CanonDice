using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [Header("Pool Manager")]
    public List<ObjectPool> ObjectPools = new List<ObjectPool>();
    
    public enum NameObject
    {
        Cube,
        Dice,
        PlusObject,
        CubeBreak,
        PlusNumberText
    }

    [System.Serializable]
    public class ObjectPool
    {
        public Transform parent;
        public int amount;
        public Object objectPrefab;
        public NameObject nameObject;

        [HideInInspector]
        public List<Object> listObject = new List<Object>();
    }

    private void Awake()
    {
        Instance = (Instance == null) ? this : Instance;
        GenerateObjectPool();
    }

    public void GenerateObjectPool()
    {
        int count = ObjectPools.Count;

        for (int i = 0; i < count; i++)
        {
            int amount = ObjectPools[i].amount;
            Object prefab = ObjectPools[i].objectPrefab;
            Transform parent = ObjectPools[i].parent;

            for (int j = 0; j < amount; j++)
            {
                Object objectClone = Instantiate(prefab, parent) as Object;
                GameObject _go = objectClone as GameObject;
                _go.SetActive(false);
                ObjectPools[i].listObject.Add(objectClone);
            }
        }
    }

    public Object GetObject(NameObject name)
    {
        int count = ObjectPools.Count;
        ObjectPool objectPool = null;

        for (int i = 0; i < count; i++)
        {
            if (ObjectPools[i].nameObject == name)
            {
                objectPool = ObjectPools[i];
            }
        }

        if (objectPool == null) return null;

        int childCount = objectPool.listObject.Count;

        for (int i = 0; i < childCount; i++)
        {
            Object childObject = objectPool.listObject[i];
            GameObject _go = childObject as GameObject;
            if (_go.activeInHierarchy == false)
            {
                return childObject;
            }
        }

        Object objectClone = Instantiate(objectPool.objectPrefab, objectPool.parent) as Object;
        GameObject _go2 = objectClone as GameObject;
        _go2.SetActive(false);
        objectPool.listObject.Add(objectClone);
        return objectClone;
    }

    public void RefreshItem(NameObject name)
    {
        for (int i = 0; i < ObjectPools.Count; i++)
        {
            if (ObjectPools[i].nameObject == name)
            {
                for (int k = 0; k < ObjectPools[i].parent.childCount; k++)
                {
                    ObjectPools[i].parent.GetChild(k).gameObject.SetActive(false);
                }
            }
        }
    }

    public void RefreshAll()
    {
        for (int i = 0; i < ObjectPools.Count; i++)
        {
            for (int k = 0; k < ObjectPools[i].parent.childCount; k++)
            {
                ObjectPools[i].parent.GetChild(k).gameObject.SetActive(false);
            }
        }
    }
}
