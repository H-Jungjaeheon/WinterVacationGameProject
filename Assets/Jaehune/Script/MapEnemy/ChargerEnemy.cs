using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargerEnemy : BasicEnemyScript
{
    [SerializeField] float MaxSkillTime;
    [SerializeField] GameObject SkillHand;
    [SerializeField] Image GrapBar;
    [SerializeField] bool GrabCountStop = false;
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
        Skill();
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
    }
    private void FixedUpdate()
    {
        if (Player != null)
        {                                                             
            if (Player.GetComponent<Player>().IsGrab == true)
            {
                GrabCountStop = true;
            }
            else
            {
                GrabCountStop = false;
            }
        }
    }
    void Skill() //스킬 풀면 SkillTime = 0
    {
        if (Player != null && GameManager.Instance.IsBattleStart == false)
        {
            Color color = GrapBar.color;
            GrapBar.transform.position = Camera.main.WorldToScreenPoint(Player.transform.position + new Vector3(-0.1f, 1.5f, 0));
            GrapBar.fillAmount = GameObject.Find("Player").GetComponent<Player>().GrapCount / GameObject.Find("Player").GetComponent<Player>().MaxGrapCount;
            if (SkillTime >= MaxSkillTime)
            {
                color.a = 1;
                GameObject.Find("Player").GetComponent<Player>().IsGrab = true;
                IsSkill = true;
                IsMove = false;
                Player.transform.position = Vector3.MoveTowards(Player.transform.position, this.transform.position, 1f * Time.deltaTime);
                SkillHand.SetActive(true);
                SkillHand.transform.position = Player.transform.position;
            }
            else
            {
                color.a = 0;
                GameObject.Find("Player").GetComponent<Player>().IsGrab = false;
                GameObject.Find("Player").GetComponent<Player>().GrapCount = 0;
                IsSkill = false;
                IsMove = true;
                SkillHand.SetActive(false);
            }

            if (GameObject.Find("Player").GetComponent<Player>().GrapCount >= GameObject.Find("Player").GetComponent<Player>().MaxGrapCount)
            {
                SkillTime = 0;
            }
        }
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
            else if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                WarningObj.SetActive(false);
                IsFind = false;
            }
        }
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
        if(GrabCountStop == false)
        {
            SkillTime += Time.deltaTime;
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
}
