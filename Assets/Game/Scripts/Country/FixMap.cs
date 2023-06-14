using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVoxelizer;
using UnityEditor;

public class FixMap : MonoBehaviour
{
    [Header("Status")]
    public bool isGenerating;
    public GameObject test;

    [Header("References Input")]
    public List<Player> listPlayer = new List<Player>();

    [Header("References Output")]
    public List<Vector3> listPositionCube = new List<Vector3>();
    public List<Vector3> listPositionWall = new List<Vector3>();
    public List<Cube> listCube = new List<Cube>();
    public List<GameObject> listWall = new List<GameObject>();

    private VoxelGroup voxelGroup;
    private void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            listPlayer.Add(transform.GetChild(i).GetComponent<Player>());
        }
    }


#if UNITY_EDITOR
    // nau here
    public void FixMap_Func()
    {
        listCube.Clear();
        voxelGroup = GetComponent<VoxelGroup>();

        List<GameObject> listG = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject _g = transform.GetChild(i).gameObject;
            listG.Add(_g);
            _g.gameObject.tag = "Cube";
            _g.gameObject.layer = 8;

            Cube _cube = _g.AddComponent<Cube>();
            _cube.rend = _g.GetComponent<MeshRenderer>();
            listCube.Add(_cube);

            BoxCollider _boxCollider = _g.AddComponent<BoxCollider>();
            _boxCollider.size = _boxCollider.size + Vector3.forward * 10.0f;
            _boxCollider.isTrigger = true;
            //   _cube.gameObject.SetActive(false);
        }

        GenerateWall();
    }

    private void SaveData()
    {
        for (int i = 0; i < listCube.Count; i++)
        {
            listPositionCube.Add(listCube[i].transform.position);
        }

        for (int i = 0; i < listWall.Count; i++)
        {
            listPositionWall.Add(listWall[i].transform.position);
        }

        while (listCube.Count > 0)
        {
            DestroyImmediate(listCube[0].gameObject);
            listCube.RemoveAt(0);
        }

        while (listWall.Count > 0)
        {
            DestroyImmediate(listWall[0].gameObject);
            listWall.RemoveAt(0);
        }

        listCube.Clear();
        listWall.Clear();

        Country _country = transform.parent.gameObject.GetComponent<Country>();
        if(_country == null) _country = transform.parent.gameObject.AddComponent<Country>();

        _country.fixMapObject = this;
        DestroyImmediate(voxelGroup);
    }

    // nau here
    public void GenerateWall()
    {
        if (isGenerating) return;
        StartCoroutine(C_GenerateWall());
    }

    private IEnumerator C_GenerateWall()
    {
        isGenerating = true;
        yield return null;

        for (int i = 0; i < listCube.Count; i++)
        {
            if (listCube[i] != null)
                CheckRayCast(listCube[i].transform);
        }

        isGenerating = false;

        SaveData();
    }

    private void CheckRayCast(Transform cube)
    {
        float scale = Vector3.Distance(listCube[0].transform.position, listCube[1].transform.position);
        Vector3 a = cube.position + Vector3.left * scale;
        Vector3 b = cube.position + Vector3.right * scale;
        Vector3 c = cube.position + Vector3.back * scale;
        Vector3 d = cube.position + Vector3.forward * scale;
        RayCastGenerateWall(a);
        RayCastGenerateWall(b);
        RayCastGenerateWall(c);
        RayCastGenerateWall(d);
    }

    private void RayCastGenerateWall(Vector3 _origin)
    {
        Vector3 origin = _origin + Vector3.up * 100.0f;
        Ray ray = new Ray(origin, Vector3.down);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200.0f))
        {

        }
        else
        {
            GenerateWall(_origin);
        }
    }

    private void GenerateWall(Vector3 _pos)
    {
        GameObject _wall = Instantiate(Resources.Load("Country/Wall", typeof(GameObject)), transform) as GameObject;
        //      _wall.GetComponent<MeshFilter>().mesh = voxelGroup.m_mesh;
        _pos.y = 0.0f;
        _wall.transform.position = _pos;
        _wall.transform.SetParent(transform);
        listWall.Add(_wall);
    }

    // nau here
    public void StopGenerateWall()
    {
        StopAllCoroutines();
        isGenerating = false;
    }
#endif

}
