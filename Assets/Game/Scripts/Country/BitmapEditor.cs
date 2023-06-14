using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitmapEditor : MonoBehaviour
{
    [Header("Input")]
    public bool isBonus;

    [Header("References")]
    public bool isGenerating;
    public Color color;
    public Texture2D texture2d;
    private CanonDiceGame CDG;
    private List<Cube> listCube = new List<Cube>();
    private List<Color> listColor = new List<Color>();
    private List<bool> listIsPixelCube = new List<bool>();
    private List<GameObject> listWallEditor = new List<GameObject>();
    private CountryBitMap countryBitmap;

    [NaughtyAttributes.Button]
    public void TestColor()
    {
        int width = texture2d.width;
        int height = texture2d.height;
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                Color32 _color = texture2d.GetPixel(w, h);
                float _r = _color.r;
                float _g = _color.g;
                float _b = _color.b;

                if (_r == 255 && _g == 242 && _b == 0)
                {
                    Debug.Log("Yellow");
                }
                else if (_r == 0 && _g == 162 && _b == 232)
                {
                    Debug.Log("Blue");
                }
                else if (_r == 237 && _g == 28 && _b == 36)
                {
                    Debug.Log("Red");
                }
                else if (_r == 34 && _g == 177 && _b == 76)
                {
                    Debug.Log("Green");
                }
                else
                {
                    Debug.Log(_color);


                }
            }
        }
    }

    // nau-button here
    [NaughtyAttributes.Button]
    public void Generete()
    {
        countryBitmap = GetComponent<CountryBitMap>();
        if (countryBitmap == null) countryBitmap = gameObject.AddComponent<CountryBitMap>();
        countryBitmap.GetReferences();
        CDG = GameObject.Find("CanonDiceGame").GetComponent<CanonDiceGame>();

        isGenerating = false;
        StopAllCoroutines();
        listCube.Clear();
        listIsPixelCube.Clear();
        listColor.Clear();
        listWallEditor.Clear();
        countryBitmap.listWall.Clear();
        countryBitmap.listCube.Clear();

        int width = texture2d.width;
        int height = texture2d.height;
        Debug.Log(width + " _ " + height);
        for(int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                Color32 _color = texture2d.GetPixel(w, h);
                float _r = _color.r;
                float _g = _color.g;
                float _b = _color.b;
                Color _color2 = _color;

                if (isBonus)
                {
                    bool _isYB = (_r == 255 && _g == 242 && _b == 0) || (_r == 0 && _g == 162 && _b == 232) ? true : false;
                    Cube _cube = Instantiate(CDG.cubePrefab, transform);
                    _cube.transform.localPosition = new Vector3(w, 100.0f, h);
                    listCube.Add(_cube);
                    listColor.Add(_color2);

                    bool isPixelCube = !_isYB;
                    listIsPixelCube.Add(isPixelCube);
                }
                else
                {
                    if (_color == Color.black || _color.a == 0.0f)
                    {

                    }
                    else if (_r == 255 && _g == 242 && _b == 0)
                    {
                        // yellow
                        Cube _cube = Instantiate(CDG.cubePrefab, transform);
                        _cube.transform.localPosition = new Vector3(w, 1000.0f, h);
                        listCube.Add(_cube);
                    }
                    else if (_r == 0 && _g == 162 && _b == 232)
                    {
                        // blue
                        Cube _cube = Instantiate(CDG.cubePrefab, transform);
                        _cube.transform.localPosition = new Vector3(w, 2000.0f, h);
                        listCube.Add(_cube);
                    }
                    else if (_r == 237 && _g == 28 && _b == 36)
                    {
                        // red
                        Cube _cube = Instantiate(CDG.cubePrefab, transform);
                        _cube.transform.localPosition = new Vector3(w, 3000.0f, h);
                        listCube.Add(_cube);
                    }
                    else
                    {
                        Cube _cube = Instantiate(CDG.cubePrefab, transform);
                        _cube.transform.localPosition = new Vector3(w, 500.0f, h);
                        listCube.Add(_cube);
                    }
                }
            }
        }

        for (int i = 0; i < listCube.Count; i++)
        {
            countryBitmap.listCube.Add(listCube[i].gameObject.transform.position);
        }

        countryBitmap.listColor = listColor;
        countryBitmap.listIsPixelCube = listIsPixelCube;

        GenerateWall(countryBitmap);
    }

    public void GenerateWall(CountryBitMap _bitmap)
    {
        if (isGenerating) return;
        StartCoroutine(C_GenerateWall(_bitmap));
    }

    private IEnumerator C_GenerateWall(CountryBitMap _bitmap)
    {
        isGenerating = true;
        yield return null;

        for (int i = 0; i < _bitmap.listCube.Count; i++)
        {
            if (_bitmap.listCube[i] != Vector3.zero)
                CheckRayCast(_bitmap.listCube[i]);
        }

        for (int i = 0; i < listWallEditor.Count; i++)
        {
            _bitmap.listWall.Add(listWallEditor[i].transform.position);
        }

        for(int i = 0; i < listWallEditor.Count; i++)
        {
            DestroyImmediate(listWallEditor[i].gameObject);
        }
        listWallEditor.Clear();

        for (int i = 0; i < listCube.Count; i++)
        {
            DestroyImmediate(listCube[i].gameObject);
        }
        listCube.Clear();

        isGenerating = false;
    }

    private void CheckRayCast(Vector3 pos)
    {
        float scale = 1.0f;
        Vector3 a = pos + Vector3.left * scale;
        Vector3 b = pos + Vector3.right * scale;
        Vector3 c = pos + Vector3.back * scale;
        Vector3 d = pos + Vector3.forward * scale;
        RayCastGenerateWall(a);
        RayCastGenerateWall(b);
        RayCastGenerateWall(c);
        RayCastGenerateWall(d);
    }

    private void RayCastGenerateWall(Vector3 _origin)
    {
        Vector3 origin = _origin + Vector3.up * 10000.0f;
        Ray ray = new Ray(origin, Vector3.down);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 30000.0f))
        {

        }
        else
        {
            if(_origin.x == 3023 && _origin.z == 15)
            {
                Debug.Log(_origin + " TEST");
            }
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
        listWallEditor.Add(_wall);
    }
}
