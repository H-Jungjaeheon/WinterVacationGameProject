using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    public bool isDamage = false;
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.IsBattleStart == false) //GameManager.Instance.IsMove == true
        {
            Move();
        }
        Hide();
        SurviveDamage();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {   
        if (collision.CompareTag("Obj") && Obj.Instance.isIt == true && Input.GetKey(KeyCode.F))
        {                  
             Debug.Log("f키 누름");
             Obj.Instance.Drop();               
             Obj.Instance.Interaction.SetActive(false);
             Obj.Instance.isIt = false;
        }
    }
    void Move()
    {
        float x = Input.GetAxis("Horizontal");

        transform.Translate(new Vector2(x, 0) * Time.deltaTime * speed);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //GameManager.Instance.IsMove = false;
            GameManager.Instance.IsBattleStart = true;
        }
        if (collision.CompareTag("Gas"))
        {
            Debug.Log("가스에닿음");
            isDamage = true;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Gas"))
        {
            Debug.Log("가스에 안 닿음");
            isDamage = false;
        }
    }
    void SurviveDamage()
    {
        if (isDamage == true)
        {
            GameManager.Instance.curSurvive += Time.deltaTime*2f;

        }

    }
    void Hide()
    {
        if(GameManager.Instance.IsBattleStart == true)
        {
            //this.gameObject.SetActive(false);
        }
    }

}
