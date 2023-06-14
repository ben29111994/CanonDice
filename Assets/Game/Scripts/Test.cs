using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float duration;
    public float strength;
    public int vibrato;
    public float random;
    public bool fade;

    public void Shake()
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 0.55f);
        DOTween.Kill(0);
        transform.DOShakeScale(duration, strength, vibrato, random, fade).SetId(0);
    }
}
