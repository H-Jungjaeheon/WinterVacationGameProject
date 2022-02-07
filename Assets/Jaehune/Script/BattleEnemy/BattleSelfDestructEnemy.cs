using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSelfDestructEnemy : BattleBasicEnemy
{
    [SerializeField] bool IsBoom = false;
    [SerializeField] GameObject NullAngerBar;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        this.transform.position = EnemySpawner.transform.position + new Vector3(0, 0.6f, 0);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
    public override void AttackGone()
    {
        if (GoToPlayer == true && BattleManager.Instance.IsPlayerTurn == false && StopGone == false)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, Player.transform.position + new Vector3(2.5f, -0.2f, 0), 10 * Time.deltaTime);
        }
    }
    public override void Hpbar()
    {
        HpBar.fillAmount = Hp / MaxHp;
        AngerBar.fillAmount = Anger / MaxAnger;
        HpBar.transform.position = this.transform.position + new Vector3(0.1f, BarUp + 0.43f, 0);
        AngerBar.transform.position = this.transform.position + new Vector3(0.1f, BarUp - 0.1f, 0);
        NullAngerBar.transform.position = this.transform.position + new Vector3(0.1f, BarUp - 0.1f, 0);
        HpBarNull.transform.position = this.transform.position + new Vector3(0.1f, BarUp + 0.43f, 0);
        EnemyPicture.transform.position = this.transform.position + new Vector3(0.1f, BarUp + 0.9f, 0);
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
        if (Hp <= 0 || IsBoom == true)
        {
            animator.SetBool("IsDead", true);
            StartCoroutine("Dead2", 0.5f);
        }
    }
    public override IEnumerator Dead2(float FaidTime)
    {
        yield return new WaitForSeconds(2);
        if (Dead == false)
        {
            Dead = true;
            if(IsBoom != true)
            {
                GameObject.Find("GameManager").GetComponent<PlayerStats>().ExpUp(GExp);
            }
        }
        BattleManager.Instance.IsEnemyDead = true;
        BattleManager.Instance.IsEnemyTurn = false;
        Color color = SR.color;
        Color color2 = HpBar.color;
        Color color3 = HpBarNull.color;
        Color color4 = EnemyPicture.color;
        Color color5 = AngerBar.color;
        while (color.a > 0f && color2.a > 0f && color3.a > 0f && color4.a > 0f) //���� �� �� ��� �׷��� �ֱ� 
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
                yield return new WaitForSeconds(1);
                Destroy(this.gameObject);
            }
        }
    }
    public override IEnumerator EnemyAttack()
    {
        animator.SetBool("IsWalk", false);
        GameManager.Instance.BattleSkillBackGround.SetActive(true);
        if (Anger < MaxAnger) //AttackRand == 1
        {
            GameManager.Instance.BattleSkillText.text = "ȭ�� �ۿ� �� ���� �غ�";
            BattleManager.Instance.IsEnemyTurn = false;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamP = true;
            animator.SetBool("IsAttack", true);
            //���� �� �ִϸ��̼� Ȱ��ȭ
            yield return new WaitForSeconds(2);
            animator.SetBool("IsAttack", false);
            BattleManager.Instance.CamP = false;
            //���� �� �ִϸ��̼� ��Ȱ��ȭ
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            Anger += 25;
            yield return new WaitForSeconds(3);
            BattleManager.Instance.IsPlayerTurn = true;
            GameManager.Instance.BattleButtonUi.SetActive(true);
        }
        else if (Anger >= MaxAnger) //AttackRand == 2
        {
            Anger = 0;
            GameManager.Instance.BattleSkillText.text = "ȭ�� �ۿ� �� ����";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsAttack", true);
            StopGone = true;
            transform.position = this.transform.position + new Vector3(-0.9f, 0f, 0);
            GameObject DT = Instantiate(DmgText);
            DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
            DT.transform.position = Player.transform.position;
            DT.GetComponent<BattleDamageText>().damage = Damage;
            GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
            Player.GetComponent<BattlePlayer>().IsHit = true;
            GameManager.Instance.stackDamage += Damage;
            yield return new WaitForSeconds(1);
            transform.position = this.transform.position + new Vector3(0.9f, 0f, 0);
            StopGone = false;
            IsBoom = true;
            animator.SetBool("IsAttack", false);
            BattleManager.Instance.CamE = false;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
        }
        yield return null;
    }
}
