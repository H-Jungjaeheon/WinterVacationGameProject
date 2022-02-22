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
        animator.SetBool("IsIdle", false);
        MoveCount = 0;
        IsTurns = false;
        IsMoveTurn = true;
        if (IsMove == true)
        {
            if (Speed > 0 && GameManager.Instance.isEunsin == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position + new Vector3(0, 0.42f, 0), Speed * 1.3f * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position + new Vector3(0, 0.42f, 0), Speed * -1.3f * Time.deltaTime);
            }
        }
        else
        {
            if (Speed > 0 && GameManager.Instance.isEunsin == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position + new Vector3(0, 0.42f, 0), Speed * 2f * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position + new Vector3(0, 0.42f, 0), Speed * -2f * Time.deltaTime);
            }
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
