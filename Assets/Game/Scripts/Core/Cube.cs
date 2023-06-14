using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GPUInstancer;

public class Cube : MonoBehaviour
{
    [Header("Status")]
    public SkinType skinType;
    public DefaultType defaultType;
    public int SkinTypeIndex;
    public int DefaultTypeIndex;
    public int NumberID;
    public int SkinTypeID;
    public bool isHitAnim;
    public bool isLineBang;
    public bool isPixelCube;
    public bool isTruePixel;
    public bool isHardColor;

    [Header("References")]
    public GPUInstancerPrefab outlinePrefab;
    public MeshFilter meshFilter;
    public Renderer rend;
    public Renderer rendOutline;
    public Material m_pixel;
    public Material m_nopixel;
    private Player myPlayer;
    private Color colorPixel;
    private MaterialPropertyBlock mpb;

    public enum SkinType
    {
        none,
        face1,
        face2,
        face3,
        face4,
        face5,
        face6,
        face7,
        face8,
        face9,
        face10,
        face11,
        face12,
        face13
    }

    public enum DefaultType
    {
        none,
        stone_0,
        stone_1,
        stone_2,
        stone_3,
        stone_4
    }

    private Vector3 startPosition;

    private void Awake()
    {
        GenerateOutLine();
    }

    private void GenerateOutLine()
    {
        GPUInstancerPrefab prefabInstance = Instantiate(outlinePrefab, transform);
        GameManager.Instance.UpdateGPUList(prefabInstance);
        rendOutline = prefabInstance.GetComponent<Renderer>();
    }

    public void GenerateCube(Vector3 _position,float _scale,int _defaultTypeIndex,int _skinIndex,bool _animActive,bool _isPixelCube,Color _colorPixel)
    {
        isTruePixel = false;
        isHardColor = false;
        isLineBang = (_position.y == 10.0f) ? true : false;
        myPlayer = null;
        DefaultTypeIndex = _defaultTypeIndex;
        SkinTypeIndex = _skinIndex;
        NumberID = -1;
        gameObject.layer = (9 + NumberID);

        isPixelCube = _isPixelCube;
        colorPixel = _colorPixel;

        if (_position.y == 1000.0f)
        {
            NumberID = 0;
            isHardColor = true;
        }
        else if (_position.y == 2000.0f)
        {
            NumberID = 1;
            isHardColor = true;
        }
        else if (_position.y == 3000.0f)
        {
            NumberID = 2;
            isHardColor = true;
        }
        else if (_position.y == 4000.0f)
        {
            NumberID = 3;
            isHardColor = true;
        }

        rend.material = GameManager.Instance.canonDiceGame.m_Cube[0];
        rend.transform.localPosition = Vector3.up * -1.5f;
        rend.transform.localScale = Vector3.one * 1.0f + Vector3.up * 3.0f;
        _position.y = 1.0f;
        transform.position = _position;
        transform.localScale = Vector3.one * _scale;
        startPosition = transform.position;
        rendOutline.material = GameManager.Instance.canonDiceGame.m_CubeOutline[0];

        if(isLineBang) rend.material = GameManager.Instance.canonDiceGame.m_CubeOutline[2];

        gameObject.SetActive(true);
        if(_animActive) Active_Effect();

        // fix rotate when map is -90 angle
        if (GameManager.Instance.canonDiceGame.isMap90)
        {
            rend.transform.localEulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
        }
        else
        {
            rend.transform.localEulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        }

        mpb = new MaterialPropertyBlock();
        rend.GetPropertyBlock(mpb);
        mpb.Clear();
        rend.SetPropertyBlock(mpb);
    }

    public void Hide()
    {
        if (myPlayer != null)
        {
            myPlayer.listCube.Remove(this);
        }

        rend.material = m_nopixel;
        transform.SetParent(PoolCube.Instance.gameObject.transform);
        gameObject.SetActive(false);
    }

