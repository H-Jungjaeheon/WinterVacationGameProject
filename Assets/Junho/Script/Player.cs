using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    [SerializeField] float speed = 5, jumpPower;
    [SerializeField] bool isGound, isLadder, isDamage = false;
    Animator anim;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.IsBattleStart == false) //GameManager.Instance.IsMove == true
        {
            Move();
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
            if (rigid.velocity.normalized.x==0)
            {
                anim.SetBool("IsWalk",false);
            }
            else anim.SetBool("IsWalk", true);
        }
    }
    void Update()
    {
        SurviveDamage();

    }
   
    

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        if (x < 0)
        {
        transform.rotation = Quaternion.Euler(0, 0, 0);

        }else if(x > 0)transform.rotation = Quaternion.Euler(0, 180, 0);
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
  
    void ObjManager()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && GameManager.Instance.BattleEndCount == 0)
        {
            GameManager.Instance.IsBattleStart = true;
        }
        else if (collision.CompareTag("Gas"))
        {
            Debug.Log("가스에닿음");
            isDamage = true;
        }
        else if (collision.CompareTag("Ladder"))
        {
            Debug.Log("사다리에 닿음");
            isLadder = true;
        }
        else if (collision.CompareTag("Plan") || collision.CompareTag("Corridor"))
        {
            isGound = true;
        }
        else if (collision.CompareTag("Corridor"))
        {
            Debug.Log("d");
            GameManager.Instance.isRoom = false;
        }
        else if (collision.CompareTag("Lime"))
        {
            speed *= 0.2f;
        }       
        

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Gas"))
        {
            Debug.Log("가스에 안 닿음");
            isDamage = false;
        }
        else if (collision.CompareTag("Ladder"))
        {
            Debug.Log("사다리에 안 닿음");

            isLadder = false;
        }
        else if (collision.CompareTag("Plan"))
        {
            isGound = false;
        }
        else if (collision.CompareTag("Lime"))
        {
            speed = 5f;
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
