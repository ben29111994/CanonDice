using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBullet : MonoBehaviour
{
    public static PoolBullet Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Bullet GetBulletInPool(int _number)
    {
        Bullet _bullet = null;

        if(_number == 0)
        {
            _bullet = PoolBullet_Yellow.Instance.GetBulletInPool();
        }
        else if(_number == 1)
        {
            _bullet = PoolBullet_Blue.Instance.GetBulletInPool();
        }
        else if (_number == 2)
        {
            _bullet = PoolBullet_Red.Instance.GetBulletInPool();
        }
        else if (_number == 3)
        {
            _bullet = PoolBullet_Green.Instance.GetBulletInPool();
        }

        return _bullet;
    }

    public void HideAll()
    {
        PoolBullet_Yellow.Instance.HideAll();
        PoolBullet_Blue.Instance.HideAll();
        PoolBullet_Red.Instance.HideAll();
        PoolBullet_Green.Instance.HideAll();
    }
}
