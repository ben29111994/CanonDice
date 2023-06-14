using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pixel : MonoBehaviour
{
    public bool isFilled;
    public Renderer rend;
    public Color mainColor;
    public Material[] m_pixel;
    public AnimationCurve animCurve;
    private MaterialPropertyBlock mpb;

    public void Active(Vector3 _position, Vector3 _scale, Color _mainColor,bool _usePhysic2D)
    {
        isFilled = false;
        transform.localScale = _scale;
        transform.position = _position;
        rend.enabled = true;
        mainColor = _mainColor;

        rend.material = m_pixel[0];

        gameObject.SetActive(true);
    }

    public void AutoFill()
    {
        Stack _stack = PoolStack.Instance.GetStackInPool();
        isFilled = true;
        _stack.transform.parent = GameManager.Instance.canonDiceGame.pixelArt.art.transform;
        Vector3 pos = transform.position + Vector3.up * transform.localScale.y * 0.5f;
        Vector3 scale = transform.localScale;
        Vector3 rot = Vector3.zero;
        _stack.ActiveNoAnim(pos, rot, scale, mainColor);
    }

    public void WaveAnimation()
    {
        StartCoroutine(C_WaveAnimtion());
    }

    private IEnumerator C_WaveAnimtion()
    {
        float t = 0.0f;

        float currentY = transform.localPosition.y;

        while(t < 1.0f)
        {
            t += Time.deltaTime;

            float t2 = animCurve.Evaluate(t);
            Vector3 pos = transform.localPosition;
            pos.y = currentY * (t2 + 1.0f) * 2.0f;
            transform.localPosition = pos;

            yield return null;
        }
    }

    public void Hide()
    {
        isFilled = false;
        transform.position = Vector3.one;
        transform.localScale = Vector3.one;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        return;
        if (other.CompareTag("WaveTrigger"))
        {
            WaveAnimation();
        }
    }
}