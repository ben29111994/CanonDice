using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryBitMap : MonoBehaviour
{
    public List<bool> listIsPixelCube = new List<bool>();
    public List<Color> listColor = new List<Color>();
    public List<Vector3> listCube = new List<Vector3>();
    public List<Vector3> listWall = new List<Vector3>();
    public Transform offsetCamera;
    public List<Player> listPlayer = new List<Player>();

    public void GetReferences()
    {
        offsetCamera = transform.GetChild(0);

        listPlayer.Clear();
        for (int i = 1; i < transform.childCount; i++)
        {
            listPlayer.Add(transform.GetChild(i).GetComponent<Player>());
        }
    }
}
