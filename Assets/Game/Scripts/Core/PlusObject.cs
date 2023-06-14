using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlusObject : MonoBehaviour
{
    public int Number;
    public Text numberText;
    public SphereCollider sphereCollider;
    public GameObject main;
    public GameObject[] boosterObject;
    public BoosterType boosterType;
    public Player player;

    public enum BoosterType
    {
        Empty,
        Booster_0_Freezing,
        Booster_1_Lazer,
        Booster_2_Rocket,
        Booster_3_Hammer,
        Booster_4_Shield,
        Booster_5_Bomb,
        Booster_6_Dice
    }

    public void SetBooster(BoosterType _b)
    {
        for (int i = 0; i < boosterObject.Length; i++) boosterObject[i].SetActive(false);
        boosterType = _b;
        int n = (int)boosterType;
        if (n == 0) return;
        main.SetActive(false);
        boosterObject[n].SetActive(true);
    }

    public void ActivePlusObject(ref float radius)
    {
        player = null;
        int maxNumber = 200;
        if(GameManager.Instance.levelGame < 5)
        {
            maxNumber = 120;
        }
        else if(GameManager.Instance.levelGame < 10)
        {
            maxNumber = 160;
        }
        else
        {
            maxNumber = 200;
        }
        Number = (int)(Random.Range(40, maxNumber));
        numberText.text = "x" + Number.ToString();

        float t = (float)Number / (float)(maxNumber);
        float r = (GameManager.Instance.levelGame >= 4) ? 0.0f : GameManager.Instance.canonDiceGame.ScaleRatio;
        float scale = Mathf.Lerp(1.0f, 1.8f, t) * GameManager.Instance.canonDiceGame.ScaleRatio;
        int lvl = GameManager.Instance.levelGame;
        float scalebitmap123 = (lvl == 3 || lvl == 4 || lvl == 5) ? 1.30f : 1.0f;
        transform.localScale = Vector3.one * scale * scalebitmap123;
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.parent.forward);
        radius = sphereCollider.radius * scale;

        main.SetActive(false);
        gameObject.SetActive(true);
    }
}
