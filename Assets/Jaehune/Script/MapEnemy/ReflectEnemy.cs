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
    public override void Update()
    {
        base.Update();
        if(IsDash == true)
        {
            Dashs();
        }
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
        Debug.DrawRay(transform.position, Vector3.left * SeeCrossroad * IsPlus, Color.red);
        var rayHit = Physics2D.RaycastAll(transform.position, Vector3.left, SeeCrossroad * IsPlus);
        foreach (var hit in rayHit)
        {
            if (hit.collider.gameObject.CompareTag("Player") && GameManager.Instance.isEunsin == false)
            {
                WarningObj.SetActive(true);
                IsFind = true;
                Player = hit.collider.gameObject;
            }
            else if(hit.collider.gameObject.CompareTag("Enemy"))
            {
                WarningObj.SetActive(false);
                IsFind = false;
            }
        }
    }
    public override void FindPlayer()
    {
        Dash += Time.deltaTime;
        MoveCount = 0;
        if (Speed > 0 && GameManager.Instance.isEunsin == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position - new Vector3(0, 0.12f, 0), Speed * 1.3f * Time.deltaTime);
        }
        else if(Speed < 0 && GameManager.Instance.isEunsin == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position - new Vector3(0, 0.12f, 0), Speed * -1.3f * Time.deltaTime);
        }
        if(Dash >= MaxDashTime && IsDash == false && GameManager.Instance.isEunsin == false) // && GameManager.Instance.isEunsin == false 
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
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
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
