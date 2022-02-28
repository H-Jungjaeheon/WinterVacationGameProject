using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSuperEnemy : BattleBasicEnemy
{
    [SerializeField] int SkillAttackRand;
    [SerializeField] GameObject HealText;
    [SerializeField] bool IsSkill;
    [SerializeField] Vector2 BarTransform, PictureTransform;
    [SerializeField] GameObject EnergyBullet;
    [SerializeField] SpriteRenderer SRS;

    public override void Start()
    {
        EnergyBullet.SetActive(false);
        animator = GetComponent<Animator>();
        Anger = 0;
        IsSkill = false;
        MaxHp *= GameManager.Instance.Stage;
        Hp *= GameManager.Instance.Stage;
        SRS = this.GetComponent<SpriteRenderer>();
        SR = this.GetComponent<SpriteRenderer>();
        this.transform.position = EnemySpawner.transform.position + new Vector3(0f, 2.1f, 0);
        SkillAttackRand = Random.Range(1, 4);
    }

    public override void Update()
    {
        base.Update();
        if (Hp >= MaxHp)
        {
            Hp = MaxHp;
        }
        EnergyBullet.transform.position = Player.transform.position + new Vector3(1.5f, 1.5f, 0);
    }
    public override void AttackGone()
    {
        if (GoToPlayer == true && BattleManager.Instance.IsPlayerTurn == false && StopGone == false && IsSkill == false)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, Player.transform.position + new Vector3(3f, 1.27f, 0), 10 * Time.deltaTime);
        }
        else if (GoToPlayer == true && BattleManager.Instance.IsPlayerTurn == false && StopGone == false && IsSkill == true)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, Player.transform.position + new Vector3(6f, 1.27f, 0), 10 * Time.deltaTime);
        }
        else if (GoToReturn == true)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, EnemySpawner.transform.position + new Vector3(0f, 2.1f, 0), 10 * Time.deltaTime);
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
        HpBar.transform.position = new Vector2(this.transform.position.x + BarTransform.x, this.transform.position.y + BarTransform.y);
        AngerBar.transform.position = new Vector2(this.transform.position.x + BarTransform.x, this.transform.position.y + BarTransform.y - 0.26f);
        HpBarNull.transform.position = new Vector2(this.transform.position.x + BarTransform.x, this.transform.position.y + BarTransform.y);
        EnemyPicture.transform.position = new Vector2(this.transform.position.x + PictureTransform.x, this.transform.position.y + PictureTransform.y); 
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
        return base.Dead2(FaidTime);
    }
    public override IEnumerator EnemyAttack()
    {
        SkillAttackRand = Random.Range(1, 4);
        animator.SetBool("IsWalk", false);
        GameManager.Instance.BattleSkillBackGround.SetActive(true);
        if (Anger < MaxAnger)
        {
            GameManager.Instance.BattleSkillText.text = "피해 데이터 분석";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsAttack", true);
            StopGone = true;
            transform.position = this.transform.position + new Vector3(-1f, 0f, 0);
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
            transform.position = this.transform.position + new Vector3(1f, 0f, 0);
            StopGone = false;
            animator.SetBool("IsAttack", false);
            BattleManager.Instance.CamE = false;
            GoToReturn = true;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            GoToPlayer = false;
            Anger += 50;
            yield return new WaitForSeconds(1);
            GoToReturn = false;
            yield return new WaitForSeconds(2);
            BattleManager.Instance.IsPlayerTurn = true;
            if (GameManager.Instance.curHp > 0)
            {
                GameManager.Instance.BattleButtonUi.SetActive(true);
            }
        }
        else if (Anger >= MaxAnger && SkillAttackRand == 1)
        {
            Anger = 0;
            GameManager.Instance.BattleSkillText.text = "결과 도출 : 회복";
            BattleManager.Instance.IsEnemyTurn = false;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamP = true;
            SRS.sortingOrder = 2;
            animator.SetBool("IsSkill3", true);
            GameObject DT = Instantiate(HealText);
            DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
            DT.transform.position = this.transform.position;
            if(Hp + (MaxHp / 4) > MaxHp)
            {
                DT.GetComponent<BattleDamageText>().damage = MaxHp - Hp;
            }
            else
            {
                DT.GetComponent<BattleDamageText>().damage = MaxHp / 4;
            }
            Hp += MaxHp / 4;
            yield return new WaitForSeconds(1);
            SRS.sortingOrder = -1;
            animator.SetBool("IsSkill3", false);
            BattleManager.Instance.CamP = false;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            yield return new WaitForSeconds(1);
            BattleManager.Instance.IsPlayerTurn = true;
            GameManager.Instance.BattleButtonUi.SetActive(true);            
        }
        else if (Anger >= MaxAnger && SkillAttackRand == 2)
        {
            Anger = 0;
            IsSkill = true;
            GameManager.Instance.BattleSkillText.text = "결과 도출 : 발사";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            SRS.sortingOrder = 2;
            animator.SetBool("IsSkill", true);
            EnergyBullet.SetActive(true);
            StopGone = true;
            GameObject DT = Instantiate(DmgText);
            if (Player.GetComponent<BattlePlayer>().IsBarrier == false && Damage * 2 > GameManager.Instance.defense)
            {
                DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT.transform.position = Player.transform.position;
                DT.GetComponent<BattleDamageText>().damage = (Damage * 2) - GameManager.Instance.defense;
                GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                Player.GetComponent<BattlePlayer>().IsHit = true;
                GameManager.Instance.stackDamage += (Damage * 2) - GameManager.Instance.defense;
            }
            else if (Player.GetComponent<BattlePlayer>().IsBarrier == true || Damage * 2 <= GameManager.Instance.defense)
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
            SRS.sortingOrder = -1;
            EnergyBullet.SetActive(false);
            StopGone = false;
            animator.SetBool("IsSkill", false);
            BattleManager.Instance.CamE = false;
            GoToReturn = true;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            GoToPlayer = false;
            yield return new WaitForSeconds(1);
            GoToReturn = false;
            IsSkill = false;
            yield return new WaitForSeconds(2);
            BattleManager.Instance.IsPlayerTurn = true;
            if (GameManager.Instance.curHp > 0)
            {
                GameManager.Instance.BattleButtonUi.SetActive(true);
            }
        }
        else if (Anger >= MaxAnger && SkillAttackRand == 3)
        {
            Anger = 0;
            GameManager.Instance.BattleSkillText.text = "결과 도출 : 강탈";
            BattleManager.Instance.IsEnemyTurn = false;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamP = true;
            SRS.sortingOrder = 2;
            animator.SetBool("IsSkill2", true);
            GameManager.Instance.curMana -= 25;
            Anger += 50;
            yield return new WaitForSeconds(1);
            SRS.sortingOrder = -1;
            animator.SetBool("IsSkill2", false);
            BattleManager.Instance.CamP = false;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            yield return new WaitForSeconds(1);
            BattleManager.Instance.IsPlayerTurn = true;
            GameManager.Instance.BattleButtonUi.SetActive(true);            
        }
        yield return null;
    }
}