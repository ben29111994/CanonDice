using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Country : MonoBehaviour
{

    // 1. generate voxel (check w-h (30-50))
    // 2. fixMap button func
    // 3. add player to fixmap and drag player to child fixmap
    // 4. save to resource
    // 5. check camera offeset DONE

    [Header("Status")]
    public bool IsDone;

    [Header("Input")]
    public Transform offsetCameraTransform;

    [Header("References")]
    public FixMap fixMapObject;
    public List<Renderer> extraCountry = new List<Renderer>();
    private Renderer rend;

    public void UpdateDone(bool _isDone)
    {
        IsDone = _isDone;
        int _mIndex = IsDone ? 1 : 0;
        rend = transform.GetChild(0).GetComponent<Renderer>();
        rend.material = GameManager.Instance.canonDiceGame.m_country[_mIndex];

        for (int i = 0; i < extraCountry.Count; i++)
        {
            extraCountry[i].material = GameManager.Instance.canonDiceGame.m_country[_mIndex];
        }
    }
}
