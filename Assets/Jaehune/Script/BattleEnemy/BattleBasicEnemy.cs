using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleBasicEnemy : MonoBehaviour
{
    public float MaxHp, BarUp, GExp, Hp, Anger, MaxAnger, HitAnger; //최대 체력, 체력바 높이, 주는 경험치, 분노 게이지, 최대 분노 게이지
    public int Damage; //공격력
    public GameObject Player, EnemySpawner, DmgText; //플레이어, 전투 적 스폰 위치
    public Image HpBar, HpBarNull, EnemyPicture, AngerBar; //전투 시작 시 나타나는 체력바, 시각적 편의를 위한 빈 체력바, 초상화, 
    public bool GoToPlayer = false, Dead = false, GoToReturn = false, StopGone = false, IsHit = false, //플레이어의 위치(근접 공격시)로 갈지 판단, 사망 판단, 공격 후 제자리, 이동 멈춤, 맞았는가?
    IsReflect = false, IsStun, IsPoison, IsInstantDead; //특수 능력 모음
    public SpriteRenderer SR; //죽을 때 점점 사라지게
    public Animator animator;
    public ParticleSystem ps;
    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        Anger = 0;
        MaxHp *= GameManager.Instance.Stage;
        Hp *= GameManager.Instance.Stage;
        SR = this.GetComponent<SpriteRenderer>();
        this.transform.position = EnemySpawner.transform.position;
        ps.Stop();
    }
    public virtual void Update()
    {
        RayCasting();
        Hpbar();
        Dead1();
        AttackGone();
        if (BattleManager.Instance.IsEnemyTurn == true && BattleManager.Instance.IsEnemyDead == false)
        {
            StartCoroutine("EnemyAttack");
        }
        if(IsHit == true)
        {
            StartCoroutine("EnemyHit");
        }
    }
    public virtual void AttackGone()
    {
        if(GoToPlayer == true && BattleManager.Instance.IsPlayerTurn == false && StopGone == false)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, Player.transform.position + new Vector3(2.5f, 0, 0), 10 * Time.deltaTime);
        }
        else if(GoToReturn == true)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, EnemySpawner.transform.position, 10 * Time.deltaTime);
        }
        else if(GoToReturn == false)
        {
            animator.SetBool("IsWalk", false);
        }
    }
    public virtual void Hpbar() 
    {
        HpBar.fillAmount = Hp / MaxHp;
        AngerBar.fillAmount = Anger / MaxAnger;
        HpBar.transform.position = this.transform.position + new Vector3(0.35f, BarUp + 0.05f, 0);
        AngerBar.transform.position = this.transform.position + new Vector3(0.35f, BarUp - 0.19f, 0);
        HpBarNull.transform.position = this.transform.position + new Vector3(0.35f, BarUp + 0.05f, 0);
        EnemyPicture.transform.position = this.transform.position + new Vector3(-1.3f, BarUp, 0);
    }
    public virtual void RayCasting() 
    {
        Debug.DrawRay(transform.position, Vector3.left * 50, Color.red);
        var rayHit = Physics2D.RaycastAll(transform.position, Vector3.left, 50);
        foreach (var hit in rayHit)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                Player = hit.collider.gameObject;
            }
        }
    }
    public virtual IEnumerator EnemyHit()
    {
        IsHit = false;
        animator.SetBool("IsDamage", true);
        Anger += HitAnger; 
        ps.Play();
        yield return new WaitForSeconds(1);
        animator.SetBool("IsDamage", false);
        ps.Stop();
        yield return null;
    }
    public virtual void Dead1()
    {
        if (Hp <= 0)
        {
            BattleManager.Instance.IsEnemyDead = true;
            animator.SetBool("IsDead", true);
            StartCoroutine("Dead2", 0.5f);
        }
    }
    public virtual IEnumerator Dead2(float FaidTime)
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
        while (color.a > 0f && color2.a > 0f && color3.a > 0f && color4.a > 0f) 
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
            }
        }        
    }
    public virtual IEnumerator EnemyAttack()
    {
        animator.SetBool("IsWalk", false);
        GameManager.Instance.BattleSkillBackGround.SetActive(true);
        if(Anger < MaxAnger) 
        {
            GameManager.Instance.BattleSkillText.text = "강력한 두개골";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsAttack", true);
            StopGone = true;
            transform.position = this.transform.position + new Vector3(-0.9f, 0.5f, 0);
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
            else if(Player.GetComponent<BattlePlayer>().IsBarrier == true || Damage <= GameManager.Instance.defense)
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
            yield return new WaitForSeconds(1);
            BattleManager.Instance.IsPlayerTurn = true;
            if (GameManager.Instance.curHp > 0)
            {
                GameManager.Instance.BattleButtonUi.SetActive(true);
            }
        }
        else if (Anger >= MaxAnger) 
        {
            Anger = 0;
            GameManager.Instance.BattleSkillText.text = "날카로운 이빨";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsSkill", true);
            StopGone = true;
            transform.position = this.transform.position + new Vector3(-0.9f, 0.5f, 0);
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
            else if(Player.GetComponent<BattlePlayer>().IsBarrier == true || Damage * 2 <= GameManager.Instance.defense)
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
            transform.position = this.transform.position + new Vector3(0.9f, -0.5f, 0);
            StopGone = false;
            animator.SetBool("IsSkill", false);
            BattleManager.Instance.CamE = false;
            GoToReturn = true;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            GoToPlayer = false;
            yield return new WaitForSeconds(1);
            GoToReturn = false;
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