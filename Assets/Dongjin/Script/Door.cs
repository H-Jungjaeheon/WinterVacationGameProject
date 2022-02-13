using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Door : MonoBehaviour
{
    private GameObject text;
    private bool opendoor = false;
    public GameObject door;
    private GameObject player;
    Image FadIn;
    private void Start()
    {
        FadIn = GameObject.Find("GameManager").GetComponent<GameManager>().FadIn;
        player = GameObject.Find("Player");
        text = transform.GetChild(0).gameObject;
    }
   
    private void Update()
    {
        if (opendoor == true && Input.GetKeyDown(KeyCode.F) &&GameObject.Find("Player").GetComponent<Player>().IsGrab == false)
        {
            player.transform.position = door.transform.position;
            GameManager.Instance.isRoom = true;
            Debug.Log("¾À ³Ñ¾î°¨¿ä");
            //StartCoroutine(BattleStartFaidIn(1f));
            StartCoroutine(BattleStartFaidOut(0.01f));


        }
    }
    IEnumerator BattleStartFaidOut(float FaidTime)
    {
        Color color = FadIn.color;
        //Color color2 = BattleStartImage.color;
        while (color.a < 1f /*&& color2.a < 1f*/)
        {
            color.a += Time.deltaTime / FaidTime;
            //color2.a += Time.deltaTime / FaidTime;
            FadIn.color = color;
            //BattleStartImage.color = color2;
            if (color.a >= 1f /*&& color2.a >= 1f*/)
            {
                color.a = 1f;
                //color2.a = 1f;
            }
            yield return null;
        }
        StartCoroutine(BattleStartFaidIn(1f));

    }
    IEnumerator BattleStartFaidIn(float FaidTime)
    {
        Color color = FadIn.color;
        //Color color2 = BattleStartImage.color;
        while (color.a > 0f /*&& color2.a > 0f*/)
        {
            color.a -= Time.deltaTime / FaidTime;
            //color2.a -= Time.deltaTime / FaidTime;
            FadIn.color = color;
            //BattleStartImage.color = color2;
            if (color.a <= 0f /*&& color2.a <= 0f*/)
            {
                color.a = 0f;
                //color2.a = 0f;
            }
            yield return null;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            text.SetActive(true);
            opendoor = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            text.SetActive(false);
            opendoor = false;
        }
    }
}
