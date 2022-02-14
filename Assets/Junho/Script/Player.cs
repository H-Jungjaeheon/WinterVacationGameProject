using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    public float speed = 5, jumpPower;
    [SerializeField] bool isGound, isLadder, isDamage = false;
    [SerializeField] Image img_gasmask;
    Animator anim;
    public bool IsGrab = false, isHidecollision = false, isHide = false, isParalysis = false, GetOutElectricity = false;
    public float GrapCount, MaxGrapCount;
    public float cnt = 0;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gasmask(3f);
    }
    private void FixedUpdate()
    {       
        SurviveDamage();
        if (GameManager.Instance.IsBattleStart == false) //GameManager.Instance.IsMove == true
        {
            /*
            if (GameManager.Instance.isBurns)
            {*/
                transform.GetChild(0).gameObject.SetActive(GameManager.Instance.isBurns);
                transform.GetChild(1).gameObject.SetActive(GameManager.Instance.isBurns);
/*
            }
            else
            {
                Burns[0].SetActive(false);
                Burns[1].SetActive(false);
            }*/


            if (IsGrab == false && isHide == false && isParalysis == false&&GameManager.Instance.isGameOver==false)
            {
                Move();
            }
            //Jump();
            if (isLadder)
            {
                bool isF;
                if (Input.GetKey(KeyCode.F) && IsGrab == false)
                {
                    isF = true;
                }
                else isF = false;
                if (isF && IsGrab == false)
                {
                    rigid.gravityScale = 0;
                    rigid.velocity = new Vector2(rigid.velocity.x, Time.deltaTime * speed * 50);
                }
            }
            else
            {
                rigid.gravityScale = 3f;
            }
            if (rigid.velocity.normalized.x == 0)
            {
                anim.SetBool("IsWalk", false);
            }
            else
            {
                anim.SetBool("IsWalk", true);
            }
        }

    }
    void Update()
    {
        if (GameManager.Instance.isEunsin)
        {
            this.spriteRenderer.color = new Color(spriteRenderer.color.b, spriteRenderer.color.g, spriteRenderer.color.r, 0.4f);
        }
        else this.spriteRenderer.color = new Color(spriteRenderer.color.b, spriteRenderer.color.g, spriteRenderer.color.r, 1f);


        if (IsGrab == true)
        {
            Grabbing();
        }
        if (GameManager.Instance.IsBattleStart == false) //GameManager.Instance.IsMove == true
        { 
            if (isHidecollision == true && isHide == false && Input.GetKeyDown(KeyCode.F) && IsGrab == false)
            {
                isHide = true;
                GameManager.Instance.isEunsin = true;
                this.spriteRenderer.enabled = false;
                Debug.Log("¼ûÀ½");

            }
            else if (isHidecollision == true && isHide == true && Input.GetKeyDown(KeyCode.F) && IsGrab == false)
            {
                Debug.Log("¾È¼ûÀ½");
                isHide = false;
                GameManager.Instance.isEunsin = false;
                this.spriteRenderer.enabled = true;
            }
        }

    }
    void Grabbing()
    {
        GrapCount -= Time.deltaTime * 18;
        if (Input.GetKeyDown(KeyCode.F))
        {
            GrapCount += 8; //MaxGrapCount
        }
        if (GrapCount <= 0)
        {
            GrapCount = 0;
        }
    }   
    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        if (x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);

        }
        else if (x > 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        rigid.velocity = new Vector2(x * speed, rigid.velocity.y);
    }
    public void gasmask(float time)
    {
        while(time > 1.0f)
        {
            time -= Time.deltaTime;
            img_gasmask.fillAmount = (1.0f / time);
        }
    }
    
    void Jump()
    {
        if (isGound == true && Input.GetKey(KeyCode.Space))
        {
            rigid.velocity = Vector2.up * jumpPower;
        }
        else return;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                if (GameManager.Instance.BattleEndCount == 0 && GameManager.Instance.isEunsin == false)
                {
                    IsGrab = false;
                    GameManager.Instance.IsBattleStart = true;
                    BattleManager.Instance.IsPlayerTurn = true;
                }
                break;

            case "Gas":
                if (GameManager.Instance.isTrapBarrier == false)
                {
                    isDamage = true;

                }
                break;
            case "Ladder":
                isLadder = true;
                break;
            case "Plan":
                isGound = true;

                break;
            case "Lime":
                if (speed == 10)
                {
                    speed = 5;
                }
                speed *= 0.2f;
                break;
            case "Corridor":
                GameManager.Instance.isRoom = false;
                break;
            case "hideObj":
                isHidecollision = true;
                break;
            case "Electricity":
                GameManager.Instance.stackDamage += 10;
                break;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Gas":
                isDamage = false;
                break;
            case "Ladder":
                isLadder = false;
                break;
            case "Plan":
                isGound = false;
                break;
            case "Lime":
                speed = 5f;
                break;
            case "hideObj":
                isHidecollision = false;
                break;
            case "Electricity":
                GetOutElectricity = true;
                cnt = 0;
                break;
        }

    }
    void SurviveDamage()
    {
        if (isDamage == true)
        {
            GameManager.Instance.curMana -= Time.deltaTime * 2f;
        }
    }
}
