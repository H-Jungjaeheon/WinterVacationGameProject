using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleBerserkerEnemy : BattleBasicEnemy
{
    [SerializeField] float SuperSkillCount, MaxSuperSkillCount, SkillAttackRand, IsMad;
    [SerializeField] GameObject HealText;
    [SerializeField] bool End = false;
    [SerializeField] Image MadBar;
    public bool IsSuperSkillng;
    // Start is called before the first frame update
    public override void Start()
    {
        IsSuperSkillng = false;
        IsMad = 0;
        animator = GetComponent<Animator>();
        Anger = 0;
        MaxHp *= GameManager.Instance.Stage;
        Hp *= GameManager.Instance.Stage;
        SR = this.GetComponent<SpriteRenderer>();
        this.transform.position = EnemySpawner.transform.position + new Vector3(0, 0.9f, 0);
        SkillAttackRand = Random.Range(1, 3);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(End == true)
        {
            End = false;
            IsMad = 0;
            IsSuperSkillng = false;
        }
        if(Hp <= 0)
        {
            Hp = 0;
        }
    }
    public override void AttackGone()
    {
        if (GoToPlayer == true && BattleManager.Instance.IsPlayerTurn == false && StopGone == false)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, Player.transform.position + new Vector3(2.5f, 0.06f, 0), 10 * Time.deltaTime);
        }
        else if (GoToReturn == true)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, EnemySpawner.transform.position + new Vector3(0, 0.9f, 0), 10 * Time.deltaTime);
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
        MadBar.fillAmount = SuperSkillCount / MaxSuperSkillCount;
        HpBar.transform.position = this.transform.position + new Vector3(0.35f, BarUp + 0.05f, 0);
        AngerBar.transform.position = this.transform.position + new Vector3(0.35f, BarUp - 0.2f, 0);
        MadBar.transform.position = this.transform.position + new Vector3(0.35f, BarUp + 0.3f, 0);
        HpBarNull.transform.position = this.transform.position + new Vector3(0.35f, BarUp + 0.05f, 0);
        EnemyPicture.transform.position = this.transform.position + new Vector3(-1.3f, BarUp, 0);
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
        if (Hp <= 0 && IsSuperSkillng != true)
        {
            BattleManager.Instance.IsEnemyDead = true;
            animator.SetBool("IsDead", true);
            StartCoroutine("Dead2", 0.5f);
        }
    }
    public override IEnumerator Dead2(float FaidTime)
    {
        return base.Dead2(FaidTime);
    }
    public override IEnumerator EnemyAttack()
    {
        Debug.Log("°ø°Ý ½ÇÇà");
        SkillAttackRand = Random.Range(1, 3);
        animator.SetBool("IsWalk", false);
        GameManager.Instance.BattleSkillBackGround.SetActive(true);
        if (Anger < MaxAnger && SuperSkillCount < MaxSuperSkillCount) //AttackRand == 1
        {
            GameManager.Instance.BattleSkillText.text = "»ìÀ» Âõ´Â ¼Õ³¯";
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
                if (IsSuperSkillng == true)
                {
                    DT.GetComponent<BattleDamageText>().damage = Damage + 4;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += Damage + 4;
                }
                else
                {
                    DT.GetComponent<BattleDamageText>().damage = Damage;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += Damage;
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
            animator.SetBool("IsAttack", false);
            BattleManager.Instance.CamE = false;
            GoToReturn = true;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            GoToPlayer = false;
            Anger += 40;
            yield return new WaitForSeconds(1);
            GoToReturn = false;
            if (IsSuperSkillng == true)
            {
                IsMad += 1;
            }
            if (IsMad == 3)
            {
                End = true;
            }
            yield return new WaitForSeconds(2);
            if (IsSuperSkillng == false && Hp <= 0)
            {
                Dead1();
                End = true;
                GameManager.Instance.BattleButtonUi.SetActive(false);
                BattleManager.Instance.IsPlayerTurn = false;
                Debug.Log("Á×À½ ½ÇÇà");
            }
            else
            {
                BattleManager.Instance.IsPlayerTurn = true;
                GameManager.Instance.BattleButtonUi.SetActive(true);
            }
        }
        else if (Anger >= MaxAnger && SkillAttackRand == 1 && SuperSkillCount < MaxSuperSkillCount) //ÀÚ±â Ã¼·Â ±ð°í Å« µ¥¹ÌÁö ÀÔÈû
        {
            Anger = 0;
            if(IsSuperSkillng == false)
            {
                SuperSkillCount += 25;
            }
            GameManager.Instance.BattleSkillText.text = "À°Âü°ñ´Ü(ë¿óÖÍéÓ¨)";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsSkill", true);
            StopGone = true;
            transform.position = this.transform.position + new Vector3(-0.9f, 0.5f, 0);
            GameObject DT = Instantiate(DmgText);
            GameObject DT1 = Instantiate(DmgText);
            if (Player.GetComponent<BattlePlayer>().IsBarrier == false)
            {
                DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT1.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                if (IsSuperSkillng == true)
                {
                    DT.transform.position = Player.transform.position;
                    DT1.transform.position = this.transform.position;
                    DT.GetComponent<BattleDamageText>().damage = Damage * 4;
                    DT1.GetComponent<BattleDamageText>().damage = 5;
                    Hp -= 5;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += Damage * 4;
                }
                else
                {
                    DT.transform.position = Player.transform.position;
                    DT1.transform.position = this.transform.position;
                    DT.GetComponent<BattleDamageText>().damage = Damage * 3;
                    DT1.GetComponent<BattleDamageText>().damage = 5;
                    Hp -= 5;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += Damage * 3;
                }
            }
            else
            {
                DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT1.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT.transform.position = Player.transform.position;
                DT1.transform.position = this.transform.position;
                DT.GetComponent<BattleDamageText>().damage = 0;
                DT1.GetComponent<BattleDamageText>().damage = 5;
                Hp -= 5;
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
            if (IsSuperSkillng == true)
            {
                IsMad += 1;
            }
            if (IsMad == 3)
            {
                End = true;
            }
            yield return new WaitForSeconds(2);
            if (IsSuperSkillng == false && Hp <= 0)
            {
                Debug.Log("Á×À½ ½ÇÇà");
                Dead1();
                End = true;
                GameManager.Instance.BattleButtonUi.SetActive(false);
            }
            else
            {
                BattleManager.Instance.IsPlayerTurn = true;
                GameManager.Instance.BattleButtonUi.SetActive(true);
                SkillAttackRand = Random.Range(1, 3);
            }
        }
        else if (Anger >= MaxAnger && SkillAttackRand == 2 && SuperSkillCount < MaxSuperSkillCount)//µ¥¹ÌÁö ÀÔÈ÷°í ±×¸¸Å­ Ã¼·Â È¸º¹
        {
            Anger = 0;
            if (IsSuperSkillng == false)
            {
                SuperSkillCount += 25;
            }
            GameManager.Instance.BattleSkillText.text = "³ÊÀÇ ÇÇ¸¦ ÈûÀÔ¾î";
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
                DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT2.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT.transform.position = Player.transform.position;
                DT2.transform.position = this.transform.position;
                if (IsSuperSkillng == true)
                {
                    DT.GetComponent<BattleDamageText>().damage = Damage * 3;
                    DT2.GetComponent<BattleDamageText>().damage = Damage + 6;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += Damage * 3;
                    Hp += Damage + 6;
                }
                else
                {
                    DT.GetComponent<BattleDamageText>().damage = Damage * 2;
                    DT2.GetComponent<BattleDamageText>().damage = Damage + 3;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += Damage * 2;
                    Hp += Damage + 3;
                }
            }
            else
            {
                DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT2.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT.transform.position = Player.transform.position;
                DT2.transform.position = this.transform.position;
                DT.GetComponent<BattleDamageText>().damage = 0;
                DT2.GetComponent<BattleDamageText>().damage = 0;
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
            if (IsSuperSkillng == true)
            {
                IsMad += 1;
            }
            if (IsMad == 3)
            {
                End = true;
            }
            yield return new WaitForSeconds(2);
            if(IsSuperSkillng == false && Hp <= 0)
            {
                Debug.Log("Á×À½ ½ÇÇà");
                Dead1();
                End = true;
                GameManager.Instance.BattleButtonUi.SetActive(false);
            }
            else
            {
                BattleManager.Instance.IsPlayerTurn = true;
                GameManager.Instance.BattleButtonUi.SetActive(true);
                SkillAttackRand = Random.Range(1, 3);
            }
        }
        else if(SuperSkillCount >= MaxSuperSkillCount)
        {
            GameManager.Instance.BattleSkillText.text = "ºí·¯µå ½ºÅ×ÀÎ";
            BattleManager.Instance.IsEnemyTurn = false;
            yield return new WaitForSeconds(1.5f);
            SuperSkillCount = 0;
            IsSuperSkillng = true;
            BattleManager.Instance.CamP = true;
            yield return new WaitForSeconds(2);
            animator.SetBool("IsAttack", false);
            BattleManager.Instance.CamP = false;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            Anger += 25;
            yield return new WaitForSeconds(3);
            BattleManager.Instance.IsPlayerTurn = true;
            GameManager.Instance.BattleButtonUi.SetActive(true);
        }
        yield return null;
    }
}
