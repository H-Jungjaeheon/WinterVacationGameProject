using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleChargerEnemy : BattleBasicEnemy
{
    [SerializeField] int RandomAbility, AbilityCount, MaxAbilityCount;
    [SerializeField] Image AbilityHand;
    [SerializeField] bool IsDead;
    public override void Start() 
    {
        IsDead = false;
        RandomAbility = 0;
        AbilityCount = 0;
        IsStun = false;
        animator = GetComponent<Animator>();
        Anger = 0;
        MaxHp *= GameManager.Instance.Stage;
        Hp *= GameManager.Instance.Stage;
        SR = this.GetComponent<SpriteRenderer>();
        this.transform.position = EnemySpawner.transform.position + new Vector3(-0.7f, 1.1f, 0);
    }

    public override void Update()
    {
        base.Update();
        AbilityHand.transform.position = Player.transform.position;
        if(AbilityCount >= MaxAbilityCount)
        {
            IsStun = false;
            AbilityCount = 0;
            StartCoroutine(AbillityHandFadeOut(1f));
        }
        else if (IsDead == true)
        {
            IsDead = false;
            this.transform.position = this.transform.position + new Vector3(0f, 1f, 0);
        }
    }
    public override void AttackGone()
    {
        if (GoToPlayer == true && BattleManager.Instance.IsPlayerTurn == false && StopGone == false)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, Player.transform.position + new Vector3(2f, 0.2f, 0), 10 * Time.deltaTime);
        }
        else if (GoToReturn == true)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, EnemySpawner.transform.position + new Vector3(-0.7f, 1.1f, 0), 10 * Time.deltaTime);
        }
        else if (GoToReturn == false)
        {
            animator.SetBool("IsWalk", false);
        }
    }
    public override void Hpbar()
    {
        HpBar.fillAmount = Hp / MaxHp;
        AngerBar.fillAmount = Anger / MaxAnger;
        HpBar.transform.position = this.transform.position + new Vector3(1f, BarUp + 0.9f, 0);
        AngerBar.transform.position = this.transform.position + new Vector3(1f, BarUp + 0.7f, 0);
        HpBarNull.transform.position = this.transform.position + new Vector3(1f, BarUp + 0.9f, 0);
        EnemyPicture.transform.position = this.transform.position + new Vector3(-0.28f, BarUp + 0.85f, 0);
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
        if (Hp <= 0)
        {
            BattleManager.Instance.IsEnemyDead = true;
            animator.SetBool("IsDead", true);
            StartCoroutine("Dead2", 0.5f);
            IsStun = false;
        }
    }
    public override IEnumerator Dead2(float FaidTime)
    {
        return base.Dead2(FaidTime);
    }
    public override IEnumerator EnemyAttack()
    {
        animator.SetBool("IsWalk", false);
        GameManager.Instance.BattleSkillBackGround.SetActive(true);
        if (Anger < MaxAnger)
        {
            GameManager.Instance.BattleSkillText.text = "³»ÀåÀ¸·Î ÀÌ·ç¾îÁø ¼Õ";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsAttack", true);
            StopGone = true;
            transform.position = this.transform.position + new Vector3(-0.9f, 0f, 0);
            GameObject DT = Instantiate(DmgText);
            if (Player.GetComponent<BattlePlayer>().IsBarrier == false)
            {
                DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT.transform.position = Player.transform.position;
                DT.GetComponent<BattleDamageText>().damage = Damage - GameManager.Instance.defense;
                GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                Player.GetComponent<BattlePlayer>().IsHit = true;
                GameManager.Instance.stackDamage += Damage - GameManager.Instance.defense;
            }
            else
            {
                DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT.transform.position = Player.transform.position;
                DT.GetComponent<BattleDamageText>().damage = 0;
                GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                Player.GetComponent<BattlePlayer>().IsHit = true;
            }
            if (GameManager.Instance.curHp > 0)
            {
                GameManager.Instance.BattleSkillBackGround.SetActive(false);
            }
            yield return new WaitForSeconds(1);
            transform.position = this.transform.position + new Vector3(0.9f, 0f, 0);
            StopGone = false;
            animator.SetBool("IsAttack", false);
            BattleManager.Instance.CamE = false;
            if (IsStun == true)
            {
                AbilityCount++;
            }
            GoToReturn = true;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            GoToPlayer = false;
            Anger += 40;
            yield return new WaitForSeconds(1);
            GoToReturn = false;
            yield return new WaitForSeconds(2);
            if (IsStun == true)
            {
                StartCoroutine(EnemyAttack());
            }
            else
            {
                BattleManager.Instance.IsPlayerTurn = true;
                if (GameManager.Instance.curHp > 0)
                {
                    GameManager.Instance.BattleButtonUi.SetActive(true);
                }
            }
        }
        else if (Anger >= MaxAnger) 
        {
            RandomAbility = Random.Range(1, 5); 
            Anger = 0;
            GameManager.Instance.BattleSkillText.text = "Àû¼ö°ø±Ç(îåâ¢ÍöÏë)";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsSkill", true);
            StopGone = true;
            transform.position = this.transform.position + new Vector3(-0.9f, 0f, 0);
            GameObject DT = Instantiate(DmgText);
            if (Player.GetComponent<BattlePlayer>().IsBarrier == false)
            {
                DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT.transform.position = Player.transform.position;
                DT.GetComponent<BattleDamageText>().damage = (Damage * 2) - GameManager.Instance.defense;
                GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                Player.GetComponent<BattlePlayer>().IsHit = true;
                GameManager.Instance.stackDamage += (Damage * 2) - GameManager.Instance.defense;
                if(RandomAbility == 1 && IsStun != true)
                {
                    StartCoroutine(AbillityHandFadeIn(1f));
                    RandomAbility = Random.Range(1, 5);
                    MaxAbilityCount = Random.Range(1, 4);
                    IsStun = true;
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
            if (GameManager.Instance.curHp > 0)
            {
                GameManager.Instance.BattleSkillBackGround.SetActive(false);
            }
            yield return new WaitForSeconds(1);
            transform.position = this.transform.position + new Vector3(0.9f, 0f, 0);
            StopGone = false;
            animator.SetBool("IsSkill", false);
            BattleManager.Instance.CamE = false;
            GoToReturn = true;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            GoToPlayer = false;
            yield return new WaitForSeconds(1);
            GoToReturn = false;
            yield return new WaitForSeconds(2);
            if (IsStun == true)
            {
                StartCoroutine(EnemyAttack());
            }
            else
            {
                BattleManager.Instance.IsPlayerTurn = true;
                if (GameManager.Instance.curHp > 0)
                {
                    GameManager.Instance.BattleButtonUi.SetActive(true);
                }
            }
        }
        yield return null;
    }
    IEnumerator AbillityHandFadeIn(float FaidTime)
    {
        Color color = AbilityHand.color;
        while (color.a < 1f)
        {
            color.a += Time.deltaTime / FaidTime;
            AbilityHand.color = color;
            if (color.a >= 1f)
            {
                color.a = 1f;
            }
            yield return null;
        }
    }
    IEnumerator AbillityHandFadeOut(float FaidTime)
    {
        Color color = AbilityHand.color;
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / FaidTime;
            AbilityHand.color = color;          
            if (color.a <= 0f)
            {
                color.a = 0f;                
            }
            yield return null;
        }
    }
}
