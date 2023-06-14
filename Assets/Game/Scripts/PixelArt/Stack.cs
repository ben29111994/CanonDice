using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Stack : MonoBehaviour
{
    public AnimationCurve animCurve;

    public Color startColor;
    public Color pixelColor;
    public Renderer rend;
    private MaterialPropertyBlock mpb;

    private void OnEnable()
    {
        mpb = new MaterialPropertyBlock();
        rend.GetPropertyBlock(mpb);
        mpb.SetColor("_Color", startColor);
        rend.SetPropertyBlock(mpb);
    }

    public void Active(Vector3 _pos,Vector3 _rot,Vector3 _scale,Color _pixelColor)
    {
        pixelColor = _pixelColor;
        StartCoroutine(C_Active(_pos, _rot, _scale));
    }

    public void ActiveNoAnim(Vector3 _pos, Vector3 _rot, Vector3 _scale, Color _pixelColor)
    {
        gameObject.SetActive(true);

        _scale.y = transform.localScale.y;
        pixelColor = _pixelColor;
        transform.position = _pos;
        transform.localScale = _scale;
        transform.eulerAngles = _rot;
        mpb = new MaterialPropertyBlock();
        rend.GetPropertyBlock(mpb);
        mpb.SetColor("_Color", pixelColor);
        rend.SetPropertyBlock(mpb);
        GameManager.Instance.canonDiceGame.pixelArt.UpdatePixelIndex();
    }

    public IEnumerator C_Active(Vector3 _pos, Vector3 _rot, Vector3 _scale)
    {
        float t = 0.0f;

        Vector3 _pos0 = transform.position;
        Vector3 _rot0 = transform.eulerAngles;
        Vector3 _scale0 = transform.localScale;
        _scale.y = _scale0.y;
        transform.DOJump(_pos, 4.0f, 1, 0.5f).SetEase(Ease.Flash);
        transform.DOScale(_scale, 0.8f).SetEase(Ease.Flash);

        while(t < 1.0f)
        {
            t += Time.deltaTime * 2.0f;

          //  transform.position = Vector3.Lerp(_pos0, _pos, t);
            transform.eulerAngles = Vector3.Lerp(_rot0, _rot, t);
       //     transform.localScale = Vector3.Lerp(_scale0, _scale, t);

            Color c = Color.Lerp(startColor, pixelColor, t);
            mpb = new MaterialPropertyBlock();
            rend.GetPropertyBlock(mpb);
            mpb.SetColor("_Color", c);
            rend.SetPropertyBlock(mpb);

            yield return null;
        }

        GameManager.Instance.canonDiceGame.pixelArt.UpdatePixelIndex();
        GameManager.Instance.canonDiceGame.pixelArt.CheckWin();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void WaveAnimation()
    {
        StartCoroutine(C_WaveAnimtion());
    }

    private IEnumerator C_WaveAnimtion()
    {
        float t = 0.0f;

        float currentY = transform.localPosition.y;

        while (t < 1.0f)
        {
            t += Time.deltaTime;

            float t2 = animCurve.Evaluate(t);
            Vector3 pos = transform.localPosition;
            pos.y = currentY * (t2 + 1.0f) * 3.0f;
            transform.localPosition = pos;

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WaveTrigger"))
        {
            WaveAnimation();
        }
    }
}
