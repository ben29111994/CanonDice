using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBullet_Yellow : MonoBehaviour
{
    public static PoolBullet_Yellow Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Transform parent;
    public int amount;
    public Bullet objectPrefab;

    [HideInInspector]
    public List<Bullet> listObject = new List<Bullet>();

    private void Start()
    {
        StartCoroutine(C_Start());
    }

    private IEnumerator C_Start()
    {
        for (int i = 0; i < amount; i++)
        {
            Bullet objectClone = Instantiate(objectPrefab, parent);
            objectClone.gameObject.SetActive(false);
            listObject.Add(objectClone);

            if(i % 20 == 0) yield return null;
        }
    }

    public Bullet GetBulletInPool()
    {
        int childCount = listObject.Count;

        for (int i = 0; i < childCount; i++)
        {
            Bullet childObject = listObject[i];
            if (childObject.gameObject.activeInHierarchy == false)
            {
                return childObject;
            }
        }

        Bullet objectClone = Instantiate(objectPrefab, parent);
        objectClone.gameObject.SetActive(false);
        listObject.Add(objectClone);
        return objectClone;
    }

    public void HideAll()
    {
        for (int i = 0; i < amount; i++)
        {
            listObject[i].DisableBullet();
        }
    }
}
