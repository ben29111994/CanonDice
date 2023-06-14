using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public int halfCoin;

    [Header("References")]
    public GameObject MainMenuUI;
    public GameObject InGameUI;
    public GameObject CompleteUI;
    public GameObject CompleteLevelBonusUI;
    public GameObject FailUI;
    public GameObject FailLevelBonusUI;
    public GameObject UpgradeUI;
    public GameObject Shop;
    public GameObject Shop_Skin;
    public GameObject Shop_Block;
    public GameObject dragToAim;
    public UI[] skinblockUI;

    [Header("Complete")]
    public GameObject ButtonClaim;
    public GameObject Gem;
    public Text coinEarnText;
    public GameObject ButtonClaimLevelBonus;
    public GameObject GemClaimLevelBonus;
    public Text coinEarnTextLevelBonus;

    public RectTransform[] barPlayers;
    private float[] _targetWidth = new float[4];

    private void Start()
    {
        skinblockUI[0].OnStart();
        skinblockUI[1].OnStart();
    }

    private void Update()
    {
        UpdateBarUI(GameManager.Instance.canonDiceGame.listPlayer, GameManager.Instance.canonDiceGame.listCube.Count);
    }

    public void Show_MainMenuUI()
    {
        MainMenuUI.SetActive(true);
        InGameUI.SetActive(false);
        CompleteUI.SetActive(false);
        CompleteLevelBonusUI.SetActive(false);
        FailUI.SetActive(false);
        FailLevelBonusUI.SetActive(false);
        //  UpgradeUI.SetActive(true);
    }

    public void Disable_All_Menu()
    {
        MainMenuUI.SetActive(false);
        InGameUI.SetActive(false);
        CompleteUI.SetActive(false);
        FailUI.SetActive(false); 
        FailLevelBonusUI.SetActive(false);
        CompleteLevelBonusUI.SetActive(false);
    }
    public void Show_InGameUI()
    {
        Debug.Log("Show_InGameUI");

        if(GameManager.Instance.canonDiceGame.isBitmapLevel)
        dragToAim.SetActive(true);

        MainMenuUI.SetActive(false);
        InGameUI.SetActive(true);
        CompleteUI.SetActive(false);
        CompleteLevelBonusUI.SetActive(false);
        FailUI.SetActive(false);
        FailLevelBonusUI.SetActive(false);

        GameManager.Instance.isShopping = false;
        if (GameManager.Instance.canonDiceGame.isWorldBitmap || GameManager.Instance.canonDiceGame.isUSAmap)
            GameManager.Instance.canonDiceGame.StartGenerate();
    }

    public void ShowCompleteUI(bool _isLevelBonus)
    {
        MainMenuUI.SetActive(false);
        InGameUI.SetActive(false);
        CompleteUI.SetActive(!_isLevelBonus);
        CompleteLevelBonusUI.SetActive(_isLevelBonus);
        FailUI.SetActive(false);
        FailLevelBonusUI.SetActive(false);

        halfCoin = (int)((DataManager.Instance.coinSpeedUp + DataManager.Instance.coinPowerUp) / 2.0f) + DataManager.Instance.LevelSpeedUp * 20;
        halfCoin = (int)(halfCoin * 0.5f);
        coinEarnText.text = coinEarnTextLevelBonus.text = halfCoin.ToString();
    }

    public void UpdateHalfCoin()
    {
        halfCoin = (int)((DataManager.Instance.coinSpeedUp + DataManager.Instance.coinPowerUp) / 2.0f) + DataManager.Instance.LevelSpeedUp * 20;
        halfCoin = (int)(halfCoin * 0.5f);

        InGameUI.SetActive(false);
    }

    public void Show_FailUI(bool _isLevelBonus)
    {
        MainMenuUI.SetActive(false);
        InGameUI.SetActive(false);
        CompleteUI.SetActive(false);
        CompleteLevelBonusUI.SetActive(false);

        if (_isLevelBonus)
        {
            FailLevelBonusUI.SetActive(true);
        }
        else
        {
            FailUI.SetActive(true);
        }
    }

    public void UpdateBarUI(List<Player> _listPlayer, int _maxCube)
    {
        if (_listPlayer.Count == 0 || _maxCube == 0)
        {
            for (int i = 0; i < barPlayers.Length; i++)
            {
                barPlayers[i].sizeDelta = new Vector2(0.0f, 200.0f);
            }
            return;
        }
           

        int _count = _listPlayer.Count;
        float width = 776.0f;
        float currentWidth = 0.0f;

        for(int i = 0; i < _count; i++)
        {
            int _pCount = _listPlayer[i].listCube.Count;
            float _ratio = (float)_pCount / (float)_maxCube;
            float _myWith = width * _ratio;
            float _myWidthFixed = _myWith + currentWidth;
            _targetWidth[i] = _myWidthFixed;
            currentWidth += _myWith;
        }

        for (int i = 0; i < _count; i++)
        {
            Vector2 _sizeDelta = barPlayers[i].sizeDelta;
            _sizeDelta.x = Mathf.Lerp(_sizeDelta.x, _targetWidth[i], Time.deltaTime * 2.0f);
            barPlayers[i].sizeDelta = _sizeDelta;
        }
    }

    public void OnClick_Claim()
    {
        StartCoroutine(C_OnClick_Claim());
    }

    private IEnumerator C_OnClick_Claim()
    {
        ButtonClaim.GetComponent<Button>().enabled = false;
        ButtonClaim.transform.GetChild(0).gameObject.SetActive(false);
        Gem.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        DataManager.Instance.Coin += int.Parse(coinEarnText.text);
        yield return new WaitForSeconds(1.0f);
        ButtonClaim.GetComponent<Button>().enabled = true;
        ButtonClaim.transform.GetChild(0).gameObject.SetActive(true);
        Gem.SetActive(false);
        GameManager.Instance.GenerateLevel();
        Disable_All_Menu();
    }

    public void OnClick_ClaimLevelBonus()
    {
        StartCoroutine(C_OnClick_ClaimLevelBonus());
    }

    private IEnumerator C_OnClick_ClaimLevelBonus()
    {
        ButtonClaimLevelBonus.GetComponent<Button>().enabled = false;
        ButtonClaimLevelBonus.transform.GetChild(0).gameObject.SetActive(false);
        GemClaimLevelBonus.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        DataManager.Instance.Coin += int.Parse(coinEarnText.text);
        yield return new WaitForSeconds(1.0f);
        ButtonClaimLevelBonus.GetComponent<Button>().enabled = true;
        ButtonClaimLevelBonus.transform.GetChild(0).gameObject.SetActive(true);
        GemClaimLevelBonus.SetActive(false);
        GameManager.Instance.GenerateLevel();
        Disable_All_Menu();
    }

    public void OnClick_OpenShop()
    {
        Shop.SetActive(true);
        OnClick_OpenSkin();
    }

    public void OnClick_CloseShop()
    {
        Shop.SetActive(false);
    }

    public void OnClick_OpenSkin()
    {
        Shop_Skin.SetActive(true);
        Shop_Block.SetActive(false);
    }

    public void OnClick_OpenBlock()
    {
        Shop_Skin.SetActive(false);
        Shop_Block.SetActive(true);
    }
}
