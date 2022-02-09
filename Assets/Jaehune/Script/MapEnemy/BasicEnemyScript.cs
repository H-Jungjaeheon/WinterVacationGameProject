using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemyScript : MonoBehaviour
{
    public float Speed, MoveCount, MaxMoveCount, SeeCrossroad; //배회 속도, 배회 시간, 최대 배회 시간, 인식 사거리
    public bool IsFind = false, IsMove = true; //플레이어 발견, 자신의 움직임 판별
    public GameObject Player, WarningObj, CrossRoadObj; //플레이어 오브젝트(프리펩), 발견시 느낌표 오브젝트, 시야 범위 표시 오브젝트
    public RaycastHit2D hit;
    public int SpawnMonsterCount, TurnCount; //전투 시작 시 전투 필드에 소환할 몬스터 (0 ~ ... & 자기 스프라이트에 맞는 몬스터 전투 필드에 소환)
    public Animator animator;

    // Start is called before the first frame update
    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        TurnCount = 1;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        RayCasting();
        if(IsMove == true)
        {
            Moving();
        }
        else if(IsMove == false)
        {
            MoveCount = 0;
        }
        if (IsFind == true)
        {
            FindPlayer();
            CrossRoadObj.SetActive(false);
        }
        else
        {
            CrossRoadObj.SetActive(true);
        }
    }

    public virtual void Moving()
    {
        MoveCount += Time.deltaTime;
        transform.position += new Vector3(Speed * Time.deltaTime, 0, 0);
        if(MoveCount >= MaxMoveCount)
        {
            animator.SetBool("IsIdle", true);
            MoveCount = 0;
            IsMove = false;
            Invoke("Trun", 4f);
        }
    }
    public virtual void Trun()
    {
        Speed *= -1;
        SeeCrossroad *= -1;
        IsMove = true;
        if (TurnCount % 2 == 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        TurnCount++;
        if(TurnCount >= 3)
        {
            TurnCount = 1;
        }
        animator.SetBool("IsIdle", false);
    }
    public virtual void RayCasting()
    {
        Debug.DrawRay(transform.position, Vector3.left * SeeCrossroad, Color.red);
        var rayHit = Physics2D.RaycastAll(transform.position, Vector3.left, SeeCrossroad);
        foreach (var hit in rayHit)
        {
            if (hit.collider.gameObject.CompareTag("Player") && GameManager.Instance.isEunsin == false) 
            {
                Player = hit.collider.gameObject;
                WarningObj.SetActive(true);
                IsFind = true;
            }
            else if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                WarningObj.SetActive(false);
                IsFind = false;
            }
        }
    }
    public virtual void FindPlayer()
    {
        MoveCount = 0;
        if(Speed > 0 && GameManager.Instance.isEunsin == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position - new Vector3(0, 0.5f, 0), Speed * 1.3f * Time.deltaTime);
        }
        else if(Speed < 0 && GameManager.Instance.isEunsin == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position - new Vector3(0, 0.5f, 0), Speed * -1.3f * Time.deltaTime);
        }
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && GameManager.Instance.IsBattleStart == false && GameManager.Instance.BattleEndCount == 0 && GameManager.Instance.isEunsin == false)
        {
            Speed = 0;
            Instantiate(BattleManager.Instance.Enemy[SpawnMonsterCount], BattleManager.Instance.EnemySpawner.transform.position, Quaternion.Euler(0,0,0));
            Invoke("Delete", 2f);
        }
    }
    public virtual void Delete()
    {
        Destroy(this.gameObject);
    }
}
