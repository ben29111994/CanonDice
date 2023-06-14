using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberOnFloor : MonoBehaviour
{
    public Image frame;
    public Text numberText;
    public void Active(Vector3 _pos, int _ID,int _number,Sprite _iconBooster)
    {
        _pos.y = 0.0f;
        frame.color = GameManager.Instance.canonDiceGame.colorFrameBullet[_ID];
        Vector3 euler = transform.eulerAngles;
        euler.y = Camera.main.transform.eulerAngles.y;
        transform.eulerAngles = euler;
        transform.position = _pos;
        numberText.text = _number + "";
        gameObject.SetActive(true);
        StartCoroutine(C_Active());

        if(GameManager.Instance.canonDiceGame.isBooster && GameManager.Instance.canonDiceGame.phaseBooster == CanonDiceGame.PhaseBooster.Booster)
        {
            Image iconImg = numberText.transform.parent.GetChild(6).GetComponent<Image>();

            if (_iconBooster == null)
            {
                iconImg.enabled = false;
            }
            else
            {
                iconImg.enabled = true;
            }
            numberText.gameObject.SetActive(false);
            iconImg.gameObject.SetActive(true);
            iconImg.sprite = _iconBooster;
        }
        else
        {
            numberText.gameObject.SetActive(true);
            numberText.transform.parent.GetChild(6).gameObject.SetActive(false);   
        }
    }

    private IEnumerator C_Active()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
