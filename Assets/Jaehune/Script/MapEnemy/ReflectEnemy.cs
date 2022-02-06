using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectEnemy : BasicEnemyScript
{
    [SerializeField] float Dash, MaxDashTime;
    [SerializeField] bool IsDash;
    // Start is called before the first frame update
    public override void Start()
    {
        Dash = 0;
        IsDash = true;
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
        Debug.DrawRay(transform.position, Vector3.left * SeeCrossroad, Color.red);
        var rayHit = Physics2D.RaycastAll(transform.position, Vector3.left, SeeCrossroad);
        foreach (var hit in rayHit)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
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
        if (Speed > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, Speed * 1.3f * Time.deltaTime);
        }
        else if(Speed < 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, Speed * -1.3f * Time.deltaTime);
        }
        if(Dash >= MaxDashTime && IsDash == true)
        {
            IsDash = false;
            StartCoroutine("Dashs");
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
    IEnumerator Dashs()
    {
        if(Speed > 0)
        {
            Speed += 2;
        }
        else
        {
            Speed -= 2;
        }
        yield return new WaitForSeconds(2.5f);
        if (Speed > 0)
        {
            Speed -= 2;
        }
        else
        {
            Speed += 2;
        }
        Dash = 0;
        IsDash = true;
        yield return null;
    }
}
