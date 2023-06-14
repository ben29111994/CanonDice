using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class CanonDiceGame : MonoBehaviour
{
    public Vector3 pivotPosition;
    public Vector3 pivotNoneFix;

    [Header("Status")]
    public bool isAim;
    public bool isBooster;
    public float ScaleRatio;
    public int defaultSkinIndex;
    public int skinIndex;
    public Vector2 Frame;
    public bool isMap90;
    public bool isUSAmap;
    public bool isBitmapLevel;
    public bool isWorldBitmap;
    public bool isBonusLevel;
    public bool isClearWorldBitMap;
    public bool is3player;
    public int TurretIndex;

    [Header("Input")]
    public bool isWaitPhaseShoot;
    public TypeMap typeMap;
    public PhaseGame phaseGame;
    public PhaseBooster phaseBooster;

    [Header("Level")]
    public Country currentCountry;
    public Country[] country;

    public int CountryIndex
    {
        get
        {
            return PlayerPrefs.GetInt("CountryIndex");
        }
        set
        {
            PlayerPrefs.SetInt("CountryIndex", value);
        }
    }

    [Header("Bitmap")]
    public bool isGenerating;
    public Transform bitmapParent;
    public CountryBitMap[] bitmap_nonwo;
    public WorldBitMap[] worldBitMapArray;
    public CountryBitMap[] bitmap_bonus;

    [Header("References")]
    public PixelArt pixelArt;
    public Transform Hand;
    public CubeSkinTrigger cubeSkinTrigger;
    public WaveObject waveObject;
    public GameObject mapObject;
    public Light spotLight;
    public Text killText;
    public Animator killAnim;
    public PlusMap plusMap;
    public Transform offsetCamera;
    public List<Player> listPlayer;
    public List<Player> mainListPlayer;
    public GameObject[] boardArray;
    public Renderer[] bgRendArray;
    public GameObject[] planeArray;
    public Renderer bangRenderer;
    public Renderer vienBangRenderer;
    public NumberOnFloor[] numberOnFloorArray;
    public Cube cubePrefab;
    public CountryBitMap bitmap123;
    public GameObject bgLevelBonus;

    [Header("Material")]
    public Material[] m_bang;
    public Material[] m_vienbang;
    public Material[] m_planeArray;
    public Material[] m_Cube;
    public Material[] m_Bullet;
    public Material[] m_BulletTrail;
    public Material[] m_country;
    public Material[] m_Turret;
    public Material[] m_Trajectory;
    public Material[] m_CubeOutline;
    public Material[] m_lazer;
    public Color[] colorPlayers;
    public Color[] colorFrameBullet;
    public Color[] colorWorldCountryBitMap;

    [Header("Texture")]
    public Texture[] m_vangDice;
    public Texture[] m_xanhDice;
    public Texture[] m_doDice;
    public Texture[] m_laDice;

    [Header("Mesh")]
    public Mesh cubeMesh;
    public Mesh cubeSkinMesh;
    public Mesh[] cubeStoneMesh;

    [Header("Material Skin")]
    public List<MaterialSkinCube> cubeRock = new List<MaterialSkinCube>();
    public List<MaterialSkinCube> cubeCash = new List<MaterialSkinCube>();
    public List<MaterialSkinCube> cubeDiamond = new List<MaterialSkinCube>();
    public List<MaterialSkinCube> cubeBitcoin = new List<MaterialSkinCube>();
    public List<MaterialSkinCube> cubeGold = new List<MaterialSkinCube>();
    public List<MaterialSkinCube> cubeFace = new List<MaterialSkinCube>();
    public List<TextureSkinCube> cubeFace1 = new List<TextureSkinCube>();
    public List<TextureSkinCube> cubeFace2 = new List<TextureSkinCube>();
    public List<TextureSkinCube> cubeFace3 = new List<TextureSkinCube>();
    public List<TextureSkinCube> cubeFace4 = new List<TextureSkinCube>();
    public List<TextureSkinCube> cubeFace5 = new List<TextureSkinCube>();
    public List<TextureSkinCube> cubeFace6 = new List<TextureSkinCube>();
    public List<TextureSkinCube> cubeFace7 = new List<TextureSkinCube>();
    public List<TextureSkinCube> cubeFace8 = new List<TextureSkinCube>();
    public List<TextureSkinCube> cubeFace9 = new List<TextureSkinCube>();
    public List<TextureSkinCube> cubeFace10 = new List<TextureSkinCube>();
    public List<TextureSkinCube> cubeFace11 = new List<TextureSkinCube>();
    public List<TextureSkinCube> cubeFace12 = new List<TextureSkinCube>();
    public List<TextureSkinCube> cubeFace13 = new List<TextureSkinCube>();

    public LayerMask layerCube;

    public List<Cube> listCube = new List<Cube>();
    private List<Cube> listCubeTruePixel = new List<Cube>();
    private List<Wall> listWall = new List<Wall>();
    private Vector3 pivotMap;
    private int BoosterStt;
    private WorldBitMap currentWorldBitmap;
    private int BitMapVuongIndex = 3;

    [System.Serializable]
    public class MaterialSkinCube
    {
        public Material[] _mArray;
    }

    [System.Serializable]
    public class TextureSkinCube
    {
        public Texture[] _textureArray;
    }

    public enum TypeMap
    {
        m_2player,
        m_4player
    }

    public enum PhaseGame
    {
        Dice,
        Shoot
    }

    public enum PhaseBooster
    {
        Empty,
        Booster
    }

    public int IsDoneLevel
    {
        get
        {
            return PlayerPrefs.GetInt("IsDoneLevel");
        }
        set
        {
            PlayerPrefs.SetInt("IsDoneLevel",value);
        }
    }
    public int WorldCountryBitMapIndex
    {
        get
        {
            return PlayerPrefs.GetInt("WorldCountryBitMapIndex");
        }
        set
        {
            PlayerPrefs.SetInt("WorldCountryBitMapIndex", value);
        }
    }
    public int CountryBitMapIndex
    {
        get
        {
            return PlayerPrefs.GetInt("CountryBitMapIndex");
        }
        set
        {
            PlayerPrefs.SetInt("CountryBitMapIndex", value);
        }
    }

    public int BitmapBonusIndex
    {
        get
        {
            return PlayerPrefs.GetInt("BitmapBonusIndex");
        }
        set
        {
            PlayerPrefs.SetInt("BitmapBonusIndex", value);
        }
    }

    private void Start()
    {
        planeArray[0].SetActive(true);
        planeArray[1].SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Win_Func();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
           Lose_Func();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(0);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            GameManager.Instance.Complete();
            SceneManager.LoadScene(0);
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
          //  Test_RotateCube90();
        }

        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    Material m_0 = m_bang[0];
        //    Material m_target = m_bang[1];
        //    m_0.SetColor("_Color", m_target.GetColor("_Color"));
        //    m_0 = m_vienbang[0];
        //    m_target = m_vienbang[1];
        //    m_0.SetColor("_Color", m_target.GetColor("_Color"));
        //    m_CubeOutline[0].SetColor("_Color", m_target.GetColor("_Color"));
        //}
        //else if (Input.GetKeyDown(KeyCode.X))
        //{
        //    Material m_0 = m_bang[0];
        //    Material m_target = m_bang[2];
        //    m_0.SetColor("_Color", m_target.GetColor("_Color"));
        //    m_0 = m_vienbang[0];
        //    m_target = m_vienbang[2];
        //    m_0.SetColor("_Color", m_target.GetColor("_Color"));
        //    m_CubeOutline[0].SetColor("_Color", m_target.GetColor("_Color"));
        //}
    }

    public void WorldBitMap_LevelUp()
    {
        Debug.Log("+++++2");

        if (isWorldBitmap == false) return;
        CanonDiceGame CDG = this;

        CDG.CountryBitMapIndex++;
        Debug.Log("+++++");
        if (CDG.CountryBitMapIndex == currentWorldBitmap.countryBitMapArray.Count)
        {
            CDG.CountryBitMapIndex = 0;
            CDG.WorldCountryBitMapIndex++;
        }
    }

    public void Delete()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }

    public void PHASE_DICE()
    {
        if (GameManager.Instance.isComplete) return;
        StartCoroutine(C_PHASE_DICE());
    }

    private IEnumerator C_PHASE_DICE()
    {
       

        // set phase game or break
        if (phaseGame == PhaseGame.Shoot)
        {
            phaseGame = PhaseGame.Dice;
        }
        else
        {
            yield break;
        }

        // set map object
        plusMap.ActiveMap();
        while (plusMap.isAciveMap == false) yield return null;

        // set player type
        pivotMap = Vector3.zero;
        for (int i = 0; i < listPlayer.Count; i++)
        {
            pivotMap += listPlayer[i].transform.position;
            listPlayer[i].SetTypePlayer(Player.TypePlayer.Dice);
        }
        pivotMap /= listPlayer.Count;

        // wait all player dice
        bool isWaitForPlayerDice = true;
        yield return null;
        while (isWaitForPlayerDice)
        {
            int d = 0;
            for (int i = 0; i < listPlayer.Count; i++)
            {
                if (listPlayer[i].IsDoneDice || listPlayer[i].isDead)
                {
                    d++;
                }
            }

            if (d == listPlayer.Count) isWaitForPlayerDice = false;
            yield return null;
        }


        // done dice => update dice number for player

        if(phaseBooster == PhaseBooster.Empty)
        {
            for (int i = 0; i < listPlayer.Count; i++)
            {
                listPlayer[i].UpdateDiceNumber();
            }
        }
       

        // change phase
        PHASE_SHOOT();

        yield return new WaitForSeconds(1.5f);
        // disable plus map
        plusMap.DisableMap();

    }

    public void PHASE_SHOOT()
    {
        if (GameManager.Instance.isComplete) return;
        StartCoroutine(C_PHASE_SHOOT());
    }

    private IEnumerator C_PHASE_SHOOT()
    {
        // set phase game or break
        if (phaseGame == PhaseGame.Dice)
        {
            phaseGame = PhaseGame.Shoot;
        }
        else
        {
            yield break;
        }

        // set player type
        for (int i = 0; i < listPlayer.Count; i++)
        {
            listPlayer[i].SetTypePlayer(Player.TypePlayer.AutoRotateShoot);
        }

        // wait all player shoot
        yield return StartCoroutine(C_WaitFor5s());
        AutoSetPhaseBooster();

        PHASE_DICE();
    }

    private IEnumerator C_WaitFor5s()
    {
        bool isWaitPlayerDoneShoot = true;
        float t = 0.0f;

        while (isWaitPlayerDoneShoot)
        {
            t += Time.deltaTime;

            int d = 0;
            for (int i = 0; i < listPlayer.Count; i++)
            {
                if (listPlayer[i].IsDoneShoot || listPlayer[i].isDead)
                {
                    d++;
                }
            }

            if (d == listPlayer.Count && t >= 1.6f) isWaitPlayerDoneShoot = false;
            if(t >= 5.0f)
            {
                isWaitPlayerDoneShoot = false;
                yield return new WaitForSeconds(2.0f);
                isWaitPlayerDoneShoot = false;
            }
            yield return null;
        }
    }

    private void Refresh()
    {
        BoosterStt = 3;
        phaseBooster = PhaseBooster.Empty;
        phaseGame = PhaseGame.Shoot;
        PoolManager.Instance.RefreshAll();

        for(int i = 0; i < mainListPlayer.Count; i++)
        {
            mainListPlayer[i].DisablePlayer();
        }

        for(int i = 0; i < listCube.Count; i++)
        {
            listCube[i].Hide();
        }

        for (int i = 0; i < listWall.Count; i++)
        {
            listWall[i].Hide();
        }

        listCube.Clear();
        listPlayer.Clear();
        listWall.Clear();

        isBooster = true;

    }
 
    public bool IsClearLevel()
    {
        return (CountryIndex >= country.Length) ? true : false;
    }

    public void OnRefresh()
    {
        // refresh all object
        StopAllCoroutines();
        Refresh();
        UpdateCountry();
        SetUpMap();
    }

    private void SetUpMap()
    {
        int levelGame = GameManager.Instance.levelGame;
        isBitmapLevel = (levelGame < bitmap_nonwo.Length) ? true : false;
        if (IsClearLevel()) isBitmapLevel = false;
        isWorldBitmap = IsClearLevel();
        isUSAmap = (isBitmapLevel == false && isWorldBitmap == false);

        bgLevelBonus.SetActive(isBonusLevel);
        if (isBonusLevel)
        {
            isBitmapLevel = isWorldBitmap = isUSAmap = false;
        }

        if (WorldCountryBitMapIndex >= worldBitMapArray.Length)
        {
            isClearWorldBitMap = true;
            isWorldBitmap = false;
            isBitmapLevel = false;
            PlayerPrefs.SetInt("ClearAllLevel", 1);
        }
        else
        {
            currentWorldBitmap = worldBitMapArray[WorldCountryBitMapIndex];
        }

        if (PlayerPrefs.GetInt("ClearAllLevel") == 1)
        {
            isBitmapLevel = true;
        }

        if (isBitmapLevel || isWorldBitmap || isBonusLevel)
        {
            pixelArt.USAmap.gameObject.SetActive(false);
        }
        else if(isUSAmap)
        {
            pixelArt.USAmap.gameObject.SetActive(true);
        }

        for (int i = 0; i < worldBitMapArray.Length; i++)
        {
            worldBitMapArray[i].SetActiveWorldCountryBitmap(false);
            worldBitMapArray[i].HideWorldMap(false);
        }

        if (isWorldBitmap)
        {
            currentWorldBitmap.SetActiveWorldCountryBitmap(true);
            currentWorldBitmap.ActiveCountryBitMap(CountryBitMapIndex);
            currentWorldBitmap.HideWorldMap(true);
        }         
    }

    public void UpdateCountry()
    {
        if (CountryIndex >= country.Length)
        {
            IsDoneLevel = 1;
            //if (PlayerPrefs.GetInt("level4") == 0)
            //{
            //    PlayerPrefs.SetInt("level4", 1);
            //    GameManager.Instance.levelGame = 3;
            //}
            //else
            //{
            //    GameManager.Instance.levelGame = 0;
            //}
            //PlayerPrefs.DeleteAll();
            //SceneManager.LoadScene(0);
        }


        // refresh world country
        for (int i = 0; i < country.Length; i++)
        {
            if (CountryIndex > i)
            {
                country[i].UpdateDone(true);
            }
            else
            {
                country[i].UpdateDone(false);
            }
        }

        // init
        if (CountryIndex < country.Length) currentCountry = country[CountryIndex];
    }

    private Transform GetOffsetCameraMap()
    {
        if (isBonusLevel)
        {
            return bitmap123.offsetCamera;
        }
        else
        {
            int lvl = (isClearWorldBitMap) ? BitMapVuongIndex : GameManager.Instance.levelGame;
            Transform _result = null;
            if (lvl < bitmap_nonwo.Length)
            {
                _result = bitmap_nonwo[lvl].offsetCamera.transform;
            }
            else
            {
                _result = currentCountry.offsetCameraTransform.transform;
            }
            return _result;
        }
    }

    private void AutoSetPosition4Player()
    {
        Vector3 pos = Vector3.zero;
        Vector3 tar = offsetCamera.transform.localPosition;
        tar.z -= 5.0f;
        float distance = 14.0f;
        for (int i = 0; i < listPlayer.Count; i++)
        {
            pos.y = listPlayer[i].transform.position.y;
            //4 player
            if (i == 0)
            {
                pos.x = tar.x - distance;
                pos.z = tar.z - distance;
            }
            else if (i == 1)
            {
                pos.x = tar.x - distance;
                pos.z = tar.z + distance;
            }
            else if (i == 2)
            {
                pos.x = tar.x + distance;
                pos.z = tar.z + distance;
            }
            else if (i == 3)
            {
                pos.x = tar.x + distance;
                pos.z = tar.z - distance;
            }
            listPlayer[i].transform.position = pos;
        }
    }
    public void StartGenerate()
    {
        StartCoroutine(C_StartGenerate());
    }

    private IEnumerator C_StartGenerate()
    {
        int levelGame = GameManager.Instance.levelGame;
        CountryBitMap _bitmap123 = null;

        // zoom camera to country
        if (isBitmapLevel || isBonusLevel)
        {
            GameManager.Instance.cameraControl.CameraZoomToCountry();
            float _timeMove = 0.0f;

            if (isBonusLevel)
            {
                int index = BitmapBonusIndex;
                if (index >= bitmap_bonus.Length) index = Random.Range(0, bitmap_bonus.Length);
                _bitmap123 = bitmap_bonus[index];
                bitmap123 = _bitmap123;

            }

            offsetCamera.transform.DOMove(GetOffsetCameraMap().localPosition, _timeMove).SetEase(Ease.InOutSine);
            offsetCamera.transform.DORotate(GetOffsetCameraMap().localEulerAngles, _timeMove).SetEase(Ease.InOutSine);
            //   yield return new WaitForSeconds(_timeMove);
            SpotLight(true);
            isMap90 = false;
            if (Camera.main.transform.parent.transform.localEulerAngles.y < 280.0f)
            {
                if (Camera.main.transform.parent.transform.localEulerAngles.y > 260.0f)
                    isMap90 = true;
            }
        }
        else if (isWorldBitmap)
        {
            currentWorldBitmap.HideWorldMap(false);
            _bitmap123 = currentWorldBitmap.currentCountryBitMap;
            offsetCamera.transform.localPosition = _bitmap123.offsetCamera.transform.localPosition;
            offsetCamera.transform.localEulerAngles = _bitmap123.offsetCamera.transform.localEulerAngles;
            GameManager.Instance.cameraControl.CameraSnapToCountry();
            SpotLight(true);
            isMap90 = false;
        }
        else
        {
            GameManager.Instance.cameraControl.CameraZoomToCountry();
            float _timeMove = (isBitmapLevel) ? 1.0f : 2.0f;
            offsetCamera.transform.DOMove(GetOffsetCameraMap().localPosition, _timeMove).SetEase(Ease.InOutSine);
            offsetCamera.transform.DORotate(GetOffsetCameraMap().localEulerAngles, _timeMove).SetEase(Ease.InOutSine);
            yield return new WaitForSeconds(_timeMove);
            SpotLight(true);
            isMap90 = false;
            if (Camera.main.transform.parent.transform.localEulerAngles.y < 280.0f)
            {
                if (Camera.main.transform.parent.transform.localEulerAngles.y > 260.0f)
                    isMap90 = true;
            }
        }
      

        // generate map from country data
        List<Vector3> cubeArray = new List<Vector3>();
        Vector3[] wallArray = new Vector3[0];

        if (isBonusLevel)
        {
            for (int i = 0; i < _bitmap123.listCube.Count; i++)
            {
                cubeArray.Add(_bitmap123.listCube[i]);
            }

            wallArray = _bitmap123.listWall.ToArray();

            // get player
            for (int i = 0; i < _bitmap123.listPlayer.Count; i++)
            {
                listPlayer.Add(_bitmap123.listPlayer[i]);
            }

            bitmap123 = _bitmap123;
        }
        else if (isBitmapLevel)
        {
            int index = (isClearWorldBitMap) ? BitMapVuongIndex : levelGame;
            _bitmap123 = bitmap_nonwo[index];
            for (int i = 0; i < _bitmap123.listCube.Count; i++)
            {
                cubeArray.Add(_bitmap123.listCube[i]);
            }

            wallArray = _bitmap123.listWall.ToArray();

            // get player
            for (int i = 0; i < _bitmap123.listPlayer.Count; i++)
            {
                listPlayer.Add(_bitmap123.listPlayer[i]);
            }

            bitmap123 = _bitmap123;
        }
        else if (isWorldBitmap)
        {
          
            for (int i = 0; i < _bitmap123.listCube.Count; i++)
            {
                cubeArray.Add(_bitmap123.listCube[i]);
            }

            wallArray = _bitmap123.listWall.ToArray();

            // get player
            for (int i = 0; i < _bitmap123.listPlayer.Count; i++)
            {
                listPlayer.Add(_bitmap123.listPlayer[i]);
            }

            AutoSetPosition4Player();
        }
        else
        {
            for (int i = 0; i < currentCountry.fixMapObject.listPositionCube.Count; i++)
            {
                cubeArray.Add(currentCountry.fixMapObject.listPositionCube[i]);
            }

            wallArray = currentCountry.fixMapObject.listPositionWall.ToArray();

            // get player
            for (int i = 0; i < currentCountry.fixMapObject.listPlayer.Count; i++)
            {
                listPlayer.Add(currentCountry.fixMapObject.listPlayer[i]);
            }
        }

        is3player = (listPlayer.Count == 3) ? true : false;

        Vector3[] posArray = cubeArray.ToArray();
        List<Vector3> cubeArray1 = new List<Vector3>();
        List<Vector3> cubeArray2 = new List<Vector3>();
        List<Vector3> cubeArray3 = new List<Vector3>();
        List<Vector3> cubeArray4 = new List<Vector3>();
        float scale = ScaleRatio = 12.0f;
        Vector3 _posA = cubeArray[0];
        for (int i = 1; i < posArray.Length; i++)
        {
            Vector3 _posB = posArray[i];
            float _a = (_posA - _posB).magnitude;
            if (_a < scale)
            {
                scale = ScaleRatio = _a;
            }
        }
        int count = cubeArray.Count;
        int half = (int)(cubeArray.Count * 0.25f);
        int c = 0;
        while (c < half)
        {
            cubeArray1.Add(cubeArray[0]);
            cubeArray.RemoveAt(0);
            c++;
        }
        c = 0;
        while (c < half)
        {
            cubeArray2.Add(cubeArray[0]);
            cubeArray.RemoveAt(0);
            c++;
        }
        c = 0;
        while (c < half)
        {
            cubeArray3.Add(cubeArray[0]);
            cubeArray.RemoveAt(0);
            c++;
        }
        c = 0;
        while (cubeArray.Count > 0)
        {
            cubeArray4.Add(cubeArray[0]);
            cubeArray.RemoveAt(0);
            c++;
        }
        cubeArray2.Reverse();
        cubeArray4.Reverse();
        bool _anim = (isBitmapLevel) ? false : true;

        if (isBonusLevel)
        {
            Vector3[] listPos = posArray;
            List<bool> listIsPixelCube = bitmap123.listIsPixelCube;
            List<Color> listColor = bitmap123.listColor;

            yield return StartCoroutine(C_GeneratePixelCube(listPos,listIsPixelCube,listColor, scale, false));
        }
        else
        {
            StartCoroutine(C_GenerateCube(cubeArray1, scale, _anim));
            StartCoroutine(C_GenerateCube(cubeArray2, scale, _anim));
            StartCoroutine(C_GenerateCube(cubeArray3, scale, _anim));
            yield return StartCoroutine(C_GenerateCube(cubeArray4, scale, _anim));
        }
      

        // tutorial
        if (GameManager.Instance.levelGame == 0)
        {
            GameManager.Instance.tutorial.SetActive(true);
        }
        else
        {
            GameManager.Instance.tutorial.SetActive(false);
        }

        // set skin
        yield return new WaitForSeconds(0.6f);
        Mathf_Skin(posArray, listCube, scale);

        // wall generate
       
        for (int i = 0; i < wallArray.Length; i++)
        {
            float _scale = (cubeArray1[1] - cubeArray1[0]).magnitude;
            Vector3 _position = wallArray[i];
            Wall _wall = PoolWall.Instance.GetCubeInPool();
            listWall.Add(_wall);
            _wall.transform.SetParent(mapObject.transform);
            _wall.GenerateWall(_position, _scale);
        }

        // active player 
        Vector3 pivot = Vector3.zero;
        for(int i = 0; i < listPlayer.Count; i++)
        {
            pivot += listPlayer[i].transform.position;
        }
        pivot /= listPlayer.Count;

        for (int i = 0; i < listPlayer.Count; i++)
        {         
            listPlayer[i].ActivePlayer(i,pivot,this);
        }

        // show upgrade ui
       //   UIManager.Instance.UpgradeUI.SetActive(false); // true
      //  GameManager.Instance.isShopping = false;// true

        // dicephase
        PHASE_DICE();
        yield return null;
    }

    private IEnumerator C_GenerateCube(List<Vector3> _listCube, float _scale,bool _isAnim)
    {
        for(int i = 0; i < _listCube.Count; i++)
        {
            Vector3 _position = _listCube[i];
            Cube _cube = PoolCube.Instance.GetCubeInPool();
            listCube.Add(_cube);
            _cube.transform.SetParent(mapObject.transform);
            _cube.GenerateCube(_position, _scale,defaultSkinIndex,skinIndex, _isAnim,false,Color.black);

            if(_isAnim && i % 10 == 0) yield return null;
        }
    }

    private IEnumerator C_GeneratePixelCube(Vector3[] _listCube,List<bool> listIsPixelCube,List<Color> listColor, float _scale, bool _isAnim)
    {
        Debug.Log(_listCube.Length + " TEST TEST");

        for (int i = 0; i < _listCube.Length; i++)
        {
            Vector3 _position = _listCube[i];
            Cube _cube = PoolCube.Instance.GetCubeInPool();
            listCube.Add(_cube);
            if (listIsPixelCube[i]) listCubeTruePixel.Add(_cube);
            _cube.transform.SetParent(mapObject.transform);
            _cube.GenerateCube(_position, _scale, defaultSkinIndex, skinIndex, _isAnim,listIsPixelCube[i],listColor[i]);
            if (_isAnim && i % 10 == 0) yield return null;
        }
    }

    public void CheckWinLevelBonus()
    {
        int count = listCubeTruePixel.Count;
        int n = 0;
        for(int i = 0; i < count; i++)
        { 
            if(listCubeTruePixel[i].isPixelCube && listCubeTruePixel[i].isTruePixel)
            {

            }
            else
            {
                return;
            }
        }

        Win_Func();
    }

    public void CompleteLevelBonus()
    {
        GameManager.Instance.Complete();
    }
    public void LevelUpLevelBonus()
    {
        if (isBonusLevel)
        {
            GameManager.Instance.IsBonusLevel = 0;
            isBonusLevel = false;
            BitmapBonusIndex++;
        }
    }

    public int GetID(int _h,int _w,int _currentH,int _currentW)
    {
        int _result = 0;

        int _h12 = (int)(_h / 2.0f) - 1;
        int _w12 = (int)(_w / 2.0f) - 1;

        if(typeMap == TypeMap.m_4player)
        {
            // trai duoi
            if (_currentH <= _h12 && _currentW <= _w12)
            {
                _result = 0;
            }
            // phai duoi
            else if (_currentH <= _h12 && _currentW > _w12)
            {
                _result = 3;
            }
            // trai tren
            else if (_currentH > _h12 && _currentW <= _w12)
            {
                _result = 1;
            }
            // phai tren
            else if (_currentH > _h12 && _currentW > _w12)
            {
                _result = 2;
            }
        }
        else if (typeMap == TypeMap.m_2player)
        {
            if(_currentH <= _h12)
            {
                _result = 0;
            }
            else
            {
                _result = 1;
            }
        }



        return _result;
    }

    public void AutoSetPhaseBooster()
    {
        if (GameManager.Instance.levelGame <= 1)
        {
            phaseBooster = PhaseBooster.Empty;
            return;
        }

        if(phaseBooster == PhaseBooster.Empty && BoosterStt == 3)
        {
            phaseBooster = PhaseBooster.Booster;
            BoosterStt = 0;
            for (int i = 0; i < listPlayer.Count; i++) listPlayer[i].ActiveNumberText(false);
        }
        else
        {
            phaseBooster = PhaseBooster.Empty;

            for (int i = 0; i < listPlayer.Count; i++) listPlayer[i].ActiveNumberText(true);
        }

        BoosterStt++;
        //     phaseBooster = PhaseBooster.Booster;
    }

    public void GenerateBullet(int _numberID, Player _player, int _stt, Bullet.TypeBullet _typeBullet,int _healthIndex)
    {
        Bullet _bullet = null;
        if (_typeBullet == Bullet.TypeBullet.Bullet)
        {
            _bullet = PoolBullet.Instance.GetBulletInPool(_numberID);
        }
        else
        {
            _bullet = PoolBulletPower.Instance.GetBulletInPool();
        }

        _bullet.ActiveBullet(_numberID, _player, _stt, _typeBullet, _healthIndex);
    }

    public void CubeBreakEffect(Player _player)
    {
        GameObject _o = PoolManager.Instance.GetObject(PoolManager.NameObject.CubeBreak) as GameObject;
        ParticleSystemRenderer _p = _o.GetComponent<ParticleSystemRenderer>();
        _p.material = _player.canonRend.material;
        Vector3 _pos = _player.transform.position;
        _pos.y = 3.0f;
        _o.transform.position = _pos;
        StartCoroutine(C_ActiveObject(_o, 3.0f));

    }

    private IEnumerator C_ActiveObject(GameObject _obj, float _time)
    {
        _obj.SetActive(true);
        yield return new WaitForSeconds(_time);
        _obj.SetActive(false);
    }

    public void ShowKillText(Player _player0,Player _player1)
    {
        killText.text = _player0.myName + " Kill " + _player1.myName;
        killAnim.SetTrigger("Active");
    }

    public void CheckWin()
    {
        int n = 0;
        for(int i = 1; i < listPlayer.Count;i++)
        {
            if (listPlayer[i].isDead) n++;
        }

        if(n == listPlayer.Count - 1)
        {
            StopAllCoroutines();
            Win_Func();
        }
    }

    public void CheckLose()
    {
        if (listPlayer[0].isDead)
        {
            StopAllCoroutines();
            Lose_Func();
        }
    }

    public void Win_Func()
    {
        StartCoroutine(C_Win_Func());
    }

    private IEnumerator C_Win_Func()
    {
        GameManager.Instance.isComplete = true;

        // hide player
        for(int i = 0; i < listPlayer.Count; i++)
        {
            listPlayer[i].DisablePlayer();
        }
        listPlayer.Clear();

        // hide bullet
        PoolBullet.Instance.HideAll();
        PoolBulletPower.Instance.HideAll();

        // hide plusobject
        plusMap.DisaleMapp_StopAction();

        if (isAim)
        {
            AimController.Instance.Hide();
        }

        // animation wave
        waveObject.Active(pivotMap);
        currentCountry.UpdateDone(true);

        while(listCube.Count > 0)
        {
            for(int i = 0; i < listCube.Count; i++)
            {
                if(listCube[i].gameObject.activeSelf == false)
                {
                    listCube.Remove(listCube[i]);
                }
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.4f);

        if (isBonusLevel)
        {
            CompleteLevelBonus();
        }
        else
        {
            PixelArt();
        }
    }

    private void PixelArt()
    {
        Refresh();
        pixelArt.Show_PixelUI();
    }

    private void Lose_Func()
    {
        StartCoroutine(C_Lose_Func());
    }

    private IEnumerator C_Lose_Func()
    {
        yield return new WaitForSeconds(0.2f);
        GameManager.Instance.Fail();
    }

    public void ClearLevel_WhenFail()
    {
        GameManager.Instance.isComplete = true;

        // hide player
        for (int i = 0; i < listPlayer.Count; i++)
        {
            listPlayer[i].DisablePlayer();
        }
        listPlayer.Clear();

        // hide bullet
        PoolBullet.Instance.HideAll();
        PoolBulletPower.Instance.HideAll();

        // hide plusobject
        plusMap.DisaleMapp_StopAction();

        // animation wave
        for (int i = 0; i < listCube.Count; i++)
        {
            listCube[i].Hide();
        }

        if (isAim)
        {
            AimController.Instance.Hide();
        }
    }

    public void ShowNumberText(int _number,Vector3 _pos)
    {
        GameObject _g = PoolManager.Instance.GetObject(PoolManager.NameObject.PlusNumberText) as GameObject;
        _g.transform.rotation = Quaternion.LookRotation(Camera.main.transform.parent.forward);
        _g.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+" + _number;
        _pos.y = _g.transform.position.y;
        _g.transform.position = _pos;
        StartCoroutine(C_ActiveObject(_g, 2.0f));
    }


    private void Mathf_Skin(Vector3[] _posArray,List<Cube> _listCube,float _scale)
    {
        float minX = 99999.0f;
        float maxX = -99999.0f;
        float minZ = 99999.0f;
        float maxZ = -99999.0f;

        int Length = _posArray.Length;
        for (int i = 0; i < Length; i++)
        {
            if (_posArray[i].x < minX) minX = _posArray[i].x;
            if (_posArray[i].x > maxX) maxX = _posArray[i].x;
            if (_posArray[i].z < minZ) minZ = _posArray[i].z;
            if (_posArray[i].z > maxZ) maxZ = _posArray[i].z;
        }

        Transform tests = null;
        Transform tests2 = null;
        tests = (isMap90) ? GetCubeWeightest(minX) : GetCubeHightest(maxZ);
        Vector3 _posCube = tests.position;
        Vector3 _dir = -Camera.main.transform.parent.forward;
        Ray ray = new Ray(_posCube, _dir);
        RaycastHit hit;
        Vector3 _targetPosition = Vector3.zero;
        Vector3 _dir2 = Vector3.zero;
        if (Physics.Raycast(ray,out hit,_scale * 5.0f, layerCube))
        {
            if(hit.collider != null)
            {
                _targetPosition = hit.collider.gameObject.transform.position;
                _dir2 = (_targetPosition - _posCube) * -1.0f;
            }
        }
        else
        {
            if (_targetPosition == Vector3.zero)
            {
                ray = new Ray(_posCube, -_dir);
                if (Physics.Raycast(ray, out hit, _scale * 5.0f, layerCube))
                {
                    if (hit.collider != null)
                    {
                        tests2 = hit.collider.gameObject.transform;
                        _targetPosition = hit.collider.gameObject.transform.position;
                        _dir2 = _targetPosition - _posCube;
                    }
                }
            }
        }

        // set ID
        Vector3 hitpoint = Vector3.zero;
        RaycastHit hit2;
        Ray ray2 = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray2, out hit2,Mathf.Infinity))
        {
            hitpoint = hit2.collider.gameObject.transform.position;
        }

        Vector3 hitpoint2 = Vector3.zero;
        for(int i = 0;i < listPlayer.Count; i++)
        {
            hitpoint2 += listPlayer[i].transform.position;
        }

        hitpoint2 /= listPlayer.Count;

        pivotNoneFix = hitpoint2;
        Vector3 pivot = Vector3.zero;
        //Debug.Log(minX + " __  " + maxX);
        //pivot.x = (isMap90) ? Mathf.Lerp(minZ, maxZ, 0.5f) : Mathf.Lerp(minX,maxX,0.5f);
        //pivot.z = (isMap90) ? Mathf.Lerp(minX, maxX, 0.5f) : Mathf.Lerp(minZ, maxZ, 0.5f);
        pivot.x = (isMap90) ? hitpoint2.z : hitpoint2.x;
        pivot.z = (isMap90) ? hitpoint2.x : hitpoint2.z;
        pivotPosition = pivot;

        Debug.Log("mahfskin " + hitpoint2);
        for (int i = 0; i < listCube.Count; i++)
        {
            int _numberID = 0;
            Cube _cube = listCube[i];
            if (listPlayer.Count == 2)
            {
                float _cubeZ = (isMap90) ? _cube.transform.position.x : _cube.transform.position.z;
                float _cubeX = (isMap90) ? _cube.transform.position.z : _cube.transform.position.x;
                // map 2 player
                if (_cubeZ < pivot.z)
                {
                    _numberID = (isMap90) ? 1 : 0;
                }
                else
                {
                    _numberID = (isMap90) ? 0 : 1;
                }
            }
            else
            {
                // map 4 player
                float _cubeZ = (isMap90) ? _cube.transform.position.x : _cube.transform.position.z;
                float _cubeX = (isMap90) ? _cube.transform.position.z : _cube.transform.position.x;

                if (_cubeX <= pivot.x && _cubeZ <= pivot.z)
                {
                    // tai vi minX => maxZ , maxX = minZ ; minZ = minX , maxZ = maxX
                    _numberID = (isMap90) ? 1 : 0;
                }
                else if (_cubeX <= pivot.x && _cubeZ > pivot.z)
                {
                    _numberID = (isMap90) ? 0 : 1;
                }
                else if (_cubeX > pivot.x && _cubeZ > pivot.z)
                {
                    _numberID = (isMap90) ? 3 : 2;
                }
                else if (_cubeX > pivot.x && _cubeZ <= pivot.z)
                {
                    _numberID = (isMap90) ? 2 : 3;
                }
            }
            listCube[i].ChangeColorStart(_numberID, skinIndex);
        }

        // set skin
        cubeSkinTrigger.Active(_scale,_posCube,_dir2);
    }

    private Transform GetCubeHightest(float _h)
    {
        Transform _t = null;
        int count = listCube.Count;

        for(int i = 0; i < count; i++)
        {
            if(listCube[i].transform.position.z == _h)
            {
                _t = listCube[i].transform;
                return _t;
            }
        }

        return _t;
    }

    private Transform GetCubeWeightest(float _w)
    {
        Transform _t = null;
        int count = listCube.Count;

        for (int i = 0; i < count; i++)
        {
            if (listCube[i].transform.position.x == _w)
            {
                _t = listCube[i].transform;
                return _t;
            }
        }

        return _t;
    }

    //private IEnumerator C_Mathf_Skin()
    //{
    //    int count = listCube.Count;

    //}
    public List<Cube> listCubeTest = new List<Cube>();
    public Transform parentMesh;
    public List<Mesh> Test_AllMesh = new List<Mesh>();
    public List<Material> Test_AllMaterial = new List<Material>();

    public int Test_MeshIndex;
    public void Test_ChangeMeshCube()
    {
        for(int i = 0; i < listCube.Count; i++)
        {
            listCube[i].Test_ChangeMesh(Test_AllMesh[Test_MeshIndex],Test_AllMaterial[Test_MeshIndex]);
        }
        for (int i = 0; i < listCubeTest.Count; i++)
        {
            listCubeTest[i].Test_ChangeMesh(Test_AllMesh[Test_MeshIndex], Test_AllMaterial[Test_MeshIndex]);
        }

        Test_MeshIndex++;
        if (Test_MeshIndex >= Test_AllMesh.Count) Test_MeshIndex = 0;
    }

    public void Test_RotateCube90()
    {
        for (int i = 0; i < listCube.Count; i++)
        {
            listCube[i].rend.transform.Rotate(Vector3.up * 90.0f);
        }

        for (int i = 0; i < listCubeTest.Count; i++)
        {
            listCubeTest[i].rend.transform.Rotate(Vector3.up * 90.0f);
        }
    }

   // [NaughtyAttributes.Button]
    public void GetAllMeshFromParentMesh()
    {
        Test_AllMesh.Clear();
        Test_AllMaterial.Clear();

        for(int i = 0; i < parentMesh.childCount; i++)
        {
            Transform child_0 = parentMesh.GetChild(i);
            for(int k = 0; k < child_0.childCount; k++)
            {
                Mesh _mesh = child_0.GetChild(k).gameObject.GetComponent<MeshFilter>().sharedMesh;
                Test_AllMesh.Add(_mesh);
                Material _material = child_0.GetChild(k).gameObject.GetComponent<MeshRenderer>().sharedMaterial;
                Test_AllMaterial.Add(_material);
            }
        }
    }


    public void SpotLight(bool _isEnable)
    {
        StartCoroutine(C_SpotLight(_isEnable));
    }
    private IEnumerator C_SpotLight(bool _isEnable)
    {
        float c = 0.0f;
        float t = 8.0f;
        float n = 0.0f;

        if(_isEnable == false)
        {
            spotLight.intensity = 0.0f;
            yield break;
        }

        while(n < 1.0f)
        {
            n += Time.deltaTime * 0.2f;
            if (_isEnable)
            {
                spotLight.intensity = Mathf.Lerp(c, t, n);
            }
            yield return null;
        }

    }

    public void UpdateBlockSkin(int _n)
    {
        skinIndex = _n;
        for (int i = 0; i < listCube.Count; i++)
        {
            listCube[i].SetSkinType(skinIndex, false);
        }

        _skinIndexText.text = skinIndex.ToString();
    }

    public Text _skinIndexText;
    public Text _defaultSkinIndexText;

    public void OnClick_NextSkin()
    {
        skinIndex++;
        if (skinIndex >= 14) skinIndex = 0;

        for(int i = 0; i < listCube.Count; i++)
        {
            listCube[i].SetSkinType(skinIndex,false);
        }

        _skinIndexText.text = skinIndex.ToString();
    }

    public void OnClick_NextDefaultSkin()
    {
        defaultSkinIndex++;
        if (defaultSkinIndex >= 6) defaultSkinIndex = 0;

        for (int i = 0; i < listCube.Count; i++)
        {
            listCube[i].SetDefaultType(defaultSkinIndex);
        }

        _defaultSkinIndexText.text = defaultSkinIndex.ToString();
    }

    public int CurrentBGIndex;
    public GameObject waterObject;
    public void OnClick_NextBackGround()
    {
        CurrentBGIndex++;

        if (CurrentBGIndex >= 3) CurrentBGIndex = 0;

        waterObject.SetActive(false);
        if (CurrentBGIndex == 0)
        {
            Material m_0 = m_bang[0];
            Material m_target = m_bang[1];
            m_0.SetColor("_Color", m_target.GetColor("_Color"));
            m_0 = m_vienbang[0];
            m_target = m_vienbang[1];
            m_0.SetColor("_Color", m_target.GetColor("_Color"));
            m_CubeOutline[0].SetColor("_Color", m_target.GetColor("_Color"));
        }
        else if (CurrentBGIndex == 1)
        {
            Material m_0 = m_bang[0];
            Material m_target = m_bang[2];
            m_0.SetColor("_Color", m_target.GetColor("_Color"));
            m_0 = m_vienbang[0];
            m_target = m_vienbang[2];
            m_0.SetColor("_Color", m_target.GetColor("_Color"));
            m_CubeOutline[0].SetColor("_Color", m_target.GetColor("_Color"));
        }
        else if (CurrentBGIndex == 2)
        {
            waterObject.SetActive(true);
        }
    }

    public void UpdateTurretPlayer(int _n)
    {
        TurretIndex = _n;

        if (listPlayer.Count > 0) listPlayer[0].UpdateTurret(_n);
    }

    // test func

}