using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleBasicEnemy : MonoBehaviour
{
    [SerializeField] float MaxHp, BarUp, GExp; //�ִ� ü��, ü�¹� ����, �ִ� ����ġ
    public float Hp; //���� ü��
    public int AttackRand; //���� ���� �������� ���ϱ�
    [SerializeField] int Damage; //���ݷ�
    [SerializeField] GameObject Player, EnemySpawner, DmgText; //�÷��̾�, ���� �� ���� ��ġ
    [SerializeField] Image HpBar, HpBarNull, EnemyPicture; //���� ���� �� ��Ÿ���� ü�¹�, �ð��� ���Ǹ� ���� �� ü�¹�, �ʻ�ȭ
    [SerializeField] bool GoToPlayer = false, Dead = false; //�÷��̾��� ��ġ(���� ���ݽ�)�� ���� �Ǵ�
    SpriteRenderer SR; //���� �� ���� �������

    void Start()
    {
        MaxHp *= GameManager.Instance.Stage;
        Hp *= GameManager.Instance.Stage;
        SR = this.GetComponent<SpriteRenderer>();
        this.transform.position = EnemySpawner.transform.position;
        AttackRand = Random.Range(1, 3);
    }
 
    void Update()
    {
        RayCasting();
        Hpbar();
        Dead1();
        AttackGone();
        if (BattleManager.Instance.IsEnemyTurn == true && BattleManager.Instance.IsEnemyDead == false)
        {
            StartCoroutine("EnemyAttack");
        }
    }
    void AttackGone()
    {
        if(GoToPlayer == true && BattleManager.Instance.IsPlayerTurn == false)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, Player.transform.position + new Vector3(2, 0, 0), 10 * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(this.transform.position, EnemySpawner.transform.position, 10 * Time.deltaTime);
        }
    }
    void Hpbar()
    {
        HpBar.fillAmount = Hp / MaxHp;
        HpBar.transform.position = this.transform.position + new Vector3(0.2f, BarUp, 0);
        HpBarNull.transform.position = this.transform.position + new Vector3(0.2f, BarUp, 0);
        EnemyPicture.transform.position = this.transform.position + new Vector3(-1.1f, BarUp, 0);
    }
    void RayCasting() //�÷��̾� ���� ĳ���� �ν�
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
    void Dead1() //���� ��
    {
        if (Hp <= 0)
        {
            StartCoroutine("Dead2", 0.5f);
        }
    }
    IEnumerator Dead2(float FaidTime)
    {
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
        while (color.a > 0f && color2.a > 0f && color3.a > 0f && color4.a > 0f) //���� �� �� ��� �׷��� �ֱ� 
        {
            color.a -= Time.deltaTime / FaidTime;
            color2.a -= Time.deltaTime / FaidTime;
            color3.a -= Time.deltaTime / FaidTime;
            color4.a -= Time.deltaTime / FaidTime;
            SR.color = color;
            HpBar.color = color;
            HpBarNull.color = color;
            EnemyPicture.color = color;
            if (color.a <= 0f)
            {
                color.a = 0f;
                color2.a = 0f;
                color3.a = 0f;
                color4.a = 0f;
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
    IEnumerator EnemyAttack()
    {
        GameManager.Instance.BattleSkillBackGround.SetActive(true);
        if(AttackRand == 1)
        {
            GameManager.Instance.BattleSkillText.text = "����� ��ġ��";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            //���� �ִ� ���
            GameObject DT = Instantiate(DmgText);
            DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
            DT.transform.position = Player.transform.position;
            DT.GetComponent<BattleDamageText>().damage = Damage;
            GameManager.Instance.stackDamage += Damage;
            yield return new WaitForSeconds(1);
            BattleManager.Instance.CamE = false;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            GoToPlayer = false;
            yield return new WaitForSeconds(3);
            BattleManager.Instance.IsPlayerTurn = true;
            AttackRand = Random.Range(1, 3);
        }
        else if (AttackRand == 2)
        {
            GameManager.Instance.BattleSkillText.text = "�ñر� ��ġ��";
            BattleManager.Instance.IsEnemyTurn = false;
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            BattleManager.Instance.CamE = true;
            //���� �ִ� ���
            GameObject DT = Instantiate(DmgText);
            DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
            DT.transform.position = Player.transform.position;
            DT.GetComponent<BattleDamageText>().damage = Damage * 2;
            GameManager.Instance.curHp -= Damage * 2;
            yield return new WaitForSeconds(1);
            BattleManager.Instance.CamE = false;
            GameManager.Instance.BattleSkillBackGround.SetActive(false);
            GoToPlayer = false;
            yield return new WaitForSeconds(3);
            BattleManager.Instance.IsPlayerTurn = true;
            AttackRand = Random.Range(1, 3);
        }
        yield return null;
    }
}