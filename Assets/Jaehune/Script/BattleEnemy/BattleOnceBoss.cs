using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleOnceBoss : BattleBasicEnemy
{
    [SerializeField] float SuperAngerCount, MaxSuperAngerCount;
    [SerializeField] bool IsSuperAnger;
    [SerializeField] int SuperAnger;
    [SerializeField] GameObject HealText;
    [SerializeField] Vector2 BarTransform, PictureTransform;
    [SerializeField] Text HpText;
    public Image SuperAngerBar;

    public override void Start()
    {
        animator = GetComponent<Animator>();
        Anger = 0;
        MaxHp *= GameManager.Instance.Stage;
        Hp *= GameManager.Instance.Stage;
        SR = this.GetComponent<SpriteRenderer>();
        this.transform.position = EnemySpawner.transform.position + new Vector3(3, 3f, 0);
        SuperAnger = 0;
        IsSuperAnger = false;
        Invoke("BossCam", 3.5f);
    }

    public override void Update()
    {
        base.Update();
        if(SuperAnger == 3)
        {
            IsSuperAnger = false;
            SuperAnger = 0;
        }
        HpText.text = Hp + "/" + MaxHp;
    }
    public override void AttackGone()
    {
        if (GoToPlayer == true && BattleManager.Instance.IsPlayerTurn == false && StopGone == false)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, Player.transform.position + new Vector3(5, 2f, 0), 10 * Time.deltaTime);
        }
        else if (GoToReturn == true)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, EnemySpawner.transform.position + new Vector3(3, 3f, 0), 10 * Time.deltaTime);
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
        if (Hp > 0)
        {
            HpText.transform.position = new Vector2(BarTransform.x, BarTransform.y);
        }
        else if(Hp < 0)
        {
            Hp = 0;
        }
        HpBar.fillAmount = Hp / MaxHp;
        AngerBar.fillAmount = Anger / MaxAnger;
        SuperAngerBar.fillAmount = SuperAngerCount / MaxSuperAngerCount;
        HpBar.transform.position = new Vector2(BarTransform.x, BarTransform.y);
        AngerBar.transform.position = new Vector2(BarTransform.x, BarTransform.y - 0.7f);
        HpBarNull.transform.position = new Vector2(BarTransform.x, BarTransform.y);
        SuperAngerBar.transform.position = new Vector2(BarTransform.x, BarTransform.y + 0.7f);
        EnemyPicture.transform.position = new Vector2(PictureTransform.x, PictureTransform.y);
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
        Color color6 = SuperAngerBar.color;
        while (color.a > 0f) 
        {
            color.a -= Time.deltaTime / FaidTime;
            SR.color = color;
            HpBar.color = color;
            HpBarNull.color = color;
            EnemyPicture.color = color;
            AngerBar.color = color;
            SuperAngerBar.color = color;
            HpText.color = color;
            if (color.a <= 0f)
            {
                color.a = 0f;
                color2.a = 0f;
                color3.a = 0f;
                color4.a = 0f;
                color5.a = 0f;
                color6.a = 0f;
            }
            else
            {
                color.a = 0f;
                color2.a = 0f;
                color3.a = 0f;
                color4.a = 0f;
                color5.a = 0f;
                color6.a = 0f;
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
        Destroy(HpText);
    }
    public override IEnumerator EnemyAttack()
    {
        animator.SetBool("IsWalk", false);
        GameManager.Instance.BattleSkillBackGround.SetActive(true);
        if (Anger < MaxAnger && SuperAngerCount < MaxSuperAngerCount) 
        {
            GameManager.Instance.BattleSkillText.text = "???? ????";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsAttack", true);
            StopGone = true;
            transform.position = this.transform.position;
            GameObject DT = Instantiate(DmgText);
            GameObject DT2 = Instantiate(HealText);
            if (Player.GetComponent<BattlePlayer>().IsBarrier == false && Damage > GameManager.Instance.defense)
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
            else if (Player.GetComponent<BattlePlayer>().IsBarrier == true || Damage <= GameManager.Instance.defense)
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
            if (GameManager.Instance.curHp > 0)
            {
                GameManager.Instance.BattleSkillBackGround.SetActive(false);
            }
            yield return new WaitForSeconds(1);
            transform.position = this.transform.position;
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
                SuperAngerCount += 20;
            }
            yield return new WaitForSeconds(1.3f);
            GoToReturn = false;
            BattleManager.Instance.IsPlayerTurn = true;
            if (GameManager.Instance.curHp > 0)
            {
                GameManager.Instance.BattleButtonUi.SetActive(true);
            }
        }
        else if (Anger >= MaxAnger && SuperAngerCount < MaxSuperAngerCount)
        {
            Anger = 0;
            GameManager.Instance.BattleSkillText.text = "???? ????";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsSkill", true);
            StopGone = true;
            transform.position = this.transform.position;
            GameObject DT = Instantiate(DmgText);
            GameObject DT2 = Instantiate(HealText);
            if (Player.GetComponent<BattlePlayer>().IsBarrier == false && Damage * 2 > GameManager.Instance.defense)
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
            else if (Player.GetComponent<BattlePlayer>().IsBarrier == true || Damage * 2 <= GameManager.Instance.defense)
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
            if (GameManager.Instance.curHp > 0)
            {
                GameManager.Instance.BattleSkillBackGround.SetActive(false);
            }
            yield return new WaitForSeconds(1);
            transform.position = this.transform.position;
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
            yield return new WaitForSeconds(1.3f);
            GoToReturn = false;
            BattleManager.Instance.IsPlayerTurn = true;
            if (GameManager.Instance.curHp > 0)
            {
                GameManager.Instance.BattleButtonUi.SetActive(true);
            }
        }
        else if (SuperAngerCount >= MaxSuperAngerCount)
        {
            GameManager.Instance.BattleSkillText.text = "??????...?????!";
            BattleManager.Instance.IsEnemyTurn = false;
            yield return new WaitForSeconds(1.5f);
            SuperAngerCount = 0;
            IsSuperAnger = true;
            BattleManager.Instance.CamP = true;
            animator.SetBool("IsAttack", true);
            GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(1f);
            yield return new WaitForSeconds(2);
            animator.SetBool("IsAttack", false);
            BattleManager.Instance.CamP = false;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            yield return new WaitForSeconds(1);
            BattleManager.Instance.IsPlayerTurn = true;
            GameManager.Instance.BattleButtonUi.SetActive(true);
        }
        yield return null;
    }
}