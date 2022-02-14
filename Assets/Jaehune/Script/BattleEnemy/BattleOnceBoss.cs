using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleOnceBoss : BattleBasicEnemy
{
    [SerializeField] float SuperAngerCount, MaxSuperAngerCount;
    [SerializeField] bool IsSuperAnger;
    [SerializeField] int SuperAnger;
    [SerializeField] GameObject HealText;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        SuperAnger = 0;
        IsSuperAnger = false;
        Invoke("BossCam", 3.5f);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(SuperAnger == 3)
        {
            IsSuperAnger = false;
            SuperAnger = 0;
        }
    }
    public override void AttackGone()
    {
        if (GoToPlayer == true && BattleManager.Instance.IsPlayerTurn == false && StopGone == false)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, Player.transform.position + new Vector3(2.5f, -0.8f, 0), 10 * Time.deltaTime);
        }
        else if (GoToReturn == true)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, EnemySpawner.transform.position, 10 * Time.deltaTime);
        }
        else if (GoToReturn == false)
        {
            animator.SetBool("IsWalk", false);
        }
    }
    void BossCam()
    {
        GameObject.Find("Main Camera").GetComponent<CameraMove>().BossBattleStart = true;
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
                GameObject.Find("Main Camera").GetComponent<CameraMove>().BossBattleStart = false;
            }
        }
    }
    public override IEnumerator EnemyAttack()
    {
        animator.SetBool("IsWalk", false);
        GameManager.Instance.BattleSkillBackGround.SetActive(true);
        if (Anger < MaxAnger && SuperAngerCount < MaxSuperAngerCount) //AttackRand == 1
        {
            GameManager.Instance.BattleSkillText.text = "먹이 수확";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsAttack", true);
            StopGone = true;
            transform.position = this.transform.position + new Vector3(-0.9f, 0.5f, 0);
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
                    DT2.GetComponent<BattleDamageText>().damage = (Damage / 3) - GameManager.Instance.defense;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += Damage - GameManager.Instance.defense;
                    Hp += (Damage / 3) - GameManager.Instance.defense;
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
            yield return new WaitForSeconds(1);
            transform.position = this.transform.position + new Vector3(0.9f, -0.5f, 0);
            StopGone = false;
            animator.SetBool("IsAttack", false);
            BattleManager.Instance.CamE = false;
            GoToReturn = true;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            GoToPlayer = false;
            Anger += 40;
            if(IsSuperAnger == true)
            {
                SuperAnger++;
            }
            else
            {
                SuperAngerCount += 10;
            }
            yield return new WaitForSeconds(1);
            GoToReturn = false;
            yield return new WaitForSeconds(2);
            BattleManager.Instance.IsPlayerTurn = true;
            GameManager.Instance.BattleButtonUi.SetActive(true);
        }
        else if (Anger >= MaxAnger && SuperAngerCount < MaxSuperAngerCount)
        {
            Anger = 0;
            GameManager.Instance.BattleSkillText.text = "먹이 손질";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsSkill", true);
            StopGone = true;
            transform.position = this.transform.position + new Vector3(-0.9f, 0.5f, 0);
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
                    DT2.GetComponent<BattleDamageText>().damage = ((Damage * 2) / 3) - GameManager.Instance.defense;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += (Damage * 2) - GameManager.Instance.defense;
                    Hp += (Damage * 2) / 3 - GameManager.Instance.defense;
                }
                else
                {
                    DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                    DT.transform.position = Player.transform.position;
                    DT.GetComponent<BattleDamageText>().damage = (Damage * 2) - GameManager.Instance.defense;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += (Damage * 2) - GameManager.Instance.defense;
                }
            }
            else
            {
                if(IsSuperAnger == true)
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
            yield return new WaitForSeconds(1);
            transform.position = this.transform.position + new Vector3(0.9f, -0.5f, 0);
            StopGone = false;
            animator.SetBool("IsSkill", false);
            BattleManager.Instance.CamE = false;
            GoToReturn = true;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            GoToPlayer = false;
            if (IsSuperAnger == true)
            {
                SuperAnger++;
            }
            else
            {
                SuperAngerCount += 50;
            }
            yield return new WaitForSeconds(1);
            GoToReturn = false;
            yield return new WaitForSeconds(2);
            BattleManager.Instance.IsPlayerTurn = true;
            GameManager.Instance.BattleButtonUi.SetActive(true);
        }
        else if (SuperAngerCount >= MaxSuperAngerCount)
        {
            GameManager.Instance.BattleSkillText.text = "먹이가...반항?!";
            BattleManager.Instance.IsEnemyTurn = false;
            yield return new WaitForSeconds(1.5f);
            SuperAngerCount = 0;
            IsSuperAnger = true;
            BattleManager.Instance.CamP = true;
            yield return new WaitForSeconds(2);
            animator.SetBool("IsAttack", false);
            BattleManager.Instance.CamP = false;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            yield return new WaitForSeconds(2);
            BattleManager.Instance.IsPlayerTurn = true;
            GameManager.Instance.BattleButtonUi.SetActive(true);
        }
        yield return null;
    }
}
