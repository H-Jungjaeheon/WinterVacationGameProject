using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayer : MonoBehaviour
{
    [SerializeField] int Damage; //적에게 줄 데미지
    [SerializeField] RaycastHit2D hit; //적 인식 레이캐스트
    [SerializeField] GameObject Enemy, PlayerSpawner; //전투시 인식한 적 오브젝트 담는 그릇
    [SerializeField] bool GoToEnemy = false; //적의 위치(근접 공격시)로 갈지 판단

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
        if(GoToEnemy == true && GameManager.Instance.IsBattleStart == true)
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
        GameManager.Instance.BattleSkillBackGround.SetActive(true);
        GameManager.Instance.BattleSkillText.text = "주인공 박치기";
        BattleManager.Instance.IsPlayerTurn = false;
        GoToEnemy = true;
        yield return new WaitForSeconds(1.5f);
        //공격 애니 재생
        Enemy.GetComponent<BattleBasicEnemy>().Hp -= Damage;
        yield return new WaitForSeconds(1);
        GameManager.Instance.BattleSkillBackGround.SetActive(false);
        GoToEnemy = false;
        yield return new WaitForSeconds(3);
        if(GameManager.Instance.IsCamMove == true)
        {
            BattleManager.Instance.IsEnemyTurn = true;
        }
        else
        {
            BattleManager.Instance.IsPlayerTurn = true;
        }
        yield return null;
    }
}
