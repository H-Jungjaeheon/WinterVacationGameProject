using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleBerserkerEnemy : BattleBasicEnemy
{
    [SerializeField] float SuperSkillCount, MaxSuperSkillCount, SkillAttackRand, IsMad;
    [SerializeField] GameObject HealText;
    [SerializeField] bool End = false, IsDead;
    [SerializeField] Image MadBar, SuperSkillImage;
    [SerializeField] Vector2 BarPosition, PicturePosition, SuperSkillPosition;
    [SerializeField] Text SuperSkillText;
    public bool IsSuperSkillng;
    public override void Start()
    {
        IsSuperSkillng = false;
        IsDead = false;
        IsMad = 0;
        animator = GetComponent<Animator>();
        Anger = 0;
        MaxHp *= GameManager.Instance.Stage;
        Hp *= GameManager.Instance.Stage;
        SR = this.GetComponent<SpriteRenderer>();
        this.transform.position = EnemySpawner.transform.position + new Vector3(1.5f, 0.7f, 0);
        SkillAttackRand = Random.Range(1, 3);
    }

    public override void Update()
    {
        base.Update();
        HpAndText();
    }
    void HpAndText()
    {
        if (Hp <= 0)
        {
            Hp = 0;
        }
        if (Hp >= MaxHp)
        {
            Hp = MaxHp;
        }
        if (End == true)
        {
            StartCoroutine(SuperSkillImageFadeOut(1));
            End = false;
            IsSuperSkillng = false;
            SuperSkillText.text = (IsMad - 3).ToString();
        }
        else
        {
            SuperSkillText.text = (3 - IsMad).ToString();
        }
    }
    public override void AttackGone()
    {
        if (GoToPlayer == true && BattleManager.Instance.IsPlayerTurn == false && StopGone == false)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, Player.transform.position + new Vector3(4f, -0.1f, 0), 10 * Time.deltaTime);
        }
        else if (GoToReturn == true && IsDead == false)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, EnemySpawner.transform.position + new Vector3(1.5f, 0.7f, 0), 10 * Time.deltaTime);
        }
    }
    public override void Hpbar()
    {
        HpBar.fillAmount = Hp / MaxHp;
        AngerBar.fillAmount = Anger / MaxAnger;
        MadBar.fillAmount = SuperSkillCount / MaxSuperSkillCount;
        HpBar.transform.position = new Vector2(this.transform.position.x + BarPosition.x, this.transform.position.y + BarPosition.y);
        AngerBar.transform.position = new Vector2(this.transform.position.x + BarPosition.x, this.transform.position.y + BarPosition.y - 0.2f);
        MadBar.transform.position = new Vector2(this.transform.position.x + BarPosition.x, this.transform.position.y + BarPosition.y + 0.2f);
        SuperSkillImage.transform.position = new Vector2(this.transform.position.x + SuperSkillPosition.x, this.transform.position.y + SuperSkillPosition.y);
        HpBarNull.transform.position = new Vector2(this.transform.position.x + BarPosition.x , this.transform.position.y + BarPosition.y);
        EnemyPicture.transform.position = new Vector2(this.transform.position.x + PicturePosition.x, this.transform.position.y + PicturePosition.y);
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
        Color color6 = MadBar.color;
        while (color.a > 0f && color2.a > 0f && color3.a > 0f && color4.a > 0f)
        {
            color.a -= Time.deltaTime / FaidTime;
            color2.a -= Time.deltaTime / FaidTime;
            color3.a -= Time.deltaTime / FaidTime;
            color4.a -= Time.deltaTime / FaidTime;
            color5.a -= Time.deltaTime / FaidTime;
            color6.a -= Time.deltaTime / FaidTime;
            SR.color = color;
            HpBar.color = color;
            HpBarNull.color = color;
            EnemyPicture.color = color;
            AngerBar.color = color;
            MadBar.color = color;
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
                yield return null;
                yield return new WaitForSeconds(1);
                GameManager.Instance.IsBattleStart = false;
                BattleManager.Instance.IsEnemyTurn = true;
                BattleManager.Instance.IsPlayerTurn = true;
                yield return new WaitForSeconds(1);
                Destroy(this.gameObject);
            }
        }
    }
    IEnumerator SuperSkillImageFadeIn(float FaidTime)
    {
        Color color = SuperSkillImage.color;
        Color color2 = SuperSkillText.color;
        while (color.a < 1f)
        {
            color.a += Time.deltaTime / FaidTime;
            color2.a += Time.deltaTime / FaidTime;
            SuperSkillImage.color = color;
            SuperSkillText.color = color;
            if (color.a >= 1f)
            {
                color.a = 1f;
                color2.a = 1f;
            }
            yield return null;
        }
    }
    IEnumerator SuperSkillImageFadeOut(float FaidTime)
    {
        Color color = SuperSkillImage.color;
        Color color2 = SuperSkillText.color;
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / FaidTime;
            color2.a -= Time.deltaTime / FaidTime;
            SuperSkillImage.color = color;
            SuperSkillText.color = color;
            if (color.a <= 0f)
            {
                color.a = 0f;
                color2.a = 0f;
                IsMad = 0;
            }
            yield return null;
        }
    }
    public override IEnumerator EnemyAttack()
    {
        Debug.Log("°ø°Ý ½ÇÇà");
        SkillAttackRand = Random.Range(1, 3);
        GameManager.Instance.BattleSkillBackGround.SetActive(true);
        if (Anger < MaxAnger && SuperSkillCount < MaxSuperSkillCount)
        {
            GameManager.Instance.BattleSkillText.text = "»ìÀ» Âõ´Â ¼Õ³¯";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsAttack", true);
            StopGone = true;
            transform.position = this.transform.position - new Vector3(1.5f, 0f, 0);
            GameObject DT = Instantiate(DmgText);
            if (Player.GetComponent<BattlePlayer>().IsBarrier == false)
            {
                DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
                DT.transform.position = Player.transform.position;
                if (IsSuperSkillng == true)
                {
                    DT.GetComponent<BattleDamageText>().damage = (Damage + 4) - GameManager.Instance.defense;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += (Damage + 4) - GameManager.Instance.defense;
                }
                else
                {
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
            if (GameManager.Instance.curHp > 0)
            {
                GameManager.Instance.BattleSkillBackGround.SetActive(false);
            }
            yield return new WaitForSeconds(1);
            transform.position = this.transform.position + new Vector3(1.5f, 0f, 0);
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
                if (GameManager.Instance.curHp > 0)
                {
                    GameManager.Instance.BattleButtonUi.SetActive(true);
                }
            }
        }
        else if (Anger >= MaxAnger && SkillAttackRand == 1 && SuperSkillCount < MaxSuperSkillCount)
        {
            Anger = 0;
            if(IsSuperSkillng == false)
            {
                SuperSkillCount += 30;
            }
            GameManager.Instance.BattleSkillText.text = "À°Âü°ñ´Ü(ë¿óÖÍéÓ¨)";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsSkillTwice", true);
            StopGone = true;
            transform.position = this.transform.position - new Vector3(1.5f, 0f, 0);
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
                    DT.GetComponent<BattleDamageText>().damage = (Damage * 4) - GameManager.Instance.defense;
                    DT1.GetComponent<BattleDamageText>().damage = 5;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += (Damage * 4) - GameManager.Instance.defense;
                    Hp -= 5;
                }
                else
                {
                    DT.transform.position = Player.transform.position;
                    DT1.transform.position = this.transform.position;
                    DT.GetComponent<BattleDamageText>().damage = (Damage * 3) - GameManager.Instance.defense;
                    DT1.GetComponent<BattleDamageText>().damage = 5;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += (Damage * 3) - GameManager.Instance.defense;
                    Hp -= 5;
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
            if (GameManager.Instance.curHp > 0)
            {
                GameManager.Instance.BattleSkillBackGround.SetActive(false);
            }
            yield return new WaitForSeconds(1);
            transform.position = this.transform.position + new Vector3(1.5f, 0f, 0);
            StopGone = false;
            animator.SetBool("IsSkillTwice", false);
            BattleManager.Instance.CamE = false;
            if (Hp <= 0 && IsSuperSkillng == false)
            {
                GoToReturn = false;
            }
            else
            {
                GoToReturn = true;
            }
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
                if (GameManager.Instance.curHp > 0)
                {
                    GameManager.Instance.BattleButtonUi.SetActive(true);
                }
                SkillAttackRand = Random.Range(1, 3);
            }
        }
        else if (Anger >= MaxAnger && SkillAttackRand == 2 && SuperSkillCount < MaxSuperSkillCount)
        {
            Anger = 0;
            if (IsSuperSkillng == false)
            {
                SuperSkillCount += 40;
            }
            GameManager.Instance.BattleSkillText.text = "³ÊÀÇ ÇÇ¸¦ ÈûÀÔ¾î";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsSkillOnce", true);
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
                    DT.GetComponent<BattleDamageText>().damage = (Damage * 3) - GameManager.Instance.defense;
                    DT2.GetComponent<BattleDamageText>().damage = Damage + 6;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += (Damage * 3) - GameManager.Instance.defense;
                    Hp += Damage + 6;
                }
                else
                {
                    DT.GetComponent<BattleDamageText>().damage = (Damage * 2) - GameManager.Instance.defense;
                    DT2.GetComponent<BattleDamageText>().damage = Damage + 3;
                    GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
                    Player.GetComponent<BattlePlayer>().IsHit = true;
                    GameManager.Instance.stackDamage += (Damage * 2) - GameManager.Instance.defense;
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
            if (GameManager.Instance.curHp > 0)
            {
                GameManager.Instance.BattleSkillBackGround.SetActive(false);
            }
            yield return new WaitForSeconds(1);
            transform.position = this.transform.position + new Vector3(0.9f, -0.5f, 0);
            StopGone = false;
            animator.SetBool("IsSkillOnce", false);
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
                if (GameManager.Instance.curHp > 0)
                {
                    GameManager.Instance.BattleButtonUi.SetActive(true);
                }
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
            StartCoroutine(SuperSkillImageFadeIn(1));
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
