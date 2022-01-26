using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleBasicEnemy : MonoBehaviour
{
    public int Hp; //현재 체력
    [SerializeField] int MaxHp, Damage; //최대 체력, 공격력
    [SerializeField] RaycastHit2D hit;
    [SerializeField] GameObject Player, EnemySpawner; //플레이어, 전투 적 스폰 위치
    [SerializeField] Image HpBar; //전투 시작 시 나타나는 체력바(더미데이터)
    [SerializeField] bool GoToPlayer = false; //플레이어의 위치(근접 공격시)로 갈지 판단

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = EnemySpawner.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        RayCasting();
        if (Hp <= 0)
        {
            Dead();
        }
        if (BattleManager.Instance.IsEnemyTurn == true)
        {
            BattleManager.Instance.IsEnemyTurn = false;
            StartCoroutine("EnemyAttack", 1f);
        }
        if (GoToPlayer == true)
        {
           transform.position = Vector3.MoveTowards(this.transform.position, Player.transform.position + new Vector3(2, 0, 0), 10 * Time.deltaTime);
        }
        else
        {
           transform.position = Vector3.MoveTowards(this.transform.position, EnemySpawner.transform.position, 10 * Time.deltaTime);
        }
        HpBar.transform.position = this.transform.position + new Vector3(0, 0.4f, 0);
        HpBar.fillAmount = Hp / MaxHp;
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
    public IEnumerator EnemyAttack()
    {
        GameManager.Instance.BattleSkillBackGround.SetActive(true);
        GameManager.Instance.BattleSkillText.text = "파괴의 일격";
        BattleManager.Instance.IsEnemyTurn = false;
        GoToPlayer = true;
        yield return new WaitForSeconds(1.5f);
        //공격 애니 재생
        GameManager.Instance.curHp -= Damage;
        //Player.GetComponent<BattlePlayer>().Hp -= Damage;
        yield return new WaitForSeconds(1);
        GameManager.Instance.BattleSkillBackGround.SetActive(false);
        GoToPlayer = false;
        yield return new WaitForSeconds(4);
        BattleManager.Instance.IsPlayerTurn = true;
        yield return null;
    }
}
