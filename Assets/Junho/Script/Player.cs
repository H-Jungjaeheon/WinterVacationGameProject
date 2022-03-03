using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    public float speed = 5, jumpPower;
    [SerializeField] bool isGound, isLadder, isDamage,gasdam = false,gasmaskon =false;
    [SerializeField] Image img_gasmask;
    [SerializeField] GameObject GrabWarningObj;
    public Animator anim;
    public bool IsGrab, isHidecollision, isHide, isParalysis, GetOutElectricity, gasmasktrue, isF;
    public float GrapCount, MaxGrapCount,time,time2;
    public float cnt = 0;
    public List<GameObject> Chest = new List<GameObject>();
    
    void Start()
    {
        img_gasmask.gameObject.SetActive(false);
        GrabWarningObj.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        SurviveDamage();
        if(gasmasktrue == true)
        {
            time2 = 0;
            img_gasmask.gameObject.SetActive(true);
            gasmaskon = true;
            gasmasktrue = false;

        }
        if (gasdam == true&&gasmaskon == true)
        {
            time2 += Time.deltaTime;
            if(time2 >= time)
            {
                gasmaskon = false;
                isDamage = true;
                img_gasmask.gameObject.SetActive(false);
            }
        }
        img_gasmask.fillAmount = ((time - time2) / time);
        if (GameManager.Instance.IsBattleStart == false) //GameManager.Instance.IsMove == true
        {
            transform.GetChild(0).gameObject.SetActive(GameManager.Instance.isBurns);
            transform.GetChild(1).gameObject.SetActive(GameManager.Instance.isBurns);
            if (IsGrab == false && isHide == false && isParalysis == false&&GameManager.Instance.isGameOver==false&& GameManager.Instance.BossRoom == false)
            {
                Move();
            }
            if (isLadder)
            {
                
                if (Input.GetKey(KeyCode.F) && IsGrab == false)
                {
                    GameManager.Instance.is2F = true;
                    isF = true;
                }
                else 
                {
                    isF = false;                  
                }
                if (isF && IsGrab == false)
                {
                    anim.SetBool("IsLadder", true);
                    rigid.gravityScale = 0;
                    rigid.velocity = new Vector2(rigid.velocity.x, Time.deltaTime * 5 * 50);
                }
            }
            else
            {
                anim.SetBool("IsLadder", false);

                rigid.gravityScale = 3f;
            }
            if (rigid.velocity.normalized.x == 0&&GameManager.Instance.BossRoom == false)
            {
                anim.SetBool("IsWalk", false);
            }
            else if(GameManager.Instance.BossRoom == false)
            {
                anim.SetBool("IsWalk", true);
            }
        }
        else
        {
            IsGrab = false;
            GrapCount = 0;
        }

    }
    void Update()
    {
        Grabbing();
        if (isElDam) ElDamage();
        if (GameManager.Instance.isEunsin)
        {
            this.spriteRenderer.color = new Color(spriteRenderer.color.b, spriteRenderer.color.g, spriteRenderer.color.r, 0.4f);
        }
        else this.spriteRenderer.color = new Color(spriteRenderer.color.b, spriteRenderer.color.g, spriteRenderer.color.r, 1f);
           
        if (GameManager.Instance.IsBattleStart == false) //GameManager.Instance.IsMove == true
        { 
            if (isHidecollision == true && isHide == false && Input.GetKeyDown(KeyCode.F) && IsGrab == false)
            {
                isHide = true;
                GameManager.Instance.isEunsin = true;
                this.spriteRenderer.enabled = false;

            }
            else if (isHide == true && Input.GetKeyDown(KeyCode.F) && IsGrab == false)
            {
                isHide = false;
                GameManager.Instance.isEunsin = false;
                this.spriteRenderer.enabled = true;
            }

        }
        

    }
    void Grabbing()
    {
        if (IsGrab == true)
        {
            GrabWarningObj.SetActive(true);
            GrapCount -= Time.deltaTime * 18;
            if (Input.GetKeyDown(KeyCode.F))
            {
                GrapCount += 8;
            }
            if (GrapCount <= 0)
            {
                GrapCount = 0;
            }
            GameObject.Find("Main Camera").GetComponent<CameraMove>().IsGrap = true;
        }
        else
        {
            GrabWarningObj.SetActive(false);
        }
    }   
    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        if (x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (x > 0) 
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); 
        }
        rigid.velocity = new Vector2(x * speed, rigid.velocity.y);
    }
    
    void Jump()
    {
        if (isGound == true && Input.GetKey(KeyCode.Space))
        {
            rigid.velocity = Vector2.up * jumpPower;
        }
        else return;
    }

    bool isElDam;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Lime":
                speed -= 3;
                break;
            case "Obj":
                if (collision.gameObject.GetComponent<Obj1>().BoxDrop == false)
                {
                    Chest.Add(collision.gameObject);

                }
                break;
            case "Obj_2":
                if (collision.gameObject.GetComponent<Obj_2>().BoxDrop == false)
                {
                    Chest.Add(collision.gameObject);
                }
                break;
            case "Obj_3":
                if (collision.gameObject.GetComponent<Obj_3>().BoxDrop == false)
                {
                    Chest.Add(collision.gameObject);
                }
                break;
        }
    } 
    void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                if (GameManager.Instance.BattleEndCount == 0 && GameManager.Instance.isEunsin == false&& GameManager.Instance.isDoor==false&& isF==false)
                {
                    IsGrab = false;
                    BattleManager.Instance.IsPlayerTurn = true;
                }
                break;

            case "Gas":
                if (GameManager.Instance.isTrapBarrier == false)
                {
                    gasdam = true;
                    if(gasmaskon == false)
                        isDamage = true;
                }
                break;
            case "Ladder":
                isLadder = true;
                break;
            case "Plan":
                anim.SetBool("IsLadder", false);
                isGound = true;
                GameManager.Instance.isBossRoom = false;

                break;
            case "Corridor":
                GameManager.Instance.isRoom = false;
                GameManager.Instance.isBossRoom = false;
                anim.SetBool("IsLadder", false);

                break;
            case "hideObj":
                isHidecollision = true;
                break;
            case "Electricity":
                isElDam = true;
                break;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Gas":
                isDamage = false;
                gasdam = false;
                break;
            case "Ladder":
                isLadder = false;
                break;
            case "Plan":
                isGound = false;
                break;
            case "Lime":
                speed += 3;
                break;
            case "hideObj":
                isHidecollision = false;
                break;
            case "Electricity":
                isElDam=false;
                break;
            case "Obj":
                Chest.Remove(collision.gameObject);
                break;
            case "Obj_2":
                Chest.Remove(collision.gameObject);
                break;
            case "Obj_3":
                Chest.Remove(collision.gameObject);
                break;
        }

    }
    void ElDamage()
    {
        GameManager.Instance.stackDamage += 5;
        isElDam = false;

    }
    void SurviveDamage()
    {
        if (isDamage == true)
        {
            GameManager.Instance.curMana -= Time.deltaTime;
        }
    }
}
