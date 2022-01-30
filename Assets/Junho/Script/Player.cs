using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    [SerializeField] float speed = 5, jumpPower;
    [SerializeField] bool isGound, isLadder, isDamage = false;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
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
        }
    }
    void Update()
    {
        SurviveDamage();

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        {
            if (collision.CompareTag("Obj") && Obj.Instance.isIt == true && Input.GetKey(KeyCode.F))
            {
                Debug.Log("기본 드롭");
                GameObject.Find("Obj").GetComponent<Obj>().Drop();

            }
            if (collision.CompareTag("Obj_2") && Obj.Instance.isIt == true && Input.GetKey(KeyCode.F))
            {
                Debug.Log("n초 동안 누르면 드롭");

                Cnt();
                if (cnt > 2)
                {
                    GameObject.Find("Obj_2").GetComponent<Obj>().Drop();
                    cnt = 0;
                }

            }

        }
    }
    float cnt;
    void Cnt()
    {
        cnt += Time.deltaTime;
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
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
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && GameManager.Instance.BattleEndCount == 0)
        {
            GameManager.Instance.IsBattleStart = true;
        }
        if (collision.CompareTag("Gas"))
        {
            Debug.Log("가스에닿음");
            isDamage = true;
        }
        if (collision.CompareTag("Ladder"))
        {
            Debug.Log("사다리에 닿음");
            isLadder = true;
        }
        if (collision.CompareTag("Plan") || collision.CompareTag("Corridor"))
        {
            isGound = true;
        }
        if (collision.CompareTag("Corridor"))
        {
            Debug.Log("d");
            GameManager.Instance.isRoom = false;
        }
        if (collision.CompareTag("Lime"))
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
