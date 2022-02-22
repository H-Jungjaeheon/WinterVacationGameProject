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
    public bool IsGrab, isHidecollision, isHide, isParalysis, GetOutElectricity, gasmasktrue;
    public float GrapCount, MaxGrapCount;
    public float cnt = 0;
    public List<GameObject> Chest = new List<GameObject>();

    void Start()
    {
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
            StartCoroutine(gasmask(30f));
            gasmaskon = true;
            gasmasktrue = false;
        }
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
                bool isF;
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
                    rigid.velocity = new Vector2(rigid.velocity.x, Time.deltaTime * speed * 50);
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
            else if (isHidecollision == true && isHide == true && Input.GetKeyDown(KeyCode.F) && IsGrab == false)
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
    IEnumerator gasmask(float time)
    {
        float time2 = 0;
        img_gasmask.gameObject.SetActive(true);
        while (time >= time2)
        {
            if(gasdam == true)
            {
                time2 += Time.deltaTime;
                img_gasmask.fillAmount = ((time - time2) / time);
            }
            yield return new WaitForFixedUpdate();
        }
        gasmaskon = false;
        img_gasmask.gameObject.SetActive(false);
        yield return null;
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
    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                if (GameManager.Instance.BattleEndCount == 0 && GameManager.Instance.isEunsin == false&& GameManager.Instance.isDoor==false)
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
            case "Lime":
                if (speed == 10)
                {
                    speed = 5;
                }
                speed /= 2f;
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
            case "Box":
                if (collision.gameObject.GetComponent<Obj1>().BoxDrop == false)
                {
                    Chest.Add(collision.gameObject);

                }
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
                speed = 5f;
                break;
            case "hideObj":
                isHidecollision = false;
                break;
            case "Electricity":
                isElDam=false;
                break;
            case "Box":
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
            GameManager.Instance.curMana -= Time.deltaTime * 2f;
        }
    }
}
