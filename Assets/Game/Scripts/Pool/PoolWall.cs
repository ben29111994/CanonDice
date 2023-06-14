using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolWall : MonoBehaviour
{
    public static PoolWall Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Transform parent;
    public int amount;
    public Wall objectPrefab;

    [HideInInspector]
    public List<Wall> listObject = new List<Wall>();

    private void Start()
    {
        StartCoroutine(C_Start());
    }

    private IEnumerator C_Start()
    {
        for (int i = 0; i < amount; i++)
        {
            Wall objectClone = Instantiate(objectPrefab, parent);
            objectClone.gameObject.SetActive(false);
            listObject.Add(objectClone);
            yield return null;
        }
    }

    public Wall GetCubeInPool()
    {
        int childCount = listObject.Count;

        for (int i = 0; i < childCount; i++)
        {
            Wall childObject = listObject[i];
            if (childObject.gameObject.activeInHierarchy == false)
            {
                return childObject;
            }
        }

        Wall objectClone = Instantiate(objectPrefab, parent);
        objectClone.gameObject.SetActive(false);
        listObject.Add(objectClone);
        return objectClone;
    }
}
