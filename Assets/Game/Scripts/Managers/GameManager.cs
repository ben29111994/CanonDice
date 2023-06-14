using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;
using UnityEngine.SceneManagement;
using GPUInstancer;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Status Game")]
    public bool isShopping;
    public bool isFirstLoad;
    public bool isComplete;
    public bool isVibration;
    public bool isShakeCamera;
    public bool isShowInGameUI;
    public bool isLoadingMap;

    [Header("Level Manager")]
    public int levelGame;

    [Header("References")]
    public CameraControl cameraControl;
    public CanonDiceGame canonDiceGame;

    [Header("References")]
    public GameObject[] nameGame;
    public GameObject tutorial;
    public GameObject taptoplay;
    public Text levelText;
    public Text startText;

    public GPUInstancerPrefabManager GPUInstancerPM;
    public List<GPUInstancerPrefab> asteroidInstances = new List<GPUInstancerPrefab>();


    [Header("Animation Curve")]
    public AnimationCurve animCurve;
    public AnimationCurve animCurve2;
    public AnimationCurve animCurve3;
    public AnimationCurve animCurve4;

    public int LevelFakeIndexx
    {
        get
        {
            return PlayerPrefs.GetInt("LevelFakeIndexx");
        }
        set
        {
            PlayerPrefs.SetInt("LevelFakeIndexx", value);
        }
    }

    public int IsBonusLevel
    {
        get
        {
            return PlayerPrefs.GetInt("IsBonusLevel");
        }
        set
        {
            PlayerPrefs.SetInt("IsBonusLevel", value);
        }
    }


    public List<NameBot> listNameBot = new List<NameBot>();

    [System.Serializable]
    public class NameBot
    {
        public string[] nameArray;
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Instance = this;
        InitPlugin();
    }
    
    private void Start()
    {
        // PlayerPrefs.SetInt("levelGame", 4);

        //if (PlayerPrefs.GetInt("levelGame") == 0)
        //{
        //    for (int i = 0; i < 24; i++)
        //    {
        //        LevelUp();
        //    }

        //    SceneManager.LoadScene(0);
        //}

        GenerateLevel();
    }

    public void UpdateGPUList(GPUInstancerPrefab _prefab)
    {
      //  asteroidInstances.Add(_prefab);
   //     GPUInstancerAPI.AddPrefabInstance(GPUInstancerPM, _prefab);
    //    GPUInstancerAPI.RegisterPrefabInstanceList(GPUInstancerPM, asteroidInstances);
    //    GPUInstancerAPI.InitializeGPUInstancer(GPUInstancerPM);
    }
    public void OnClick_StartGame()
    {
        if (isLoadingMap) return;
        if (isShowInGameUI == false)
        {
            isShowInGameUI = true;
            UIManager.Instance.Show_InGameUI();
        }
    }

    public void InitPlugin()
    {
#if UNITY_IOS
        MMVibrationManager.iOSInitializeHaptics();
#else

#endif
    }

    public IEnumerator C_Analytics_StartEvent()
    {
        yield return new WaitForSeconds(1.0f);
     //   AnalyticsManager.instance.CallEvent(AnalyticsManager.EventType.StartEvent);
    }

    public IEnumerator C_Analytics_EndEvent()
    {
        yield return new WaitForSeconds(1.0f);
    //    AnalyticsManager.instance.CallEvent(AnalyticsManager.EventType.EndEvent);
    }

    public void Loading()
    {
        StartCoroutine(C_Loading());
    }
    private IEnumerator C_Loading()
    {
        isLoadingMap = true;
        startText.text = "LOADING";
        yield return new WaitForSeconds(1.1f);
        startText.text = "START";
        isLoadingMap = false;
    }

    public void GenerateLevel()
    {
        StartCoroutine(C_GenerateLevel());
    }

    private IEnumerator C_GenerateLevel()
    {
        // refresh
        canonDiceGame.pixelArt.HidePixelArt();
        Loading();
        isComplete = false;
        isShowInGameUI = false;
        isShopping = true;
        canonDiceGame.SpotLight(false);
        canonDiceGame.isBonusLevel = (IsBonusLevel == 1) ? true : false;
     // get level
     levelGame = PlayerPrefs.GetInt("levelGame");
        levelText.text = "Level " + (levelGame + 1 - LevelFakeIndexx);
        if(canonDiceGame.isBonusLevel)
        {
            levelText.text = "Bonus Level";
        }

        nameGame[0].SetActive(!canonDiceGame.isBonusLevel);
        nameGame[1].SetActive(canonDiceGame.isBonusLevel);



        if (canonDiceGame.IsClearLevel())
        {
            levelGame  = 0;

            if (PlayerPrefs.GetInt("level4") == 0)
            {
                PlayerPrefs.SetInt("level4", 1);
                levelGame = 3;
            }
  
        }

        // generate game
        canonDiceGame.OnRefresh();


        if (canonDiceGame.isBitmapLevel)
        {
            cameraControl.CameraMainMenuBitmap();
        }
        else if (canonDiceGame.isWorldBitmap)
        {
            cameraControl.CameraMainMenu_WorldBitMap();
    //        cameraControl.CameraMainMenuBitmap();
        }
        else if (canonDiceGame.isBonusLevel)
        {
            cameraControl.CameraMainMenuBitmap();
        }
        else
        {
            cameraControl.CameraMainMenu();
         //   yield return new WaitForSeconds(1.0f);
        }

        if (isFirstLoad == false)
        {
            isFirstLoad = true;
        }
        else
        {
            taptoplay.SetActive(true);

            if (canonDiceGame.isBitmapLevel)
            {
                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
           //     yield return new WaitForSeconds(2.0f);
            }
               
        }

        // show ui game

        if(canonDiceGame.isBitmapLevel == false) taptoplay.SetActive(true);
       
        UIManager.Instance.Show_MainMenuUI();

        // analytics
        StartCoroutine(C_Analytics_StartEvent());

        if (canonDiceGame.isBitmapLevel || canonDiceGame.isBonusLevel)
        {
            canonDiceGame.StartGenerate();
        }
    }

    private void LevelUp()  
    {
        if(canonDiceGame.isBonusLevel == false)
        {
            if (levelGame >= canonDiceGame.bitmap_nonwo.Length) canonDiceGame.CountryIndex++;
            levelGame = PlayerPrefs.GetInt("levelGame");
            levelGame++;
            levelText.text = "Level " + (levelGame + 1 - LevelFakeIndexx);
            PlayerPrefs.SetInt("levelGame", levelGame);
            if (levelGame % 3 == 0)
            {
                IsBonusLevel = 1;
                levelText.text = "Level Bonus";
            }
        }
        else
        {
           
        }




        canonDiceGame.WorldBitMap_LevelUp();

        // analytics
        StartCoroutine(C_Analytics_EndEvent());
    }



    public void Complete()
    {
        //  if (isComplete) return;
        Debug.Log("B");
        isComplete = true;
        StartCoroutine(C_Complete());
    }

    private IEnumerator C_Complete()
    {
        Debug.Log("C");
        LevelUp();
        yield return null;
        UIManager.Instance.ShowCompleteUI(canonDiceGame.isBonusLevel);
        if (canonDiceGame.isBonusLevel)
        {
            canonDiceGame.LevelUpLevelBonus();
        }
    }

    public void Fail()
    {
        isComplete = true;
        StartCoroutine(C_Fail());
    }

    private IEnumerator C_Fail()
    {
        yield return null;
        UIManager.Instance.Show_FailUI(canonDiceGame.isBonusLevel);
        canonDiceGame.LevelUpLevelBonus();
    }

    public void Vibration()
    {
        if (isVibration) return;

        StartCoroutine(C_Vibarion());
    }

    private IEnumerator C_Vibarion()
    {
//#if UNITY_IOS
//        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
//#elif UNITY_ANDROID
//      //     MMVibrationManager.Haptic(HapticTypes.LightImpact);
//#endif

        isVibration = true;
        yield return new WaitForSecondsRealtime(0.2f);
        isVibration = false;
    }
}
