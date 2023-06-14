using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Storage : MonoBehaviour
{
    public int Number;
    public Text numberText;

    public void UpdateNumber(int _n)
    {
        Number = _n;
        numberText.text = Number.ToString();
    }
}
