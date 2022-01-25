using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayer : MonoBehaviour
{
    public int Hp;
    [SerializeField] int Damage;
    [SerializeField] RaycastHit2D hit;
    [SerializeField] GameObject Enemy;
    [SerializeField] bool GoToEnemy = false;
    public GameObject PlayerSpawner;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = PlayerSpawner.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        RayCasting();
        if (Input.GetKeyDown(KeyCode.Space) && BattleManager.Instance.IsPlayerTurn == true && GameManager.Instance.AttackOk == true)
        {
            Playerattack();
        }
        if(GoToEnemy == true)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, Enemy.transform.position - new Vector3(2, 0, 0), 10 * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(this.transform.position, PlayerSpawner.transform.position, 10 * Time.deltaTime);
        }
    }

    void RayCasting()
    {
        Debug.DrawRay(this.transform.position, Vector3.right * 50, Color.red);
        var rayHit = Physics2D.RaycastAll(transform.position, Vector3.right, 50);
        foreach (var hit in rayHit)
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Enemy = hit.collider.gameObject; 
            }
        }
    }
    public void Playerattack()
    {
        if (BattleManager.Instance.IsPlayerTurn == true && GameManager.Instance.AttackOk == true)
        {
            StartCoroutine("PlayerAttack", 1f);
        }
    }
    public IEnumerator PlayerAttack()
    {
        BattleManager.Instance.IsPlayerTurn = false;
        GoToEnemy = true;
        yield return new WaitForSeconds(1.5f);
        //공격 애니 재생
        Enemy.GetComponent<BattleBasicEnemy>().Hp -= Damage;
        yield return new WaitForSeconds(1);
        GoToEnemy = false;
        yield return new WaitForSeconds(4);
        BattleManager.Instance.IsEnemyTurn = true;
        yield return null;
    }
}
