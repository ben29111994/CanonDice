using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Input")]
    public float moveSpeed;

    [Header("Status")]
    public int ID;
    public TypeBullet typeBullet;
    public bool isDelayTriggerWall;
    public bool isRotateToTarget;
    public bool isLockRotateToTarget;
    public int healthIndex;
    private int HitHammer;

    [Header("References")]
    public GameObject vfxHammerPrefab;
    public GameObject vfxHammer;
    public HammerExplosion hammerExplosion;
    public GameObject hammer;
    public BombExplosion bombExplosion;
    public GameObject bomb;
    public RocketExplosion rocketExplosion;
    public GameObject rocket;
    public Player player;
    public Rigidbody rigid;
    public Renderer rend;
    public TrailRenderer trail;
    public LayerMask layerPlane;
    public LayerMask layerPlaneHammer;
    public LayerMask layerCube;

    public Vector3 testPoint;
    private Vector3 startPosition;
    private Vector3 lastPosition;
    private Vector3 target;
    private float speedRotate;

    public enum TypeBullet
    {
        Bullet,
        Rocket,
        Bomb,
        Hammer,
    }

    private void Update()
    {
        if(typeBullet == TypeBullet.Hammer)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed * 1.2f);
        }
        else
        {
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);

        }

        if (typeBullet == TypeBullet.Bomb)
        {
            bomb.transform.Rotate(Vector3.one * Time.deltaTime * 200.0f);
        }
        else if(typeBullet == TypeBullet.Hammer)
        {
            hammer.transform.Rotate(Vector3.up * Time.deltaTime * 800.0f);
        }

        if((transform.position - startPosition).sqrMagnitude > 10000)
        {
            DisableBullet();
        }

        if (isLockRotateToTarget)
        {
            target = GameManager.Instance.canonDiceGame.pivotNoneFix;
            Vector3 dir = target - transform.position;
            dir.y = 0.0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * (11.8f + speedRotate));
            return;
        }

        if (isRotateToTarget && typeBullet != TypeBullet.Hammer)
        {
            target = Target();
            testPoint = target;
            Vector3 dir = target - transform.position;
            dir.y = 0.0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * (0.4f + speedRotate));
        }
    }

    private void LateUpdate()
    {
        lastPosition = transform.position;
    }

    private void OnDisable()
    {
        trail.Clear();
    }

    public Vector3 Target()
    {
        Vector3 a = transform.position;
        float d = 9999.0f;
        Vector3 b = Vector3.zero;
        
        for(int i = 0; i < GameManager.Instance.canonDiceGame.listPlayer.Count; i++)
        {
            Player _tPlayer = GameManager.Instance.canonDiceGame.listPlayer[i];
            if (_tPlayer.isDead == false && _tPlayer.ID != ID)
            {
                float c = Vector3.Distance(a, _tPlayer.transform.position);
                if (c < d)
                {
                    d = c;
                    b = _tPlayer.transform.position;
                }
            }
        }

        return b;
    }

    public void ActiveBullet(int _ID, Player _player, int _stt, TypeBullet _typeBullet,int _healthIndex)
    {
        // set up rocket
        typeBullet = _typeBullet;
        healthIndex = _healthIndex;

        if (vfxHammer != null) Destroy(vfxHammer);

        if(typeBullet == TypeBullet.Bullet)
        {
       //     hammer.gameObject.SetActive(false);
       //     rocket.SetActive(false);
      //      bomb.SetActive(false);
      //      rend.enabled = true;
            trail.emitting = true;
        }
        else if(typeBullet == TypeBullet.Rocket)
        {
            hammer.gameObject.SetActive(false);
            rocket.SetActive(true);
            bomb.SetActive(false);
            rend.enabled = false;
            trail.emitting = false;
        }
        else if(typeBullet == TypeBullet.Bomb)
        {
            hammer.gameObject.SetActive(false);
            rocket.SetActive(false);
            bomb.SetActive(true);
            rend.enabled = false;
            trail.emitting = false;
        }
        else if(typeBullet == TypeBullet.Hammer)
        {
            hammer.gameObject.SetActive(true);
            rocket.SetActive(false);
            bomb.SetActive(false);
            rend.enabled = false;
            trail.emitting = false;
            HitHammer = 0;
            vfxHammer = Instantiate(vfxHammerPrefab, hammer.transform);
            vfxHammer.transform.localPosition = Vector3.zero;
        }



        // init ID - Layer
        isLockRotateToTarget = false;
        isRotateToTarget = false;
        player = _player;
        ID = _ID;
        speedRotate = 0.0f;
        int layerIndex = (9 + ID);
        gameObject.layer = layerIndex;
        trail.material = GameManager.Instance.canonDiceGame.m_BulletTrail[ID];
      //  rend.material = GameManager.Instance.canonDiceGame.m_Bullet[ID];

        //test money model
  //      rend.enabled = false;
    //    sprMoney.sprite = moneyArray[_ID];

        // rotate to direction
        Transform canon = player.canon[_stt];
        Vector3 _posFixed = canon.position;
        transform.rotation = canon.rotation;
        transform.position = _posFixed + canon.forward * 2.0f;
        startPosition = transform.position;

        // active object bullet
        gameObject.SetActive(true);
        StopAllCoroutines();

        if (typeBullet == TypeBullet.Hammer)
        {
         //   hammer.Active(transform.position, ID, gameObject.layer, player,this);
            StartCoroutine(C_HideHammer());
        }
    }

    public void ChangeDirection()
    {
        Vector3 _inNormal = Vector3.zero;
        Vector3 _origin = transform.position - transform.forward * 0.4f;
        Ray ray = new Ray(_origin, transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, 10.0f, layerPlane))
        {
            speedRotate += 0.4f;
            isRotateToTarget = true;
            _inNormal = hit.normal;
            Vector3 _reflectDirection = Vector3.Reflect(transform.forward, _inNormal);
            _reflectDirection.y = 0.0f;
            Vector3 _reflectDirectionFixed = Quaternion.Euler(0, Random.Range(-15.0f,15.0f), 0) * _reflectDirection;
            transform.rotation = Quaternion.LookRotation(_reflectDirectionFixed.normalized);
        }
        else
        {
            LockRotateTotarget();

           // DisableBullet();
        }
    }

    public void ChangeDirectionHammer(bool _isToTarget)
    {
        if (_isToTarget)
        {
            Vector3 dir = Target() - transform.position;
            dir.y = 0.0f;
            transform.rotation = Quaternion.LookRotation(dir.normalized);
            return;
        }

        Vector3 _inNormal = Vector3.zero;
        Vector3 _origin = transform.position - transform.forward * 0.4f;
        Ray ray = new Ray(_origin, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10.0f, layerPlaneHammer))
        {
            speedRotate += 0.4f;
            isRotateToTarget = true;
            _inNormal = hit.normal;
            Vector3 _reflectDirection = Vector3.Reflect(transform.forward, _inNormal);
            _reflectDirection.y = 0.0f;
            Vector3 _reflectDirectionFixed = Quaternion.Euler(0, Random.Range(-15.0f, 15.0f), 0) * _reflectDirection;
            transform.rotation = Quaternion.LookRotation(_reflectDirectionFixed.normalized);
        }
        else
        {
            DisableBullet();
        }
    }

    public void DisableBullet()
    {
        if(typeBullet == TypeBullet.Bomb || typeBullet == TypeBullet.Rocket)
        {

        }
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    public void HitCube(Cube _cube)
    {
        _cube.ChangeColor(ID);

        healthIndex--;
        if(healthIndex <= 0) DisableBullet();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            if (typeBullet == TypeBullet.Rocket)
            {
                RocketExplosion _re = Instantiate(rocketExplosion);
                _re.Active(transform.position,ID, gameObject.layer,player);
            }
            else if(typeBullet == TypeBullet.Bomb)
            {
                BombExplosion _bomb = Instantiate(bombExplosion);
                _bomb.Active(transform.position, ID, gameObject.layer, player);
            }
            else if(typeBullet == TypeBullet.Hammer)
            {
                if (isDelayTriggerWall) return;
                if (HitHammer >= 5)
                {
                    StopAllCoroutines();
                    gameObject.SetActive(false);
                }

                HitHammer++;
                HammerExplosion _bomb = Instantiate(hammerExplosion);
                _bomb.Active(transform.position, ID, gameObject.layer, player);
                ChangeDirectionHammer(false);
                if (gameObject.activeSelf) StartCoroutine(C_DelayTriggerWall());
                return;
            }

            Cube _cube = other.GetComponent<Cube>();
            HitCube(_cube);


            // generate rocket
       
        }
        else if (other.CompareTag("Player"))
        {
            Player _player = other.gameObject.GetComponent<Player>();
            if(ID != _player.ID)
            {
                GameManager.Instance.canonDiceGame.ShowKillText(player, _player);
                _player.Dead();
                _player.ChangeCubeToWinPlayer(player.ID);
            }
        }
        else if (other.CompareTag("Wall"))
        {
            if (isDelayTriggerWall || isLockRotateToTarget) return;

            if(typeBullet == TypeBullet.Hammer)
            {
                ChangeDirectionHammer(true);
            }
            else
            {
                ChangeDirection();
            }
     
            if (gameObject.activeSelf) StartCoroutine(C_DelayTriggerWall());
        }
        else if (other.CompareTag("Shield"))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isLockRotateToTarget) return;

        if (other.CompareTag("Wall"))
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, 20.0f, layerCube))
            {

            }
            else
            {
                LockRotateTotarget();
                // DisableBullet();
            }
        }
    }


    private IEnumerator C_DelayTriggerWall()
    {
        isDelayTriggerWall = true;
        yield return null;
        isDelayTriggerWall = false;
    }

    private void LockRotateTotarget()
    {
        if(C2_C_LockRotateTotarget  != null) StopCoroutine(C2_C_LockRotateTotarget);
        C2_C_LockRotateTotarget = C_LockRotateTotarget();
        StartCoroutine(C2_C_LockRotateTotarget);
    }

    private IEnumerator C2_C_LockRotateTotarget;
    private IEnumerator C_LockRotateTotarget()
    {
        isLockRotateToTarget = true;
        yield return new WaitForSeconds(1.0f);
        isLockRotateToTarget = false;
        isRotateToTarget = true;
    }

    private IEnumerator C_HideHammer()
    {
        yield return new WaitForSeconds(8.0f);
        gameObject.SetActive(false);
    }
}
