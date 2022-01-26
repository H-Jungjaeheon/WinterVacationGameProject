using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemyScript : MonoBehaviour
{
    [SerializeField] float Speed, MoveCount, MaxMoveCount, SeeCrossroad; //배회 속도, 배회 시간, 최대 배회 시간, 인식 사거리
    [SerializeField] bool IsFind = false, IsMove = true; //플레이어 발견, 자신의 움직임 판별
    [SerializeField] GameObject Player, WarningObj; //플레이어 오브젝트, 발견시 느낌표 오브젝트
    [SerializeField] RaycastHit2D hit; 
    [SerializeField] int SpawnMonsterCount; //전투 시작 시 전투 필드에 소환할 몬스터 (0 ~ ... & 자기 스프라이트에 맞는 몬스터 전투 필드에 소환)

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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
        if(IsFind == true)
        {
            FindPlayer();
        }
    }

    void Moving()
    {
        MoveCount += Time.deltaTime;
        transform.position += new Vector3(Speed * Time.deltaTime, 0, 0);
        if(MoveCount >= MaxMoveCount)
        {
            MoveCount = 0;
            IsMove = false;
            Invoke("Trun", 4f);
        }
    }
    void Trun()
    {
        Speed *= -1;
        SeeCrossroad *= -1;
        IsMove = true;
    }
    void RayCasting()
    {
        Debug.DrawRay(transform.position, Vector3.left * SeeCrossroad, Color.red);
        var rayHit = Physics2D.RaycastAll(transform.position, Vector3.left, SeeCrossroad);
        foreach(var hit in rayHit)
        {
            if (hit.collider.gameObject.CompareTag("Player")) 
            {
                WarningObj.SetActive(true);
                IsFind = true;
            }
            else
            {
                WarningObj.SetActive(false);
                IsFind = false;
            }
        }
    }
    void FindPlayer()
    {
        MoveCount = 0;
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, Speed * 1.3f * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && GameManager.Instance.IsBattleStart == false)
        {
            Speed = 0;
            Invoke("Delete", 2f);
            Instantiate(BattleManager.Instance.Enemy[SpawnMonsterCount], BattleManager.Instance.EnemySpawner.transform.position, transform.rotation);    
        }
    }
    void Delete()
    {
        Destroy(this.gameObject);
    }
}
