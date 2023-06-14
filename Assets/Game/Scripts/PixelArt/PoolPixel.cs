using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolPixel : MonoBehaviour
{
    public static PoolPixel Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Transform parent;
    public int amount;
    public Pixel objectPrefab;

    [HideInInspector]
    public List<Pixel> listObject = new List<Pixel>();

    private void Start()
    {
        StartCoroutine(C_Start());
    }

    private IEnumerator C_Start()
    {
        for (int i = 0; i < amount; i++)
        {
            Pixel objectClone = Instantiate(objectPrefab, parent);
            objectClone.gameObject.SetActive(false);
            listObject.Add(objectClone);
            if(i % 10== 0) yield return null;         
        }
    }

    public Pixel GetPixelInPool()
    {
        int childCount = listObject.Count;

        for (int i = 0; i < childCount; i++)
        {
            Pixel childObject = listObject[i];
            if (childObject.gameObject.activeInHierarchy == false)
            {
                return childObject;
            }
        }

        Pixel objectClone = Instantiate(objectPrefab, parent);
        objectClone.gameObject.SetActive(false);
        listObject.Add(objectClone);
        return objectClone;
    }

    public void HideAll()
    {
        int childCount = listObject.Count;

        for (int i = 0; i < childCount; i++)
        {
            Pixel childObject = listObject[i];
            childObject.transform.SetParent(parent);
            childObject.Hide();
        }
    }
}
