using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleBasicEnemy : MonoBehaviour
{
    [SerializeField] float MaxHp, BarUp; //현재 체력
    public float Hp;
    public int AttackRand;
    [SerializeField] int Damage; //공격력
    [SerializeField] RaycastHit2D hit;
    [SerializeField] GameObject Player, EnemySpawner; //플레이어, 전투 적 스폰 위치
    [SerializeField] Image HpBar, HpBarNull, EnemyPicture; //전투 시작 시 나타나는 체력바, 시각적 편의를 위한 빈 체력바, 초상화
    [SerializeField] bool GoToPlayer = false; //플레이어의 위치(근접 공격시)로 갈지 판단

    // Start is called before the first frame update
    void Start()
    {
        GoToPlayer = false;
        this.transform.position = EnemySpawner.transform.position;
        AttackRand = Random.Range(1, 3);
    }
    // Update is called once per frame
    void Update()
    {
        RayCasting();
        Hpbar();
        if (Hp <= 0)
        {
            Dead();
        }
        if (BattleManager.Instance.IsEnemyTurn == true)
        {
            StartCoroutine("EnemyAttack");
        }
        if (GoToPlayer == true && BattleManager.Instance.IsPlayerTurn == false)
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
    void RayCasting()
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
    void Dead()
    {
        GameManager.Instance.IsBattleStart = false;
        Destroy(this.gameObject);
    }
    IEnumerator EnemyAttack()
    {
        GameManager.Instance.BattleSkillBackGround.SetActive(true);
        if(AttackRand == 1)
        {
            GameManager.Instance.BattleSkillText.text = "고양이 박치기";
        }
        else
        {
            GameManager.Instance.BattleSkillText.text = "궁극기 박치기";
        }
        BattleManager.Instance.IsEnemyTurn = false;
        GoToPlayer = true;
        yield return new WaitForSeconds(1.5f);
        //공격 애니 재생
        if(AttackRand == 1)
        {
            GameManager.Instance.curHp -= Damage;
        }
        else
        {
            GameManager.Instance.curHp -= Damage * 2;
        }
        yield return new WaitForSeconds(1);
        GameManager.Instance.BattleSkillBackGround.SetActive(false);
        GoToPlayer = false;
        yield return new WaitForSeconds(3);
        BattleManager.Instance.IsPlayerTurn = true;
        AttackRand = Random.Range(0, 2);
        yield return null;
    }
}
