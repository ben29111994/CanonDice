using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanTrigger : MonoBehaviour
{
    public Human human;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pixel"))
        {
            Pixel _pixel = other.GetComponent<Pixel>();
            if (_pixel.isFilled) return;
            human.FillStackToPixel(_pixel);
        }
    }
}
