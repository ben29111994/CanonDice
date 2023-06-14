using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 0 vang , 1 duong , 2 do , 3 la

public class Player : MonoBehaviour
{
    [Header("Status")]
    public int OneFaceNumber;
    public string myName;
    public float canonLerpSpeed;
    public float PowerRatio;
    public TypePlayer typePlayer;
    public PlusObject.BoosterType currentBoosterType;
    public int ID;
    public int bulletNumber;
    public bool IsBot;
    public bool IsDoneShoot;
    public bool IsShootDice;
    public bool IsDoneDice;
    public bool isDead;
    public bool isMultiDice;
    public bool isBooster;
    public bool isFirstTap;
    public string[] nameArray;

    [Header("References")]
    public GameObject ghostModel;
    public GameObject humanModel;
    public Shield shield;
    public GameObject lazerMuzzle;
    public GameObject lazerEndMuzzle;
    public LineRenderer lazerRenderer;
    public GameObject[] TurretArray;
    public RectTransform nameUI;
    public RectTransform bulletnumberUI;
    public Transform UI;
    public SpriteRenderer outLine;
    public Transform stickMan;
    public Renderer canonRend;
    public PlusObject targetPlusObject;
    public Transform[] canon;
    public Trajectory trajectory;
    public Animator[] anim;
    public List<Cube> listCube = new List<Cube>();

    private Dice currentDice;
    private GameObject CurrentTurret;
    private CanonDiceGame CDG;

    [Header("References UI")]
    public Image[] frameBulletNumber;
    public Transform CanvasBulletNumber;
    public RectTransform BulletNumberRect;
    public RectTransform NameRect;
    public Text bulletNumberText;
    public Text nameText;
    public Image boostIcon;

    public LayerMask layerPlane;

    public enum TypePlayer
    {
        AutoRotateShoot,
        Dice
    }

    public List<LayerMaskEnemy> listLayerMaskEnemy = new List<LayerMaskEnemy>();

    [System.Serializable]
    public class LayerMaskEnemy
    {
        public LayerMask layerEnemy;
    }



    private void LateUpdate()
    {
        UpdateRotateTurrret();
    }

    public void SetTypePlayer(TypePlayer _typePlayer)
    {
        if (isDead)
        {
            return;
        }

        typePlayer = _typePlayer;

        if(typePlayer == TypePlayer.AutoRotateShoot)
        {
            Type_AutoRotate();
        }
        else
        {
            Type_Dice();
        }
    }

    public void UpdateAutoShoot()
    {
        StartCoroutine(C_UpdateAutoShoot());
    }

    private IEnumerator C_UpdateAutoShoot()
    {
        bool isLoop = true;
        while (isLoop)
        {
            while (bulletNumber > 0 && isBooster == false)
            {
                int _healthIndex = 1;
                if (bulletNumber > 20) _healthIndex = 2;

                if (bulletNumber >= 100)
                {
                    for(int i = 0;  i < 3; i++)
                    {
                        bulletNumber -= _healthIndex;
                        bulletNumberText.text = bulletNumber.ToString();
                        GameManager.Instance.canonDiceGame.GenerateBullet(ID, this, i, Bullet.TypeBullet.Bullet, _healthIndex);
                    }              
                }
                else if(bulletNumber < 100 && bulletNumber >= 2)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        bulletNumber -= _healthIndex;
                        bulletNumberText.text = bulletNumber.ToString();
                        GameManager.Instance.canonDiceGame.GenerateBullet(ID, this, i, Bullet.TypeBullet.Bullet, _healthIndex);
                    }
                }
                else
                {
                    for (int i = 0; i < 1; i++)
                    {
                        bulletNumber -= _healthIndex;
                        bulletNumberText.text = bulletNumber.ToString();
                        GameManager.Instance.canonDiceGame.GenerateBullet(ID, this, i, Bullet.TypeBullet.Bullet, _healthIndex);
                    }
                }

                // shake animation
                if (IsBot)
                {
                    CurrentTurret.transform.parent.localScale = Vector3.one;
                    DOTween.Kill(ID);
                    CurrentTurret.transform.parent.DOShakeScale(0.1f, 0.2f, 100, 200, true).SetId(ID);
                }
                else
                {
                    CurrentTurret.transform.GetChild(0).localScale = Vector3.one;
                    DOTween.Kill(ID);
                    CurrentTurret.transform.GetChild(0).DOShakeScale(0.1f, 0.2f, 100, 200, true).SetId(ID);
                }
                


                float timeDelayShoot = 0.06f;
                float ratio = 1.0f;
                if(bulletNumber < 200)
                {
                    ratio = 0.8f;
                }
                else
                {
                    ratio = 0.6f;
                }

                float speedExtraRecord = Mathf.Clamp(((float)(bulletNumber - 200) / 1000.0f), 0.0f, 0.8f);
                float timeExtraRecord = (1.0f - speedExtraRecord);
                float timeDelayFixed = timeDelayShoot * ratio * timeExtraRecord;
                float speedAnimationFixed = (1.0f / ratio + speedExtraRecord);
                SetSpeedAnimation(speedAnimationFixed);
                yield return new WaitForSeconds(timeDelayFixed);

                int c = 0;

                for(int i = 0; i < GameManager.Instance.canonDiceGame.listPlayer.Count; i++)
                {
                    if (GameManager.Instance.canonDiceGame.listPlayer[i].isDead)
                    {
                        c++;
                    }
                }

                if (c == GameManager.Instance.canonDiceGame.listPlayer.Count - 1)
                {
                    yield break;
                }
            }

