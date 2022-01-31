using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleBasicEnemy : MonoBehaviour
{
    [SerializeField] float MaxHp, BarUp, GExp, Anger, MaxAnger; //최대 체력, 체력바 높이, 주는 경험치, 분노 게이지, 최대 분노 게이지
    public float Hp; //현재 체력
    public bool IsHit = false;
    //public int AttackRand; //다음 공격 랜덤으로 정하기
    [SerializeField] int Damage; //공격력
    [SerializeField] GameObject Player, EnemySpawner, DmgText; //플레이어, 전투 적 스폰 위치
    [SerializeField] Image HpBar, HpBarNull, EnemyPicture, AngerBar; //전투 시작 시 나타나는 체력바, 시각적 편의를 위한 빈 체력바, 초상화, 
    [SerializeField] bool GoToPlayer = false, Dead = false, GoToReturn = false, StopGone = false; //플레이어의 위치(근접 공격시)로 갈지 판단
    SpriteRenderer SR; //죽을 때 점점 사라지게
    Animator animator;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        Anger = 0;
        MaxHp *= GameManager.Instance.Stage;
        Hp *= GameManager.Instance.Stage;
        SR = this.GetComponent<SpriteRenderer>();
        this.transform.position = EnemySpawner.transform.position;
        //AttackRand = Random.Range(1, 3);
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
    public virtual void AttackGone() //때리러 이동 및 복귀
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
    public virtual void Hpbar() //체력바 외 여러가지 바
    {
        HpBar.fillAmount = Hp / MaxHp;
        AngerBar.fillAmount = Anger / MaxAnger;
        HpBar.transform.position = this.transform.position + new Vector3(0.35f, BarUp + 0.05f, 0);
        AngerBar.transform.position = this.transform.position + new Vector3(0.35f, BarUp - 0.1f, 0);
        HpBarNull.transform.position = this.transform.position + new Vector3(0.35f, BarUp + 0.05f, 0);
        EnemyPicture.transform.position = this.transform.position + new Vector3(-0.83f, BarUp, 0);
    }
    public virtual void RayCasting() //플레이어 전투 캐릭터 인식
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
        yield return new WaitForSeconds(1);
        animator.SetBool("IsDamage", false);
        yield return null;
    }
    public virtual void Dead1() //죽을 때
    {
        if (Hp <= 0)
        {
            animator.SetBool("IsDead", true);
            StartCoroutine("Dead2", 0.5f);
        }
    }
    public virtual IEnumerator Dead2(float FaidTime)
    {
        yield return new WaitForSeconds(2);
        if (Dead == false)
        {
            Dead = true;
            GameObject.Find("GameManager").GetComponent<PlayerStats>().ExpUp(GExp);
        }
        BattleManager.Instance.IsEnemyDead = true;
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
                yield return new WaitForSeconds(1);
                Destroy(this.gameObject);
            }
        }        
    }
    public virtual IEnumerator EnemyAttack()
    {
        animator.SetBool("IsWalk", false);
        GameManager.Instance.BattleSkillBackGround.SetActive(true);
        if(Anger < MaxAnger) //AttackRand == 1
        {
            GameManager.Instance.BattleSkillText.text = "강력한 두개골";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsAttack", true);
            StopGone = true;
            transform.position = this.transform.position + new Vector3(-0.6f, 0.3f, 0);
            GameObject DT = Instantiate(DmgText);
            DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
            DT.transform.position = Player.transform.position;
            DT.GetComponent<BattleDamageText>().damage = Damage;
            GameManager.Instance.stackDamage += Damage;
            yield return new WaitForSeconds(1);
            transform.position = this.transform.position + new Vector3(0.6f, -0.3f, 0);
            StopGone = false;
            animator.SetBool("IsAttack", false);
            BattleManager.Instance.CamE = false;
            GoToReturn = true;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            GoToPlayer = false;
            Anger += 40;
            yield return new WaitForSeconds(1);
            GoToReturn = false;
            yield return new WaitForSeconds(2);
            BattleManager.Instance.IsPlayerTurn = true;
            //AttackRand = Random.Range(1, 3);
        }
        else if (Anger >= MaxAnger) //AttackRand == 2
        {
            Anger = 0;
            GameManager.Instance.BattleSkillText.text = "날카로운 이빨";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            animator.SetBool("IsAttack", true);
            StopGone = true;
            GameObject DT = Instantiate(DmgText);
            DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
            DT.transform.position = Player.transform.position;
            DT.GetComponent<BattleDamageText>().damage = Damage * 2;
            GameManager.Instance.curHp -= Damage * 2;
            yield return new WaitForSeconds(1);
            transform.position = this.transform.position + new Vector3(0.6f, -0.3f, 0);
            StopGone = false;
            animator.SetBool("IsAttack", false);
            BattleManager.Instance.CamE = false;
            GoToReturn = true;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            GoToPlayer = false;
            yield return new WaitForSeconds(1);
            GoToReturn = false;
            yield return new WaitForSeconds(2);
            BattleManager.Instance.IsPlayerTurn = true;
            //AttackRand = Random.Range(1, 3);
        }
        yield return null;
    }
}