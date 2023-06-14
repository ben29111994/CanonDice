using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public int ID;
    public Player player;

    public void Active(Vector3 _pos, int _ID, Player _player,float _time)
    {
        player = _player;
        ID = _ID;
        int layerIndex = (9 + ID);
        gameObject.layer = layerIndex;
        _pos.y = 2.0f;
        transform.position = _pos;
        gameObject.SetActive(true);
        StartCoroutine(C_ShieldTime(_time));
    }

    private IEnumerator C_ShieldTime(float _time)
    {
        float t = 0.0f;
        while(t < _time)
        {
            if(GameManager.Instance.canonDiceGame.phaseGame == CanonDiceGame.PhaseGame.Shoot)
            {
                t += Time.deltaTime;
            }
            yield return null;
        }
        yield return new WaitForSeconds(_time);
        gameObject.SetActive(false);
    }
}
