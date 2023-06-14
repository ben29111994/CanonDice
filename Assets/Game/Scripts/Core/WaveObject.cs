using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaveObject : MonoBehaviour
{
    public void Active(Vector3 _position)
    {
        _position.y = 0.0f;
        transform.position = _position;
        transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
        transform.DOScale(200.0f, 5.0f).SetEase(Ease.Linear);
        StartCoroutine(C_Active());
    }

    private IEnumerator C_Active()
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            Cube _cube = other.GetComponent<Cube>();
            _cube.End_Effect();
        }
    }
}
