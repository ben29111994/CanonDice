using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [Header("Status")]
    public bool isFaceNumber;
    public float torqueSpeed;
    public bool isDone;
    public int ID;
    public int FaceNumber;
    public PlusObject plusObject;

    [Header("References")]
    public Player player;
    public Rigidbody rigid;
    public GameObject[] faceNumbers;
    public GameObject[] dices;
    public LayerMask layerDice;

    public DiceNumberUpgrade currentDiceNumberUpgrade;
    public List<DiceNumberUpgrade> listDiceNumberUpgrade = new List<DiceNumberUpgrade>();
    [System.Serializable]
    public class DiceNumberUpgrade
    {
        public int[] numbers = new int[6];
    }

    private void Update()
    {
        UpdateFaceNumber();
    }

    public void UpdateFaceNumber()
    {
        if (isDone) return;

        for (int i = 0; i < faceNumbers.Length; i++)
        {
            Vector3 origin = faceNumbers[i].transform.position + Vector3.forward * 100.0f;
            Vector3 direction = Vector3.down;
            Ray ray = new Ray(origin, direction);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, 0.1f, layerDice))
            {
                FaceNumber = currentDiceNumberUpgrade.numbers[i]    ;
                break;
            }
        }
    }


    public void ActiveDice(int _ID, Vector3 _position,Vector3 _velocity,Player _player,bool _isJumpToPoint,Vector3 _targetPoint)
    {
        // init
        ID = _ID;
        isDone = false;
        plusObject = null;
        player = _player;

        // reset default value
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        transform.eulerAngles = Vector3.zero;

        // set dice model
        dices[ID].SetActive(true);
        for(int i = 0; i < dices.Length; i++)
        {
            if (i != ID) dices[i].SetActive(false);
        }

        // test
        isFaceNumber = true;
        if (isFaceNumber)
        {
            currentDiceNumberUpgrade = listDiceNumberUpgrade[player.OneFaceNumber];
            int index = player.OneFaceNumber - 1;

            // set texture for material
            Texture[] _tex = new Texture[0];
            if (_ID == 0)
            {
                _tex = GameManager.Instance.canonDiceGame.m_vangDice;
            }
            else if (_ID == 1)
            {
                _tex = GameManager.Instance.canonDiceGame.m_xanhDice;
            }
            else if (_ID == 2)
            {
                _tex = GameManager.Instance.canonDiceGame.m_doDice;
            }
            else if (_ID == 3)
            {
                _tex = GameManager.Instance.canonDiceGame.m_laDice;
            }

            int n = (index == -1) ? GameManager.Instance.canonDiceGame.m_vangDice.Length - 1 : index;
            dices[ID].GetComponent<MeshRenderer>().material.SetTexture("_MainTex", _tex[n]);
        }

        gameObject.SetActive(true);
        int lvl = GameManager.Instance.levelGame;
        float scalebitmap123 = (lvl == 3 || lvl == 4 || lvl == 5) ? 1.35f : 1.0f;
        transform.localScale = Vector3.one * 2.8f * GameManager.Instance.canonDiceGame.ScaleRatio * scalebitmap123;
        transform.position = _position;
        transform.eulerAngles = new Vector3(Random.Range(0.0f, 180.0f), Random.Range(0.0f, 180.0f), Random.Range(0.0f, 180.0f));

        if (_isJumpToPoint)
        {
            JumpToTargetPoint(_targetPoint);
        }
        else
        {
            rigid.velocity = _velocity; rigid.AddTorque(torqueSpeed * _velocity.magnitude * _velocity.normalized);
        }
        // rigid.AddTorque(torqueSpeed * -_velocity.magnitude * new Vector3(Random.Range(-1.0f,1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)));
        StartCoroutine(C_CheckDone());
    }

    private IEnumerator C_CheckDone()
    {
        yield return null;
        float vel = rigid.velocity.magnitude;
        while(vel > 0.1f)
        {
            vel = rigid.velocity.magnitude;
            yield return null;
        }

        vel = rigid.angularVelocity.magnitude;
        while (vel > 0.1f)
        {
            vel = rigid.angularVelocity.magnitude;
            yield return null;
        }

        isDone = true;
        player.DoneDice();
    }

    public void DisableDice()
    {

        gameObject.SetActive(false);
    }

    public int PlusNumber()
    {
        int result = 1;

        if (plusObject != null) result = plusObject.Number;

        return result;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlusObject"))
        {
            PlusObject p = other.gameObject.GetComponent<PlusObject>();
            
            if(GameManager.Instance.canonDiceGame.phaseBooster == CanonDiceGame.PhaseBooster.Empty)
            {
                plusObject = p;
            }
            else
            {
                if (p.player == null)
                {
                    p.player = player;
                    plusObject = p;
                }
            }
    
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlusObject"))
        {
            if(plusObject != null && plusObject == other.gameObject.GetComponent<PlusObject>())
            {
                plusObject.player = null;
                plusObject = null;
            }
        }
    }

    private void JumpToTargetPoint(Vector3 point)
    {
        var rigid = GetComponent<Rigidbody>();

        Vector3 p = point;

        float gravity = Physics.gravity.magnitude;
        // Selected angle in radians
        float angle = 60.0f * Mathf.Deg2Rad;

        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        Vector3 planarPostion = new Vector3(transform.position.x, 0, transform.position.z);

        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);
        // Distance along the y axis between objects
        float yOffset = transform.position.y - p.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion) * (p.x > transform.position.x ? 1 : -1);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        // Fire!
        rigid.velocity = finalVelocity;

        // Alternative way:
        // rigid.AddForce(finalVelocity * rigid.mass, ForceMode.Impulse);
    }
}
