using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCube : MonoBehaviour
{
    public static PoolCube Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Transform parent;
    public int amount;
    public Cube objectPrefab;

    [HideInInspector]
    public List<Cube> listObject = new List<Cube>();

    private void Start()
    {
        StartCoroutine(C_Start());
    }

    private IEnumerator C_Start()
    {
        for (int i = 0; i < amount; i++)
        {
            Cube objectClone = Instantiate(objectPrefab, parent);
            objectClone.gameObject.SetActive(false);
            listObject.Add(objectClone);
            yield return null;
        }
    }

    public Cube GetCubeInPool()
    {
        int childCount = listObject.Count;

        for (int i = 0; i < childCount; i++)
        {
            Cube childObject = listObject[i];
            if (childObject.gameObject.activeInHierarchy == false)
            {
                return childObject;
            }
        }

        Cube objectClone = Instantiate(objectPrefab, parent);
        objectClone.gameObject.SetActive(false);
        listObject.Add(objectClone);
        return objectClone;
    }
}
