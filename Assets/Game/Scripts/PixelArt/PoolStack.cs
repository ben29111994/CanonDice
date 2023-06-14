using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolStack : MonoBehaviour
{
    public static PoolStack Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Transform parent;
    public int amount;
    public Stack objectPrefab;

    [HideInInspector]
    public List<Stack> listObject = new List<Stack>();

    private void Start()
    {
        StartCoroutine(C_Start());
    }

    private IEnumerator C_Start()
    {
        for (int i = 0; i < amount; i++)
        {
            Stack objectClone = Instantiate(objectPrefab, parent);
            objectClone.gameObject.SetActive(false);
            listObject.Add(objectClone);
            if (i % 10 == 0) yield return null;
        }
    }

    public Stack GetStackInPool()
    {
        int childCount = listObject.Count;

        for (int i = 0; i < childCount; i++)
        {
            Stack childObject = listObject[i];
            if (childObject.gameObject.activeInHierarchy == false)
            {
                return childObject;
            }
        }

        Stack objectClone = Instantiate(objectPrefab, parent);
        objectClone.gameObject.SetActive(false);
        listObject.Add(objectClone);
        return objectClone;
    }

    public void HideAll()
    {
        int childCount = listObject.Count;

        for (int i = 0; i < childCount; i++)
        {
            Stack childObject = listObject[i];
            childObject.transform.SetParent(parent);
            childObject.gameObject.SetActive(false);
        }
    }
}