            yield return null;
        }
    }

    public void Type_AutoRotate()
    {
        StartCoroutine(C_Type_AutoRotate());
    }

    private IEnumerator C_Type_AutoRotate()
    {
        IsDoneShoot = false;

        isBooster = true;

        if (currentBoosterType == PlusObject.BoosterType.Empty)
        {
            if(CDG.phaseBooster == CanonDiceGame.PhaseBooster.Empty)
            {
                isBooster = false;

                while (bulletNumber > 0)
                {
                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }
        else if(currentBoosterType == PlusObject.BoosterType.Booster_0_Freezing)
        {
            yield return C_Booster_0_Freezing();
        }
        else if (currentBoosterType == PlusObject.BoosterType.Booster_1_Lazer)
        {
            yield return C_Booster_1_Lazer();
        }
        else if (currentBoosterType == PlusObject.BoosterType.Booster_2_Rocket)
        {
            yield return C_Booster_2_Rocket();
        }
        else if (currentBoosterType == PlusObject.BoosterType.Booster_3_Hammer)
        {
            yield return C_Booster_3_Hammer();
        }
        else if (currentBoosterType == PlusObject.BoosterType.Booster_4_Shield)
        {
            yield return C_Booster_4_Shield();
        }
        else if (currentBoosterType == PlusObject.BoosterType.Booster_5_Bomb)
        {
            yield return C_Booster_5_Bomb();
        }
        else if (currentBoosterType == PlusObject.BoosterType.Booster_6_Dice)
        {
            yield return C_Booster_6_Dice();
        }
        isBooster = false;
        boostIcon.gameObject.SetActive(false);
        IsDoneShoot = true;
    }

    public void Type_Dice()
    {
        StartCoroutine(C_Type_Dice());
    }

    private IEnumerator C_Type_Dice()
    {
        IsDoneDice = false;

        Vector3 _startPos = Vector3.zero;
        Vector3 _currentPos = Vector3.zero;
        Transform hand = GameManager.Instance.canonDiceGame.Hand;
        bool isTouchDown = false;
        bool isAim = CDG.isAim;

        if (IsBot == false)
        {
            if (isAim)
            {
                if (isFirstTap || CDG.isUSAmap || CDG.isWorldBitmap)
                {
                    AimController.Instance.ActiveAim();
                }
            }

            while (IsDoneDice == false)
            {
                if (Input.GetMouseButtonDown(0) && GameManager.Instance.isShopping == false)
                {
                    if (CDG.isUSAmap || CDG.isWorldBitmap) isFirstTap = true;
                    if (isFirstTap == false)
                    {
                        isFirstTap = true;
                        if (isAim && IsBot == false)
                        {
                            AimController.Instance.ActiveAim();
                        }
                    }
                    else
                    {
                        UIManager.Instance.dragToAim.SetActive(false);
                        nameText.gameObject.SetActive(true);
                        isTouchDown = true;
                        _startPos = _currentPos = TouchRayCast();
                        GameManager.Instance.tutorial.SetActive(false);
                        //      UIManager.Instance.UpgradeUI.SetActive(false);
                        hand.gameObject.SetActive(true);

 
                        if (isAim)
                        {
                            Vector3 targetPoint = Vector3.zero;
                            AimController.Instance.NextStep(ref targetPoint);
                            if (targetPoint != Vector3.zero)
                            {
                                trajectory.AimShoot(isMultiDice, targetPoint);
                                if (isMultiDice) isMultiDice = false;
                            }
                        }
                    }
                }
                else if (Input.GetMouseButton(0) && isTouchDown)
                {
                    if(isAim == false)
                    {
                        _currentPos = TouchRayCast();
                    }
                }
                else if (Input.GetMouseButtonUp(0) && isTouchDown)
                {
                    hand.gameObject.SetActive(false);

                    if(isAim == false)
                    {
                        trajectory.Shoot(isMultiDice);
                        if (isMultiDice) isMultiDice = false;
                        break;
                    }
                }

                if (isTouchDown && isAim)
                {
                    Vector3 _dir = -(_startPos - _currentPos);
                    if (_dir != Vector3.zero) trajectory.transform.rotation = Quaternion.Lerp(trajectory.transform.rotation, Quaternion.LookRotation(_dir.normalized), Time.deltaTime * 12.0f);
                    float t = _dir.magnitude;
                    float t2 = t / 20.0f;
                    trajectory.RenderTrajectory(t2);
                }

                yield return null;
            }
        }
        else
        {
            while (GameManager.Instance.isShopping) yield return null;
            nameText.gameObject.SetActive(true);
            float timeDelayAI = Random.Range(1.0f, 2.0f);
            if (CDG.phaseBooster == CanonDiceGame.PhaseBooster.Booster) timeDelayAI += 1.0f;
            yield return new WaitForSeconds(timeDelayAI);
            // AI pick Dice
            // pick plusObject (random)
            targetPlusObject = GameManager.Instance.canonDiceGame.plusMap.listPlusObject[Random.Range(0, GameManager.Instance.canonDiceGame.plusMap.listPlusObject.Count)];
            float speed_AI = Random.Range(0.3f, 0.5f);
            float t_AI = 0.0f;
            Vector3 _dir = targetPlusObject.transform.position - transform.position;
            _dir.y = 0.0f;
            if (_dir != Vector3.zero) trajectory.transform.rotation = Quaternion.LookRotation(_dir.normalized);
            bool isShootDice = false;

            while (t_AI < 1.0f)
            {
                t_AI += Time.deltaTime * speed_AI;
                trajectory.RenderTrajectory_AI(t_AI,ref isShootDice);

                if (isShootDice || t_AI >= 1.0f)
                {
                    float timeHold = 0.0f;
                    float targetTimeHold = Random.Range(0.1f, 0.2f);
                    while(timeHold < targetTimeHold)
                    {
                        t_AI += Time.deltaTime * speed_AI;
                        trajectory.RenderTrajectory_AI(t_AI, ref isShootDice);
                        timeHold += Time.deltaTime;
                        yield return null;
                    }
                    IsShootDice = true;
                    trajectory.Shoot(isMultiDice);
                    if (isMultiDice) isMultiDice = false;
                    break;
                }
                yield return null;
            }
        }
    }

    public void DoneDice()
    {
        IsDoneDice = true;
        
        if(CDG.phaseBooster == CanonDiceGame.PhaseBooster.Booster)
        {
            UpdateDiceNumber();
        }
    }

    public void UpdateOneFaceNumberBot(int _p1Number)
    {
        OneFaceNumber = Mathf.Clamp(_p1Number - ID, 0, 24);
    }

    public void Refresh()
    {
        lazerRenderer.SetPosition(0, Vector3.zero);
        lazerRenderer.SetPosition(1, Vector3.zero);
        lazerMuzzle.SetActive(false);
        lazerEndMuzzle.SetActive(false);
    }

    public void ActivePlayer(int _ID,Vector3 _lookPosition,CanonDiceGame _CDG)
    {
        Refresh();
        currentBoosterType = PlusObject.BoosterType.Empty;

        CDG = _CDG;
        ID = _ID;
        gameObject.layer = (9 + ID);


        //test
     //   OneFaceNumber =  GameManager.Instance.canonDiceGame.NumberOnFaceEnemy;
        if (ID == 0)
        {
            OneFaceNumber = Mathf.Clamp(DataManager.Instance.LevelPowerUp, 0, 24);
        }
        else
        {
            UpdateOneFaceNumberBot(DataManager.Instance.LevelPowerUp);
        }

        // refresh
        isDead = false;
        targetPlusObject = null;
        isFirstTap = false;

        nameArray = GameManager.Instance.listNameBot[_ID].nameArray;
        myName = nameArray[Random.Range(0, nameArray.Length)];
        nameText.text = myName.ToString();

        // set transform
        int lvl = GameManager.Instance.levelGame;
        float scalebitmap123 = (lvl == 3 || lvl == 4 || lvl == 5) ? 1.15f : 1.0f;
        transform.localScale = Vector3.one * GameManager.Instance.canonDiceGame.ScaleRatio * scalebitmap123;
        Vector3 _pos = transform.position;
        _pos.y = 1.0f;
        transform.position = _pos;

        // set ui
        float _posZ = (ID == 0 || ID == 3) ? -20.0f : 20.0f;
        bulletnumberUI.anchoredPosition3D = new Vector3(0.0f, 20.0f, _posZ);
        float _posX = (ID == 0 || ID == 3) ? -150.0f : 150.0f;
        if (CDG.is3player && ID == 2)
        {
            if(transform.position.z < CDG.pivotPosition.z)
            {
                _posX = -150.0f;
            }
        }
        nameUI.anchoredPosition3D = new Vector3(0.0f, 120.0f, _posX);
        nameText.gameObject.SetActive(false);

        // set turret
        if(ID == 0)
        {
            UpdateTurret(GameManager.Instance.canonDiceGame.TurretIndex);
        }
        else
        {
            CurrentTurret = canonRend.gameObject;
        }

        // init
        bulletNumber = 0;
        bulletNumberText.text = bulletNumber.ToString();
        if (ID != 0) IsBot = true;
        trajectory.ActiveRenderer(false);
        PowerRatio = 1.0f;

        // trajectory color
        trajectory.lineRendererArray.material = GameManager.Instance.canonDiceGame.m_Trajectory[ID];
        if (ID != 0)
        {
            trajectory.transform.GetChild(0).GetComponent<SpriteRenderer>().color = GameManager.Instance.canonDiceGame.colorPlayers[ID];
        }
        else
        {
            trajectory.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f, 1.0f);
        }
        outLine.color = GameManager.Instance.canonDiceGame.colorPlayers[ID];

        // canon rend
        canonRend.material = GameManager.Instance.canonDiceGame.m_Turret[ID];
        stickMan.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = canonRend.sharedMaterial;

        // rotate canon
        Vector3 dir = Vector3.zero;
        for (int i = 0; i < canon.Length; i++)
        {
            dir = _lookPosition - canon[i].transform.parent.position;
            dir.y = 0.0f;
            canon[i].transform.parent.transform.rotation = Quaternion.LookRotation(dir);
        }

        stickMan.transform.position = canon[0].transform.parent.transform.position - dir.normalized * 3.0f;
        stickMan.transform.rotation = Quaternion.LookRotation(dir);
        UI.rotation = Quaternion.LookRotation(GameManager.Instance.canonDiceGame.offsetCamera.forward);
        CanvasBulletNumber.transform.rotation = UI.rotation;
        for (int i = 0; i < frameBulletNumber.Length; i++) frameBulletNumber[i].color = GameManager.Instance.canonDiceGame.colorFrameBullet[ID];
        if (ID == 0 || ID == 3) BulletNumberRect.anchoredPosition3D = Vector3.forward * 300.0f;
       
        if(CDG.listPlayer.Count == 2)
        {
            Vector3 pos = BulletNumberRect.anchoredPosition3D;
            pos.x -= 250.0f;
            BulletNumberRect.anchoredPosition3D = pos;
        }

        // set human skin

        if (IsBot)
        {
            if (GameManager.Instance.canonDiceGame.isBonusLevel)
            {
                ghostModel.SetActive(true);
                humanModel.gameObject.SetActive(false);
            }
            else
            {
                ghostModel.SetActive(false);
                humanModel.gameObject.SetActive(true);
            }
        }


        // active
        gameObject.SetActive(true);
        Active_Effect();

        if(GameManager.Instance.levelGame == 1)
        {
            SetAnimCanon("map4p");
         //   SetAnimCanon("p0_bitmap1");
        }
        else
        {
            SetAnimCanon("map4p");
        }

        // start func
        UpdateAutoShoot();
    }

    public void DisablePlayer()
    {
        trajectory.DisableDice();
        gameObject.SetActive(false);
    }

    public void SetNumber(int _n)
    {
        bulletNumber += _n;
        bulletNumberText.text = bulletNumber.ToString();
    }

    public Vector3 TouchRayCast()
    {
        Vector3 _result = Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit, Mathf.Infinity, layerPlane))
        {
            if(hit.collider != null)
            {
                _result = hit.point;
            }
        }
        else
        {
          
        }

        return _result;
    }

    public void UpdateDiceNumber()
    {
        if (trajectory.currentListDice.Count == 0)
        {
            return; 
        }

        for(int i = 0; i < trajectory.currentListDice.Count; i++)
        {
            if (trajectory.currentListDice[i].plusObject == null)
            {
                currentBoosterType = PlusObject.BoosterType.Empty;
            }
            else
            {
                currentBoosterType = trajectory.currentListDice[i].plusObject.boosterType;
            }
        }

 

        if (isDead)
        {
            trajectory.DisableDice();
            return;
        }

        int sum = 0;

        for (int i = 0; i < trajectory.currentListDice.Count; i++)
        {
            sum += (int)(trajectory.currentListDice[i].PlusNumber() * trajectory.currentListDice[i].FaceNumber);
        }

        if (currentBoosterType == PlusObject.BoosterType.Empty)
        {
            SetNumber(sum);
        }

        if (CDG.phaseBooster == CanonDiceGame.PhaseBooster.Booster)
        {
            for (int i = 0; i < trajectory.currentListDice.Count; i++)
            {
                if (trajectory.currentListDice[i].plusObject != null)
                    trajectory.currentListDice[i].plusObject.gameObject.SetActive(false);

            }
        }

        StartCoroutine(C_ActiveNumberOnFloor(sum,trajectory.currentListDice[0].plusObject));
    }

    private IEnumerator C_ActiveNumberOnFloor(int sum,PlusObject _plusObject)
    {
        int stt = (int)currentBoosterType;
        Sprite _booster = null;
        if(_plusObject != null) _booster = (currentBoosterType == PlusObject.BoosterType.Empty) ? null : _plusObject.boosterObject[stt].transform.GetChild(2).GetComponent<SpriteRenderer>().sprite;
        GameManager.Instance.canonDiceGame.numberOnFloorArray[ID].Active(trajectory.currentListDice[0].transform.position, ID, sum, _booster);
       
        BulletNumberRect.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        BulletNumberRect.gameObject.SetActive(true);

        bool isPhaseBooster = GameManager.Instance.canonDiceGame.phaseBooster == CanonDiceGame.PhaseBooster.Booster ? true : false;
        if (isPhaseBooster)
        {
            boostIcon.gameObject.SetActive(true);
            bulletNumberText.gameObject.SetActive(false);
            boostIcon.sprite = _booster;
            if(boostIcon.sprite == null) boostIcon.gameObject.SetActive(false);
        }
        else
        {
            boostIcon.gameObject.SetActive(false);
            bulletNumberText.gameObject.SetActive(true);
        }

        trajectory.DisableDice();

    }

    public void ActiveNumberText(bool _active)
    {
        boostIcon.sprite = null;
        boostIcon.gameObject.SetActive(false);


        bulletNumberText.gameObject.SetActive(_active);
    }

    public void Dead()
    {
        if (isDead) return;
        isDead = true;

        if(ID == 0)
        {
            if (CDG.isAim)
            {
                AimController.Instance.Hide();
            }
            GameManager.Instance.canonDiceGame.CheckLose();
        }
        else
        {
            GameManager.Instance.canonDiceGame.CheckWin();
        }

        GameManager.Instance.canonDiceGame.CubeBreakEffect(this);
        gameObject.SetActive(false);
    }

    public void SetAnimCanon(string _triggerName)
    {
        StartCoroutine(C_SetAnimCanon(_triggerName));
    }

    private IEnumerator C_SetAnimCanon(string _triggerName)
    {
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].SetTrigger(_triggerName);
            yield return new WaitForSeconds(0.14f);
        }
    }
    private void SetSpeedAnimation(float _speed)
    {
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].speed = _speed;
        }
    }
    public void Active_Effect()
    {
        StartCoroutine(C_Active_Effect());
    }

    private IEnumerator C_Active_Effect()
    {
        float cY = 0.0f;
        float tY = transform.localScale.x;
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * 1.2f;
            float t2 = GameManager.Instance.animCurve3.Evaluate(t);
            float y2 = Mathf.Lerp(cY, tY, t2);
            transform.localScale = Vector3.one * y2;
            yield return null;
        }
    }

    public void ChangeCubeToWinPlayer(int _ID)
    {
        if (GameManager.Instance.isComplete) return;
        while(listCube.Count > 0)
        {
            listCube[0].ChangeColor(_ID);
        }
    }

    public void UpdateTurret(int _number)
    {
        for (int i = 0; i < TurretArray.Length; i++) TurretArray[i].SetActive(false);
        TurretArray[_number].SetActive(true);
        CurrentTurret = TurretArray[_number];
    }

    public void UpdateRotateTurrret()
    {
        if (ID != 0) return;
        Transform t = CurrentTurret.transform.GetChild(0);
        Vector3 _eular = canonRend.transform.eulerAngles;
        t.transform.eulerAngles = _eular;
    }

    private IEnumerator C_Booster_0_Freezing()
    {
        yield return null;
    }

    private IEnumerator C_Booster_1_Lazer()
    {
        LayerMask _layerMaskEnemy = listLayerMaskEnemy[ID].layerEnemy;
        lazerRenderer.material = CDG.m_lazer[ID];
        lazerMuzzle.SetActive(true);
        lazerEndMuzzle.SetActive(true);
        int n = 0;
        float time = 8.0f;
        while(time > 0.0f)
        {
            Vector3 startPoint = canon[0].transform.position + canon[0].forward * 2.0f;
            Vector3 endPoint = startPoint + canon[0].forward * 100.0f;
            Ray ray = new Ray(startPoint, canon[0].forward);
            RaycastHit hit;
            Cube _cube = null;

            if(Physics.Raycast(ray,out hit, 100.0f, _layerMaskEnemy))
            {
                endPoint = hit.point;
                _cube = hit.collider.gameObject.GetComponent<Cube>();
                if(_cube != null) _cube.ChangeColor(ID);

                Player _player = hit.collider.gameObject.GetComponent<Player>();
                if (_player != null && ID != _player.ID)
                {
                    GameManager.Instance.canonDiceGame.ShowKillText(this, _player);
                    _player.Dead();
                    _player.ChangeCubeToWinPlayer(this.ID);
                }
            }

            lazerRenderer.SetPosition(0, startPoint);
            lazerRenderer.SetPosition(1, endPoint);
            lazerEndMuzzle.transform.position = endPoint;


            time -= Time.deltaTime;
            n++;

            if(n%3 == 0)
            {
                if (Physics.Raycast(ray, out hit, 100.0f, _layerMaskEnemy))
                {
                    endPoint = hit.point;
                    _cube = hit.collider.gameObject.GetComponent<Cube>();
                    if (_cube != null) _cube.ChangeColor(ID);

                    Player _player = hit.collider.gameObject.GetComponent<Player>();
                    if (_player != null && ID != _player.ID)
                    {
                        GameManager.Instance.canonDiceGame.ShowKillText(this, _player);
                        _player.Dead();
                        _player.ChangeCubeToWinPlayer(this.ID);
                    }
                }
            }
            yield return null;
        }

        lazerRenderer.SetPosition(0, Vector3.zero);
        lazerRenderer.SetPosition(1, Vector3.zero);
        lazerMuzzle.SetActive(false);
        lazerEndMuzzle.SetActive(false);
    }
    private IEnumerator C_Booster_2_Rocket()
    {
        float time = 5.0f;
        while (time > 0.0f)
        {
            GameManager.Instance.canonDiceGame.GenerateBullet(ID, this, 0, Bullet.TypeBullet.Rocket, 1);
            yield return new WaitForSeconds(0.2f);
            time -= 0.2f;
        }
        yield return null;
    }
    private IEnumerator C_Booster_3_Hammer()
    {
        GameManager.Instance.canonDiceGame.GenerateBullet(ID, this, 0, Bullet.TypeBullet.Hammer, 1);
        yield return new WaitForSeconds(6.0f);
        yield return null;
    }
    private IEnumerator C_Booster_4_Shield()
    {
        Shield _shield = Instantiate(shield);
        _shield.Active(transform.position, ID,this, 5.0f);
        yield return new WaitForSeconds(0.5f);
    }
    private IEnumerator C_Booster_5_Bomb()
    {
        float time = 4.0f;
        while (time > 0.0f)
        {
            GameManager.Instance.canonDiceGame.GenerateBullet(ID, this, 0, Bullet.TypeBullet.Bomb, 1);
            yield return new WaitForSeconds(1.0f);
            time -= 1.0f;
        }
        yield return null;
    }
    private IEnumerator C_Booster_6_Dice()
    {
        isMultiDice = true;
        yield return null;
    }
}
