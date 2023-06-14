using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusMap : MonoBehaviour
{
    public int[] boostIndexArray;

    public bool isAciveMap;
    public List<PlusObject> listPlusObject = new List<PlusObject>();
    public LayerMask layerDice;

    public void ActiveMap()
    {
        StartCoroutine(C_ActiveMap());
    }

    private IEnumerator C_ActiveMap()
    {
        int lvl = GameManager.Instance.levelGame;
        isAciveMap = false;
        int amount = Random.Range(3, 6);
        if (GameManager.Instance.canonDiceGame.phaseBooster == CanonDiceGame.PhaseBooster.Booster)
        {
            amount = 3;

            if(GameManager.Instance.canonDiceGame.listPlayer.Count <= 3)
            {
                amount = 2;
            }
        }

        int h = 0;
        boostIndexArray = new int[3];
        int r = Random.Range(1, 7);
        if (lvl == 2)
        {
            boostIndexArray[0] = 0;
            boostIndexArray[1] = 1;
            boostIndexArray[2] = Random.Range(0, 2);
        }
        else if (lvl == 3)
        {
            if (PlayerPrefs.GetInt("boost3") == 0)
            {
                boostIndexArray[0] = 2;
                boostIndexArray[1] = 2;
                boostIndexArray[2] = 2;
                PlayerPrefs.SetInt("boost3", 1);
            }
            else
            {
                boostIndexArray = SimpleMathf.RandomIntArray(3, 3);
            }
        }
        else if (lvl == 4)
        {
            if (PlayerPrefs.GetInt("boost4") == 0)
            {
                boostIndexArray[0] = 3;
                boostIndexArray[1] = 3;
                boostIndexArray[2] = 3;
                PlayerPrefs.SetInt("boost4", 1);
            }
            else
            {
                boostIndexArray = SimpleMathf.RandomIntArray(3, 4);
            }
        }
        else if (lvl == 5)
        {
            if (PlayerPrefs.GetInt("boost5") == 0)
            {
                boostIndexArray[0] = 4;
                boostIndexArray[1] = 4;
                boostIndexArray[2] = 4;
                PlayerPrefs.SetInt("boost5", 1);
            }
            else
            {
                boostIndexArray = SimpleMathf.RandomIntArray(3, 5);
            }
        }
        else
        {
            boostIndexArray = SimpleMathf.RandomIntArray(3, 5);
        }


        for (int i = 0; i < amount; i++)
        {
            GameObject go = PoolManager.Instance.GetObject(PoolManager.NameObject.PlusObject) as GameObject;
            PlusObject plusObject = go.GetComponent<PlusObject>();
            float radius = 0.0f;
            plusObject.ActivePlusObject(ref radius);

            bool isSetTransorm = true;
            Vector3 randomPosition = Vector3.zero;
            int loopTimes = 0;

            while (isSetTransorm)
            {
                bool isMap90 = GameManager.Instance.canonDiceGame.isMap90;
                Transform offetCamera = GameManager.Instance.canonDiceGame.offsetCamera;
                //    Vector3 pivotPosition = offetCamera.position;
                Vector3 pivotPosition = GameManager.Instance.canonDiceGame.pivotNoneFix;
                float minX = (false) ? pivotPosition.z - 10.0f : pivotPosition.x - 10.0f;
                float maxX = (false) ? pivotPosition.z + 10.0f : pivotPosition.x + 10.0f;
                float minZ = (false) ? pivotPosition.x - 10.0f : pivotPosition.z - 10.0f;
                float maxZ = (false) ? pivotPosition.x + 10.0f : pivotPosition.z + 10.0f;
                randomPosition.x = Random.Range(minX, maxX);
                randomPosition.z = Random.Range(minZ, maxZ);
                if (GameManager.Instance.levelGame == 3) randomPosition.z += 4.0f;
                randomPosition.y = 1.0f;
                Vector3 origin = randomPosition + Vector3.up * 50.0f;
                Ray ray = new Ray(origin, Vector3.down);
                RaycastHit hit;
      
                if(Physics.SphereCast(ray,radius * 2.0f,out hit,Mathf.Infinity, layerDice))
                {
                    if (hit.collider.CompareTag("PlusObject"))
                    {
                      
                    }
                    else
                    {
                        isSetTransorm = false;
                    }
                }
                else
                {
                    isSetTransorm = false;
                }

                loopTimes++;
                if(loopTimes >= 200)
                {
                    break;
                }
            }

            if(loopTimes >= 200)
            {
                go.gameObject.SetActive(false);
                break;
            }
            
            bool _isBooster = GameManager.Instance.canonDiceGame.phaseBooster == CanonDiceGame.PhaseBooster.Booster ? true : false; ;
            if (_isBooster)
            {
      

                r = boostIndexArray[h] + 1;
                h++;
                SetBooster(r,plusObject);
            }
            else
            {
                plusObject.SetBooster(PlusObject.BoosterType.Empty);
            }

            listPlusObject.Add(plusObject);
            go.transform.position = randomPosition;
            yield return new WaitForSeconds(0.04f);
        }

        for (int i = 0; i < listPlusObject.Count; i++)
        {
            if (listPlusObject[i].boosterType == PlusObject.BoosterType.Empty)
                listPlusObject[i].main.SetActive(true);
        }

        isAciveMap = true;
    }

    public void SetBooster(int _index,PlusObject plusObject)
    {
        if(_index == 0)
        {
            plusObject.SetBooster(PlusObject.BoosterType.Booster_0_Freezing);
        }
        else if(_index == 1)
        {
            plusObject.SetBooster(PlusObject.BoosterType.Booster_4_Shield);
        }
        else if (_index == 2)
        {
            plusObject.SetBooster(PlusObject.BoosterType.Booster_5_Bomb);
        }
        else if (_index ==3 )
        {
            plusObject.SetBooster(PlusObject.BoosterType.Booster_2_Rocket);
        }
        else if (_index == 4)
        {
            plusObject.SetBooster(PlusObject.BoosterType.Booster_6_Dice);
        }
        else if (_index == 5)
        {
            plusObject.SetBooster(PlusObject.BoosterType.Booster_1_Lazer);
        }
        else if (_index == 6)
        {
            plusObject.SetBooster(PlusObject.BoosterType.Booster_3_Hammer);
        }
        else
        {
            plusObject.SetBooster(PlusObject.BoosterType.Empty);
        }
    }

    public void DisableMap()
    {
        isAciveMap = false;

        for(int i = 0; i < listPlusObject.Count; i++)
        {
            listPlusObject[i].gameObject.SetActive(false);
        }

        listPlusObject.Clear();
    }

    public void DisaleMapp_StopAction()
    {
        isAciveMap = false;

        StopAllCoroutines();
        for (int i = 0; i < listPlusObject.Count; i++)
        {
            listPlusObject[i].gameObject.SetActive(false);
        }

        listPlusObject.Clear();
    }
}
