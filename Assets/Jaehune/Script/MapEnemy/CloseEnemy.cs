using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseEnemy : BasicEnemyScript
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void Moving()
    {
        MoveCount += Time.deltaTime;
        transform.position += new Vector3(Speed * Time.deltaTime, 0, 0);
        if (MoveCount >= MaxMoveCount)
        {
            IsStop = true;
            animator.SetBool("IsIdle", true);
            MoveCount = 0;
            IsMove = false;
            Invoke("Trun", 4f);
        }
    }
    public override void WallRayCasting()
    {
        base.WallRayCasting();
    }
    public override void Trun()
    {
        animator.SetBool("IsIdle", false);
        base.Trun();
    }
    public override void RayCasting()
    {
        base.RayCasting();
    }
    public override void FindPlayer()
    {
        base.FindPlayer();
    }
    public override void Delete()
    {
        base.Delete();
    }
    public override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
    }
    public override void CrossroadPlus()
    {
        base.CrossroadPlus();
    }
}
