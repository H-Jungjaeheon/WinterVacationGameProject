using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleChargerEnemy : BattleBasicEnemy
{
    [SerializeField] int RandomAbility, AbilityCount, MaxAbilityCount;
    [SerializeField] Image AbilityHand;
    // Start is called before the first frame update
    public override void Start() //어빌리티 핸드 플레이어 자리에 활성화 + 배틀에너미의 이즈 스턴 트루, 조건으로 이즈 스턴이 트루면 플레이어가 공격 할 때 턴 넘기고 어빌리티 카운트 ++, 어빌리티 카운트가 맥스 어빌리티 카운트보다 같거나 크면 능력 비활성화
    {
        RandomAbility = 0;
        AbilityCount = 0;
        IsStun = false;
        animator = GetComponent<Animator>();
        Anger = 0;
        MaxHp *= GameManager.Instance.Stage;
        Hp *= GameManager.Instance.Stage;
        SR = this.GetComponent<SpriteRenderer>();
        this.transform.position = EnemySpawner.transform.position + new Vector3(0, 0.7f, 0);
    }

    // Update is called once per frame
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
    }
    public override void AttackGone()
    {
        if (GoToPlayer == true && BattleManager.Instance.IsPlayerTurn == false && StopGone == false)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, Player.transform.position + new Vector3(2.5f, 0f, 0), 10 * Time.deltaTime);
        }
        else if (GoToReturn == true)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, EnemySpawner.transform.position + new Vector3(0, 0.7f, 0), 10 * Time.deltaTime);
        }
        else if (GoToReturn == false)
        {
            animator.SetBool("IsWalk", false);
        }
    }
    public override void Hpbar()
    {
        base.Hpbar();
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
        IsStun = false;
    }
    public override IEnumerator Dead2(float FaidTime)
    {
        return base.Dead2(FaidTime);
    }
    public override IEnumerator EnemyAttack()
    {
        animator.SetBool("IsWalk", false);
        GameManager.Instance.BattleSkillBackGround.SetActive(true);
        if (Anger < MaxAnger) //AttackRand == 1
        {
            GameManager.Instance.BattleSkillText.text = "내장으로 이루어진 손";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsAttack", true);
            StopGone = true;
            transform.position = this.transform.position + new Vector3(-0.9f, 0.5f, 0);
            GameObject DT = Instantiate(DmgText);
            if (Player.GetComponent<BattlePlayer>().IsBarrier == false)
            {
                DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT.transform.position = Player.transform.position;
                DT.GetComponent<BattleDamageText>().damage = Damage;
                GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                Player.GetComponent<BattlePlayer>().IsHit = true;
                GameManager.Instance.stackDamage += Damage;
            }
            else
            {
                DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT.transform.position = Player.transform.position;
                DT.GetComponent<BattleDamageText>().damage = 0;
                GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                Player.GetComponent<BattlePlayer>().IsHit = true;
            }
            yield return new WaitForSeconds(1);
            transform.position = this.transform.position + new Vector3(0.9f, -0.5f, 0);
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
                GameManager.Instance.BattleButtonUi.SetActive(true);
            }
            //AttackRand = Random.Range(1, 3);
        }
        else if (Anger >= MaxAnger) //AttackRand == 2
        {
            RandomAbility = Random.Range(1, 5); //1, 4
            Anger = 0;
            GameManager.Instance.BattleSkillText.text = "적수공권(赤手空拳)";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsSkill", true);
            StopGone = true;
            transform.position = this.transform.position + new Vector3(-0.9f, 0.5f, 0);
            GameObject DT = Instantiate(DmgText);
            if (Player.GetComponent<BattlePlayer>().IsBarrier == false)
            {
                DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT.transform.position = Player.transform.position;
                DT.GetComponent<BattleDamageText>().damage = Damage * 2;
                GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                Player.GetComponent<BattlePlayer>().IsHit = true;
                GameManager.Instance.stackDamage += Damage * 2;
                if(RandomAbility == 1 && IsStun != true)
                {
                    StartCoroutine(AbillityHandFadeIn(1f));
                    RandomAbility = 0;
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
            yield return new WaitForSeconds(1);
            transform.position = this.transform.position + new Vector3(0.9f, -0.5f, 0);
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
                GameManager.Instance.BattleButtonUi.SetActive(true);
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
