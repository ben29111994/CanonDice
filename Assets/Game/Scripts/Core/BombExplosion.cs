using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    public int ID;
    public Player player;
    public SphereCollider col;

    public void Active(Vector3 _pos, int _ID, int _layerIndex, Player _player)
    {
        col.enabled = true;
        player = _player;
        ID = _ID;
        gameObject.layer = _layerIndex;
        _pos.y = 2.0f;
        transform.position = _pos;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            Cube _cube = other.GetComponent<Cube>();
            _cube.ChangeColor(ID);
            StartCoroutine(C_DisableCollider());
            // generate rocket
        }
        else if (other.CompareTag("Player"))
        {
            Player _player = other.gameObject.GetComponent<Player>();
            if (ID != _player.ID)
            {
                GameManager.Instance.canonDiceGame.ShowKillText(player, _player);
                _player.Dead();
                _player.ChangeCubeToWinPlayer(player.ID);
            }
        }
    }

    public IEnumerator C_DisableCollider()
    {
        yield return null;
        col.enabled = false;
    }
}
