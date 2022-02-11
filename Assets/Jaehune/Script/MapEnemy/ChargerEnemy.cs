using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargerEnemy : BasicEnemyScript
{
    [SerializeField] float MaxSkillTime;
    [SerializeField] GameObject SkillHand;
    [SerializeField] Image GrapBar, NullBar;
    [SerializeField] bool GrabCountStop = false;
    public LineRenderer SkillLine;
    public float SkillTime;
    public bool IsSkill = false;
    //스킬 사거리 5.5
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        SkillLine = GetComponentInChildren<LineRenderer>();
        SkillLine.widthMultiplier = 1;
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
            CrossroadPlus();
            CrossRoadObj.SetActive(false);
        }
        else
        {
            CrossRoadObj.SetActive(true);
            IsPlus = 1;
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
        if (Player != null && GameManager.Instance.IsBattleStart == false && GameManager.Instance.isEunsin == false)
        {
            Color color = GrapBar.color;
            Color color2 = NullBar.color;
            GrapBar.transform.position = Camera.main.WorldToScreenPoint(Player.transform.position + new Vector3(-0.1f, 1.5f, 0));
            NullBar.transform.position = Camera.main.WorldToScreenPoint(Player.transform.position + new Vector3(-0.1f, 1.5f, 0));
            GrapBar.fillAmount = GameObject.Find("Player").GetComponent<Player>().GrapCount / GameObject.Find("Player").GetComponent<Player>().MaxGrapCount;
            if (SkillTime >= MaxSkillTime)
            {
                color.a = 1;
                color2.a = 1;
                GameObject.Find("Main Camera").GetComponent<CameraMove>().IsGrab = true;
                GameObject.Find("Player").GetComponent<Player>().IsGrab = true;
                IsSkill = true;
                IsMove = false;
                Player.transform.position = Vector3.MoveTowards(Player.transform.position, this.transform.position, 2f * Time.deltaTime);
                Debug.Assert(SkillLine != null);
                SkillLine.SetPosition(0, this.transform.position - new Vector3(0, 0.6f, 0));
                SkillLine.SetPosition(1, Player.transform.position);
                SkillHand.SetActive(true);
                SkillHand.transform.position = Player.transform.position + new Vector3(0, 0.1f, 0);
            }
            else
            {
                GameObject.Find("Main Camera").GetComponent<CameraMove>().IsGrab = false;
                SkillLine.SetPosition(0, this.transform.position - new Vector3(0, 0.6f, 0));
                SkillLine.SetPosition(1, this.transform.position - new Vector3(0, 0.6f, 0));
                color.a = 0;
                color2.a = 0;
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
        if (Speed > 0 && IsMove == true && GameManager.Instance.isEunsin == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position + new Vector3(0, 0.6f, 0), Speed * 1.3f * Time.deltaTime);
        }
        else if (Speed < 0 && IsMove == true && GameManager.Instance.isEunsin == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position + new Vector3(0, 0.6f, 0), Speed * -1.3f * Time.deltaTime);
        }
        if(GrabCountStop == false && GameManager.Instance.isEunsin == false)
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
    public override void CrossroadPlus()
    {
        base.CrossroadPlus();
    }
}
