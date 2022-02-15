using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTwiceBoss : BattleBasicEnemy
{
    [SerializeField] int SkillAttackRand, PoisonCount, MaxPoisonCount, DmgUpCount;
    [SerializeField] Image Poison, DmgUp;
    [SerializeField] bool IsDmgUp, IsFarAway;
    public override void Start()
    {
        IsPoison = false;
        IsDmgUp = false;
        if (IsPoison == false)
        {
            SkillAttackRand = Random.Range(1, 4);
        }
        else
        {
            SkillAttackRand = Random.Range(1, 3);
        }
        animator = GetComponent<Animator>();
        Anger = 0;
        MaxHp *= GameManager.Instance.Stage;
        Hp *= GameManager.Instance.Stage;
        SR = this.GetComponent<SpriteRenderer>();
        this.transform.position = EnemySpawner.transform.position + new Vector3(1, 1.85f, 0);
        Invoke("BossCam", 3.5f);
    }

    public override void Update()
    {
        base.Update();
        DamageUp();
        PoisonUse();
    }
    public override void AttackGone()
    {
        if (GoToPlayer == true && BattleManager.Instance.IsPlayerTurn == false && StopGone == false && IsFarAway == false)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, Player.transform.position + new Vector3(3, 1f, 0), 10 * Time.deltaTime);
        }
        else if(GoToPlayer == true && BattleManager.Instance.IsPlayerTurn == false && StopGone == false && IsFarAway == true)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, Player.transform.position + new Vector3(9, 1f, 0), 10 * Time.deltaTime);
        }
        else if (GoToReturn == true)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, EnemySpawner.transform.position + new Vector3(1, 1.85f, 0), 10 * Time.deltaTime);
        }
        else if (GoToReturn == false)
        {
            animator.SetBool("IsWalk", false);
        }
    }
    void PoisonUse()
    {
        Poison.transform.position = Player.transform.position + new Vector3(0, 3, 0);
        if(IsPoison == true)
        {
            StartCoroutine(PoisonIconFadeIn(0.5f));
        }
        else
        {
            StartCoroutine(PoisonIconFadeOut(0.5f));
        }
        if(PoisonCount > MaxPoisonCount)
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
    IEnumerator DmgUpIconFadeIn(float FaidTime)
    {
        Color color = DmgUp.color;
        while (color.a < 1f)
        {
            color.a += Time.deltaTime / FaidTime;
            DmgUp.color = color;
            if (color.a >= 1f)
            {
                color.a = 1f;
            }
            yield return null;
        }
    }
    IEnumerator DmgUpIconFadeOut(float FaidTime)
    {
        Color color = DmgUp.color;
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / FaidTime;
            DmgUp.color = color;
            if (color.a <= 0f)
            {
                color.a = 0f;
            }
            yield return null;
        }
    }
    void DamageUp()
    {
        DmgUp.transform.position = this.transform.position + new Vector3(0, 4, 0);
        if (IsDmgUp == true)
        {
            StartCoroutine(DmgUpIconFadeIn(0.5f));
        }
        else
        {
            StartCoroutine(DmgUpIconFadeOut(0.5f));
        }
        if (DmgUpCount > 3)
        {
            DmgUpCount = 0;
            IsDmgUp = false;
        }
    }
    void BossCam()
    {
        GameObject.Find("Main Camera").GetComponent<CameraMove>().BossBattleStart = true;
    }
    public override void Hpbar()
    { 
        HpBar.fillAmount = Hp / MaxHp;
        AngerBar.fillAmount = Anger / MaxAnger;
        HpBar.transform.position = new Vector3(0.65f, BarUp + 65.05f, 0);
        AngerBar.transform.position = new Vector3(0.65f, BarUp + 64.35f, 0);
        HpBarNull.transform.position = new Vector3(0.65f, BarUp + 65.05f, 0);
        EnemyPicture.transform.position = new Vector3(-6.4f, BarUp + 65, 0);
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
                color.a = 0f;
                color2.a = 0f;
                color3.a = 0f;
                color4.a = 0f;
                color5.a = 0f;
                yield return null;
                yield return new WaitForSeconds(1);
                GameManager.Instance.IsBattleStart = false;
                BattleManager.Instance.IsEnemyTurn = true;
                BattleManager.Instance.IsPlayerTurn = true;
                yield return new WaitForSeconds(1);
                Destroy(this.gameObject);
                GameObject.Find("Main Camera").GetComponent<CameraMove>().BossBattleStart = false;
                GameObject.Find("Main Camera").GetComponent<CameraMove>().IsBossCamMove = false;
            }
        }
    }
    public override IEnumerator EnemyAttack()
    {
        animator.SetBool("IsWalk", false);
        GameManager.Instance.BattleSkillBackGround.SetActive(true);
        if (Anger < MaxAnger)
        {
            IsFarAway = true;
            if (IsPoison == false)
            {
                SkillAttackRand = Random.Range(1, 4);
            }
            else
            {
                SkillAttackRand = Random.Range(1, 3);
            }
            GameManager.Instance.BattleSkillText.text = "살아있는 대동맥";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsAttack", true);
            StopGone = true;
            GameObject DT = Instantiate(DmgText);
            if (Player.GetComponent<BattlePlayer>().IsBarrier == false)
            {
                if (IsDmgUp == true)
                {
                    DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT.transform.position = Player.transform.position;
                    DT.GetComponent<BattleDamageText>().damage = (Damage + 7) - GameManager.Instance.defense;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += (Damage + 7) - GameManager.Instance.defense;
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
                DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT.transform.position = Player.transform.position;
                DT.GetComponent<BattleDamageText>().damage = 0;
                GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                Player.GetComponent<BattlePlayer>().IsHit = true;
            }
            yield return new WaitForSeconds(2.5f);
            StopGone = false;
            IsFarAway = false;
            animator.SetBool("IsAttack", false);
            BattleManager.Instance.CamE = false;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            GoToPlayer = false;
            GoToReturn = true;
            Anger += 40;
            if(IsDmgUp == true)
            {
                DmgUpCount++;
            }
            if (IsPoison == false)
            {
                yield return new WaitForSeconds(2);
            }
            else
            {
                GameObject DT2 = Instantiate(DmgText);
                Player.GetComponent<BattlePlayer>().IsHit = true;
                DT2.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT2.transform.position = Player.transform.position;
                DT2.GetComponent<BattleDamageText>().damage = (Damage / 3);
                GameManager.Instance.stackDamage += Damage / 3;
                PoisonCount++;
                yield return new WaitForSeconds(2);
            }
            GoToReturn = false;
            BattleManager.Instance.IsPlayerTurn = true;
            GameManager.Instance.BattleButtonUi.SetActive(true);
        }
        else if (Anger >= MaxAnger && SkillAttackRand == 1)
        {
            if (IsPoison == false)
            {
                SkillAttackRand = Random.Range(1, 4);
            }
            else
            {
                SkillAttackRand = Random.Range(1, 3);
            }
            Anger = 0;
            GameManager.Instance.BattleSkillText.text = "아름다운 노래";
            BattleManager.Instance.IsEnemyTurn = false;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamP = true;
            animator.SetBool("IsSkill", true);
            GameObject DT = Instantiate(DmgText);
            if (Player.GetComponent<BattlePlayer>().IsBarrier == false)
            {
                if (IsDmgUp == true)
                {
                    DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT.transform.position = Player.transform.position;
                    DT.GetComponent<BattleDamageText>().damage = (Damage / 2 + 7) - GameManager.Instance.defense;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += (Damage / 2 + 7) - GameManager.Instance.defense;
                    GameManager.Instance.curMana -= 35;
                }
                else
                {
                    DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT.transform.position = Player.transform.position;
                    DT.GetComponent<BattleDamageText>().damage = (Damage / 2 + 7) - GameManager.Instance.defense;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += (Damage / 2 + 7) - GameManager.Instance.defense;
                    GameManager.Instance.curMana -= 35;
                }
            }
            else
            {
                
                DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT.transform.position = Player.transform.position;
                DT.GetComponent<BattleDamageText>().damage = 0;
                GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                Player.GetComponent<BattlePlayer>().IsHit = true;               
            }
            yield return new WaitForSeconds(2.5f);
            animator.SetBool("IsSkill", false);
            BattleManager.Instance.CamP = false;
            if (IsDmgUp == true)
            {
                DmgUpCount++;
            }
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            if(IsPoison == false)
            {
                yield return new WaitForSeconds(2);
            }
            else
            {
                GameObject DT2 = Instantiate(DmgText);
                Player.GetComponent<BattlePlayer>().IsHit = true;
                DT2.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT2.transform.position = Player.transform.position;
                DT2.GetComponent<BattleDamageText>().damage = (Damage / 3);
                GameManager.Instance.stackDamage += Damage / 3;
                PoisonCount++;
                yield return new WaitForSeconds(1);
            }
            BattleManager.Instance.IsPlayerTurn = true;
            GameManager.Instance.BattleButtonUi.SetActive(true);
        }
        else if (Anger >= MaxAnger && SkillAttackRand == 2)
        {
            if (IsPoison == false)
            {
                SkillAttackRand = Random.Range(1, 4);
            }
            else
            {
                SkillAttackRand = Random.Range(1, 3);
            }
            Anger = 0;
            GameManager.Instance.BattleSkillText.text = "심장박동 증가";
            BattleManager.Instance.IsEnemyTurn = false;
            yield return new WaitForSeconds(1f);
            BattleManager.Instance.CamP = true;
            animator.SetBool("IsSkill", true);
            IsDmgUp = true;
            DmgUpCount = 0;
            yield return new WaitForSeconds(1);
            animator.SetBool("IsSkill", false);
            BattleManager.Instance.CamP = false;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            if (IsPoison == false)
            {
                yield return new WaitForSeconds(2);
            }
            else
            {
                GameObject DT2 = Instantiate(DmgText);
                Player.GetComponent<BattlePlayer>().IsHit = true;
                DT2.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT2.transform.position = Player.transform.position;
                DT2.GetComponent<BattleDamageText>().damage = (Damage / 3);
                GameManager.Instance.stackDamage += Damage / 3;
                PoisonCount++;
                yield return new WaitForSeconds(2);
            }
            BattleManager.Instance.IsPlayerTurn = true;
            GameManager.Instance.BattleButtonUi.SetActive(true);
        }
        else if (Anger >= MaxAnger && SkillAttackRand == 3)
        {
            if (IsPoison == false)
            {
                SkillAttackRand = Random.Range(1, 4);
            }
            else
            {
                SkillAttackRand = Random.Range(1, 3);
            }
            Anger = 0;
            GameManager.Instance.BattleSkillText.text = "하대정맥 분비물";
            BattleManager.Instance.IsEnemyTurn = false;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamP = true;
            animator.SetBool("IsSkill", true);
            if (Player.GetComponent<BattlePlayer>().IsBarrier == false)
            {
                IsPoison = true;
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
            animator.SetBool("IsSkill", false);
            BattleManager.Instance.CamP = false;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            if (IsDmgUp == true)
            {
                DmgUpCount++;
            }
            if (IsPoison == false)
            {
                yield return new WaitForSeconds(2);
            }
            else
            {
                GameObject DT2 = Instantiate(DmgText);
                Player.GetComponent<BattlePlayer>().IsHit = true;
                DT2.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT2.transform.position = Player.transform.position;
                DT2.GetComponent<BattleDamageText>().damage = (Damage / 3);
                GameManager.Instance.stackDamage += Damage / 3;
                PoisonCount++;
                yield return new WaitForSeconds(2);
            }
            BattleManager.Instance.IsPlayerTurn = true;
            GameManager.Instance.BattleButtonUi.SetActive(true);
        }
        yield return null;
    }
}
