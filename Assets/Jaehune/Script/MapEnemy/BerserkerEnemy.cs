using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerEnemy : BasicEnemyScript
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
        Vector3 PTransform = new Vector3(Player.transform.position.x, this.transform.position.y, this.transform.position.z);
        MoveCount = 0;
        IsTurns = false;
        IsMoveTurn = true;
        if (IsMove == true)
        {
            if (Speed > 0 && GameManager.Instance.isEunsin == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, PTransform, Speed * 1.05f * Time.deltaTime);
            }
            else if (Speed < 0 && GameManager.Instance.isEunsin == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, PTransform, Speed * -1.05f * Time.deltaTime);
            }
        }
        else
        {
            if (Speed > 0 && GameManager.Instance.isEunsin == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, PTransform, Speed * 2f * Time.deltaTime);
            }
            else if (Speed < 0 && GameManager.Instance.isEunsin == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, PTransform, Speed * -2f * Time.deltaTime);
            }
        }
    }
    public override void WallRayCasting()
    {
        base.WallRayCasting();
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
        IsPlus = 400;
    }
}
