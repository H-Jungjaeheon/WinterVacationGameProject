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
    [SerializeField] GameObject[] Burns;
    Animator anim;
    public bool isSpeedPotion = false, isEunsinPotion = false,isManaBarrier = false, IsGrab = false , isHidecollision = false, isHide=false, isParalysis = false, GetOutElectricity =false;
    public float GrapCount, MaxGrapCount;
    float cnt = 0;

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
            if (GameManager.Instance.isBurns)
            {
                Burns[0].SetActive(true);
                Burns[1].SetActive(true);

            }
            else 
            {
                Burns[0].SetActive(false);
                Burns[1].SetActive(false);
            } 
                

            if (IsGrab == false && isHide==false && isParalysis==false)
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
            if (GetOutElectricity)
            {
                cnt+=Time.deltaTime;
                if (cnt >= 5f)
                {
                    Debug.Log("³¡");
                    GetOutElectricity = false;
                    isParalysis = false;
                    cnt = 0;
                }
            }

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
            else if (isHidecollision == true && isHide == false && Input.GetKeyDown(KeyCode.F) && IsGrab == false)
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
    IEnumerator Paralysis()
    {
        isParalysis = true;
        yield return new WaitForSeconds(1f);
        isParalysis = false;
        if (GetOutElectricity)
        {
            StartCoroutine(Paralysis());

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
                if (GameManager.Instance.BattleEndCount==0 && GameManager.Instance.isEunsin == false)
                {
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
                GameManager.Instance.isRoom = false;
                break;
            case "Eunsin":
                isEunsinPotion = true;
                break;
            case "ManaBarrier":
                isManaBarrier = true;
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
                if (GameManager.Instance.isTrapBarrier==false)
                {
                    isDamage = false;

                }
                break;
            case "Ladder":
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
                break;
            case "Electricity":
                GetOutElectricity = true;
                cnt = 0;
                StartCoroutine(Paralysis());
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
