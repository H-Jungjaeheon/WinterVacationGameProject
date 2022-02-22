using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleFinalBoss : BattleBasicEnemy
{
    [SerializeField] float SuperAngerCount, MaxSuperAngerCount, InstantDeathCount, MaxInstantDeathCount, EyeSpawnCount = 0, MaxEyeSpawnCount = 0; //각성 카운트, 즉사 패턴 카운트
    [SerializeField] bool IsSuperAnger, IsFarAway,IsUseSkill; //각성 판별, 원거리 공격 판별, 즉사 패턴 이미지 판별
    [SerializeField] int RandBasicAttack, RandSkill, PoisonCount, SuperAnger, MaxPoisonCount; //(기본 공격 랜덤, 스킬 공격 랜덤, 독 공격 카운터, 체력 흡수 카운터
    [SerializeField] GameObject HealText, Warning; //체력 회복 텍스트
    [SerializeField] GameObject[] InstantEye;
    public Image Poison, SuperAngerBar, InstantDeathBar, MaxInstantDeathBar, InstantImage, Eye, SuperEye, PoisonEffect; //각성 바, 즉사 패턴 바(빈 즉사 패턴 바) 즉사 패턴 이미지
    [SerializeField] Vector2 PoisonEffectTransform;

    public override void Start()
    {
        GameObject.Find("Main Camera").GetComponent<CameraMove>().IsBossDeadSkill = true;
        Warning.SetActive(false);
        animator = GetComponent<Animator>();
        IsPoison = false;
        IsInstantDead = false;
        Anger = 0;
        MaxHp *= GameManager.Instance.Stage;
        Hp *= GameManager.Instance.Stage;
        SR = this.GetComponent<SpriteRenderer>();
        this.transform.position = EnemySpawner.transform.position + new Vector3(2, 3.5f, 0);
        SuperAnger = 0;
        IsSuperAnger = false;
        IsUseSkill = false;
        Invoke("BossCam", 3.5f);
        MaxEyeSpawnCount = Random.Range(5, 7);
    }

    public override void Update()
    {
        base.Update();
        if (InstantDeathCount <= 20)
        {
            IsUseSkill = false;
        }
        else
        {
            IsUseSkill = true;
        }
        PoisonUse();
        SuperAngers();
        StartCoroutine(FinalSkill());
        if(IsInstantDead == true)
        {
            Destroy(GetComponent<BattleEye>());
        }
    }
    public override void AttackGone()
    {
        if (GoToPlayer == true && BattleManager.Instance.IsPlayerTurn == false && StopGone == false && IsFarAway == false)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, Player.transform.position + new Vector3(3, 2f, 0), 10 * Time.deltaTime);
        }
        else if (GoToPlayer == true && BattleManager.Instance.IsPlayerTurn == false && StopGone == false && IsFarAway == true)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, Player.transform.position + new Vector3(5, 2.5f, 0), 10 * Time.deltaTime);
        }
        else if (GoToReturn == true)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, EnemySpawner.transform.position + new Vector3(2, 3.5f, 0), 10 * Time.deltaTime); //
        }
    }
    IEnumerator SuperEyeColor(float FaidTime)
    {
        Debug.Log("실행");
        Color color = SuperEye.color;
        while (color.a < 1f)
        {
            color.a += Time.deltaTime / FaidTime;
            Debug.Log("실행2");
            SuperEye.color = color;
            if (color.a >= 1f)
            {
                color.a = 1f;              
            }
            yield return Time.deltaTime;
        }
    }
    
    IEnumerator FinalSkill()
    {
        Color color = SuperEye.color;
        Camera.main.GetComponent<CameraMove>().FinalSkillCount = InstantDeathCount;
        float randomX = Random.Range(-20f, 8f);
        float randomY = Random.Range(-6f, 14f);
        if (IsUseSkill == true)
        {
            EyeSpawnCount += Time.deltaTime;
        }
        if (InstantDeathCount <= 40 && InstantDeathCount > 20 && IsUseSkill == true)
        {
            if (EyeSpawnCount >= MaxEyeSpawnCount && IsUseSkill == true)
            {
                color.a = 0f;
                SuperEye.color = color;
                MaxEyeSpawnCount = Random.Range(5, 7);
                EyeSpawnCount = 0;
                Instantiate(InstantEye[2], Camera.main.WorldToScreenPoint(this.transform.position + new Vector3(randomX, randomY, 0)), Quaternion.Euler(0, 0, 0), GameObject.Find("Canvas2").transform);
                randomX = Random.Range(-10f, 4f);
                randomY = Random.Range(-4f, 10f);
                yield return new WaitForSeconds(2);
                Instantiate(InstantEye[2], Camera.main.WorldToScreenPoint(this.transform.position + new Vector3(randomX, randomY, 0)), Quaternion.Euler(0, 0, 0), GameObject.Find("Canvas2").transform);
            }
        }
        else if (InstantDeathCount <= 80 && InstantDeathCount > 40 && IsUseSkill == true)
        {
            color.a = 0.1f;
            SuperEye.color = color;
            yield return null;
            if (EyeSpawnCount >= MaxEyeSpawnCount && IsUseSkill == true)
            {
                MaxEyeSpawnCount = Random.Range(4, 6);
                EyeSpawnCount = 0;
                Instantiate(InstantEye[1], Camera.main.WorldToScreenPoint(this.transform.position + new Vector3(randomX, randomY, 0)), Quaternion.Euler(0, 0, 0), GameObject.Find("Canvas2").transform);
                randomX = Random.Range(-10f, 4f);
                randomY = Random.Range(-4f, 10f);
                yield return new WaitForSeconds(2);
                Instantiate(InstantEye[1], Camera.main.WorldToScreenPoint(this.transform.position + new Vector3(randomX, randomY, 0)), Quaternion.Euler(0, 0, 0), GameObject.Find("Canvas2").transform);
            }
        }
        else if (InstantDeathCount <= 100 && InstantDeathCount > 80 && IsUseSkill == true)
        {
            color.a = 0.17f;
            SuperEye.color = color;
            yield return null;
            if (EyeSpawnCount >= MaxEyeSpawnCount && IsUseSkill == true)
            {
                MaxEyeSpawnCount = 4;
                EyeSpawnCount = 0;
                Instantiate(InstantEye[0], Camera.main.WorldToScreenPoint(this.transform.position + new Vector3(randomX, randomY, 0)), Quaternion.Euler(0, 0, 0), GameObject.Find("Canvas2").transform);
                randomX = Random.Range(-10f, 4f);
                randomY = Random.Range(-4f, 10f);
                yield return new WaitForSeconds(1);
                Instantiate(InstantEye[0], Camera.main.WorldToScreenPoint(this.transform.position + new Vector3(randomX, randomY, 0)), Quaternion.Euler(0, 0, 0), GameObject.Find("Canvas2").transform);
                randomX = Random.Range(-10f, 4f);
                randomY = Random.Range(-4f, 10f);
                yield return new WaitForSeconds(0.5f);
                Instantiate(InstantEye[0], Camera.main.WorldToScreenPoint(this.transform.position + new Vector3(randomX, randomY, 0)), Quaternion.Euler(0, 0, 0), GameObject.Find("Canvas2").transform);
            }
        }
        else
        {
            color.a = 0;
            SuperEye.color = color;
            yield return null;
        }
        if(InstantDeathCount >= MaxInstantDeathCount)
        {
            InstantDeathCount = MaxInstantDeathCount;
        }
        yield return null;
    }
    void SuperAngers()
    {
        if (SuperAnger == 5)
        {
            IsSuperAnger = false;
            SuperAnger = 0;
        }
    }
    IEnumerator WarningTXT()
    {
        yield return null;
    }
    void BossCam()
    {
        GameObject.Find("Main Camera").GetComponent<CameraMove>().BossBattleStart = true;
        GameObject.Find("Main Camera").GetComponent<CameraMove>().IsLastBoss = true;
    }
    public override void Hpbar()
    {
        HpBar.fillAmount = Hp / MaxHp;
        AngerBar.fillAmount = Anger / MaxAnger;
        SuperAngerBar.fillAmount = SuperAngerCount / MaxSuperAngerCount;
        InstantDeathBar.fillAmount = InstantDeathCount / MaxInstantDeathCount;
        HpBar.transform.position = new Vector3(2f, BarUp + 72.95f, 0);
        AngerBar.transform.position = new Vector3(2f, BarUp + 72.25f, 0);
        HpBarNull.transform.position = new Vector3(2f, BarUp + 72.95f, 0);
        SuperAngerBar.transform.position = new Vector3(2f, BarUp + 73.6f, 0);
        EnemyPicture.transform.position = new Vector3(-5.05f, BarUp + 72.9f, 0);
        InstantDeathBar.transform.position = new Vector3(1.65f, BarUp + 67f, 0);
        MaxInstantDeathBar.transform.position = new Vector3(1.65f, BarUp + 67, 0);
        InstantImage.transform.position = new Vector3(1.65f, BarUp + 66.05f, 0);
        Eye.transform.position = new Vector3(-8.5f, BarUp + 61f, 0);
        SuperEye.transform.position = new Vector3(0f, BarUp + 60f, 0);
        Warning.transform.position = Player.transform.position + new Vector3(6, 4, 0);
        PoisonEffect.transform.position = new Vector2(Player.transform.position.x + PoisonEffectTransform.x, Player.transform.position.y + PoisonEffectTransform.y);
    }
    public override void RayCasting()
    {
        base.RayCasting();
    }
    public override IEnumerator EnemyHit()
    {
        return base.EnemyHit();
    }
    public override void Dead1()
    {
        base.Dead1();
    }
    void PoisonUse()
    {
        Poison.transform.position = Player.transform.position + new Vector3(0, 3, 0);
        if (IsPoison == true)
        {
            StartCoroutine(PoisonIconFadeIn(0.5f));
        }
        else
        {
            StartCoroutine(PoisonIconFadeOut(0.5f));
        }
        if (PoisonCount > MaxPoisonCount)
        {
            PoisonCount = 0;
            IsPoison = false;
        }
    }
    IEnumerator PoisonIconFadeIn(float FaidTime)
    {
        Color color = Poison.color;
        while (color.a < 1f)
        {
            color.a += Time.deltaTime / FaidTime;
            Poison.color = color;
            if (color.a >= 1f)
            {
                color.a = 1f;
            }
            yield return null;
        }
    }
    IEnumerator PoisonIconFadeOut(float FaidTime)
    {
        Color color = Poison.color;
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / FaidTime;
            Poison.color = color;
            if (color.a <= 0f)
            {
                color.a = 0f;
            }
            yield return null;
        }
    }
    IEnumerator PoisonEffectFadeIn(float FaidTime)
    {
        Color color = PoisonEffect.color;
        while (color.a < 1f)
        {
            color.a += Time.deltaTime / FaidTime;
            PoisonEffect.color = color;
            if (color.a >= 1f)
            {
                color.a = 1f;
            }
            yield return null;
        }
    }
    IEnumerator PoisonEffectFadeOut(float FaidTime)
    {
        Color color = PoisonEffect.color;
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / FaidTime;
            PoisonEffect.color = color;
            if (color.a <= 0f)
            {
                color.a = 0f;
            }
            yield return null;
        }
    }
    public override IEnumerator Dead2(float FaidTime)
    {
        yield return new WaitForSeconds(4);
        if (Dead == false)
        {
            Dead = true;
            GameObject.Find("GameManager").GetComponent<PlayerStats>().ExpUp(GExp);
        }
        BattleManager.Instance.IsEnemyTurn = false;
        Color color = SR.color;
        Color color2 = HpBar.color;
        Color color3 = HpBarNull.color;
        Color color4 = EnemyPicture.color;
        Color color5 = AngerBar.color;
        while (color.a > 0f && color2.a > 0f && color3.a > 0f && color4.a > 0f) //죽을 때 색 대신 그래픽 넣기 
        {
            color.a -= Time.deltaTime / FaidTime;
            color2.a -= Time.deltaTime / FaidTime;
            color3.a -= Time.deltaTime / FaidTime;
            color4.a -= Time.deltaTime / FaidTime;
            color5.a -= Time.deltaTime / FaidTime;
            SR.color = color;
            HpBar.color = color;
            HpBarNull.color = color;
            EnemyPicture.color = color;
            AngerBar.color = color;
            if (color.a <= 0f)
            {
                color.a = 0f;
                color2.a = 0f;
                color3.a = 0f;
                color4.a = 0f;
                color5.a = 0f;
            }
            else
            {
                yield return null;
                yield return new WaitForSeconds(1);
                GameManager.Instance.IsBattleStart = false;
                BattleManager.Instance.IsEnemyTurn = true;
                BattleManager.Instance.IsPlayerTurn = true;
                yield return new WaitForSeconds(1);
                Destroy(this.gameObject);
                //엔딩 씬 전환
                SceneManager.LoadScene("Ending");
            }
            
        }
    }
    public override IEnumerator EnemyAttack()
    {
        if (IsPoison == false) //두번째 기본 스킬 중복 방지
        {
            RandBasicAttack = Random.Range(1, 3);
        }
        else
        {
            RandBasicAttack = Random.Range(1, 2);
        }
        RandSkill = Random.Range(1, 3); //스킬 랜덤값 뽑기
        GameManager.Instance.BattleSkillBackGround.SetActive(true);
        if (Anger < MaxAnger && InstantDeathCount < MaxInstantDeathCount && RandBasicAttack == 1 && SuperAngerCount < MaxSuperAngerCount)
        {
            IsFarAway = true;
            GameManager.Instance.BattleSkillText.text = "파멸로 이끄는 손";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsAttack", true);
            StopGone = true;
            transform.position = this.transform.position + new Vector3(-1f, -0.8f, 0);
            GameObject DT = Instantiate(DmgText);
            GameObject DT2 = Instantiate(HealText);
            if (Player.GetComponent<BattlePlayer>().IsBarrier == false)
            {
                if (IsSuperAnger == true)
                {
                    DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT.transform.position = Player.transform.position;
                    DT.GetComponent<BattleDamageText>().damage = Damage - GameManager.Instance.defense;
                    DT2.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT2.transform.position = this.transform.position;
                    DT2.GetComponent<BattleDamageText>().damage = Damage / 2;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += Damage - GameManager.Instance.defense;
                    Hp += Damage / 2;
                }
                else
                {
                    DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT.transform.position = Player.transform.position;
                    DT.GetComponent<BattleDamageText>().damage = Damage - GameManager.Instance.defense;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += Damage - GameManager.Instance.defense;
                }
            }
            else
            {
                if (IsSuperAnger == true)
                {
                    DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT.transform.position = Player.transform.position;
                    DT.GetComponent<BattleDamageText>().damage = 0;
                    DT2.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT2.transform.position = this.transform.position;
                    DT2.GetComponent<BattleDamageText>().damage = 0;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                }
                else
                {
                    DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT.transform.position = Player.transform.position;
                    DT.GetComponent<BattleDamageText>().damage = 0;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                }
            }
            yield return new WaitForSeconds(2f); //독 공격 딜레이
            transform.position = this.transform.position + new Vector3(1f, 0.8f, 0);
            StopGone = false;
            animator.SetBool("IsAttack", false);
            BattleManager.Instance.CamE = false;
            GoToReturn = true;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            GoToPlayer = false;
            Anger += 30;
            if (IsSuperAnger == true)
            {
                SuperAnger++;
            }
            else
            {
                SuperAngerCount += 10;
            }
            if (IsPoison == false)
            {
                yield return new WaitForSeconds(2);
            }
            else
            {
                GameObject DT3 = Instantiate(DmgText);
                Player.GetComponent<BattlePlayer>().IsHit = true;
                DT3.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT3.transform.position = Player.transform.position;
                DT3.GetComponent<BattleDamageText>().damage = (Damage / 3);
                GameManager.Instance.stackDamage += Damage / 3;
                PoisonCount++;
                yield return new WaitForSeconds(2);
            }
            GoToReturn = false;
            IsFarAway = false;
            BattleManager.Instance.IsPlayerTurn = true;
            GameManager.Instance.BattleButtonUi.SetActive(true);
        }
        else if (Anger < MaxAnger && InstantDeathCount < MaxInstantDeathCount && RandBasicAttack == 2 && SuperAngerCount < MaxSuperAngerCount)
        {
            GameManager.Instance.BattleSkillText.text = "고통의 원인";
            BattleManager.Instance.IsEnemyTurn = false;
            yield return new WaitForSeconds(1f);
            StartCoroutine(PoisonEffectFadeIn(0.5f));
            animator.SetBool("IsAttack2", true);
            BattleManager.Instance.CamP = true;
            GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
            IsPoison = true;
            yield return new WaitForSeconds(1f);
            StartCoroutine(PoisonEffectFadeOut(0.5f));
            animator.SetBool("IsAttack2", false);
            BattleManager.Instance.CamP = false;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            Anger += 30;
            if (IsSuperAnger == true)
            {
                SuperAnger++;
            }
            else
            {
                SuperAngerCount += 10;
            }
            if (IsPoison == false)
            {
                yield return new WaitForSeconds(2);
            }
            else
            {
                yield return new WaitForSeconds(2);
                GameObject DT3 = Instantiate(DmgText);
                Player.GetComponent<BattlePlayer>().IsHit = true;
                DT3.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT3.transform.position = Player.transform.position;
                DT3.GetComponent<BattleDamageText>().damage = (Damage / 3);
                GameManager.Instance.stackDamage += Damage / 3;
                PoisonCount++;
                yield return new WaitForSeconds(1);
            }
            BattleManager.Instance.IsPlayerTurn = true;
            GameManager.Instance.BattleButtonUi.SetActive(true);
        }
        else if (Anger >= MaxAnger && InstantDeathCount < MaxInstantDeathCount && RandSkill == 1 && SuperAngerCount < MaxSuperAngerCount)
        {
            Anger = 0;
            IsFarAway = true;
            GameManager.Instance.BattleSkillText.text = "뮌하우젠 증후군";
            BattleManager.Instance.IsEnemyTurn = false;
            yield return new WaitForSeconds(1f);
            BattleManager.Instance.CamP = true;
            animator.SetBool("IsSkill", true);
            GameObject DT = Instantiate(DmgText);
            GameObject DT2 = Instantiate(HealText);
            InstantDeathCount += 30;
            if (Player.GetComponent<BattlePlayer>().IsBarrier == false)
            {
                if (IsSuperAnger == true)
                {
                    DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT.transform.position = Player.transform.position;
                    DT.GetComponent<BattleDamageText>().damage = (Damage / 2 + 3) - GameManager.Instance.defense;
                    DT2.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT2.transform.position = this.transform.position;
                    DT2.GetComponent<BattleDamageText>().damage = (Damage / 2) + 3;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += (Damage / 2 + 3) - GameManager.Instance.defense;
                    GameManager.Instance.curMana -= 45;
                    Hp += (Damage / 2) + 3;
                }
                else
                {
                    DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT.transform.position = Player.transform.position;
                    DT.GetComponent<BattleDamageText>().damage = (Damage / 2 + 3) - GameManager.Instance.defense;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += (Damage / 2 + 3) - GameManager.Instance.defense;
                    GameManager.Instance.curMana -= 45;
                }
            }
            else
            {
                if (IsSuperAnger == true)
                {
                    DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT.transform.position = Player.transform.position;
                    DT.GetComponent<BattleDamageText>().damage = 0;
                    DT2.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT2.transform.position = this.transform.position;
                    DT2.GetComponent<BattleDamageText>().damage = 0;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                }
                else
                {
                    DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT.transform.position = Player.transform.position;
                    DT.GetComponent<BattleDamageText>().damage = 0;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                }
            }
            yield return new WaitForSeconds(1f);
            animator.SetBool("IsSkill", false);
            BattleManager.Instance.CamP = false;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            if (IsPoison == false)
            {
                yield return new WaitForSeconds(2);
            }
            else
            {
                yield return new WaitForSeconds(2);
                GameObject DT3 = Instantiate(DmgText);
                Player.GetComponent<BattlePlayer>().IsHit = true;
                DT3.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT3.transform.position = Player.transform.position;
                DT3.GetComponent<BattleDamageText>().damage = (Damage / 3);
                GameManager.Instance.stackDamage += Damage / 3;
                PoisonCount++;
                yield return new WaitForSeconds(1);
            }
            BattleManager.Instance.IsPlayerTurn = true;
            GameManager.Instance.BattleButtonUi.SetActive(true);
        }
        else if (Anger >= MaxAnger && InstantDeathCount < MaxInstantDeathCount && RandSkill == 2 && SuperAngerCount < MaxSuperAngerCount)
        {
            Anger = 0;
            IsFarAway = true;
            GameManager.Instance.BattleSkillText.text = "이계의 검";
            BattleManager.Instance.IsEnemyTurn = false;
            yield return new WaitForSeconds(1.5f);
            animator.SetBool("IsSkill2-1", true); //애니메이션 검 소환
            yield return new WaitForSeconds(1.5f);
            animator.SetBool("IsSkill2-1", false); //애니메이션 검 소환 비활성화
            animator.SetBool("IsSkill2-2", true); //애니메이션 검 휘두르기
            transform.position = EnemySpawner.transform.position + new Vector3(-7, 2.5f, 0);
            BattleManager.Instance.CamE = true;
            GameObject DT = Instantiate(DmgText);
            GameObject DT2 = Instantiate(HealText);
            if (Player.GetComponent<BattlePlayer>().IsBarrier == false)
            {
                if (IsSuperAnger == true)
                {
                    DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT.transform.position = Player.transform.position;
                    DT.GetComponent<BattleDamageText>().damage = (Damage * 2) - GameManager.Instance.defense;
                    DT2.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT2.transform.position = this.transform.position;
                    DT2.GetComponent<BattleDamageText>().damage = Damage;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(1f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += (Damage * 2) - GameManager.Instance.defense;
                    Hp += Damage;
                }
                else
                {
                    DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT.transform.position = Player.transform.position;
                    DT.GetComponent<BattleDamageText>().damage = Damage - GameManager.Instance.defense;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(1f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += Damage - GameManager.Instance.defense;
                }
            }
            else
            {
                if (IsSuperAnger == true)
                {
                    DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT.transform.position = Player.transform.position;
                    DT.GetComponent<BattleDamageText>().damage = 0;
                    DT2.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT2.transform.position = this.transform.position;
                    DT2.GetComponent<BattleDamageText>().damage = 0;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(1f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                }
                else
                {
                    DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT.transform.position = Player.transform.position;
                    DT.GetComponent<BattleDamageText>().damage = 0;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(1f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                }
            }
            yield return new WaitForSeconds(2f);
            transform.position = EnemySpawner.transform.position + new Vector3(2, 3.5f, 0);
            animator.SetBool("IsSkill2-2", false);
            BattleManager.Instance.CamE = false;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            InstantDeathCount += 30;
            if (IsSuperAnger == true)
            {
                SuperAnger++;
            }
            else
            {
                SuperAngerCount += 10;
            }
            if (IsPoison == false)
            {
                yield return new WaitForSeconds(2);
            }
            else
            {
                yield return new WaitForSeconds(2);
                GameObject DT3 = Instantiate(DmgText);
                Player.GetComponent<BattlePlayer>().IsHit = true;
                DT3.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT3.transform.position = Player.transform.position;
                DT3.GetComponent<BattleDamageText>().damage = (Damage / 3);
                GameManager.Instance.stackDamage += Damage / 3;
                PoisonCount++;
                yield return new WaitForSeconds(1);
            }
            GoToReturn = false;
            IsFarAway = false;
            BattleManager.Instance.IsPlayerTurn = true;
            GameManager.Instance.BattleButtonUi.SetActive(true);
        }
        else if (SuperAngerCount >= MaxSuperAngerCount && InstantDeathCount < MaxInstantDeathCount)
        {
            GameManager.Instance.BattleSkillText.text = "기괴한 수혈";
            BattleManager.Instance.IsEnemyTurn = false;
            yield return new WaitForSeconds(1.5f);
            animator.SetBool("IsSkill", true);
            SuperAngerCount = 0;
            InstantDeathCount += 50;
            IsSuperAnger = true;
            BattleManager.Instance.CamP = true;
            yield return new WaitForSeconds(2);
            animator.SetBool("IsSkill", false);
            BattleManager.Instance.CamP = false;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            yield return new WaitForSeconds(2);
            if (IsPoison == false)
            {
                yield return new WaitForSeconds(2);
            }
            else
            {
                GameObject DT3 = Instantiate(DmgText);
                Player.GetComponent<BattlePlayer>().IsHit = true;
                DT3.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT3.transform.position = Player.transform.position;
                DT3.GetComponent<BattleDamageText>().damage = (Damage / 3);
                GameManager.Instance.stackDamage += Damage / 3;
                PoisonCount++;
                yield return new WaitForSeconds(2);
            }
            BattleManager.Instance.IsPlayerTurn = true;
            GameManager.Instance.BattleButtonUi.SetActive(true);
        }
        else if (InstantDeathCount >= MaxInstantDeathCount) 
        {
            GameManager.Instance.BattleSkillText.text = "до раю позадуж";
            BattleManager.Instance.IsEnemyTurn = false;
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("IsSkill3", true);
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            yield return new WaitForSeconds(1f);
            IsInstantDead = true;
            for(int i = 0; i < 3; i++)
            {
                Destroy(InstantEye[i]);
            }
            StartCoroutine(InstantDeadImageFadeIn(0.5f));
            yield return new WaitForSeconds(1f);
            InstantDeathCount = 0;
            yield return new WaitForSeconds(2f);
            transform.position = this.transform.position + new Vector3(0f, 20f, 0); //보스 없어짐
            GameObject DT = Instantiate(DmgText);
            if (Player.GetComponent<BattlePlayer>().IsBarrier == false)
            {                               
               DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
               DT.transform.position = Player.transform.position;
               DT.GetComponent<BattleDamageText>().damage = 6664444;
               GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(1f);
               Player.GetComponent<BattlePlayer>().IsHit = true;
               GameManager.Instance.stackDamage += 6664444;                
            }
            else
            {              
               DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
               DT.transform.position = Player.transform.position;
               DT.GetComponent<BattleDamageText>().damage = 0;
               GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(1f);
               Player.GetComponent<BattlePlayer>().IsHit = true;                
            }
            animator.SetBool("IsSkill3", false);
            yield return new WaitForSeconds(2);
            IsInstantDead = false;
            StartCoroutine(InstantDeadImageFadeOut(0.5f));
            yield return new WaitForSeconds(2);
            this.transform.position = EnemySpawner.transform.position + new Vector3(2, 3.5f, 0);
            BattleManager.Instance.CamE = false;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            if (IsPoison == false)
            {
                yield return new WaitForSeconds(2);
            }
            else
            {
                yield return new WaitForSeconds(2);
                GameObject DT3 = Instantiate(DmgText);
                Player.GetComponent<BattlePlayer>().IsHit = true;
                DT3.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT3.transform.position = Player.transform.position;
                DT3.GetComponent<BattleDamageText>().damage = (Damage / 3);
                GameManager.Instance.stackDamage += Damage / 3;
                PoisonCount++;
                yield return new WaitForSeconds(1);
            }
            GoToReturn = false;
            IsFarAway = false;
            BattleManager.Instance.IsPlayerTurn = true;
            GameManager.Instance.BattleButtonUi.SetActive(true);
        }
        if (InstantDeathCount >= 50)
        {
            StartCoroutine(WarningTXT());
            yield return new WaitForSeconds(3);
            yield return null;
        }
        else
        {
            yield return null;
        }
    }
    IEnumerator InstantDeadImageFadeIn(float FaidTime)
    {
        Color color = InstantImage.color;
        Color color2 = Eye.color;
        while (color.a < 1f)
        {
            color.a += Time.deltaTime / FaidTime;
            InstantImage.color = color;
            if (color.a >= 1f)
            {
                color.a = 1f;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        if(color.a >= 1f)
        {
            color2.a = 1f;
            Eye.color = color2;
            yield return null;
        }
    }
    IEnumerator InstantDeadImageFadeOut(float FaidTime)
    {
        Color color = InstantImage.color;
        Color color2 = Eye.color;
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / FaidTime;
            color2.a -= Time.deltaTime / FaidTime;
            InstantImage.color = color;
            Eye.color = color2;
            if (color.a <= 0f)
            {
                color.a = 0f;
                color2.a = 0f;
            }
            yield return null;
        }
    }
}
