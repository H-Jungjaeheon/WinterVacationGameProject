using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructEnemy : BasicEnemyScript
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
    public override void Moving()
    {
        base.Moving();
    }
    public override void Trun()
    {
        base.Trun();
    }
    public override void RayCasting()
    {
        base.RayCasting();
    }
    public override void FindPlayer()
    {
        if (Player.gameObject.CompareTag("Player") && GameManager.Instance.IsBattleStart == false && GameManager.Instance.BattleEndCount == 0)
        {
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
