using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player P_Instance { get; private set; }

    [SerializeField]
    private Slider hpBar;
    private float maxHp = 100;
    public float curHp = 100;

    
    [SerializeField]
    private Slider surviveBar;
    private float maxSurvive = 100;
    public float curSurvive = 100;

    public float speed;

    // Start is called before the first frame update

    void Start()
    {
        P_Instance = this;
        hpBar.value = (float)curHp / (float)maxHp;
        surviveBar.value = (float)curSurvive / (float)maxSurvive;
        
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            curHp -= 10; 
        }
        HandleSlider();
        curSurvive -= 0.001f;
        Move();

    }
    private void HandleSlider()
    {
        hpBar.value = (float)curHp / (float)maxHp;
        surviveBar.value = (float)curSurvive / (float)maxSurvive;

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Obj"))
        {
            if (Obj.Instance.isIt == true)
            {
                if (Input.GetKey(KeyCode.F))
                {
                    if (Obj.Instance.isIt == true)
                    {
                        Debug.Log("f≈∞ ¥©∏ß");
                        Obj.Instance.Drop();               
                        Obj.Instance.Interaction.SetActive(false);
                        Obj.Instance.isIt = false;

                    }
                }

            }

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
            speed = 0;
        }
    }
    //public void HpRecovery()
    //{
    //    curHp += 10;
    //}
    //public void SurviveRecovery()
    //{
    //    curSurvive += 10;
    //}


}
