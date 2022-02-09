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
    public bool isSpeedPotion = false, isEunsinPotion = false,isManaBarrier = false, IsGrab = false , isHidecollision = false,isHide=false;
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
            
            if(IsGrab == false&&isHide==false)
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
        if (GameManager.Instance.IsBattleStart == false) //GameManager.Instance.IsMove == true
        {
            if (isSpeedPotion == true && Input.GetKey(KeyCode.F))
            {
                StartCoroutine("speedPotion");
                isSpeedPotion = false;
            }
            else if (isEunsinPotion == true && Input.GetKey(KeyCode.F))
            {
                StartCoroutine("Eunsincnt");
                isEunsinPotion = false;
            }
            else if (isManaBarrier == true && Input.GetKey(KeyCode.F))
            {
                StartCoroutine("ManaBarrier");
                isManaBarrier = false;
            }
            else if (isHidecollision == true && isHide == false && Input.GetKeyDown(KeyCode.F))
            {
                isHide = true;
                GameManager.Instance.isEunsin = true;
                this.spriteRenderer.enabled = false;
                Debug.Log("º˚¿Ω");
                
            }
            else if (isHidecollision == true && isHide == true && Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("æ»º˚¿Ω");

                isHide = false;
                GameManager.Instance.isEunsin = false;
                this.spriteRenderer.enabled = true;

            }
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

                Debug.Log("∞°Ω∫ø°¥Í¿Ω");
                if (GameManager.Instance.isTrapBarrier == false)
                {
                    isDamage = true;

                }
                break;
            case "Ladder":
                Debug.Log("ªÁ¥Ÿ∏Æø° ¥Í¿Ω");
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
                Debug.Log("±«¡ÿ»£ ∞≥ªı");
                break;
            case "ManaBarrier":
                isManaBarrier = true;
                break;
            case "hideObj":
                isHidecollision = true;
                Debug.Log("hideø° ¥Í¿Ω");
                break;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Gas":
            
                Debug.Log("∞°Ω∫ø° æ» ¥Í¿Ω");
                if (GameManager.Instance.isTrapBarrier==false)
                {
                    isDamage = false;

                }
                break;
            case "Ladder":
                Debug.Log("ªÁ¥Ÿ∏Æø° æ» ¥Í¿Ω");
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
            case "hideObj":
                isHidecollision = false;
                Debug.Log("§§");
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
