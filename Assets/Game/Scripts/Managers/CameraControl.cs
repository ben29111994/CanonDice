using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraControl : MonoBehaviour
{
    public Transform[] cameraAnimation;
    public RectTransform[] topUI;

    void Start()
    {
        SetFOV();
    }

    void SetFOV()
    {
        float ratio = Camera.main.aspect;

        if (ratio >= 0.74) // 3:4
        {
            Camera.main.fieldOfView = 60;
        }
        else if (ratio >= 0.56) // 9:16
        {
            Camera.main.fieldOfView = 60;
        }
        else if (ratio >= 0.45) // 9:19
        {
            Camera.main.fieldOfView = 66;

            foreach (RectTransform r in topUI)
            {
                Vector2 current = r.anchoredPosition;
                current.y -= 150.0f;
                r.anchoredPosition = current;
            }
        }
    }

    public void CameraZoomToCountry()
    {
        transform.DOLocalRotate(cameraAnimation[1].transform.localEulerAngles, 2.0f).SetEase(Ease.InOutSine);
        transform.DOLocalMove(cameraAnimation[1].transform.localPosition, 2.0f).SetEase(Ease.InOutSine);
    }

    public void CameraSnapToCountry()
    {
        transform.localEulerAngles = cameraAnimation[1].transform.localEulerAngles;
        transform.localPosition = cameraAnimation[1].transform.localPosition;
    }

    public void CameraMainMenu()
    {
      //  transform.DOLocalRotate(cameraAnimation[0].transform.localEulerAngles, 2.0f).SetEase(Ease.InOutSine);
      //  transform.DOLocalMove(cameraAnimation[0].transform.localPosition, 2.0f).SetEase(Ease.InOutSine);

        transform.localEulerAngles = cameraAnimation[0].transform.localEulerAngles;
        transform.localPosition = cameraAnimation[0].transform.localPosition;
        Transform offetCamera = Camera.main.transform.parent;
        //   offetCamera.DOMove(Vector3.zero, 1.0f).SetEase(Ease.InOutSine);
        //   offetCamera.DORotate(Vector3.zero, 1.0f).SetEase(Ease.InOutSine);

        offetCamera.position = Vector3.zero;
        offetCamera.eulerAngles = Vector3.zero;
    }

    public void CameraMainMenu_WorldBitMap()
    {
        transform.localEulerAngles = cameraAnimation[3].transform.localEulerAngles;
        transform.localPosition = cameraAnimation[3].transform.localPosition;
        Transform offetCamera = Camera.main.transform.parent;
        offetCamera.position = Vector3.zero;
        offetCamera.eulerAngles = Vector3.zero;
    }

    public void CameraMainMenuBitmap()
    {
        //transform.DOLocalRotate(cameraAnimation[2].transform.localEulerAngles, 2.0f).SetEase(Ease.InOutSine);
        //transform.DOLocalMove(cameraAnimation[2].transform.localPosition, 2.0f).SetEase(Ease.InOutSine);
        Transform offetCamera = Camera.main.transform.parent;
        //offetCamera.DOMove(Vector3.zero, 1.0f).SetEase(Ease.InOutSine);
        //offetCamera.DORotate(Vector3.zero, 1.0f).SetEase(Ease.InOutSine);
        Vector3 pos = Vector3.zero;
        pos.x = 0000.0f;

        transform.localEulerAngles = cameraAnimation[2].transform.localEulerAngles;
        transform.localPosition = cameraAnimation[2].transform.localPosition;
        offetCamera.transform.position = pos;
        offetCamera.transform.eulerAngles = Vector3.zero;
    }
}