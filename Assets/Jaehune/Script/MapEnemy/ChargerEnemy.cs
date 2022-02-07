using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerEnemy : BasicEnemyScript
{
    [SerializeField] float MaxSkillTime;
    [SerializeField] GameObject SkillHitPlayer;
    public float SkillTime;
    public bool IsSkill = false;
    //스킬 사거리 5.5
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
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
            FindPlayer();
            CrossRoadObj.SetActive(false);
        }
        else
        {
            CrossRoadObj.SetActive(true);
        }
        if(SkillTime >= MaxSkillTime)
        {
            Skill();
        }
    }
    void Skill() //스킬 풀기 위해 SkillTime = 0
    {
        GameObject.Find("Player").GetComponent<Player>().IsGrab = true;
        IsSkill = true;
        IsMove = false;
        SkillHitPlayer.transform.position = Vector3.MoveTowards(SkillHitPlayer.transform.position, this.transform.position, 1f * Time.deltaTime);
    }
    public override void Moving()
    {
        if (IsMove == true)
        {
            
            MoveCount += Time.deltaTime;
            transform.position += new Vector3(Speed * Time.deltaTime, 0, 0);
            if (MoveCount >= MaxMoveCount && SkillTime < MaxSkillTime)
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
        if (Speed > 0 && IsMove == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position + new Vector3(0, 0.6f, 0), Speed * 1.3f * Time.deltaTime);
        }
        else if (Speed < 0 && IsMove == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position + new Vector3(0, 0.6f, 0), Speed * -1.3f * Time.deltaTime);
        }
        SkillTime += Time.deltaTime;
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
