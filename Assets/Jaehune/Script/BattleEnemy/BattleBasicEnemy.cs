using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleBasicEnemy : MonoBehaviour
{
    public int Hp; //���� ü��
    [SerializeField] int MaxHp, Damage; //�ִ� ü��, ���ݷ�
    [SerializeField] RaycastHit2D hit;
    [SerializeField] GameObject Player, EnemySpawner; //�÷��̾�, ���� �� ���� ��ġ
    [SerializeField] Image HpBar; //���� ���� �� ��Ÿ���� ü�¹�(���̵�����)
    [SerializeField] bool GoToPlayer = false; //�÷��̾��� ��ġ(���� ���ݽ�)�� ���� �Ǵ�

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
        GameManager.Instance.BattleSkillText.text = "�ı��� �ϰ�";
        BattleManager.Instance.IsEnemyTurn = false;
        GoToPlayer = true;
        yield return new WaitForSeconds(1.5f);
        //���� �ִ� ���
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
