using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperEnemy : BasicEnemyScript
{
    [SerializeField] float SkillCount;
    [SerializeField] GameObject WarningCircleObj, SkillCircleObj;
    [SerializeField] bool Skillng, Movings;

    public override void Start()
    {
        base.Start();
    }
    public override void Update()
    {
        RayCasting();
        Moving();
        if (IsMove == false)
        {
            MoveCount = 0;
        }
        if (IsFind == true)
        {
            ManaSkill();
            FindPlayer();
            CrossroadPlus();
            CrossRoadObj.SetActive(false);
        }
        else
        {
            IsMove = true;
            Skillng = false;
            SkillCount = 0;
            SkillCircleObj.SetActive(false);
            CrossRoadObj.SetActive(true);
            IsPlus = 1;
        }
        if (SkillCount >= 5 && Skillng == false)
        {
            WarningCircleObj.SetActive(true);
            Invoke("UseSkill", 2.5f);
            IsMove = false;
            Movings = false;
        }
        else
        {
            Movings = true;
        }
    }
    void ManaSkill()
    {
        SkillCount += Time.deltaTime;
    }
    void UseSkill()
    {
        if (Player != null && GameManager.Instance.IsBattleStart == false && GameManager.Instance.isEunsin == false)
        {
            Skillng = true;
            IsMove = true;
            Movings = true;
            WarningCircleObj.SetActive(false);
            SkillCircleObj.SetActive(true);
        }
    }
    public override void Moving()
    {
        if (IsMove == true && Movings == true)
        {
            MoveCount += Time.deltaTime;
            transform.position += new Vector3(Speed * Time.deltaTime, 0, 0);
            if (MoveCount >= MaxMoveCount)
            {
                animator.SetBool("IsIdle", true);
                MoveCount = 0;
                IsMove = false;
                Invoke("Trun", 4f);
            }
        }
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
        if (Speed > 0 && IsMove == true && GameManager.Instance.isEunsin == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position + new Vector3(0, 0.15f, 0), Speed * 3f * Time.deltaTime);
        }
        else if (Speed < 0 && IsMove == true && GameManager.Instance.isEunsin == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position + new Vector3(0, 0.15f, 0), Speed * -3f * Time.deltaTime);
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
        IsPlus = 6;
    }
}