    public void AddCubeToPlayer()
    {
        if(myPlayer != null)
        {
            myPlayer.listCube.Remove(this);
        }

        if (GameManager.Instance.canonDiceGame.listPlayer.Count == 0) return;

        myPlayer = GameManager.Instance.canonDiceGame.listPlayer[NumberID];
        myPlayer.listCube.Add(this);
    }

    public void ChangeColor(int _numberID)
    {
        if(_numberID == 0)
        {
            SkinTypeIndex = GameManager.Instance.canonDiceGame.skinIndex;
            GameManager.Instance.Vibration();
        }

        NumberID = _numberID;
        gameObject.layer = (9 + NumberID);
        SetSkinType(SkinTypeIndex, false);
        Hit_Effect(true);
        AddCubeToPlayer();
    }

    public void ChangeColorStart(int _numberID,int _skinTypeIndex)
    {
        if (isHardColor)
        {
            _numberID = NumberID;
        }

        if (isPixelCube)
        {
            _numberID = 1;
            _skinTypeIndex = 1;
        }

        SkinTypeIndex = _skinTypeIndex;
        if(NumberID == -1) NumberID = _numberID;
        gameObject.layer = (9 + NumberID);
        SetSkinType(SkinTypeIndex,true);
        AddCubeToPlayer();
    }

    public void ChangeColorWin()
    {
        NumberID = 0;
        rend.transform.localScale = Vector3.one * 1.0f + Vector3.up * 3.0f;
        gameObject.layer = (9 + NumberID);
        rend.material = GameManager.Instance.canonDiceGame.m_Cube[NumberID];
        meshFilter.mesh = GameManager.Instance.canonDiceGame.cubeMesh;
    }
    public void SetDefaultType(int _index)
    {
        DefaultTypeIndex = _index;

        if (NumberID != -1) return;

        if (DefaultTypeIndex == 0)
        {
            SetDefaultType(DefaultType.none);
        }
        else if (DefaultTypeIndex == 1)
        {
            SetDefaultType(DefaultType.stone_0);
        }
        else if (DefaultTypeIndex == 2)
        {
            SetDefaultType(DefaultType.stone_1);
        }
        else if (DefaultTypeIndex == 3)
        {
            SetDefaultType(DefaultType.stone_2);
        }
        else if (DefaultTypeIndex == 4)
        {
            SetDefaultType(DefaultType.stone_3);
        }
        else if (DefaultTypeIndex == 5)
        {
            SetDefaultType(DefaultType.stone_4);
        }
    }

    public void SetSkinType(int _skinIndex,bool _isStart)
    {
        if (isPixelCube)
        {
            ChangeColorPixelCube();
            return;
        }

        if (NumberID != 0 || GameManager.Instance.canonDiceGame.isBonusLevel) _skinIndex = 0;

        SkinTypeIndex = _skinIndex;
        if (NumberID == -1 || isLineBang) return;

        if (SkinTypeIndex == 0 || _isStart)
        {
            SetSkinType(SkinType.none);
        }
        else if(SkinTypeIndex == 1)
        {
            SetSkinType(SkinType.face1);
        }
        else if (SkinTypeIndex == 2)
        {
            SetSkinType(SkinType.face2);
        }
        else if (SkinTypeIndex == 3)
        {
            SetSkinType(SkinType.face3);
        }
        else if (SkinTypeIndex == 4)
        {
            SetSkinType(SkinType.face4);
        }
        else if (SkinTypeIndex == 5)
        {
            SetSkinType(SkinType.face5);
        }
        else if (SkinTypeIndex == 6)
        {
            SetSkinType(SkinType.face6);
        }
        else if (SkinTypeIndex == 7)
        {
            SetSkinType(SkinType.face7);
        }
        else if (SkinTypeIndex == 8)
        {
            SetSkinType(SkinType.face8);
        }
        else if (SkinTypeIndex == 9)
        {
            SetSkinType(SkinType.face9);
        }
        else if (SkinTypeIndex == 10)
        {
            SetSkinType(SkinType.face10);
        }
        else if (SkinTypeIndex == 11)
        {
            SetSkinType(SkinType.face11);
        }
        else if (SkinTypeIndex == 12)
        {
            SetSkinType(SkinType.face12);
        }
        else if (SkinTypeIndex == 13)
        {
            SetSkinType(SkinType.face13);
        }
    }

