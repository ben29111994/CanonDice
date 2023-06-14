using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBitMap : MonoBehaviour
{
    public int currentCountryBitMapIndex;
    public List<CountryBitMap> countryBitMapArray = new List<CountryBitMap>();
    [HideInInspector] public CountryBitMap currentCountryBitMap;
    private GameObject pointTarget;
    private List<SpriteRenderer> spr_countryBitMapArray = new List<SpriteRenderer>();

    private void GetReferences()
    {
        spr_countryBitMapArray.Clear();
        countryBitMapArray.Clear();

        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            countryBitMapArray.Add(transform.GetChild(0).GetChild(i).GetComponent<CountryBitMap>());
        }

        Transform _parent = transform.GetChild(1);
        for(int i = 2; i < _parent.childCount; i++)
        {
            spr_countryBitMapArray.Add(_parent.GetChild(i).GetComponent<SpriteRenderer>());
        }
        pointTarget = _parent.GetChild(0).gameObject;
    }

    public void ActiveCountryBitMap(int _currentCountryBitMap)
    {
        GetReferences();
        currentCountryBitMapIndex = _currentCountryBitMap;

        for(int i = 0; i < countryBitMapArray.Count; i++)
        {
            spr_countryBitMapArray[i].color = (i < currentCountryBitMapIndex) ? GameManager.Instance.canonDiceGame.colorWorldCountryBitMap[1] : GameManager.Instance.canonDiceGame.colorWorldCountryBitMap[0];
        }

        currentCountryBitMap = countryBitMapArray[currentCountryBitMapIndex];
        pointTarget.transform.position = spr_countryBitMapArray[currentCountryBitMapIndex].transform.position;
    }

    public void SetActiveWorldCountryBitmap(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    public void HideWorldMap(bool _isActive)
    {
        transform.GetChild(1).gameObject.SetActive(_isActive);
    }
}
