using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    [SerializeField] float speed = 5, jumpPower;
    [SerializeField] bool isGound, isLadder, isDamage = false;
    Animator anim;
    public bool isSpeedPotion = false, isEunsinPotion = false,isManaBarrier = false, IsGrab = false;
    public float GrapCount, MaxGrapCount;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        SurviveDamage();
        if (GameManager.Instance.IsBattleStart == false) //GameManager.Instance.IsMove == true
        {
            if (isSpeedPotion == true && Input.GetKey(KeyCode.F))
            {
                StartCoroutine(speedPotion());
                isSpeedPotion = false;
            }
            else if (isEunsinPotion == true && Input.GetKey(KeyCode.F))
            {
                StartCoroutine(Eunsincnt());
                isEunsinPotion = false;
            }
            else if (isManaBarrier == true && Input.GetKey(KeyCode.F))
            {
                StartCoroutine(ManaBarrier());
                isManaBarrier = false;
            }
            if(IsGrab == false)
            {
                Move();
            }
            //Jump();
            if (isLadder)
            {
                bool isF;
                if (Input.GetKey(KeyCode.F))
                {
                    isF = true;
                }
                else isF = false;
                if (isF)
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
        if (IsGrab == true)
        {
            Grabbing();
        }
    }
    void Grabbing()
    {
        GrapCount -= Time.deltaTime * 15;
        if (Input.GetKeyDown(KeyCode.F))
        {
            GrapCount += 8; //MaxGrapCount
        }
        if(GrapCount <= 0)
        {
            GrapCount = 0;
        }
    }
    IEnumerator ManaBarrier()
    {
        GameManager.Instance.isManaBarrier = true;
        yield return new WaitForSeconds(10f);
        GameManager.Instance.isManaBarrier = false;
    }
   
    IEnumerator speedPotion()
    {
        speed = 10;
        yield return new WaitForSeconds(5.0f);
        speed = 5;
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
    IEnumerator Eunsincnt()
    {
        GameManager.Instance.isEunsin = true;
        this.spriteRenderer.color = new Color(spriteRenderer.color.b, spriteRenderer.color.g, spriteRenderer.color.r, 0.4f);
        yield return new WaitForSeconds(5f);
        GameManager.Instance.isEunsin = false;
        this.spriteRenderer.color = new Color(spriteRenderer.color.b, spriteRenderer.color.g, spriteRenderer.color.r, 1f);
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
                if (GameManager.Instance.BattleEndCount==0)
                {
                    GameManager.Instance.IsBattleStart = true;

                }
                break;

            case "Gas":

                Debug.Log("가스에닿음");
                if (GameManager.Instance.isTrapBarrier == false)
                {
                    isDamage = true;

                }
                break;
            case "Ladder":
                Debug.Log("사다리에 닿음");
                isLadder = true;
                break;
            case "Plan":
                isGound = true;

                break;
            case "Lime":
                if (speed ==10)
                {
                    speed = 5;
                }
                speed *= 0.2f;
                break;
            case "SpeedPotion":
                isSpeedPotion = true;
                break;
            case "Corridor":
                Debug.Log("d");
                GameManager.Instance.isRoom = false;
                break;
            case "Eunsin":
                isEunsinPotion = true;
                Debug.Log("권준호 개새");
                break;
            case "ManaBarrier":
                isManaBarrier = true;
                break;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Gas":
            
                Debug.Log("가스에 안 닿음");
                if (GameManager.Instance.isTrapBarrier==false)
                {
                    isDamage = false;

                }
                break;
            case "Ladder":
                Debug.Log("사다리에 안 닿음");
                isLadder = false;
                break;
            case "Plan":
                isGound = false;
                break;
            case "Lime":
                if (speed == 5)
                {
                    speed = 10;
                }
                speed = 5f;
                break;
            case "SpeedPotion":
                isSpeedPotion = false;
                break;
            case "Eunsin":
                isEunsinPotion = false;
                break;
            case "ManaBarrier":
                isManaBarrier = false;
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
