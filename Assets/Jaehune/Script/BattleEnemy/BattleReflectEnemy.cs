using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleReflectEnemy : BattleBasicEnemy
{
    [SerializeField] int ReflectingTurn, MaxReflctingTurn;
    [SerializeField] GameObject ReflectImage;
    public override void Start()
    {
        ReflectImage.SetActive(false);
        base.Start();
        this.transform.position = EnemySpawner.transform.position + new Vector3(0.9f, 0.45f, 0);
    }

    public override void Update()
    {
        base.Update();
        if (ReflectingTurn == MaxReflctingTurn)
        {
            IsReflect = false;
            ReflectingTurn = 0;
        }
        if(IsReflect == true)
        {
            ReflectImage.SetActive(true);
        }
        else
        {
            ReflectImage.SetActive(false);
        }
    }
    public override void AttackGone()
    {
        if (GoToPlayer == true && BattleManager.Instance.IsPlayerTurn == false && StopGone == false)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, Player.transform.position + new Vector3(3.5f, -0.4f, 0), 10 * Time.deltaTime);
        }
        else if (GoToReturn == true)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, EnemySpawner.transform.position + new Vector3(0.9f, 0.45f, 0), 10 * Time.deltaTime);
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
        HpBar.transform.position = this.transform.position + new Vector3(-0.5f, BarUp + 1.3f, 0);
        AngerBar.transform.position = this.transform.position + new Vector3(-0.5f, BarUp + 1.1f, 0);
        HpBarNull.transform.position = this.transform.position + new Vector3(-0.5f, BarUp + 1.3f, 0);
        EnemyPicture.transform.position = this.transform.position + new Vector3(-2.05f, BarUp + 1.25f, 0);
        ReflectImage.transform.position = this.transform.position + new Vector3(-0.7f, BarUp + 1.3f, 0);
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
            this.transform.position = EnemySpawner.transform.position + new Vector3(0.4f, 1f, 0);
            ReflectImage.SetActive(false);
            StartCoroutine("Dead2", 0.5f);
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
            GameManager.Instance.BattleSkillText.text = "Èä°­ ºÐ¼â";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsAttack", true);
            StopGone = true;
            transform.position = this.transform.position + new Vector3(-0.5f, 0.5f, 0);
            GameObject DT = Instantiate(DmgText);
            if (Player.GetComponent<BattlePlayer>().IsBarrier == false && Damage > GameManager.Instance.defense)
            {
                DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT.transform.position = Player.transform.position;
                DT.GetComponent<BattleDamageText>().damage = Damage - GameManager.Instance.defense;
                GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                Player.GetComponent<BattlePlayer>().IsHit = true;
                GameManager.Instance.stackDamage += Damage - GameManager.Instance.defense;
            }
            else if (Player.GetComponent<BattlePlayer>().IsBarrier == true || Damage <= GameManager.Instance.defense)
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
            transform.position = this.transform.position + new Vector3(0.5f, -0.5f, 0);
            StopGone = false;
            animator.SetBool("IsAttack", false);
            BattleManager.Instance.CamE = false;
            GoToReturn = true;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            GoToPlayer = false;
            Anger += 30;
            yield return new WaitForSeconds(1);
            GoToReturn = false;
            yield return new WaitForSeconds(1);
            if(IsReflect == true)
            {
                ReflectingTurn++;
            }
            BattleManager.Instance.IsPlayerTurn = true;
            if (GameManager.Instance.curHp > 0)
            {
                GameManager.Instance.BattleButtonUi.SetActive(true);
            }
        }
        else if (Anger >= MaxAnger)
        {
            Anger = 0;
            GameManager.Instance.BattleSkillText.text = "Á¤½ÅºÐ¿­ ºû";
            BattleManager.Instance.IsEnemyTurn = false;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamP = true;
            animator.SetBool("IsSkill", true);
            IsReflect = true;
            Player.GetComponent<BattlePlayer>().IsHit = true;
            yield return new WaitForSeconds(1);
            animator.SetBool("IsSkill", false);
            BattleManager.Instance.CamP = false;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            yield return new WaitForSeconds(1);
            BattleManager.Instance.IsPlayerTurn = true;
            if (GameManager.Instance.curHp > 0)
            {
                GameManager.Instance.BattleButtonUi.SetActive(true);
            }
        }
        yield return null;
    }
}
