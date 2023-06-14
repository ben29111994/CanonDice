using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompleteUI : MonoBehaviour
{
    public UI UI_Turret;
    public Sprite[] strokeArray;
    public Sprite[] iconArray;
    public Text percentText;
    public Image strokeImg;
    public Image iconImg;

    public int CurrentUnlock
    {
        get
        {
            return PlayerPrefs.GetInt("CurrentUnlock");
        }
        set
        {
            PlayerPrefs.SetInt("CurrentUnlock", value);
        }
    }

    public int PercentUnlock
    {
        get
        {
            return PlayerPrefs.GetInt("PercentUnlock");
        }
        set
        {
            PlayerPrefs.SetInt("PercentUnlock", value);
        }
    }

    public void OnEnable()
    {
        StartCoroutine(C_OnEnable());
    }

    private IEnumerator C_OnEnable()
    {
        if (CurrentUnlock >= iconArray.Length)
        {
            iconImg.gameObject.SetActive(false);
            strokeImg.gameObject.SetActive(false);
            percentText.gameObject.SetActive(false);
            // unlock all already
            yield break;
        }

        int currentPercent = PercentUnlock;
        int targetPercent = currentPercent + 20;
        float t1 = (float)currentPercent / 100.0f;
        float t2 = (float)targetPercent / 100.0f;

        iconImg.sprite = iconArray[CurrentUnlock];
        strokeImg.sprite = strokeArray[CurrentUnlock];

        iconImg.fillAmount = t1;
        percentText.text = t1.ToString() + "%";
       
        float t = 0.0f;
        while(t < 1.0f)
        {
            t += Time.deltaTime;

            float t3 = Mathf.Lerp(t1, t2, t);
            iconImg.fillAmount = t3;
            percentText.text = (Mathf.Round((t3 * 100) * 10) / 10).ToString() + "%";

            yield return null;
        }

        PercentUnlock = targetPercent;
        if (PercentUnlock == 100)
        {
            PercentUnlock = 0;
            CurrentUnlock++;

            if (CurrentUnlock == 1)
            {
                UI_Turret.SetIngredient(2);
                UI_Turret.SelectSkinPlayer(2);
            }
            else if (CurrentUnlock == 2)
            {
                UI_Turret.SetIngredient(3);
                UI_Turret.SelectSkinPlayer(3);
            }
            else if (CurrentUnlock == 3)
            {
                UI_Turret.SetIngredient(5);
                UI_Turret.SelectSkinPlayer(5);
            }
            else if (CurrentUnlock == 4)
            {
                UI_Turret.SetIngredient(6);
                UI_Turret.SelectSkinPlayer(6);
            }
            else if (CurrentUnlock == 5)
            {
                UI_Turret.SetIngredient(8);
                UI_Turret.SelectSkinPlayer(8);
            }
            else if (CurrentUnlock == 6)
            {
                UI_Turret.SetIngredient(9);
                UI_Turret.SelectSkinPlayer(9);
            }
        }
            
       
    }
}
