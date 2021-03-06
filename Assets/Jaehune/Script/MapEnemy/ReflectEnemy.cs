using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectEnemy : BasicEnemyScript
{
    [SerializeField] float Dash, MaxDashTime, Dashing, MaxDashingTime;
    [SerializeField] bool IsDash;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Dash = 0;
        IsDash = false;
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if(IsDash == true)
        {
            Dashs();
        }
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
        Debug.DrawRay(transform.position, Vector3.left * SeeCrossroad * IsPlus, Color.red);
        if (Physics2D.Raycast(transform.position, Vector3.left, SeeCrossroad * IsPlus, LayerMask.GetMask("Player")) && GameManager.Instance.isEunsin == false)
        {
            WarningObj.SetActive(true);
            IsFind = true;
        }
        else
        {
            WarningObj.SetActive(false);
            IsFind = false;
            if (IsMoveTurn == true)
            {
                IsStop = false;
                IsMoveTurn = false;
            }
        }
    }
    public override void FindPlayer()
    {
        Vector3 PTransform = new Vector3(Player.transform.position.x, this.transform.position.y, this.transform.position.z);
        Dash += Time.deltaTime;
        MoveCount = 0;
        IsTurns = false;
        IsMoveTurn = true;
        if (IsMove == true)
        {
            if (Speed > 0 && GameManager.Instance.isEunsin == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, PTransform, Speed * 1.3f * Time.deltaTime);
            }
            else if (Speed < 0 && GameManager.Instance.isEunsin == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, PTransform, Speed * -1.3f * Time.deltaTime);
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
        if(Dash >= MaxDashTime && IsDash == false && GameManager.Instance.isEunsin == false)
        {
            if (Speed > 0)
            {
                Speed += 2;
            }
            else
            {
                Speed -= 2;
            }
            IsDash = true;
        }
    }
    public override void Delete()
    {
        base.Delete();
    }
    public override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
    }
    void Dashs()
    {
        Dashing += Time.deltaTime;
        if(Dashing >= MaxDashingTime)
        {
            if(Speed > 0)
            {
                Speed -= 2;
            }
            else
            {
                Speed += 2;
            }
            IsDash = false;
            Dashing = 0;
            Dash = 0;
        }

    }
    public override void CrossroadPlus()
    {
        base.CrossroadPlus();
    }
}
