using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePlayer : MonoBehaviour
{
    [SerializeField] RaycastHit2D hit; 
    [SerializeField] GameObject Enemy, PlayerSpawner, EnemySpawner, DmgText, HealText, GM, SkillButton, ManaText, LockText, BarrierImage, SkillImage, DefenseImage; //전투시 인식한 적 오브젝트 담는 그릇
    [SerializeField] GameObject[] HealSkillButton, BarrierSkillButton;
    [SerializeField] bool GoToEnemy = false, GoToReturn = false, IsAttackSkill = false, IsAttackOk = true, IsContinuity, IsComBo, IsEnd, IsFaild; 
    public bool IsHit = false, IsBarrier = false, IsDefense, IsDefensing;
    [SerializeField] int BarrierCount = 0, MaxBarrierCount, BasicAttackCount = 0;
    [SerializeField] Image BarrierEffect1, BarrierEffect2;
    [SerializeField] float AttackCounting = 0;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        DefenseImage.SetActive(false);
        BarrierCount = 0;
        IsDefensing = false;
        IsDefense = false;
        AttackCounting = 0;
        IsEnd = true;
        IsFaild = false;
        IsComBo = false;
        BasicAttackCount = 1;
        animator = GetComponent<Animator>();
        this.transform.position = PlayerSpawner.transform.position;
        SkillImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Defensing();
        RayCasting();
        Barrier();
        Attackcounting();
        ComboAttack();
        IsHited();
        if (GoToEnemy == true && GameManager.Instance.IsBattleStart == true && IsAttackSkill == false && BasicAttackCount == 1)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, EnemySpawner.transform.position + new Vector3(-2, 0.77f, 0), 10 * Time.deltaTime);
        }
        else if(GoToEnemy == true && GameManager.Instance.IsBattleStart == true && IsAttackSkill == true && BasicAttackCount == 1)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, EnemySpawner.transform.position + new Vector3(-8, 0.77f, 0), 10 * Time.deltaTime);
        }
        else if(GoToReturn == true)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, PlayerSpawner.transform.position, 10 * Time.deltaTime);
        }
        else if(GoToReturn == false)
        {
            animator.SetBool("IsWalk", false);
        }
        if(GameManager.Instance.IsBattleStart == true)
        {
            SkillImage.transform.position = Enemy.transform.position;
        }
        if(GameManager.Instance.curHp <= 0)
        {
            animator.SetBool("IsDead", true);
            SkillButton.SetActive(false);
        }
    }
    IEnumerator BarrierEffectIconFadeIn(float FaidTime)
    {
        Color color = BarrierEffect1.color;
        Color color2 = BarrierEffect2.color;
        while (color.a < 1f)
        {
            color.a += Time.deltaTime / FaidTime;
            color2.a += Time.deltaTime / FaidTime;
            BarrierEffect1.color = color;
            if (color.a >= 1f)
            {
                color.a = 1f;
                color2.a = 1f;
            }
            yield return null;
        }
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / FaidTime;
            color2.a -= Time.deltaTime / FaidTime;
            BarrierEffect1.color = color;
            if (color.a <= 0f)
            {
                color.a = 0f;
                color2.a = 0f;
            }
            yield return null;
        }
    }

    void IsHited()
    {
        if (IsHit == true)
        {
            StartCoroutine(PlayerHit());
        }
    }
    void ComboAttack()
    {
        if (IsComBo == true && IsFaild == false)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                IsContinuity = true;
            }
        }
        else if (IsComBo == false)
        {
            IsContinuity = false;
        }
        if (IsEnd == false)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                IsFaild = true;
            }
        }
    }
    void Defensing()
    {
        DefenseImage.transform.position = this.transform.position;
        if (IsDefense == true && IsDefensing == false)
        {
            IsDefensing = true;
            DefenseImage.SetActive(true);
            GameManager.Instance.defense += (GameManager.Instance.maxHp / 50) - 1;
        }
        else if(IsDefense == false && IsDefensing == true)
        {
            IsDefensing = false;
            DefenseImage.SetActive(false);
            GameManager.Instance.defense -= (GameManager.Instance.maxHp / 50) - 1;
        }
    }

    void RayCasting()
    {
        Debug.DrawRay(this.transform.position, Vector3.right * 50, Color.red);
        var rayHit = Physics2D.RaycastAll(transform.position, Vector3.right, 50);
        foreach (var hit in rayHit)
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Enemy = hit.collider.gameObject;
            }
        }
    }
    public void Playerattack()
    {
        if (BattleManager.Instance.IsPlayerTurn == true && GameManager.Instance.AttackOk == true && IsAttackOk == true)
        {
            IsAttackOk = false;
            StartCoroutine("PlayerAttack", 1f);
        }
    }
    public void Playerskill()
    {
        SkillButton.SetActive(true);
        if (GM.GetComponent<PlayerStats>().LV >= 10)
        {
            BarrierSkillButton[0].SetActive(true);
            BarrierSkillButton[1].SetActive(false);
            HealSkillButton[0].SetActive(true);
            HealSkillButton[1].SetActive(false);
        }
        else if (GM.GetComponent<PlayerStats>().LV >= 5 && GM.GetComponent<PlayerStats>().LV < 10)
        {
            BarrierSkillButton[0].SetActive(false);
            BarrierSkillButton[1].SetActive(true);
            HealSkillButton[0].SetActive(true);
            HealSkillButton[1].SetActive(false);
        }
        else
        {
            BarrierSkillButton[0].SetActive(false);
            BarrierSkillButton[1].SetActive(true);
            HealSkillButton[0].SetActive(false);
            HealSkillButton[1].SetActive(true);
        }
        GameManager.Instance.BattleButtonUi.SetActive(false);
    }
    public void PlayerItem()
    {       
        //StartCoroutine("PlayerAttack", 1f);
        
    }
    public void PlayerDeffence()
    {
        if (BattleManager.Instance.IsPlayerTurn == true && GameManager.Instance.AttackOk == true && IsAttackOk == true)
        {
            IsAttackOk = false;
            StartCoroutine("PlayerDefense", 1f);
        }
    }
    public void Barrier()
    {
        BarrierImage.transform.position = this.transform.position;
        if (BarrierCount >= MaxBarrierCount)
        {
            BarrierCount = 0;
            IsBarrier = false;
        }
        if (IsBarrier == true)
        {
            StartCoroutine(BarrierEffectIconFadeIn(0.5f));
            BarrierImage.SetActive(true);
        }
        else
        {
            BarrierImage.SetActive(false);
        }
    }
    public void Return()
    {
        SkillButton.SetActive(false);
        GameManager.Instance.BattleButtonUi.SetActive(true);
    }
    public void LockSkill()
    {
        if(IsAttackOk == true)
        {
            StartCoroutine(LockTextFalse());
            LockText.SetActive(true);
            IsAttackOk = false;
        }
    }
    IEnumerator LockTextFalse()
    {
        yield return new WaitForSeconds(2);
        LockText.SetActive(false);
        IsAttackOk = true;
        yield return null;
    }
    public void UseSkill()
    {
        if (BattleManager.Instance.IsPlayerTurn == true && GameManager.Instance.AttackOk == true && IsAttackOk == true)
        {
            if (GameManager.Instance.curMana >= 25)
            {
                GameManager.Instance.curMana -= 25;
                IsAttackSkill = true;
                IsAttackOk = false;
                StartCoroutine(PlayerSkill());
            }
            else
            {
                IsAttackOk = false;
                ManaText.SetActive(true);
                StartCoroutine(ManatextOff());
            }
        }
    }
    public void UseSkill2()
    {
        if (BattleManager.Instance.IsPlayerTurn == true && GameManager.Instance.AttackOk == true && IsAttackOk == true)
        {
            if (GameManager.Instance.curMana >= 40)
            {
                GameManager.Instance.curMana -= 40;
                IsAttackSkill = true;
                IsAttackOk = false;
                StartCoroutine(PlayerSkill2());
            }
            else
            {
                IsAttackOk = false;
                ManaText.SetActive(true);
                StartCoroutine(ManatextOff());
            }
        }
    }
    public void UseSkill3()
    {
        if (BattleManager.Instance.IsPlayerTurn == true && GameManager.Instance.AttackOk == true && IsAttackOk == true)
        {
            if (GameManager.Instance.curMana >= 55)
            {
                GameManager.Instance.curMana -= 55;
                IsAttackSkill = true;
                IsAttackOk = false;
                StartCoroutine(PlayerSkill3());
            }
            else
            {
                IsAttackOk = false;
                ManaText.SetActive(true);
                StartCoroutine(ManatextOff());
            }
        }
    }
    public IEnumerator PlayerHit()
    {
        IsHit = false;
        if (IsBarrier == true)
        {
            BarrierCount += 1;
            yield return new WaitForSeconds(1);
        }
        else
        {
            animator.SetBool("IsHit", true);
            yield return new WaitForSeconds(1);
            animator.SetBool("IsHit", false);
        }
        if(IsDefense == true)
        {
            IsDefense = false;
        }
        yield return null;
    }
    public IEnumerator ManatextOff()
    {
        ManaText.SetActive(true);
        yield return new WaitForSeconds(2);
        ManaText.SetActive(false);
        IsAttackOk = true;
        yield return null;
    }
    IEnumerator PlayerAttack()
    {
        IsContinuity = false;
        SkillButton.SetActive(false);
        GameManager.Instance.BattleButtonUi.SetActive(false);
        IsAttackOk = true;
        GameManager.Instance.PBattleSkillBackGround[0].SetActive(true);
        GameManager.Instance.PBattleSkillBackGroundText[0].text = "실험 : 불꽃 점화";
        BattleManager.Instance.IsPlayerTurn = false;
        GoToEnemy = true;
        yield return new WaitForSeconds(1f);
        BasicAttackCount++;
        BattleManager.Instance.CamP = true;
        animator.SetBool("IsAttack", true);
        GameObject DT = Instantiate(DmgText);
        GameObject DT2 = Instantiate(DmgText);
        GameObject DT3 = Instantiate(HealText);
        DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
        DT.transform.position = Enemy.transform.position;
        DT.GetComponent<BattleDamageText>().damage = GM.GetComponent<PlayerStats>().stats[1];
        Enemy.GetComponent<BattleBasicEnemy>().Hp -= GM.GetComponent<PlayerStats>().stats[1];
        Debug.Log("첫번째 공격");
        if (Enemy.GetComponent<BattleBasicEnemy>().IsReflect && GM.GetComponent<PlayerStats>().stats[1] == 1 && IsBarrier == false)
        {
            if (GameManager.Instance.damageabsorption < 1)
            {
                DT2.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT2.transform.position = this.transform.position;
                DT2.GetComponent<BattleDamageText>().damage = GM.GetComponent<PlayerStats>().stats[1] - GameManager.Instance.damageabsorption;
                GameManager.Instance.stackDamage += GM.GetComponent<PlayerStats>().stats[1] - GameManager.Instance.damageabsorption;
            }
            else
            {
                if(GameManager.Instance.damageabsorption != 0)
                {
                    DT3.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT3.transform.position = this.transform.position;
                    DT3.GetComponent<BattleDamageText>().damage = GameManager.Instance.damageabsorption - GM.GetComponent<PlayerStats>().stats[1];
                    GameManager.Instance.stackDamage += GameManager.Instance.damageabsorption - GM.GetComponent<PlayerStats>().stats[1];
                }
            }
        }
        else if (Enemy.GetComponent<BattleBasicEnemy>().IsReflect && GM.GetComponent<PlayerStats>().stats[1] >= 1 && IsBarrier == false)
        {
            if (GameManager.Instance.damageabsorption < GM.GetComponent<PlayerStats>().stats[1] / 2)
            {
                DT2.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT2.transform.position = this.transform.position;
                DT2.GetComponent<BattleDamageText>().damage = GM.GetComponent<PlayerStats>().stats[1] / 2 - GameManager.Instance.damageabsorption;
                GameManager.Instance.stackDamage += GM.GetComponent<PlayerStats>().stats[1] / 2 - GameManager.Instance.damageabsorption;
            }
            else
            {
                if (GameManager.Instance.damageabsorption != 0)
                {
                    DT3.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT3.transform.position = this.transform.position;
                    DT3.GetComponent<BattleDamageText>().damage = GameManager.Instance.damageabsorption - (GM.GetComponent<PlayerStats>().stats[1] / 2);
                    GameManager.Instance.stackDamage += GameManager.Instance.damageabsorption - (GM.GetComponent<PlayerStats>().stats[1] / 2);
                }
            }
        }
        else if(Enemy.GetComponent<BattleBasicEnemy>().IsReflect && IsBarrier == true)
        {
            if(GameManager.Instance.damageabsorption <= 0)
            {
                DT2.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT2.transform.position = this.transform.position;
                DT2.GetComponent<BattleDamageText>().damage = 0;
            }
            else
            {
                if (GameManager.Instance.damageabsorption != 0)
                {
                    DT3.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT3.transform.position = this.transform.position;
                    DT3.GetComponent<BattleDamageText>().damage = GameManager.Instance.damageabsorption;
                    GameManager.Instance.stackDamage -= GameManager.Instance.damageabsorption;
                }
            }
        }
        else
        {
            if (GameManager.Instance.damageabsorption != 0)
            {
                DT3.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT3.transform.position = this.transform.position;
                DT3.GetComponent<BattleDamageText>().damage = GameManager.Instance.damageabsorption;
                GameManager.Instance.stackDamage -= GameManager.Instance.damageabsorption;
            }
        }
        GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
        Enemy.GetComponent<BattleBasicEnemy>().IsHit = true;
        IsEnd = false;
        yield return new WaitForSeconds(1f);
        IsEnd = true;
        animator.SetBool("IsAttack", false);
        AttackCounting = 0;
        IsComBo = true;
        yield return new WaitForSeconds(0.5f); 
        IsComBo = false;
        GameManager.Instance.PBattleSkillBackGround[0].SetActive(false);
        if (IsContinuity == false || IsFaild == true || BattleManager.Instance.IsEnemyDead == true)
        {
            IsFaild = false;
            Debug.Log("마지막 실행");
            BasicAttackCount = 1;
            BattleManager.Instance.CamP = false;
            GoToEnemy = false;
            GoToReturn = true;
            yield return new WaitForSeconds(1);
            GoToReturn = false;
            yield return new WaitForSeconds(2);
            if (GameManager.Instance.IsCamMove == true)
            {
                BattleManager.Instance.IsEnemyTurn = true;
            }
            else
            {
                BattleManager.Instance.IsPlayerTurn = true;
            }
            yield return null;
        }
        else if(IsContinuity == true && IsFaild == false && BattleManager.Instance.IsEnemyDead == false)
        {
            StartCoroutine(PlayerAttack2());
            yield return null;
        }
    }
    IEnumerator PlayerAttack2()
    {
        IsContinuity = false;
        BattleManager.Instance.CamP = true;
        BasicAttackCount++;
        animator.SetBool("IsAttack", true);
        GameObject DT3 = Instantiate(DmgText);
        GameObject DT4 = Instantiate(DmgText);
        GameObject DT5 = Instantiate(HealText);
        DT3.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
        DT3.transform.position = Enemy.transform.position;
        DT3.GetComponent<BattleDamageText>().damage = GM.GetComponent<PlayerStats>().stats[1];
        Enemy.GetComponent<BattleBasicEnemy>().Hp -= GM.GetComponent<PlayerStats>().stats[1];
        Debug.Log("두번째 공격");
        if (Enemy.GetComponent<BattleBasicEnemy>().IsReflect && GM.GetComponent<PlayerStats>().stats[1] == 1 && IsBarrier == false)
        {
            if (GameManager.Instance.damageabsorption < 1)
            {
                DT4.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT4.transform.position = this.transform.position;
                DT4.GetComponent<BattleDamageText>().damage = GM.GetComponent<PlayerStats>().stats[1] - GameManager.Instance.damageabsorption;
                GameManager.Instance.stackDamage += GM.GetComponent<PlayerStats>().stats[1] - GameManager.Instance.damageabsorption;
            }
            else
            {
                if (GameManager.Instance.damageabsorption != 0)
                {
                    DT5.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT5.transform.position = this.transform.position;
                    DT5.GetComponent<BattleDamageText>().damage = GameManager.Instance.damageabsorption - GM.GetComponent<PlayerStats>().stats[1];
                    GameManager.Instance.stackDamage += GameManager.Instance.damageabsorption - GM.GetComponent<PlayerStats>().stats[1];
                }
            }
        }
        else if (Enemy.GetComponent<BattleBasicEnemy>().IsReflect && GM.GetComponent<PlayerStats>().stats[1] >= 1 && IsBarrier == false)
        {
            if (GameManager.Instance.damageabsorption < GM.GetComponent<PlayerStats>().stats[1] / 2)
            {
                DT4.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT4.transform.position = this.transform.position;
                DT4.GetComponent<BattleDamageText>().damage = GM.GetComponent<PlayerStats>().stats[1] / 2 - GameManager.Instance.damageabsorption;
                GameManager.Instance.stackDamage += GM.GetComponent<PlayerStats>().stats[1] / 2 - GameManager.Instance.damageabsorption;
            }
            else
            {
                if (GameManager.Instance.damageabsorption != 0)
                {
                    DT5.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT5.transform.position = this.transform.position;
                    DT5.GetComponent<BattleDamageText>().damage = GameManager.Instance.damageabsorption - (GM.GetComponent<PlayerStats>().stats[1] / 2);
                    GameManager.Instance.stackDamage += GameManager.Instance.damageabsorption - (GM.GetComponent<PlayerStats>().stats[1] / 2);
                }
            }
        }
        else if (Enemy.GetComponent<BattleBasicEnemy>().IsReflect && IsBarrier == true)
        {
            if (GameManager.Instance.damageabsorption <= 0)
            {
                DT4.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT4.transform.position = this.transform.position;
                DT4.GetComponent<BattleDamageText>().damage = 0;
            }
            else
            {
                if (GameManager.Instance.damageabsorption != 0)
                {
                    DT5.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT5.transform.position = this.transform.position;
                    DT5.GetComponent<BattleDamageText>().damage = GameManager.Instance.damageabsorption;
                    GameManager.Instance.stackDamage -= GameManager.Instance.damageabsorption;
                }
            }
        }
        else
        {
            if (GameManager.Instance.damageabsorption != 0)
            {
                DT5.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT5.transform.position = this.transform.position;
                DT5.GetComponent<BattleDamageText>().damage = GameManager.Instance.damageabsorption;
                GameManager.Instance.stackDamage -= GameManager.Instance.damageabsorption;
            }
        }
        GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
        Enemy.GetComponent<BattleBasicEnemy>().IsHit = true;
        IsEnd = false;
        yield return new WaitForSeconds(1f);
        IsEnd = true;
        animator.SetBool("IsAttack", false);
        AttackCounting = 0;
        IsComBo = true;
        yield return new WaitForSeconds(0.5f);
        IsComBo = false;
        if (IsContinuity == false || IsFaild == true || BattleManager.Instance.IsEnemyDead == true)
        {
            IsFaild = false;
            Debug.Log("마지막 실행");
            BasicAttackCount = 1;
            BattleManager.Instance.CamP = false;
            GameManager.Instance.BattleSkillBackGround.SetActive(false); 
            GoToEnemy = false;
            GoToReturn = true;
            yield return new WaitForSeconds(1);
            GoToReturn = false;
            yield return new WaitForSeconds(2);
            if (GameManager.Instance.IsCamMove == true)
            {
                BattleManager.Instance.IsEnemyTurn = true;
            }
            else
            {
                BattleManager.Instance.IsPlayerTurn = true;
            }
            yield return null;
        }
        else if(IsContinuity == true && IsFaild == false && BattleManager.Instance.IsEnemyDead == false)
        {
            StartCoroutine(PlayerAttack3());
            yield return null;
        }
    }
    IEnumerator PlayerAttack3()
    {
        IsContinuity = false;
        BattleManager.Instance.CamP = true;
        BasicAttackCount++;
        animator.SetBool("IsAttack", true);
        GameObject DT5 = Instantiate(DmgText);
        GameObject DT6 = Instantiate(DmgText);
        GameObject DT7 = Instantiate(HealText);
        DT5.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
        DT5.transform.position = Enemy.transform.position;
        DT5.GetComponent<BattleDamageText>().damage = GM.GetComponent<PlayerStats>().stats[1] + GM.GetComponent<PlayerStats>().stats[1] / 2;
        Enemy.GetComponent<BattleBasicEnemy>().Hp -= GM.GetComponent<PlayerStats>().stats[1] + GM.GetComponent<PlayerStats>().stats[1] / 2;
        GameManager.Instance.stackDamage += GameManager.Instance.damageabsorption;
        Debug.Log("세번째 공격");
        if (Enemy.GetComponent<BattleBasicEnemy>().IsReflect && GM.GetComponent<PlayerStats>().stats[1] == 1 && IsBarrier == false)
        {
            if (GameManager.Instance.damageabsorption < 1)
            {
                DT6.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT6.transform.position = this.transform.position;
                DT6.GetComponent<BattleDamageText>().damage = GM.GetComponent<PlayerStats>().stats[1] - GameManager.Instance.damageabsorption;
                GameManager.Instance.stackDamage += GM.GetComponent<PlayerStats>().stats[1] - GameManager.Instance.damageabsorption;
            }
            else
            {
                if (GameManager.Instance.damageabsorption != 0)
                {
                    DT7.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT7.transform.position = this.transform.position;
                    DT7.GetComponent<BattleDamageText>().damage = GameManager.Instance.damageabsorption - GM.GetComponent<PlayerStats>().stats[1];
                    GameManager.Instance.stackDamage += GameManager.Instance.damageabsorption - GM.GetComponent<PlayerStats>().stats[1];
                }
            }
        }
        else if (Enemy.GetComponent<BattleBasicEnemy>().IsReflect && GM.GetComponent<PlayerStats>().stats[1] >= 1 && IsBarrier == false)
        {
            if (GameManager.Instance.damageabsorption < GM.GetComponent<PlayerStats>().stats[1] / 2)
            {
                DT6.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT6.transform.position = this.transform.position;
                DT6.GetComponent<BattleDamageText>().damage = GM.GetComponent<PlayerStats>().stats[1] / 2 - GameManager.Instance.damageabsorption;
                GameManager.Instance.stackDamage += GM.GetComponent<PlayerStats>().stats[1] / 2 - GameManager.Instance.damageabsorption;
            }
            else
            {
                if (GameManager.Instance.damageabsorption != 0)
                {
                    DT7.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT7.transform.position = this.transform.position;
                    DT7.GetComponent<BattleDamageText>().damage = GameManager.Instance.damageabsorption - (GM.GetComponent<PlayerStats>().stats[1] / 2);
                    GameManager.Instance.stackDamage += GameManager.Instance.damageabsorption - (GM.GetComponent<PlayerStats>().stats[1] / 2);
                }
            }
        }
        else if (Enemy.GetComponent<BattleBasicEnemy>().IsReflect && IsBarrier == true)
        {
            if (GameManager.Instance.damageabsorption <= 0)
            {
                DT6.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT6.transform.position = this.transform.position;
                DT6.GetComponent<BattleDamageText>().damage = 0;
            }
            else
            {
                if (GameManager.Instance.damageabsorption != 0)
                {
                    DT7.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT7.transform.position = this.transform.position;
                    DT7.GetComponent<BattleDamageText>().damage = GameManager.Instance.damageabsorption;
                    GameManager.Instance.stackDamage -= GameManager.Instance.damageabsorption;
                }
            }
        }
        else
        {
            if (GameManager.Instance.damageabsorption != 0)
            {
                DT7.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT7.transform.position = this.transform.position;
                DT7.GetComponent<BattleDamageText>().damage = GameManager.Instance.damageabsorption;
                GameManager.Instance.stackDamage -= GameManager.Instance.damageabsorption;
            }
        }
        GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(1.4f);
        Enemy.GetComponent<BattleBasicEnemy>().IsHit = true;
        yield return new WaitForSeconds(1f);
        animator.SetBool("IsAttack", false);
        AttackCounting = 0;
        Debug.Log("마지막 실행");
        BasicAttackCount = 1;
        BattleManager.Instance.CamP = false;
        GameManager.Instance.BattleSkillBackGround.SetActive(false);
        GoToEnemy = false;
        GoToReturn = true;
        yield return new WaitForSeconds(1f);
        GoToReturn = false;
        yield return new WaitForSeconds(2);
        if (GameManager.Instance.IsCamMove == true)
        {
            BattleManager.Instance.IsEnemyTurn = true;
        }
        else
        {
            BattleManager.Instance.IsPlayerTurn = true;
        }
        yield return null;
    }
    void Attackcounting()
    {
        if(IsAttackOk == true && BasicAttackCount >= 2 && IsContinuity == false && IsComBo == true)
        {
            animator.SetBool("IsWalk", false);
            AttackCounting += Time.deltaTime;
        }
    }
    IEnumerator PlayerSkill()
    {
        GameManager.Instance.BattleButtonUi.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        GameObject.Find("Main Camera").GetComponent<CameraMove>().IsFarAway = true;
        SkillButton.SetActive(false);
        IsAttackOk = true;
        GameManager.Instance.PBattleSkillBackGround[2].SetActive(true);
        GameManager.Instance.PBattleSkillBackGroundText[2].text = "실험 : 불꽃 발사";
        BattleManager.Instance.IsPlayerTurn = false;
        GoToEnemy = true;
        yield return new WaitForSeconds(1f);
        BattleManager.Instance.CamP = true;
        SkillImage.SetActive(true);
        animator.SetBool("IsSkill", true);
        GameObject DT = Instantiate(DmgText);
        GameObject DT2 = Instantiate(HealText);
        DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
        DT.transform.position = Enemy.transform.position;
        DT.GetComponent<BattleDamageText>().damage = GM.GetComponent<PlayerStats>().stats[1] * 4;
        Enemy.GetComponent<BattleBasicEnemy>().Hp -= GM.GetComponent<PlayerStats>().stats[1] * 4;
        if (GameManager.Instance.damageabsorption != 0)
        {
            DT2.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
            DT2.transform.position = Enemy.transform.position;
            DT2.GetComponent<BattleDamageText>().damage = GameManager.Instance.damageabsorption;
            GameManager.Instance.stackDamage += GameManager.Instance.damageabsorption;
        }
        GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
        Enemy.GetComponent<BattleBasicEnemy>().IsHit = true;
        yield return new WaitForSeconds(1);
        GameObject.Find("Main Camera").GetComponent<CameraMove>().IsFarAway = false;
        BattleManager.Instance.CamP = false;
        SkillImage.SetActive(false);
        animator.SetBool("IsSkill", false);
        GameManager.Instance.PBattleSkillBackGround[2].SetActive(false);
        GoToEnemy = false;
        GoToReturn = true;
        yield return new WaitForSeconds(1);
        GoToReturn = false;
        yield return new WaitForSeconds(2);
        IsAttackSkill = false;
        if (GameManager.Instance.IsCamMove == true)
        {
            BattleManager.Instance.IsEnemyTurn = true;
        }
        else
        {
            BattleManager.Instance.IsPlayerTurn = true;
        }
        yield return null;
    }
    IEnumerator PlayerSkill2()
    {
        GameManager.Instance.BattleButtonUi.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        GameObject.Find("Main Camera").GetComponent<CameraMove>().IsFarAway = true;
        SkillButton.SetActive(false);
        IsAttackOk = true;
        GameManager.Instance.PBattleSkillBackGround[3].SetActive(true);
        GameManager.Instance.PBattleSkillBackGroundText[3].text = "실험 : 상처 재생";
        BattleManager.Instance.IsPlayerTurn = false;
        yield return new WaitForSeconds(1.3f);
        BattleManager.Instance.CamE = true;
        animator.SetBool("IsHeal", true);
        GameObject DT = Instantiate(HealText);
        if (GameManager.Instance.stackDamage - GameManager.Instance.maxHp / 5 <= 0)
        {
            DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
            DT.transform.position = this.transform.position;
            DT.GetComponent<BattleDamageText>().damage = GameManager.Instance.stackDamage;
            GameManager.Instance.stackDamage -= GameManager.Instance.stackDamage;
        }
        else
        {
            DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
            DT.transform.position = this.transform.position;
            DT.GetComponent<BattleDamageText>().damage = GameManager.Instance.maxHp / 5;
            GameManager.Instance.stackDamage -= GameManager.Instance.maxHp / 5;
        }
        yield return new WaitForSeconds(1);
        GameObject.Find("Main Camera").GetComponent<CameraMove>().IsFarAway = false;
        BattleManager.Instance.CamE = false;
        animator.SetBool("IsHeal", false);
        GameManager.Instance.PBattleSkillBackGround[3].SetActive(false);
        yield return new WaitForSeconds(4);
        IsAttackSkill = false;
        if (GameManager.Instance.IsCamMove == true)
        {
            BattleManager.Instance.IsEnemyTurn = true;
        }
        else
        {
            BattleManager.Instance.IsPlayerTurn = true;
        }
        yield return null;
    }
    IEnumerator PlayerSkill3()
    {
        GameManager.Instance.BattleButtonUi.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        GameObject.Find("Main Camera").GetComponent<CameraMove>().IsFarAway = true;
        SkillButton.SetActive(false);
        IsAttackOk = true;
        GameManager.Instance.PBattleSkillBackGround[4].SetActive(true);
        GameManager.Instance.PBattleSkillBackGroundText[4].text = "실험 : 피해 무효";
        BattleManager.Instance.IsPlayerTurn = false;
        yield return new WaitForSeconds(1.5f);
        BattleManager.Instance.CamE = true;
        animator.SetBool("IsHyperDefense", true);
        IsBarrier = true;
        BarrierCount = 0;
        yield return new WaitForSeconds(1);
        BattleManager.Instance.CamE = false;
        animator.SetBool("IsHyperDefense", false);
        GameManager.Instance.PBattleSkillBackGround[4].SetActive(false);
        GameObject.Find("Main Camera").GetComponent<CameraMove>().IsFarAway = false;
        yield return new WaitForSeconds(3);
        IsAttackSkill = false;
        if (GameManager.Instance.IsCamMove == true)
        {
            BattleManager.Instance.IsEnemyTurn = true;
        }
        else
        {
            BattleManager.Instance.IsPlayerTurn = true;
        }
        yield return null;
    }
    IEnumerator PlayerDefense()
    {
        GameManager.Instance.BattleButtonUi.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        GameObject.Find("Main Camera").GetComponent<CameraMove>().IsFarAway = true;
        SkillButton.SetActive(false);
        IsAttackOk = true;
        GameManager.Instance.PBattleSkillBackGround[1].SetActive(true);
        GameManager.Instance.PBattleSkillBackGroundText[1].text = "실험 : 피해 감소";
        BattleManager.Instance.IsPlayerTurn = false;
        yield return new WaitForSeconds(1);
        BattleManager.Instance.CamE = true;
        animator.SetBool("IsDefense", true);
        IsDefense = true;
        yield return new WaitForSeconds(1.5f);
        BattleManager.Instance.CamE = false;
        GameObject.Find("Main Camera").GetComponent<CameraMove>().IsFarAway = false;
        animator.SetBool("IsDefense", false);
        GameManager.Instance.PBattleSkillBackGround[1].SetActive(false);
        yield return new WaitForSeconds(2);
        IsAttackSkill = false;
        if (GameManager.Instance.IsCamMove == true)
        {
            BattleManager.Instance.IsEnemyTurn = true;
        }
        else
        {
            BattleManager.Instance.IsPlayerTurn = true;
        }
        yield return null;
    }
}