using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PixelArt : MonoBehaviour
{
    public bool isComplete;
    public Human human;
    public Texture2D[] pixelArtArray;
    public Transform art;
    public List<Pixel> listPixel = new List<Pixel>();
    public Transform offsetCamera;
    public Transform bg;
    public Storage storage;
    public Transform USAmap;
    public Transform joystick;
    public Transform waveTrigger;
    public GameObject pixelUI;
    public GameObject pixelInGameUI;
    public Text coinEarnText;
    public Text stackText;
    public Text percentText;
    public Image percentImg;
    public Vector3 limitPosition;
    public int currentPixel;
    public int numberStackStorage;
    private Texture2D pixelArt;

    private int CurrentPixelLevel
    {
        get
        {
            return PlayerPrefs.GetInt("CurrentPixelLevel");
        }
        set
        {
            PlayerPrefs.SetInt("CurrentPixelLevel",value);
        }
    }

    private string DataPixel
    {
        get
        {
            return PlayerPrefs.GetString("DataPixel");
        }
        set
        {
            PlayerPrefs.SetString("DataPixel",value);
        }
    }

    private void Start()
    {
        SetActiveAmbiance(false);
        human.gameObject.SetActive(false);
    }

    public void Show_PixelUI()
    {
        currentPixel = 0;
        percentText.text = "0%";
        percentImg.fillAmount = 0;

        UIManager.Instance.UpdateHalfCoin();
        pixelUI.SetActive(true);
        coinEarnText.text = UIManager.Instance.halfCoin.ToString();

        GeneratePixelArt();
        LoadDataPixel();
        SetNumberStackStorage();
        stackText.text = numberStackStorage.ToString();
        isComplete = false;
    }

    public void SetNumberStackStorage()
    {
        int n = listPixel.Count;
        numberStackStorage = n / 2;
        if (listPixel.Count % 2 != 0 && DataPixel == "")
        {
            numberStackStorage += 1;
        }
        //    if (GameManager.Instance.levelGame == 0) numberStackStorage = listPixel.Count;
    }

    public void StartPixelArtGamePlay()
    {
        offsetCamera.transform.eulerAngles = Vector3.zero;
        DataManager.Instance.Coin += int.Parse(coinEarnText.text);
        pixelUI.SetActive(false);
        pixelInGameUI.SetActive(true);
        SetHuman();
        SetCamera();
        SetActiveAmbiance(true);
        SetStorage();
    }

    public void LoadDataPixel()
    {
        if (DataPixel == "") return;
        string[] _data = DataPixel.Split('-');
        for(int i = 0; i < listPixel.Count; i++)
        {
            if (_data[i] == "1") listPixel[i].AutoFill();
        }
    }

    public void UpdatePercent()
    {
        float t = (float)currentPixel / (float)listPixel.Count;
        float t2 = t * 100.0f;
        percentText.text = Mathf.Round(t2 * 100.0f)/100.0f  + "%";
        percentImg.fillAmount = t;
    }

    public void UpdatePixelIndex()
    {
        currentPixel++;
        UpdatePercent();
    }

    public void CheckWin()
    {
        if (storage.Number == 0)
        {
            if (human.listStack.Count == 0 && isComplete == false)
            {
                StartCoroutine(C_CompletePixelArt());
            }
        }
    }

    private IEnumerator C_CompletePixelArt()
    {
        isComplete = true;
        string _data = "";

        storage.gameObject.SetActive(false);
        human.gameObject.SetActive(false);
        offsetCamera.transform.DOMove(art.transform.position, 1.0f).SetEase(Ease.InOutSine);
        Camera.main.transform.DOLocalMove(new Vector3(0.0f, 20.0f, -2.0f), 1.0f).SetEase(Ease.InOutSine);
        Camera.main.transform.DOLocalRotate(Vector3.right * 80.0f, 1.0f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(1.0f);

        if (currentPixel >= listPixel.Count)
        {
            CurrentPixelLevel++;
            Debug.Log("COMPLETE PIXEL ART");
            Vector3 aPos = listPixel[0].transform.position;
            aPos.x = -limitPosition.x;
            aPos.z = -limitPosition.z;
            waveTrigger.position = aPos;
            Vector3 bPos = aPos;
            bPos.x = limitPosition.x;
            bPos.z = limitPosition.z;
            waveTrigger.transform.rotation = Quaternion.LookRotation(bPos - aPos);
            float time = 0.002f * (pixelArt.height * pixelArt.width);
            waveTrigger.DOMove(bPos, time).SetEase(Ease.Linear);
            waveTrigger.gameObject.SetActive(true);
            yield return new WaitForSeconds(time);
        }
        else
        {
            
            for(int i = 0; i < listPixel.Count; i++)
            {
                _data += (listPixel[i].isFilled) ? "1" : "0";

                if (i < listPixel.Count - 1) _data += "-";
            }
        }

        DataPixel = _data;

        Debug.Log("COMPLETE LEVEL");
        waveTrigger.gameObject.SetActive(false);
        joystick.gameObject.SetActive(false);
        pixelUI.SetActive(false);
        pixelInGameUI.SetActive(false);

        GameManager.Instance.Complete();
    }

    public void HidePixelArt()
    {
        SetActiveAmbiance(false);
        PoolPixel.Instance.HideAll();
        PoolStack.Instance.HideAll();
    }
    
    public void SetStorage()
    {
        storage.UpdateNumber(numberStackStorage);
    }

    public void SetActiveAmbiance(bool _isActive)
    {
        bg.gameObject.SetActive(_isActive);
        joystick.gameObject.SetActive(_isActive);
        storage.gameObject.SetActive(_isActive);
        waveTrigger.gameObject.SetActive(false);
    }

    public void SetCamera()
    {
        Camera.main.transform.localPosition = new Vector3(0.0f, 8.0f, -2.0f);
        Camera.main.transform.localEulerAngles = new Vector3(70.0f, 0.0f, 0.0f);
        offsetCamera.position = human.transform.position;
    }

    public void SetHuman()
    {
        Vector3 pos = Vector3.zero;
        pos.z = listPixel[0].transform.position.z - 2.0f;
        human.Active(pos, limitPosition);

        pos.z -= 2.0f;
        storage.transform.position = pos;
    }

    public void GeneratePixelArt()
    {
        listPixel.Clear();
        art.transform.position = Vector3.zero;

        int n = CurrentPixelLevel;
        if (n >= pixelArtArray.Length) n = Random.Range(0, pixelArtArray.Length);
        pixelArt = pixelArtArray[n];

        float w = pixelArt.width;
        float h = pixelArt.height;
        float r = 12.0f / w;
        float fixedX = (w - 1) * r / 2.0f;
        float fixedY = (h - 1) * r / 2.0f;
        float constScaleValue = 1.0f;

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                Color _color = pixelArt.GetPixel(x, y);
                if (_color.a != 0)
                {
                    Vector3 _position = new Vector3(x, 0, y) * r;
                    _position.x -= fixedX;
                    _position.z -= fixedY;
                    Vector3 _scale = Vector3.one * r * constScaleValue;
                    Pixel _pixel = PoolPixel.Instance.GetPixelInPool();
                    _pixel.Active(_position, _scale, _color, false);
                    _pixel.transform.SetParent(art);
                    listPixel.Add(_pixel);
                }

            }
        }

        art.transform.position = Vector3.up * listPixel[0].transform.localScale.y * -0.48f;
        limitPosition = new Vector3(fixedX, 0.0f, fixedY);
    }
}