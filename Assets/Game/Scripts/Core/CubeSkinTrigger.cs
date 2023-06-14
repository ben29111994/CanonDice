using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSkinTrigger : MonoBehaviour
{
    public Transform[] triggers;
    public LayerMask layerCube;

    public void Active(float _scale,Vector3 _pos,Vector3 _dir)
    {
        Vector3 pos = _pos;
        pos.y = 0.0f;
        transform.rotation = Quaternion.LookRotation(_dir);
        transform.position = pos + transform.forward * _scale * 5.0f;
        int half = (int)(triggers.Length / 2.0f);
        for(int i = 0; i < triggers.Length; i++)
        {
            Vector3 _posTrigger = Vector3.zero;
            _posTrigger.x = (i - half) * _scale;
            _posTrigger.z = 0.0f;
            triggers[i].localPosition = _posTrigger;
            triggers[i].localScale = Vector3.one * _scale;
        }

        gameObject.SetActive(true);
        StartCoroutine(C_Move(_scale));
    }

    public IEnumerator C_Move(float _scale)
    {
        int n = 0;
     //   transform.Translate(transform.forward * -_scale * 0.5f);
        int num = 0;
        int k = 0;
        for (int i = 0; i < 100; i++)
        {
            transform.Translate(-Vector3.forward * _scale * 1.0f);
         //   transform.position += transform.forward * _scale;
            if(k == 0)
            {
                num = 0;
            }
            else if ( k == 1)
            {
                num = 4;
            }
            else if(k == 2)
            {
                num = 8;
            }
            else
            {
                num = 12;
            }
            RayCastToSetNumber(num);
            n++;
            k++;
            if (k == 4) k = 0;
            yield return null;
        }

        gameObject.SetActive(false);
    }

    private void RayCastToSetNumber(int _num)
    {
        Ray ray;
        RaycastHit hit;
        int _id = _num;

        for (int i = 0; i < triggers.Length; i++)
        {
            Vector3 origin = triggers[i].position + Vector3.up * 10.0f;
            Vector3 direction = Vector3.down;
            ray = new Ray(origin, direction);
            if(Physics.Raycast(ray, out hit, 20.0f, layerCube))
            {
                if(hit.collider != null)
                {
                    Cube _cube = hit.collider.gameObject.GetComponent<Cube>();
                    if (_cube != null) _cube.InitID(_id);

                }
            }
            _id++;
            if (_id == _num + 4) _id = _num;
        }
    }
}
