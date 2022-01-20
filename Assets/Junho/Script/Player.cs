using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Slider hpBar;
    private float maxHp = 100;
    private float curHp = 100;

    [SerializeField]
    private Slider surviveBar;
    private float maxSurvive = 100;
    private float curSurvive = 100;

    public bool IsFighting;
    public GameObject fightMenu;

    public float speed;


    // Start is called before the first frame update
    void Start()
    {
        hpBar.value = (float) curHp / (float) maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            curHp -= 10;
        }
        curSurvive -= 0.001f;
        HandleSlider();
    }
    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Jump");
       
        transform.Translate(new Vector2(x, y) * Time.deltaTime * speed);
    }
    void InputManager()
    {
        if (Input.GetKey(KeyCode.F))
        {
            //¿¿æ÷
        }
    }
    private void HandleSlider()
    {
        hpBar.value = (float) curHp / (float) maxHp;
        surviveBar.value = (float)curSurvive / (float)maxSurvive;
    }
    void Fight()
    {
        if (IsFighting==true)
        {
            fightMenu.SetActive(true);
        }
        else fightMenu.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("d");
            IsFighting = true;
        }
    }
}
