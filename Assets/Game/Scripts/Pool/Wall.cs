using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public void GenerateWall(Vector3 _position, float _scale)
    {
        transform.localEulerAngles = Vector3.zero;
        transform.position = _position;
        transform.localScale = Vector3.one * _scale;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        transform.SetParent(PoolWall.Instance.parent);
        gameObject.SetActive(false);
    }
}
