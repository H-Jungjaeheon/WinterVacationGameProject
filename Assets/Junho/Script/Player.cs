using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    [SerializeField] float speed, jumpPower;
    [SerializeField] bool isGound, isLadder, isDamage = false;

    Obj obj;
    void Start()
    {
        obj = GetComponent<Obj>();
        rigid = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if(GameManager.Instance.IsBattleStart == false) //GameManager.Instance.IsMove == true
        {
            if(GameManager.Instance.IsBattleStart == false)
            {
                Move();
                Jump();
            }
            if (isLadder)
            {
                bool isF;
                if (Input.GetKey(KeyCode.F))
                {
                    isF = true;
                } else isF = false;
                if (isF)
                {
                rigid.gravityScale = 0;
                rigid.velocity = new Vector2(rigid.velocity.x, Time.deltaTime*speed*50);
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
        if (collision.CompareTag("Obj") && Obj.Instance.isIt == true && Input.GetKey(KeyCode.F))
        {                  
             Debug.Log("fŰ ����");
            GameObject.Find("Obj").GetComponent<Obj>().Drop();
        
        }
        if (collision.CompareTag("Obj_2") && Obj.Instance.isIt == true && Input.GetKey(KeyCode.F))
        {
            Debug.Log("fŰ ����");

            Cnt();
            if (cnt > 1)
            {
            GameObject.Find("Obj_2").GetComponent<Obj>().Drop();
                cnt = 0;
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
        rigid.velocity = new Vector2 (x*speed, rigid.velocity.y);
    }
    void Jump()
    {
        if (isGound == true && Input.GetKeyDown(KeyCode.Space))
        { 
            rigid.velocity = Vector2.up * jumpPower;
        }
        else return;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.Instance.IsBattleStart = true;
        }
        else if (collision.CompareTag("Gas"))
        {
            Debug.Log("����������");
            isDamage = true;
        }
        else if (collision.CompareTag("Ladder"))
        {
            Debug.Log("��ٸ��� ����");
            isLadder = true;
        }
        else if (collision.CompareTag("Plan"))
        {
            isGound = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Gas"))
        {
            Debug.Log("������ �� ����");
            isDamage = false;
        }
        else if (collision.CompareTag("Ladder"))
        {
            Debug.Log("��ٸ��� �� ����");

            isLadder = false;
        }
        else if (collision.CompareTag("Plan"))
        {
            isGound = false;
        }
    }
    void SurviveDamage()
    {
        if (isDamage == true)
        {
            GameManager.Instance.curSurvive += Time.deltaTime*2f;
        }
    }
}
