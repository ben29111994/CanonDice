using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    public static AimController Instance;

    [Header("Status")]
    public int currentStep;
    private float t;


    [Header("Input")]
    public float speed;
    public float minX;
    public float maxX; 
    public float minY;
    public float maxY;
    public AnimationCurve ac;

    [Header("References")]
    public Transform pointTransform;
    public Transform horizontal;
    public Transform vertical;
    public LayerMask layer;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }


    public void ActiveAim()
    {
        currentStep = 0;
        t = 0.0f;
        gameObject.SetActive(true);
        StartCoroutine(C_AimUpdate());
    }

    public void NextStep(ref Vector3 point)
    {
        currentStep++;

        if(currentStep == 2)
        {
            point = GetTargetPoint();
            Hide();
        }
        else
        {
            point = Vector3.zero;
        }
    }

    public void Hide()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    private IEnumerator C_AimUpdate()
    {
        horizontal.gameObject.SetActive(true);
        vertical.gameObject.SetActive(false);


        Vector3 hopos = horizontal.localPosition;
        hopos.x = 0.0f;
        hopos.y = maxY;
        horizontal.localPosition = hopos;

        Vector3 vepos = vertical.localPosition;
        hopos.y = 0.0f;
        hopos.x = minX;
        vertical.localPosition = vepos;

        while (true)
        {
            float ratio = Mathf.Abs(maxY - minY);
            t += Time.deltaTime * speed  / ratio;
            float t2 = ac.Evaluate(t);

            Vector3 pos = horizontal.localPosition;
            if(currentStep == 0)
            {
                pos.y = Mathf.Lerp(maxY, minY, t2);   
            }
            else
            {
                t = 0.0f;
                break;
            }
            horizontal.localPosition = pos;
            if (t >= 1.0f) t = 0.0f;
            yield return null;
        }
        vertical.gameObject.SetActive(true);

        while (true)
        {
            float ratio = Mathf.Abs(maxX - minX);
            t += Time.deltaTime * speed / ratio;
            float t2 = ac.Evaluate(t);
            Vector3 pos = vertical.localPosition;
            if (currentStep == 1)
            {
                pos.x = Mathf.Lerp(minX, maxX, t2);
            }
            else
            {
                break;
            }
            vertical.localPosition = pos;
            if (t >= 1.0f) t = 0.0f;
            yield return null;
        }

        currentStep = 0;
        t = 0.0f;
    }

    // Start is called before the first frame update
    public Vector3 GetTargetPoint()
    {
        Vector3 point = Vector3.zero;

        Vector3 pos = pointTransform.localPosition;
        pos.x = vertical.localPosition.x;
        pos.y = horizontal.localPosition.y;
        pointTransform.localPosition = pos;

        Vector3 pivot = pointTransform.position;
        Vector3 pos2 = Camera.main.WorldToScreenPoint(pivot);
        Ray ray = Camera.main.ScreenPointToRay(pos2);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, 100.0f,layer))
        {
            point = hit.point;
        }

        return point;
    }

    // Update is called once per frame
}
