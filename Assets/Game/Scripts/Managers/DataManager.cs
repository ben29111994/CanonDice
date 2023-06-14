using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public GameObject[] lockImage;
    public GameObject[] popupImage;

    [Header("PowerUp")]
    public Text descriptionPowerUpText;
    public Text descriptionPowerUpText_2;
    public Text coinPowerUpText;
    public Animator powerHand;
    public int coinPowerUp;

    public int LevelPowerUp
    {
        get
        {
            return PlayerPrefs.GetInt("LevelPowerUp");
        }
        set
        {
            PlayerPrefs.SetInt("LevelPowerUp", value);
            descriptionPowerUpText.text = "Lv" + (value + 1);
            descriptionPowerUpText_2.text = "Lv" + (value + 1);
            //descriptionPowerUpText_2.text = (10 + value * 2) + "/s";
            coinPowerUp = 200 + value * 100;
            coinPowerUpText.text = CoinFixedText(coinPowerUp);
        }
    }

    [Header("SpeedUp")]
    public Text descriptionSpeedUpText;
    public Text descriptionSpeedUpText_2;
    public Text coinSpeedUpText;
    public Animator speedHand;
    public int coinSpeedUp;

    public int LevelSpeedUp
    {
        get
        {
            return PlayerPrefs.GetInt("LevelSpeedUp");
        }
        set
        {
            PlayerPrefs.SetInt("LevelSpeedUp", value);
            descriptionSpeedUpText.text = "Lv" + (value + 1);
            descriptionSpeedUpText_2.text = "+" + (20 + value * 20);
            coinSpeedUp = 200 + value * 200;
            coinSpeedUpText.text = CoinFixedText(coinSpeedUp);
        }
    }

    [Header("OfflineEarning")]
    public Text descriptionOfflineText;
    public Text descriptionOfflineText_2;
    public Text coinOfflineText;
    public Animator offlineHand;
    public int coinOffline;

    public int LevelOffline
    {
        get
        {
            return PlayerPrefs.GetInt("LevelOffline");
        }
        set
        {
            PlayerPrefs.SetInt("LevelOffline", value);
            int c = 2 + (value);
            descriptionOfflineText.text = "Lv" + (value + 1);
            int d = Mathf.Clamp((value + 1), 0, 8);
            descriptionOfflineText_2.text = d.ToString();
            // levelSpeedUpText.text = "LEVEL " + (value + 1);
            coinOffline = 200 + value * 50;

            coinOfflineText.text = CoinFixedText(coinOffline);
        }
    }

    public Text coinText;
    public Text coinText2;

    public int Coin
    {
        get
        {
            return PlayerPrefs.GetInt("Coin");
        }
        set
        {
            PlayerPrefs.SetInt("Coin", value);
            coinText.text = coinText2.text = CoinFixedText2(value);
       
        }
    }

    public string CoinFixedText(int number)
    {
        if (number < 1000)
        {
            return number.ToString();
        }
        else
        {
            int a = number / 1000;
            int b = number % 1000;
            int c = b / 100;

            if (c == 0)
            {
                return a + "K";
            }
            else
            {
                return a + "." + c + "K";
            }
        }
    }

    public string CoinFixedText2(int number)
    {
        if (number < 1000)
        {
            return number.ToString();
        }
        else
        {
            int a = number / 1000;
            int b = number % 1000;
            int c = b / 10;

            if (c == 0)
            {
                return a + "K";
            }
            else
            {
                return a + "." + c + "K";
            }
        }
    }

    public int LevelBoss
    {
        get
        {
            return PlayerPrefs.GetInt("LevelBoss");
        }
        set
        {
            PlayerPrefs.SetInt("LevelBoss",value);
        }
    }

    public int BossModel
    {
        get
        {
            return PlayerPrefs.GetInt("BossModel");
        }
        set
        {
            PlayerPrefs.SetInt("BossModel", value);
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Coin += 0;
        LevelPowerUp += 0;
        LevelSpeedUp += 0;
        LevelOffline += 0;
    }

    private void Update()
    {
        LockUpdate();
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Coin += 999;
        }
    }

    private void LockUpdate()
    {
        if (DataManager.Instance.Coin < DataManager.Instance.coinPowerUp)
        {
            lockImage[0].SetActive(true);
            popupImage[0].SetActive(false);
        }
        else
        {
            lockImage[0].SetActive(false);
            popupImage[0].SetActive(true);
        }

        if (DataManager.Instance.Coin < DataManager.Instance.coinSpeedUp)
        {
            lockImage[1].SetActive(true);
            popupImage[1].SetActive(false);
        }
        else
        {
            lockImage[1].SetActive(false);
            popupImage[1].SetActive(true);
        }

        if (DataManager.Instance.Coin < DataManager.Instance.coinOffline)
        {
            lockImage[2].SetActive(true);
            popupImage[2].SetActive(false);
        }
        else
        {
            lockImage[2].SetActive(false);
            popupImage[2].SetActive(true);
        }
    }


    public void OnClick_UpgradePowerUp()
    {
        if (DataManager.Instance.Coin < DataManager.Instance.coinPowerUp) return;

        DataManager.Instance.Coin -= DataManager.Instance.coinPowerUp;
        DataManager.Instance.LevelPowerUp++;
        lockImage[0].transform.parent.GetComponent<Animator>().SetTrigger("Bubble");
        powerHand.SetTrigger("Touch");

        if(GameManager.Instance.canonDiceGame.listPlayer.Count > 0)
        {
            if(GameManager.Instance.canonDiceGame.listPlayer[0] != null)
            {
                GameManager.Instance.canonDiceGame.listPlayer[0].OneFaceNumber = Mathf.Clamp(DataManager.Instance.LevelPowerUp, 0, 24);
            }
            else if (GameManager.Instance.canonDiceGame.listPlayer[1] != null)
            {
                GameManager.Instance.canonDiceGame.listPlayer[1].UpdateOneFaceNumberBot(LevelPowerUp);
            }
            else if (GameManager.Instance.canonDiceGame.listPlayer[2] != null)
            {
                GameManager.Instance.canonDiceGame.listPlayer[2].UpdateOneFaceNumberBot(LevelPowerUp);
            }
            else if (GameManager.Instance.canonDiceGame.listPlayer[3] != null)
            {
                GameManager.Instance.canonDiceGame.listPlayer[3].UpdateOneFaceNumberBot(LevelPowerUp);
            }
        }
    }

    public void OnClick_UpgradeSpeedUp()
    {
        if (DataManager.Instance.Coin < DataManager.Instance.coinSpeedUp) return;

        DataManager.Instance.Coin -= DataManager.Instance.coinSpeedUp;
        DataManager.Instance.LevelSpeedUp++;
        lockImage[1].transform.parent.GetComponent<Animator>().SetTrigger("Bubble");
        speedHand.SetTrigger("Touch");
    }

    public void OnClick_UpgradeOffline()
    {
        if (DataManager.Instance.Coin < DataManager.Instance.coinOffline) return;

        DataManager.Instance.Coin -= DataManager.Instance.coinOffline;
        DataManager.Instance.LevelOffline++;

        lockImage[2].transform.parent.GetComponent<Animator>().SetTrigger("Bubble");
        offlineHand.SetTrigger("Touch");
    }
}
