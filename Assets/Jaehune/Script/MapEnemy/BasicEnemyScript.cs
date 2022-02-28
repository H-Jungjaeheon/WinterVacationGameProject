using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemyScript : MonoBehaviour
{
    public float Speed, MoveCount, MaxMoveCount, SeeCrossroad, IsPlus = 1, WallCrossroad; 
    public bool IsFind, IsMove, IsTurns, IsStop, IsMoveTurn; 
    public GameObject Player, WarningObj, CrossRoadObj; 
    public RaycastHit2D hit;
    public int SpawnMonsterCount, TurnCount;
    public Animator animator;

    // Start is called before the first frame update
    public virtual void Start()
    {
        IsStop = false;
        IsMove = true;
        IsTurns = true;
        animator = GetComponent<Animator>();
        TurnCount = 1;
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        RayCasting();
        WallRayCasting();
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
            CrossroadPlus();
            CrossRoadObj.SetActive(false);
        }
        else
        {
            if (IsStop == false)
            {
                IsMove = true;
            }
            IsTurns = true;
            CrossRoadObj.SetActive(true);
            IsPlus = 1;           
        }
    }
    public virtual void Moving()
    {
        MoveCount += Time.deltaTime;
        transform.position += new Vector3(Speed * Time.deltaTime, 0, 0);
        if(MoveCount >= MaxMoveCount)
        {
            IsStop = true;
            MoveCount = 0;
            IsMove = false;
            Invoke("Trun", 4f);
        }
    }
    public virtual void Trun()
    {
        if (IsTurns == true)
        {
            Speed *= -1;
            SeeCrossroad *= -1;
            WallCrossroad *= -1;
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
            if (TurnCount >= 3)
            {
                TurnCount = 1;
            }
        }
    }
    public virtual void RayCasting()
    {
        Debug.DrawRay(transform.position, Vector3.left * (SeeCrossroad * IsPlus), Color.red);
        var rayHit = Physics2D.RaycastAll(transform.position, Vector3.left, SeeCrossroad * IsPlus, LayerMask.GetMask("Player"));
        foreach (var hit in rayHit)
        {
            if (hit.collider.gameObject.CompareTag("Player") && GameManager.Instance.isEunsin == false) 
            {
                Player = hit.collider.gameObject;
                WarningObj.SetActive(true);
                IsFind = true;
            }
            else
            {
                WarningObj.SetActive(false);
                IsFind = false;

                if(IsMoveTurn == true)
                {
                    IsStop = false;
                    IsMoveTurn = false;
                }
            }
        }
    }
    public virtual void WallRayCasting()
    {
        Debug.DrawRay(transform.position - new Vector3(0, 0.5f, 0), Vector3.left * (WallCrossroad), Color.blue);
        var rayHits = Physics2D.RaycastAll(transform.position - new Vector3(0, 0.5f, 0), Vector3.left, WallCrossroad);
        foreach (var hit in rayHits)
        {
            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                MoveCount = MaxMoveCount;
            }
        }
    }
    public virtual void FindPlayer()
    {
        animator.SetBool("IsIdle", false);
        MoveCount = 0;
        IsTurns = false;
        IsMoveTurn = true;
        if(IsMove == true)
        {
            if (Speed > 0 && GameManager.Instance.isEunsin == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position - new Vector3(0, 0.5f, 0), Speed * 1.3f * Time.deltaTime);
            }
            else if (Speed < 0 && GameManager.Instance.isEunsin == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position - new Vector3(0, 0.5f, 0), Speed * -1.3f * Time.deltaTime);
            }
        }
        else
        {
            if (Speed > 0 && GameManager.Instance.isEunsin == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position - new Vector3(0, 0.5f, 0), Speed * 2f * Time.deltaTime);
            }
            else if (Speed < 0 && GameManager.Instance.isEunsin == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position - new Vector3(0, 0.5f, 0), Speed * -2f * Time.deltaTime);
            }
        }
    }
    public virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && GameManager.Instance.IsBattleStart == false&& GameManager.Instance.BattleEndCount == 0 && GameManager.Instance.isEunsin == false && GameManager.Instance.isDoor==false)
        {
            GameManager.Instance.IsBattleStart = true;
            Speed = 0;
            Instantiate(BattleManager.Instance.Enemy[SpawnMonsterCount], BattleManager.Instance.EnemySpawner.transform.position, Quaternion.Euler(0,0,0));
            Invoke("Delete", 2f);
        }
    }
    public virtual void Delete()
    {
        Destroy(this.gameObject);
    }
    public virtual void CrossroadPlus()
    {
        IsPlus = 5;
    }
}