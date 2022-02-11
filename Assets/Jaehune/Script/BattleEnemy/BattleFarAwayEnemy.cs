using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFarAwayEnemy : BattleBasicEnemy
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        this.transform.position = EnemySpawner.transform.position + new Vector3(0f, 0.9f, 0);
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
            transform.position = Vector3.MoveTowards(this.transform.position, Player.transform.position + new Vector3(7.5f, 0f, 0), 10 * Time.deltaTime);
        }
        else if (GoToReturn == true)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, EnemySpawner.transform.position + new Vector3(-0.5f, 0.8f, 0), 10 * Time.deltaTime);
        }
        else if (GoToReturn == false)
        {
            animator.SetBool("IsWalk", false);
        }
    }
    public override void Hpbar()
    { //오른쪽으로 밀고 위로 더
        HpBar.fillAmount = Hp / MaxHp;
        AngerBar.fillAmount = Anger / MaxAnger;
        HpBar.transform.position = this.transform.position + new Vector3(0f, BarUp + 1.25f, 0);
        AngerBar.transform.position = this.transform.position + new Vector3(0f, BarUp + 1.09f, 0);
        HpBarNull.transform.position = this.transform.position + new Vector3(0f, BarUp + 1.25f, 0);
        EnemyPicture.transform.position = this.transform.position + new Vector3(-1.15f, BarUp + 1.2f, 0);
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
        animator.SetBool("IsWalk", false);
        GameManager.Instance.BattleSkillBackGround.SetActive(true);
        GameObject.Find("Main Camera").GetComponent<CameraMove>().IsFarAway = true;
        if (Anger < MaxAnger) //AttackRand == 1
        {
            GameManager.Instance.BattleSkillText.text = "촉수 강타";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsAttack", true);
            StopGone = true;
            transform.position = this.transform.position + new Vector3(-2.5f, 0f, 0);
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
            transform.position = this.transform.position + new Vector3(2.5f, 0, 0);
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
            GameObject.Find("Main Camera").GetComponent<CameraMove>().IsFarAway = false;
            BattleManager.Instance.IsPlayerTurn = true;
            GameManager.Instance.BattleButtonUi.SetActive(true);
            //AttackRand = Random.Range(1, 3);
        }
        else if (Anger >= MaxAnger) //AttackRand == 2
        {
            Anger = 0;
            GameManager.Instance.BattleSkillText.text = "기형의 팔";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsSkill", true);
            StopGone = true;
            transform.position = this.transform.position + new Vector3(-2.5f, 0.5f, 0);
            GameObject DT = Instantiate(DmgText);
            if (Player.GetComponent<BattlePlayer>().IsBarrier == false)
            {
                DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT.transform.position = Player.transform.position;
                DT.GetComponent<BattleDamageText>().damage = Damage + 1;
                GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                Player.GetComponent<BattlePlayer>().IsHit = true;
                GameManager.Instance.curHp -= Damage + 1;
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
            transform.position = this.transform.position + new Vector3(2.5f, -0.5f, 0);
            StopGone = false;
            animator.SetBool("IsSkill", false);
            BattleManager.Instance.CamE = false;
            GoToReturn = true;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            GoToPlayer = false;
            yield return new WaitForSeconds(1);
            GoToReturn = false;
            yield return new WaitForSeconds(2);
            GameObject.Find("Main Camera").GetComponent<CameraMove>().IsFarAway = false;
            BattleManager.Instance.IsPlayerTurn = true;
            GameManager.Instance.BattleButtonUi.SetActive(true);
            //AttackRand = Random.Range(1, 3);
        }
        yield return null;
    }
}
