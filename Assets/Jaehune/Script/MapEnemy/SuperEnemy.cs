using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperEnemy : BasicEnemyScript
{
    [SerializeField] float SkillCount;
    [SerializeField] GameObject WarningCircleObj, SkillCircleObj;
    [SerializeField] bool Skillng, Movings, IsSkillReady;

    public override void Start()
    {
        IsSkillReady = false;
        base.Start();
    }
    public override void FixedUpdate()
    {
        RayCasting();
        WallRayCasting();
        if (IsMove == false)
        {
            MoveCount = 0;
        }
        else if (IsMove == true)
        {
            Moving();
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
            if (IsStop == false)
            {
                IsMove = true;
            }
            IsTurns = true;
            Skillng = false;
            SkillCount = 0;
            SkillCircleObj.SetActive(false);
            CrossRoadObj.SetActive(true);
            IsPlus = 1;
        }
        if (SkillCount >= 5 && Skillng == false)
        {
            IsSkillReady = true;
            IsMove = false;
            Movings = false;
            WarningCircleObj.SetActive(true);
            Invoke("UseSkill", 2.5f);
            animator.SetBool("IsSkill", true);
        }
        else
        {
            Movings = true;
        }
    }
    private void Update()
    {

    }
    void ManaSkill()
    {
        SkillCount += Time.deltaTime;
    }
    void UseSkill()
    {
        if (Player != null && GameManager.Instance.IsBattleStart == false && GameManager.Instance.isEunsin == false)
        {
            IsSkillReady = false;
            animator.SetBool("IsSkill", false);
            Skillng = true;
            if (IsMoveTurn == false)
            {
                IsMove = true;
            }
            Movings = true;
            WarningCircleObj.SetActive(false);
            SkillCircleObj.SetActive(true);
        }
    }
    public override void WallRayCasting()
    {
        base.WallRayCasting();
    }
    public override void Moving()
    {
        if (IsMove == true && Movings == true)
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
    }
    public override void Trun()
    {
        animator.SetBool("IsIdle", false);
        base.Trun();
    }
    public override void RayCasting()
    {
        Debug.DrawRay(transform.position + new Vector3(0, -1, 0), Vector3.left * (SeeCrossroad * IsPlus), Color.red);
        if (Physics2D.Raycast(transform.position + new Vector3(0, -1, 0), Vector3.left, SeeCrossroad * IsPlus, LayerMask.GetMask("Player")) && GameManager.Instance.isEunsin == false)
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
        animator.SetBool("IsIdle", false);
        IsTurns = false;
        MoveCount = 0;
        IsMoveTurn = true;
        if (IsMove == true)
        {
            if (Speed > 0 && IsMove == true && GameManager.Instance.isEunsin == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, PTransform, Speed * 3f * Time.deltaTime);
            }
            else if (Speed < 0 && IsMove == true && GameManager.Instance.isEunsin == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, PTransform, Speed * -3f * Time.deltaTime);
            }
        }
        else if (IsSkillReady == false && IsMove == false)
        {
            if (Speed > 0 && GameManager.Instance.isEunsin == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, PTransform, Speed * 4f * Time.deltaTime);
            }
            else if (Speed < 0 && GameManager.Instance.isEunsin == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, PTransform, Speed * -4f * Time.deltaTime);
            }
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
    public override void CrossroadPlus()
    {
        IsPlus = 6;
    }
}