    public void InitID(int _skinID)
    {
        SkinTypeID = _skinID;
        SetSkinType(SkinTypeIndex, false);
    }

    public void SetDefaultType(DefaultType _defaultType)
    {
        defaultType = _defaultType;
        CanonDiceGame.MaterialSkinCube materialArray = null;

        int n = 0;
        if (defaultType == DefaultType.none)
        {
            meshFilter.mesh = GameManager.Instance.canonDiceGame.cubeMesh;
            rend.material = GameManager.Instance.canonDiceGame.m_Cube[4];
        }
        else
        {
            meshFilter.mesh = GameManager.Instance.canonDiceGame.cubeSkinMesh;
            string _a = defaultType.ToString();
            string[] _b = _a.Split('_');
            n = int.Parse(_b[1]);

            materialArray = GameManager.Instance.canonDiceGame.cubeRock[n];
            meshFilter.mesh = GameManager.Instance.canonDiceGame.cubeSkinMesh;
            Material _m = materialArray._mArray[SkinTypeID];
            rend.material = _m;
        }
        rendOutline.gameObject.SetActive(true);
    }

    private void SetSkinType(SkinType _skinType)
    {
        skinType = _skinType;

        meshFilter.mesh = GameManager.Instance.canonDiceGame.cubeSkinMesh;
        int n = NumberID;
        if (n == -1) n = 0;
        CanonDiceGame.MaterialSkinCube materialArray = GameManager.Instance.canonDiceGame.cubeFace[n];
        Texture[] textureArray = null;

        if (_skinType == SkinType.none)
        {
            rend.material = GameManager.Instance.canonDiceGame.m_Cube[NumberID];
            meshFilter.mesh = GameManager.Instance.canonDiceGame.cubeMesh;
            return;
        }
        else if (_skinType == SkinType.face1)
        {
            textureArray = GameManager.Instance.canonDiceGame.cubeFace1[n]._textureArray;
        }
        else if (_skinType == SkinType.face2)
        {
            textureArray = GameManager.Instance.canonDiceGame.cubeFace2[n]._textureArray;
        }
        else if (_skinType == SkinType.face3)
        {
            textureArray = GameManager.Instance.canonDiceGame.cubeFace3[n]._textureArray;
        }
        else if (_skinType == SkinType.face4)
        {
            textureArray = GameManager.Instance.canonDiceGame.cubeFace4[n]._textureArray;
        }
        else if (_skinType == SkinType.face5)
        {
            textureArray = GameManager.Instance.canonDiceGame.cubeFace5[n]._textureArray;
        }
        else if (_skinType == SkinType.face6)
        {
            textureArray = GameManager.Instance.canonDiceGame.cubeFace6[n]._textureArray;
        }
        else if (_skinType == SkinType.face7)
        {
            textureArray = GameManager.Instance.canonDiceGame.cubeFace7[n]._textureArray;
        }
        else if (_skinType == SkinType.face8)
        {
            textureArray = GameManager.Instance.canonDiceGame.cubeFace8[n]._textureArray;
        }
        else if (_skinType == SkinType.face9)
        {
            textureArray = GameManager.Instance.canonDiceGame.cubeFace9[n]._textureArray;
        }
        else if (_skinType == SkinType.face10)
        {
            textureArray = GameManager.Instance.canonDiceGame.cubeFace10[n]._textureArray;
        }
        else if (_skinType == SkinType.face11)
        {
            textureArray = GameManager.Instance.canonDiceGame.cubeFace11[n]._textureArray;
        }
        else if (_skinType == SkinType.face12)
        {
            textureArray = GameManager.Instance.canonDiceGame.cubeFace12[n]._textureArray;
        }
        else if (_skinType == SkinType.face13)
        {
            textureArray = GameManager.Instance.canonDiceGame.cubeFace13[n]._textureArray;
        }

        Material _m = materialArray._mArray[SkinTypeID];
        rend.material = _m;
        _m.SetTexture("_MainTex", textureArray[SkinTypeID]);
    }

