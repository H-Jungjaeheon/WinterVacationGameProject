using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructEnemy : BasicEnemyScript
{
    [SerializeField] GameObject Warning;
    [SerializeField] bool IsSpawn = false;
    // Start is called before the first frame update
    public override void Start()
    {
        Warning.SetActive(false);
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
    public override void Moving()
    {
        MoveCount += Time.deltaTime;
        transform.position += new Vector3(Speed * Time.deltaTime, 0, 0);
        if (MoveCount >= MaxMoveCount)
        {
            animator.SetBool("IsIdle", true);
            MoveCount = 0;
            IsMove = false;
            Invoke("Trun", 4f);
            Warning.SetActive(true);
        }
    }
    public override void Trun()
    {
        Warning.SetActive(false);
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
        if (TurnCount >= 3)
        {
            TurnCount = 1;
        }
        animator.SetBool("IsIdle", false);
    }
    public override void RayCasting()
    {
        base.RayCasting();
    }
    public override void FindPlayer()
    {
        if (Player.gameObject.CompareTag("Player") && GameManager.Instance.IsBattleStart == false && GameManager.Instance.BattleEndCount == 0 && IsSpawn == true)
        {
            IsSpawn = true;
            Instantiate(BattleManager.Instance.Enemy[SpawnMonsterCount], BattleManager.Instance.EnemySpawner.transform.position, Quaternion.Euler(0, 0, 0));
            Invoke("Delete", 2f);
        }
    }
    public override void Delete()
    {
        base.Delete();
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
