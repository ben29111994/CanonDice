using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    public Player player;
    public Transform target;
    public LineRenderer lineRendererArray;
    private Vector3 initalVelocity;
    public float xDistance;
    public List<Dice> currentListDice = new List<Dice>();
    public LayerMask layerDice;

    public void RenderTrajectory(float _t)
    {
        //Time of flight calculation1
        float forceZ = Mathf.Lerp(0.0f, 25.0f, _t);
        float forceY = Mathf.Lerp(10.0f, 30.0f, _t);
        float t;
        t = (-1f * forceY) / Physics.gravity.y;
        t = 2f * t;

        //Trajectory calculation
        Transform player = transform;
        Vector3 origin = player.transform.position;
        lineRendererArray.transform.position = origin;
        lineRendererArray.transform.rotation = Quaternion.LookRotation(player.forward);


        Vector3 velocity = lineRendererArray.transform.forward * forceZ;
        velocity.y = forceY;
        initalVelocity = velocity;

        LineRenderer lineRenderer = lineRendererArray;
        lineRenderer.positionCount = 40;
        Vector3 trajectoryPoint;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float time = t * i / (float)(lineRenderer.positionCount);
            trajectoryPoint = origin + velocity * time + 0.5f * Physics.gravity * time * time;
            lineRenderer.SetPosition(i, trajectoryPoint);
        }


        target.transform.position = lineRenderer.GetPosition(lineRenderer.positionCount - 1);

        Transform hand = GameManager.Instance.canonDiceGame.Hand;
        Vector3 _targetPos = target.transform.position;
        Vector3 handPos = _targetPos;
        hand.position = handPos;

        ActiveRenderer(true);
    }

    public void RenderTrajectory_AI(float _t,ref bool isLock)
    {
        //Time of flight calculation1
        float forceZ = Mathf.Lerp(0.0f, 25.0f, _t);
        float forceY = Mathf.Lerp(10.0f, 30.0f, _t);
        float t;
        t = (-1f * forceY) / Physics.gravity.y;
        t = 2f * t;

        //Trajectory calculation
        Transform player = transform;
        Vector3 origin = player.transform.position;
        lineRendererArray.transform.position = origin;
        lineRendererArray.transform.rotation = Quaternion.LookRotation(player.forward);


        Vector3 velocity = lineRendererArray.transform.forward * forceZ;
        velocity.y = forceY;
        initalVelocity = velocity;

        LineRenderer lineRenderer = lineRendererArray;
        lineRenderer.positionCount = 40;
        Vector3 trajectoryPoint;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float time = t * i / (float)(lineRenderer.positionCount);
            trajectoryPoint = origin + velocity * time + 0.5f * Physics.gravity * time * time;
            lineRenderer.SetPosition(i, trajectoryPoint);
        }


        target.transform.position = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
        ActiveRenderer(true);

        Ray ray = new Ray(target.transform.position + Vector3.up * 10.0f, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20.0f, layerDice))
        {
            if (hit.collider.CompareTag("PlusObject"))
            {
                isLock = true;
            }
            else
            {
                isLock = false;
            }
        }
        else
        {
            isLock = false;
        }
    }

    public void Shoot(bool _isMulti)
    {
        if (_isMulti)
        {
            StartCoroutine(C_Multi(false,Vector3.zero));
        }
        else
        {
            GameObject _diceGO = PoolManager.Instance.GetObject(PoolManager.NameObject.Dice) as GameObject;
            Dice currentDice = _diceGO.GetComponent<Dice>();
            currentDice.ActiveDice(player.ID, transform.position, initalVelocity, player,false,Vector3.zero);
            currentListDice.Add(currentDice);
            ActiveRenderer(false);
        }
    }

    public void AimShoot(bool _isMulti,Vector3 _point)
    {
        if (_isMulti)
        {
            StartCoroutine(C_Multi(true,_point));
        }
        else
        {
            GameObject _diceGO = PoolManager.Instance.GetObject(PoolManager.NameObject.Dice) as GameObject;
            Dice currentDice = _diceGO.GetComponent<Dice>();
            currentDice.ActiveDice(player.ID, transform.position, initalVelocity, player, true, _point);
            currentListDice.Add(currentDice);
            ActiveRenderer(false);
        }
    }

    private IEnumerator C_Multi(bool _isAim,Vector3 _point)
    {
        for(int i = 0; i < 3; i++)
        {
            GameObject _diceGO = PoolManager.Instance.GetObject(PoolManager.NameObject.Dice) as GameObject;
            Dice currentDice = _diceGO.GetComponent<Dice>();
            currentDice.ActiveDice(player.ID, transform.position, initalVelocity, player, _isAim, _point);
            currentListDice.Add(currentDice);
            ActiveRenderer(false);
            yield return new WaitForSeconds(0.2f);
        }

    }

    public void ActiveRenderer(bool _isActive)
    {
        _isActive = false;
        lineRendererArray.enabled = _isActive;
        target.gameObject.SetActive(_isActive);
    }


    public void DisableDice()
    {
        if(currentListDice.Count > 0)
        {
            for (int i = 0; i < currentListDice.Count; i++) currentListDice[i].gameObject.SetActive(false);
        }

        currentListDice.Clear();

    }
}