    public void ChangeColorPixelCube()
    {
        if (NumberID == 0)
        {
            rend.material = m_pixel;
            rend.GetPropertyBlock(mpb);
            mpb.SetColor("_Color", colorPixel);
            rend.SetPropertyBlock(mpb);
            isTruePixel = true;
            GameManager.Instance.canonDiceGame.CheckWinLevelBonus();
        }
        else
        {
            rend.material = m_nopixel;
            rend.GetPropertyBlock(mpb);
            mpb.Clear();
            rend.SetPropertyBlock(mpb);
            isTruePixel = false;
        }
    }

    public void End_Effect()
    {
        StopAllCoroutines();
        StartCoroutine(C_End_Effect());
    }

    private IEnumerator C_End_Effect()
    {
        //Vector3 pos = startPosition;
        //float cY = transform.localScale.x;
        //float tY = cY * 1.4f;
        //float t = 0.0f;
        //while (t < 1.0f)
        //{
        //    t += Time.deltaTime * 1.0f;
        //    float t2 = GameManager.Instance.animCurve4.Evaluate(t);
        //    float y2 = Mathf.Lerp(cY, tY, t2);
        //    pos.y = y2;
        //    transform.position = pos;
        //    transform.localScale = Vector3.one * y2;
        //    yield return null;
        //}
        rendOutline.material = GameManager.Instance.canonDiceGame.m_CubeOutline[1];
        ChangeColorWin();
        transform.DOScale(0.0f, 0.6f).SetEase(Ease.InSine);
        yield return new WaitForSeconds(1.0f);
        Hide();

    }

    public void Active_Effect()
    {
        StartCoroutine(C_Active_Effect());
    }

    private IEnumerator C_Active_Effect()
    {
        rendOutline.gameObject.SetActive(false);

        float cY = 0.0f;
        float tY = transform.localScale.x;
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * 2.4f;
            float t2 = GameManager.Instance.animCurve2.Evaluate(t);
            float y2 = Mathf.Lerp(cY, tY, t2);
            transform.localScale = Vector3.one * y2;
            yield return null;
        }

        rendOutline.gameObject.SetActive(true);
    }

    public void Hit_Effect(bool _isMain)
    {
        if (isHitAnim) return;
        StartCoroutine(C_Hit_Effect(_isMain));
    }

    private IEnumerator C_Hit_Effect(bool _isMain)
    {
        StartCoroutine(C_ExtraEffect(_isMain));

        isHitAnim = true;
        Transform rendTransform = rend.transform;
        Vector3 pos = rendTransform.position;
        float cY = pos.y;
        float tY = cY + 2.0f;
        float t = 0.0f;
        while(t < 1.0f)
        {
            t += Time.deltaTime * 2.4f;
            float t2 = GameManager.Instance.animCurve.Evaluate(t);
            float y2 = Mathf.Lerp(cY, tY, t2);
            pos.y = y2;
            rendTransform.position = pos;
            yield return null;
        }
        isHitAnim = false;

    }

    public IEnumerator C_ExtraEffect(bool _isMain)
    {
        if (_isMain == false) yield break;
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(0.04f);

            Vector3 dir = Vector3.zero;

            if (i == 0)
            {
                dir = Vector3.forward;
            }
            else if (i == 1)
            {
                dir = Vector3.back;
            }
            else if (i == 2)
            {
                dir = Vector3.left;
            }
            else if (i == 3)
            {
                dir = Vector3.right;
            }

            Vector3 origin = transform.position;
            Ray ray = new Ray(origin, dir);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1.0f))
            {
                if (hit.collider.gameObject.CompareTag("Cube"))
                {
                    Cube _cube = hit.collider.gameObject.GetComponent<Cube>();
                    _cube.Hit_Effect(false);
                }
            }
        }
    }

    public void Test_ChangeMesh(Mesh _mesh,Material _mat)
    {
        meshFilter.mesh = _mesh;
        rend.material = _mat;
    }
}
