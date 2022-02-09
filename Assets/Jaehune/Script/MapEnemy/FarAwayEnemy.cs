using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarAwayEnemy : BasicEnemyScript
{
    //Start is called before the first frame update
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
        MoveCount = 0;
        if (Speed > 0 && GameManager.Instance.isEunsin == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position + new Vector3(0, 0.42f, 0), Speed * 1.3f * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position + new Vector3(0, 0.42f, 0), Speed * -1.3f * Time.deltaTime);
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
    public override void CrossroadPlus()
    {
        base.CrossroadPlus();
    }
}
